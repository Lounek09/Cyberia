using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class ItemSetCommandModule : ApplicationCommandModule
    {
        [SlashCommand("panoplie", "Retourne les informations d'une panoplie à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("Nom", "Nom de la panoplie", true)]
            [Autocomplete(typeof(ItemSetAutocompleteProvider))]
            string value)
        {
            DiscordInteractionResponseBuilder? response = null;

            if (int.TryParse(value, out int id))
            {
                ItemSet? itemSet = Bot.Instance.Api.Datacenter.ItemSetsData.GetItemSetById(id);
                if (itemSet is not null)
                    response = await new ItemSetMessageBuilder(itemSet).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else
            {
                List<ItemSet> itemSets = Bot.Instance.Api.Datacenter.ItemSetsData.GetItemSetsByName(value);
                if (itemSets.Count == 1)
                    response = await new ItemSetMessageBuilder(itemSets[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
                else if (itemSets.Count > 1)
                    response = await new PaginatedItemSetMessageBuilder(itemSets, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Panoplie introuvable");
            await ctx.CreateResponseAsync(response);
        }
    }
}
