using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class QuestCommandModule : ApplicationCommandModule
    {
        [SlashCommand("quete", "Retourne les informations d'une quête à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom de la quête", true)]
            [Autocomplete(typeof(QuestAutocompleteProvider))]
            string value)
        {
            DiscordInteractionResponseBuilder? response = null;

            if (int.TryParse(value, out int id))
            {
                Quest? quest = Bot.Instance.Api.Datacenter.QuestsData.GetQuestById(id);
                if (quest is not null)
                    response = await new QuestMessageBuilder(quest).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else
            {
                List<Quest> quests = Bot.Instance.Api.Datacenter.QuestsData.GetQuestsByName(value);
                if (quests.Count == 1)
                    response = await new QuestMessageBuilder(quests[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
                else if (quests.Count > 1)
                    response = await new PaginatedQuestMessageBuilder(quests, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Quête introuvable");
            await ctx.CreateResponseAsync(response);
        }
    }
}
