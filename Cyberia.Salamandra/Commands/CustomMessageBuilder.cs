using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands
{
    public abstract class CustomMessageBuilder
    {
        protected readonly DiscordUser _user;
        protected DiscordMessage? _messageSent;

        public CustomMessageBuilder(DiscordUser user)
        {
            _user = user;
        }

        protected abstract Task<DiscordEmbedBuilder> EmbedBuilder();

        protected virtual async Task<DiscordInteractionResponseBuilder> InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = new DiscordInteractionResponseBuilder()
                .AddEmbed(await EmbedBuilder());

            return response;
        }

        protected virtual async Task<DiscordFollowupMessageBuilder> FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = new DiscordFollowupMessageBuilder()
                .AddEmbed(await EmbedBuilder());

            return followupMessage;
        }

        private async Task CreateInteractionResponse(DiscordInteraction interaction, InteractionResponseType type)
        {
            DiscordInteractionResponseBuilder response = await InteractionResponseBuilder();

            if (response.Components.Count > 0)
                AddInteraction();

            await interaction.CreateResponseAsync(type, response);
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

        public virtual async Task SendFollowupMessage(DiscordInteraction interaction)
        {
            DiscordFollowupMessageBuilder followupMessage = await FollowupMessageBuilder();

            if (followupMessage.Components.Count > 0)
                AddInteraction();

            _messageSent = await interaction.CreateFollowupMessageAsync(followupMessage);
        }

        private async Task OnComponentInteractionCreated(DiscordClient client, ComponentInteractionCreateEventArgs e)
        {
            if (e.User.IsBot || _messageSent is null || _messageSent.Id != e.Message.Id)
                return;

            DelInteraction();

            await InteractionTreatment(e);
        }

        protected virtual Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            return Task.FromResult(false);
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