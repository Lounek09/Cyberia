using Cyberia.Api;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Item;

public sealed class ItemCommandModule
{
    [Command("item"), Description("Retourne les informations d'un item à partir de son nom")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de l'item")]
        [SlashAutoCompleteProvider<ItemAutocompleteProvider>]
        [SlashMinMaxLength(MinLength = 1, MaxLength = 70)]
        string value)
    {
        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(id);
            if (itemData is not null)
            {
                response = await new ItemMessageBuilder(itemData).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var itemsData = DofusApi.Datacenter.ItemsData.GetItemsDataByName(value).ToList();
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
        await ctx.RespondAsync(response);
    }
}
