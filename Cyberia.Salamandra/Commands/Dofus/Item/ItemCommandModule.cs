using Cyberia.Api.DatacenterNS;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class ItemCommandModule : ApplicationCommandModule
    {
        [SlashCommand("item", "Retourne les informations d'un item à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom de l'item")]
            [Autocomplete(typeof(ItemAutocompleteProvider))]
            string sId)
        {
            Item? item = null;

            if (int.TryParse(sId, out int id))
                item = Bot.Instance.Api.Datacenter.ItemsData.GetItemById(id);

            if (item is null)
                await ctx.CreateResponseAsync("Item introuvable");
            else
                await new ItemMessageBuilder(item).SendInteractionResponse(ctx.Interaction);
        }
    }
}
