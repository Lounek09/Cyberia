using Cyberia.Api.Factories;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.EventHandlers;
using Cyberia.Salamandra.Extensions.DSharpPlus;
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
        await _cultureService.SetCultureAsync(ctx.Interaction);

        var effects = EffectFactory.CreateMany(value);

        if (effects.Count > 0)
        {
            var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, "Tools")
                .WithTitle("Item effects parser")
                .AddEffectFields("Effects :", effects, true);

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
        await _cultureService.SetCultureAsync(ctx.Interaction);

        var criteria = CriterionFactory.CreateMany(value);

        if (criteria.Count > 0)
        {
            var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, "Tools")
                .WithTitle("Criteria parser")
                .AddCriteriaFields(criteria);

            await ctx.RespondAsync(embed);
            return;
        }

        await ctx.RespondAsync("Incorrect value");
    }
}
