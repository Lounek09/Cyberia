using Cyberia.Api;
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

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public sealed class IncarnationCommandModule
{
    private readonly ICultureService _cultureService;
    private readonly DofusApiConfig _dofusApiConfig;
    private readonly DofusDatacenter _dofusDatacenter;
    private readonly IEmbedBuilderService _embedBuilderService;

    public IncarnationCommandModule(ICultureService cultureService, DofusApiConfig dofusApiConfig, DofusDatacenter dofusDatacenter, IEmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _dofusApiConfig = dofusApiConfig;
        _dofusDatacenter = dofusDatacenter;
        _embedBuilderService = embedBuilderService;
    }

    [Command(IncarnationInteractionLocalizer.CommandName), Description(IncarnationInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<IncarnationInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(IncarnationInteractionLocalizer.Value_ParameterName), Description(IncarnationInteractionLocalizer.Value_ParameterDescription)]
        [InteractionLocalizer<IncarnationInteractionLocalizer>]
        [SlashAutoCompleteProvider<IncarnationAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var incarnationData = _dofusDatacenter.IncarnationsRepository.GetIncarnationDataByItemId(id);
            if (incarnationData is not null)
            {
                response = await new IncarnationMessageBuilder(_dofusApiConfig, _embedBuilderService, incarnationData, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var incarnationsData = _dofusDatacenter.IncarnationsRepository.GetIncarnationsDataByItemName(value, culture).ToList();
            if (incarnationsData.Count == 1)
            {
                response = await new IncarnationMessageBuilder(_dofusApiConfig, _embedBuilderService, incarnationsData[0], culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (incarnationsData.Count > 1)
            {
                response = await new PaginatedIncarnationMessageBuilder(_embedBuilderService, incarnationsData, value, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(Translation.Get<BotTranslations>("Incarnation.NotFound", culture));
        await ctx.RespondAsync(response);
    }
}
