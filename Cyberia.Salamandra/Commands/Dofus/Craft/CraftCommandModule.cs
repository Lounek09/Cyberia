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

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public sealed class CraftCommandModule
{
    private readonly CultureService _cultureService;
    private readonly EmbedBuilderService _embedBuilderService;

    public CraftCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command(CraftInteractionLocalizer.CommandName), Description(CraftInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<CraftInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequirePermissions(DiscordPermissions.UseApplicationCommands)]
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
            var craftData = DofusApi.Datacenter.CraftsRepository.GetCraftDataById(id);
            if (craftData is not null)
            {
                response = await new CraftMessageBuilder(_embedBuilderService, craftData, quantity, false, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var craftsData = DofusApi.Datacenter.CraftsRepository.GetCraftsDataByItemName(value, culture).ToList();
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
