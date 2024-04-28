using Cyberia.Api;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public sealed class ItemSetCommandModule
{
    [Command("panoplie"), Description("Retourne les informations d'une panoplie à partir de son nom")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de la panoplie")]
        [SlashAutoCompleteProvider<ItemSetAutocompleteProvider>]
        [SlashMinMaxLength(MinLength = 1, MaxLength = 70)]
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
        await ctx.RespondAsync(response);
    }
}
