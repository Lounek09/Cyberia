using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class CraftCommandModule : ApplicationCommandModule
{
    [SlashCommand("craft", "Permet de calculer les ressources nécessaires pour craft un objet")]
    public async Task Command(InteractionContext ctx,
        [Option("quantite", "Quantité à craft")]
        [Minimum(1), Maximum(CraftMessageBuilder.MAX_QTE)]
        long qte,
        [Option("nom", "Nom de l'item à craft", true)]
        [Autocomplete(typeof(CraftAutocompleteProvider))]
        string value)
    {
        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var craftData = DofusApi.Datacenter.CraftsData.GetCraftDataById(id);
            if (craftData is not null)
            {
                response = await new CraftMessageBuilder(craftData, (int)qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var craftsData = DofusApi.Datacenter.CraftsData.GetCraftsDataByItemName(value).ToList();
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
