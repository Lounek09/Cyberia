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

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public sealed class IncarnationCommandModule
{
    private readonly CultureService _cultureService;
    private readonly EmbedBuilderService _embedBuilderService;

    public IncarnationCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
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
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var incarnationData = DofusApi.Datacenter.IncarnationsRepository.GetIncarnationDataByItemId(id);
            if (incarnationData is not null)
            {
                response = await new IncarnationMessageBuilder(_embedBuilderService, incarnationData).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var incarnationsData = DofusApi.Datacenter.IncarnationsRepository.GetIncarnationsDataByItemName(value).ToList();
            if (incarnationsData.Count == 1)
            {
                response = await new IncarnationMessageBuilder(_embedBuilderService, incarnationsData[0]).BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (incarnationsData.Count > 1)
            {
                response = await new PaginatedIncarnationMessageBuilder(_embedBuilderService, incarnationsData, value).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(BotTranslations.Incarnation_NotFound);
        await ctx.RespondAsync(response);
    }
}
