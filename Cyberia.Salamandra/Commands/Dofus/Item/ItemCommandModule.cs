using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.ContextChecks;
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
    private readonly EmbedBuilderService _embedBuilderService;

    public ItemCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command(ItemInteractionLocalizer.CommandName), Description(ItemInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<ItemInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequirePermissions(DiscordPermissions.UseApplicationCommands)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(ItemInteractionLocalizer.Value_ParameterName), Description(ItemInteractionLocalizer.Value_ParameterDescription)]
        [InteractionLocalizer<ItemInteractionLocalizer>]
        [SlashAutoCompleteProvider<ItemAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var itemData = DofusApi.Datacenter.ItemsRepository.GetItemDataById(id);
            if (itemData is not null)
            {
                response = await new ItemMessageBuilder(_embedBuilderService, itemData, 1, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var itemsData = DofusApi.Datacenter.ItemsRepository.GetItemsDataByName(value, culture).ToList();
            if (itemsData.Count == 1)
            {
                response = await new ItemMessageBuilder(_embedBuilderService, itemsData[0], 1, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (itemsData.Count > 1)
            {
                response = await new PaginatedItemMessageBuilder(_embedBuilderService, itemsData, value, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(Translation.Get<BotTranslations>("Item.NotFound", culture));
        await ctx.RespondAsync(response);
    }
}
