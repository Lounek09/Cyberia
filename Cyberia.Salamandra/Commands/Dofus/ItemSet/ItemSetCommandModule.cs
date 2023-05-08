using Cyberia.Api.DatacenterNS;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class ItemSetCommandModule : ApplicationCommandModule
    {
        [SlashCommand("panoplie", "Retourne les informations d'une panoplie à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("Nom", "Nom de la panoplie")]
            [Autocomplete(typeof(ItemSetAutocompleteProvider))]
            string sId)
        {
            ItemSet? itemSet = null;

            if (int.TryParse(sId, out int id))
                itemSet = Bot.Instance.Api.Datacenter.ItemSetsData.GetItemSetById(id);

            if (itemSet is null)
                await ctx.CreateResponseAsync("Panoplie introuvable");
            else
                await new ItemSetMessageBuilder(itemSet).SendInteractionResponse(ctx.Interaction);
        }
    }
}
