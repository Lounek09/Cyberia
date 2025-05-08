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

namespace Cyberia.Salamandra.Commands.Dofus.Quest;

public sealed class QuestCommandModule
{
    private readonly CultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;
    private readonly EmbedBuilderService _embedBuilderService;

    public QuestCommandModule(CultureService cultureService, DofusDatacenter dofusDatacenter, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
        _embedBuilderService = embedBuilderService;
    }

    [Command(QuestInteractionLocalizer.CommandName), Description(QuestInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<QuestInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(QuestInteractionLocalizer.Value_ParameterName), Description(QuestInteractionLocalizer.Value_ParameterDescription)]
        [InteractionLocalizer<QuestInteractionLocalizer>]
        [SlashAutoCompleteProvider<QuestAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var questData = _dofusDatacenter.QuestsRepository.GetQuestDataById(id);
            if (questData is not null)
            {
                response = await new QuestMessageBuilder(_embedBuilderService, questData, 0, null, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var questsData = _dofusDatacenter.QuestsRepository.GetQuestsDataByName(value, culture).ToList();
            if (questsData.Count == 1)
            {
                response = await new QuestMessageBuilder(_embedBuilderService, questsData[0], 0, null, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (questsData.Count > 1)
            {
                response = await new PaginatedQuestMessageBuilder(_embedBuilderService, questsData, value, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(Translation.Get<BotTranslations>("Quest.NotFound", culture));
        await ctx.RespondAsync(response);
    }
}
