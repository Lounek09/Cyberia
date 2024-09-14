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

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public sealed class MonsterCommandModule
{
    private readonly CultureService _cultureService;

    public MonsterCommandModule(CultureService cultureService)
    {
        _cultureService = cultureService;
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
        await _cultureService.SetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var monsterData = DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(id);
            if (monsterData is not null)
            {
                response = await new MonsterMessageBuilder(monsterData).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var monstersData = DofusApi.Datacenter.MonstersRepository.GetMonstersDataByName(value).ToList();
            if (monstersData.Count == 1)
            {
                response = await new MonsterMessageBuilder(monstersData[0]).BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (monstersData.Count > 1)
            {
                response = await new PaginatedMonsterMessageBuilder(monstersData, value).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(BotTranslations.Monster_NotFound);
        await ctx.RespondAsync(response);
    }
}
