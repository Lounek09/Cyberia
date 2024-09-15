using Cyberia.Api;
using Cyberia.Salamandra.EventHandlers;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Item;

public sealed class ItemCommandModule
{
    private readonly CultureService _cultureService;

    public ItemCommandModule(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    [Command(ItemInteractionLocalizer.CommandName), Description(ItemInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<ItemInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(ItemInteractionLocalizer.Value_ParameterName), Description(ItemInteractionLocalizer.Value_ParameterDescription)]
        [InteractionLocalizer<ItemInteractionLocalizer>]
        [SlashAutoCompleteProvider<ItemAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var itemData = DofusApi.Datacenter.ItemsRepository.GetItemDataById(id);
            if (itemData is not null)
            {
                response = await new ItemMessageBuilder(itemData).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var itemsData = DofusApi.Datacenter.ItemsRepository.GetItemsDataByName(value).ToList();
            if (itemsData.Count == 1)
            {
                response = await new ItemMessageBuilder(itemsData[0]).BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (itemsData.Count > 1)
            {
                response = await new PaginatedItemMessageBuilder(itemsData, value).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(BotTranslations.Item_NotFound);
        await ctx.RespondAsync(response);
    }
}
