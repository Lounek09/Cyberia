using Cyberia.Api.DatacenterNS;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class QuestCommandModule : ApplicationCommandModule
    {
        [SlashCommand("quete", "Retourne les informations d'une quête à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom de la quête")]
            [Autocomplete(typeof(QuestAutocompleteProvider))]
            string sId)
        {
            Quest? quest = null;

            if (int.TryParse(sId, out int id))
                quest = Bot.Instance.Api.Datacenter.QuestsData.GetQuestById(id);

            if (quest is null)
                await ctx.CreateResponseAsync("Quête introuvable");
            else
                await new QuestMessageBuilder(quest).SendInteractionResponse(ctx.Interaction);
        }
    }
}
