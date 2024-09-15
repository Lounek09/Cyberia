using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public sealed class ItemSetCommandModule
{
    private readonly CultureService _cultureService;
    private readonly EmbedBuilderService _embedBuilderService;

    public ItemSetCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command(ItemSetInteractionLocalizer.CommandName), Description(ItemSetInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<ItemSetInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(ItemSetInteractionLocalizer.Value_ParameterName), Description(ItemSetInteractionLocalizer.Value_ParameterDescription)]
        [InteractionLocalizer<ItemSetInteractionLocalizer>]
        [SlashAutoCompleteProvider<ItemSetAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var itemSetData = DofusApi.Datacenter.ItemSetsRepository.GetItemSetDataById(id);
            if (itemSetData is not null)
            {
                response = await new ItemSetMessageBuilder(_embedBuilderService, itemSetData, itemSetData.Effects.Count).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var itemSetsData = DofusApi.Datacenter.ItemSetsRepository.GetItemSetsDataByName(value).ToList();
            if (itemSetsData.Count == 1)
            {
                response = await new ItemSetMessageBuilder(_embedBuilderService, itemSetsData[0], itemSetsData[0].Effects.Count).BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (itemSetsData.Count > 1)
            {
                response = await new PaginatedItemSetMessageBuilder(_embedBuilderService, itemSetsData, value).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(BotTranslations.ItemSet_NotFound);
        await ctx.RespondAsync(response);
    }
}
