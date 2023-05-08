using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class QuestPaginationMessageManager : PaginationMessageBuilder
    {
        private readonly List<Quest> _quests;

        public QuestPaginationMessageManager(List<Quest> quests) :
            base(DofusEmbedCategory.Quests, "Livre de quêtes", "Plusieurs quêtes trouvées :")
        {
            _quests = quests;
            PopulateContent();
        }

        protected override void PopulateContent()
        {
            foreach (Quest quest in _quests)
                _content.Add($"- {Formatter.Bold(quest.Name)} ({quest.Id}) {Emojis.Quest(quest.Repeatable, quest.Account)}{(quest.HasDungeon ? Emojis.DUNGEON : "")}");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();
            foreach (Quest quest in _quests.GetRange(GetStartPageIndex(), GetEndPageIndex()))
                options.Add(new(quest.Name.WithMaxLength(100), quest.Id.ToString(), quest.Id.ToString()));

            return new("select", "Sélectionne une quête pour l'afficher", options);
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (await base.InteractionTreatment(e))
                return true;

            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());

                Quest? quest = Bot.Instance.Api.Datacenter.QuestsData.GetQuestById(id);
                if (quest is not null)
                {
                    await new QuestMessageBuilder(quest).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
