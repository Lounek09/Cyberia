using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class ItemCommandModule : ApplicationCommandModule
    {
        [SlashCommand("item", "Retourne les informations d'un item à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom de l'item", true)]
            [Autocomplete(typeof(ItemAutocompleteProvider))]
            string value)
        {
            DiscordInteractionResponseBuilder? response = null;

            if (int.TryParse(value, out int id))
            {
                Item? item = Bot.Instance.Api.Datacenter.ItemsData.GetItemById(id);
                if (item is not null)
                    response = await new ItemMessageBuilder(item).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else
            {
                List<Item> items = Bot.Instance.Api.Datacenter.ItemsData.GetItemsByName(value);
                if (items.Count == 1)
                    response = await new ItemMessageBuilder(items[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
                else if (items.Count > 1)
                    response = await new PaginatedItemMessageBuilder(items, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Item introuvable");
            await ctx.CreateResponseAsync(response);
        }
    }
}
