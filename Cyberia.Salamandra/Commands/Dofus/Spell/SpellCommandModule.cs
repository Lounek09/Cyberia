using Cyberia.Api;
using Cyberia.Salamandra.EventHandlers;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Spell;

public sealed class SpellCommandModule
{
    private readonly CultureService _cultureService;

    public SpellCommandModule(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    [Command(SpellInteractionLocalizer.CommandName), Description(SpellInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<SpellInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(SpellInteractionLocalizer.Value_ParameterName), Description(SpellInteractionLocalizer.Value_ParameterDescription)]
        [InteractionLocalizer<SpellInteractionLocalizer>]
        [SlashAutoCompleteProvider<SpellAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var spellData = DofusApi.Datacenter.SpellsRepository.GetSpellDataById(id);
            if (spellData is not null)
            {
                response = await new SpellMessageBuilder(spellData, spellData.GetMaxLevelNumber()).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var spellsData = DofusApi.Datacenter.SpellsRepository.GetSpellsDataByName(value).ToList();
            if (spellsData.Count == 1)
            {
                response = await new SpellMessageBuilder(spellsData[0], spellsData[0].GetMaxLevelNumber()).BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (spellsData.Count > 1)
            {
                response = await new PaginatedSpellMessageBuilder(spellsData, value).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(BotTranslations.Spell_NotFound);
        await ctx.RespondAsync(response);
    }
}
