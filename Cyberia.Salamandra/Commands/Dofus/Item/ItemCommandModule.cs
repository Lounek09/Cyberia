using Cyberia.Api.Data;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
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
                ItemData? itemData = Bot.Instance.Api.Datacenter.ItemsData.GetItemDataById(id);
                if (itemData is not null)
                {
                    response = await new ItemMessageBuilder(itemData).GetMessageAsync<DiscordInteractionResponseBuilder>();
                }
            }
            else
            {
                List<ItemData> itemsData = Bot.Instance.Api.Datacenter.ItemsData.GetItemsDataByName(value);
                if (itemsData.Count == 1)
                {
                    response = await new ItemMessageBuilder(itemsData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
                }
                else if (itemsData.Count > 1)
                {
                    response = await new PaginatedItemMessageBuilder(itemsData, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
                }
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Item introuvable");
            await ctx.CreateResponseAsync(response);
        }
    }
}
