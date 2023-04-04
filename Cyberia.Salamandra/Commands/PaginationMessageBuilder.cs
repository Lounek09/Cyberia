using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands
{
    public abstract class PaginationMessageBuilder
    {
        protected const int MAX_PER_PAGE = 25;

        private readonly DofusEmbedCategory _category;
        private readonly string _authorText;
        private readonly string _title;
        private readonly DiscordUser _user;
        private int _currentPage;
        private DiscordMessage? _messageSent;

        protected readonly List<string> _content;

        public PaginationMessageBuilder(DofusEmbedCategory category, string authorText, string title, DiscordUser user)
        {
            _category = category;
            _authorText = authorText;
            _title = title;
            _content = new();
            _user = user;
            _currentPage = 1;
        }

        protected abstract void PopulateContent();

        protected int NumberOfPage()
        {
            return Math.Max(1, Convert.ToInt32(Math.Ceiling(_content.Count / (double)MAX_PER_PAGE)));
        }

        protected int GetStartPageIndex()
        {
            return (_currentPage - 1) * MAX_PER_PAGE;
        }

        protected int GetEndPageIndex()
        {
            return Math.Min(_content.Count - GetStartPageIndex(), MAX_PER_PAGE);
        }

        protected List<string> GetPageContent()
        {
            return _content.GetRange(GetStartPageIndex(), GetEndPageIndex());
        }

        protected DiscordEmbedBuilder EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(_category, _authorText)
                .WithTitle(_title)
                .WithDescription(string.Join('\n', GetPageContent()))
                .AddField(Constant.ZERO_WIDTH_SPACE, $"Page {_currentPage}/{NumberOfPage()}");

            return embed;
        }

        protected static HashSet<DiscordButtonComponent> PaginationButtonsBuilder()
        {
            return new()
            {
                new(ButtonStyle.Primary, "previous", "Précédent"),
                new(ButtonStyle.Primary, "next", "Suivant")
            };
        }

        protected abstract DiscordSelectComponent SelectBuilder();

        protected DiscordInteractionResponseBuilder InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = new DiscordInteractionResponseBuilder()
                .AddEmbed(EmbedBuilder());

            if (NumberOfPage() > 1)
                response.AddComponents(PaginationButtonsBuilder());

            response.AddComponents(SelectBuilder());

            return response;
        }

        protected DiscordFollowupMessageBuilder FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = new DiscordFollowupMessageBuilder()
                .AddEmbed(EmbedBuilder());

            if (NumberOfPage() > 1)
                followupMessage.AddComponents(PaginationButtonsBuilder());

            followupMessage.AddComponents(SelectBuilder());

            return followupMessage;
        }

        private async Task CreateInteractionResponse(DiscordInteraction interaction, InteractionResponseType type)
        {
            AddInteraction();

            await interaction.CreateResponseAsync(type, InteractionResponseBuilder());
            _messageSent = await interaction.GetOriginalResponseAsync();
        }

        public virtual async Task SendInteractionResponse(DiscordInteraction interaction)
        {
            await CreateInteractionResponse(interaction, InteractionResponseType.ChannelMessageWithSource);
        }

        public async Task UpdateInteractionResponse(DiscordInteraction interaction)
        {
            await CreateInteractionResponse(interaction, InteractionResponseType.UpdateMessage);
        }

        public async Task SendFollowupMessage(DiscordInteraction interaction)
        {
            AddInteraction();

            _messageSent = await interaction.CreateFollowupMessageAsync(FollowupMessageBuilder());
        }

        private async Task OnComponentInteractionCreated(DiscordClient client, ComponentInteractionCreateEventArgs e)
        {
            if (e.User.IsBot || _messageSent is null || _messageSent.Id != e.Message.Id)
                return;

            if (_user.Id != e.User.Id)
            {
                DiscordInteractionResponseBuilder response = new()
                {
                    Content = "Vous n'avez pas le droit de faire ça !",
                    IsEphemeral = true
                };

                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
                return;
            }

            DelInteraction();

            await InteractionTreatment(e);
        }

        protected virtual async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            switch (e.Id)
            {
                case "next":
                    _currentPage = _currentPage == NumberOfPage() ? 1 : _currentPage + 1;

                    await UpdateInteractionResponse(e.Interaction);
                    return true;
                case "previous":
                    _currentPage = _currentPage == 1 ? NumberOfPage() : _currentPage - 1;

                    await UpdateInteractionResponse(e.Interaction);
                    return true;
                default:
                    return false;
            }
        }

        protected void AddInteraction()
        {
            Bot.Instance.Client.ComponentInteractionCreated += OnComponentInteractionCreated;
        }

        protected void DelInteraction()
        {
            Bot.Instance.Client.ComponentInteractionCreated -= OnComponentInteractionCreated;
        }
    }
}