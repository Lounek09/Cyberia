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
                QuestData? questData = Bot.Instance.Api.Datacenter.QuestsData.GetQuestDataById(id);
                if (questData is not null)
                    response = await new QuestMessageBuilder(questData).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else
            {
                List<QuestData> questsData = Bot.Instance.Api.Datacenter.QuestsData.GetQuestsDataByName(value);
                if (questsData.Count == 1)
                    response = await new QuestMessageBuilder(questsData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
                else if (questsData.Count > 1)
                    response = await new PaginatedQuestMessageBuilder(questsData, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Quête introuvable");
            await ctx.CreateResponseAsync(response);
        }
    }
}
