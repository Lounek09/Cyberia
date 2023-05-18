using Cyberia.Api.DatacenterNS;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class CraftCommandModule : ApplicationCommandModule
    {
        [SlashCommand("craft", "Permet de calculer les ressources nécéssaire pour craft un objet")]
        public async Task Command(InteractionContext ctx,
            [Option("quantite", "Quantité à craft")]
            [Minimum(1), Maximum(99999)]
            long qte,
            [Option("nom", "Nom de l'item à craft")]
            [Autocomplete(typeof(CraftAutocompleteProvider))]
            string sId)
        {
            Craft? craft = null;

            if (int.TryParse(sId, out int id))
                craft = Bot.Instance.Api.Datacenter.CraftsData.GetCraftById(id);

            if (craft is null)
                await ctx.CreateResponseAsync("Aucun craft trouvé");
            else
                await new CraftMessageBuilder(craft, (int)qte).SendInteractionResponse(ctx.Interaction);

        }
    }
}
