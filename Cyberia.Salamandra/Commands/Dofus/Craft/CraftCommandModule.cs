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

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public sealed class CraftCommandModule
{
    private readonly ICultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;
    private readonly IEmbedBuilderService _embedBuilderService;

    public CraftCommandModule(ICultureService cultureService, DofusDatacenter dofusDatacenter, IEmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
        _embedBuilderService = embedBuilderService;
    }

    [Command(CraftInteractionLocalizer.CommandName), Description(CraftInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<CraftInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(CraftInteractionLocalizer.Value_ParameterName), Description(CraftInteractionLocalizer.Value_ParameterDescription)]
        [InteractionLocalizer<CraftInteractionLocalizer>]
        [SlashAutoCompleteProvider<CraftAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value,
        [Parameter(CraftInteractionLocalizer.Quantity_ParameterName), Description(CraftInteractionLocalizer.Quantity_ParameterDescription)]
        [InteractionLocalizer<CraftInteractionLocalizer>]
        [MinMaxValue(1, CraftMessageBuilder.MaxQuantity)]
        int quantity = 1)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var craftData = _dofusDatacenter.CraftsRepository.GetCraftDataById(id);
            if (craftData is not null)
            {
                response = await new CraftMessageBuilder(_embedBuilderService, craftData, quantity, false, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var craftsData = _dofusDatacenter.CraftsRepository.GetCraftsDataByItemName(value, culture).ToList();
            if (craftsData.Count == 1)
            {
                response = await new CraftMessageBuilder(_embedBuilderService, craftsData[0], quantity, false, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (craftsData.Count > 1)
            {
                response = await new PaginatedCraftMessageBuilder(_embedBuilderService, craftsData, value, quantity, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(Translation.Get<BotTranslations>("Craft.NotFound", culture));
        await ctx.RespondAsync(response);
    }
}
