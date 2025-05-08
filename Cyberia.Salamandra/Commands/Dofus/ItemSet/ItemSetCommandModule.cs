using Cyberia.Api.Data;
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
    private readonly ICultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;
    private readonly IEmbedBuilderService _embedBuilderService;

    public ItemSetCommandModule(ICultureService cultureService, DofusDatacenter dofusDatacenter, IEmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
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
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var itemSetData = _dofusDatacenter.ItemSetsRepository.GetItemSetDataById(id);
            if (itemSetData is not null)
            {
                response = await new ItemSetMessageBuilder(_embedBuilderService, itemSetData, itemSetData.Effects.Count, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var itemSetsData = _dofusDatacenter.ItemSetsRepository.GetItemSetsDataByName(value, culture).ToList();
            if (itemSetsData.Count == 1)
            {
                response = await new ItemSetMessageBuilder(_embedBuilderService, itemSetsData[0], itemSetsData[0].Effects.Count, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (itemSetsData.Count > 1)
            {
                response = await new PaginatedItemSetMessageBuilder(_embedBuilderService, itemSetsData, value, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(Translation.Get<BotTranslations>("ItemSet.NotFound", culture));
        await ctx.RespondAsync(response);
    }
}
