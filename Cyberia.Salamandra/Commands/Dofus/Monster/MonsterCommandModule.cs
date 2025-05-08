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

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public sealed class MonsterCommandModule
{
    private readonly CultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;
    private readonly EmbedBuilderService _embedBuilderService;

    public MonsterCommandModule(CultureService cultureService, DofusDatacenter dofusDatacenter, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
        _embedBuilderService = embedBuilderService;
    }

    [Command(MonsterInteractionLocalizer.CommandName), Description(MonsterInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<MonsterInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(MonsterInteractionLocalizer.Value_ParameterName), Description(MonsterInteractionLocalizer.Value_ParameterDescription)]
        [InteractionLocalizer<MonsterInteractionLocalizer>]
        [SlashAutoCompleteProvider<MonsterAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var monsterData = _dofusDatacenter.MonstersRepository.GetMonsterDataById(id);
            if (monsterData is not null)
            {
                response = await new MonsterMessageBuilder(_embedBuilderService, monsterData, 1, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var monstersData = _dofusDatacenter.MonstersRepository.GetMonstersDataByName(value, culture).ToList();
            if (monstersData.Count == 1)
            {
                response = await new MonsterMessageBuilder(_embedBuilderService, monstersData[0], 1, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (monstersData.Count > 1)
            {
                response = await new PaginatedMonsterMessageBuilder(_embedBuilderService, monstersData, value, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(Translation.Get<BotTranslations>("Monster.NotFound", culture));
        await ctx.RespondAsync(response);
    }
}
