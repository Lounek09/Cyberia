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

namespace Cyberia.Salamandra.Commands.Dofus.Spell;

public sealed class SpellCommandModule
{
    private readonly CultureService _cultureService;
    private readonly EmbedBuilderService _embedBuilderService;

    public SpellCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command(SpellInteractionLocalizer.CommandName), Description(SpellInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<SpellInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequirePermissions(DiscordPermissions.UseApplicationCommands)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(SpellInteractionLocalizer.Value_ParameterName), Description(SpellInteractionLocalizer.Value_ParameterDescription)]
        [InteractionLocalizer<SpellInteractionLocalizer>]
        [SlashAutoCompleteProvider<SpellAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var spellData = DofusApi.Datacenter.SpellsRepository.GetSpellDataById(id);
            if (spellData is not null)
            {
                response = await new SpellMessageBuilder(_embedBuilderService, spellData, spellData.GetMaxLevelNumber(), culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var spellsData = DofusApi.Datacenter.SpellsRepository.GetSpellsDataByName(value, culture).ToList();
            if (spellsData.Count == 1)
            {
                response = await new SpellMessageBuilder(_embedBuilderService, spellsData[0], spellsData[0].GetMaxLevelNumber(), culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (spellsData.Count > 1)
            {
                response = await new PaginatedSpellMessageBuilder(_embedBuilderService, spellsData, value, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(Translation.Get<BotTranslations>("Spell.NotFound", culture));
        await ctx.RespondAsync(response);
    }
}
