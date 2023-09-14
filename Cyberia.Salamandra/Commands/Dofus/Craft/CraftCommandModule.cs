using Cyberia.Api.DatacenterNS;

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
                Craft? craft = Bot.Instance.Api.Datacenter.CraftsData.GetCraftById(id);
                if (craft is not null)
                    response = await new CraftMessageBuilder(craft, (int)qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else
            {
                List<Craft> crafts = Bot.Instance.Api.Datacenter.CraftsData.GetCraftsByItemName(value);
                if (crafts.Count == 1)
                    response = await new CraftMessageBuilder(crafts[0], (int)qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
                else if (crafts.Count > 1)
                    response = await new PaginatedCraftMessageBuilder(crafts, value, (int)qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Craft introuvable");
            await ctx.CreateResponseAsync(response);
        }
    }
}
