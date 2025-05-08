using Cyberia.Api.Factories;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Admin.Parse;

[Command("parse")]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
public sealed class ParseCommandModule
{
    private readonly CultureService _cultureService;
    private readonly EmbedBuilderService _embedBuilderService;

    public ParseCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command("effects"), Description("Parse the effects of an item")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task EffectsExecuteAsync(SlashCommandContext ctx,
        [Parameter("value"), Description("Effects of an item")]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var effects = EffectFactory.CreateMany(value);
        if (effects.Count > 0)
        {
            var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, "Tools", culture)
                .WithTitle("Item effects parser");

            _embedBuilderService.AddEffectFields(embed, "Effects :", effects, true, culture);

            await ctx.RespondAsync(embed);
            return;
        }

        await ctx.RespondAsync("Incorrect value");
    }

    [Command("criteria"), Description("Parse the critera")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task CriteriaExecuteAsync(SlashCommandContext ctx,
        [Parameter("value"), Description("Criteria")]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var criteria = CriterionFactory.CreateMany(value);
        if (criteria.Count > 0)
        {
            var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, "Tools", culture)
                .WithTitle("Criteria parser");

            _embedBuilderService.AddCriteriaFields(embed, criteria, culture);

            await ctx.RespondAsync(embed);
            return;
        }

        await ctx.RespondAsync("Incorrect value");
    }
}
