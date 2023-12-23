using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class ItemSetCommandModule : ApplicationCommandModule
{
    [SlashCommand("panoplie", "Retourne les informations d'une panoplie à partir de son nom")]
    public async Task Command(InteractionContext ctx,
        [Option("Nom", "Nom de la panoplie", true)]
        [Autocomplete(typeof(ItemSetAutocompleteProvider))]
        string value)
    {
        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var itemSetData = DofusApi.Datacenter.ItemSetsData.GetItemSetDataById(id);
            if (itemSetData is not null)
            {
                response = await new ItemSetMessageBuilder(itemSetData, itemSetData.Effects.Count).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var itemSetsData = DofusApi.Datacenter.ItemSetsData.GetItemSetsDataByName(value).ToList();
            if (itemSetsData.Count == 1)
            {
                response = await new ItemSetMessageBuilder(itemSetsData[0], itemSetsData[0].Effects.Count).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else if (itemSetsData.Count > 1)
            {
                response = await new PaginatedItemSetMessageBuilder(itemSetsData, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent("Panoplie introuvable");
        await ctx.CreateResponseAsync(response);
    }
}
