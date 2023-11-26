using Cyberia.Api;
using Cyberia.Api.Data;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class CraftCommandModule : ApplicationCommandModule
    {
        [SlashCommand("craft", "Permet de calculer les ressources nécéssaire pour craft un objet")]
        public async Task Command(InteractionContext ctx,
            [Option("quantite", "Quantité à craft")]
            [Minimum(1), Maximum(99999)]
            long qte,
            [Option("nom", "Nom de l'item à craft", true)]
            [Autocomplete(typeof(CraftAutocompleteProvider))]
            string value)
        {
            DiscordInteractionResponseBuilder? response = null;

            if (int.TryParse(value, out int id))
            {
                CraftData? craftData = DofusApi.Datacenter.CraftsData.GetCraftDataById(id);
                if (craftData is not null)
                {
                    response = await new CraftMessageBuilder(craftData, (int)qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
                }
            }
            else
            {
                List<CraftData> craftsData = DofusApi.Datacenter.CraftsData.GetCraftsDataByItemName(value).ToList();
                if (craftsData.Count == 1)
                {
                    response = await new CraftMessageBuilder(craftsData[0], (int)qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
                }
                else if (craftsData.Count > 1)
                {
                    response = await new PaginatedCraftMessageBuilder(craftsData, value, (int)qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
                }
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Craft introuvable");
            await ctx.CreateResponseAsync(response);
        }
    }
}
