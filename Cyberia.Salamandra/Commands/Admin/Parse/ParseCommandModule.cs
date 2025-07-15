using Cyberia.Api;
using Cyberia.Api.Factories;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;
using System.Globalization;

namespace Cyberia.Salamandra.Commands.Admin.Parse;

[Command("parse")]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
public sealed class ParseCommandModule
{
    private readonly ICultureService _cultureService;
    private readonly IEmbedBuilderService _embedBuilderService;

    public ParseCommandModule(ICultureService cultureService, IEmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command("criteria"), Description("Parse the critera")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task CriteriaExecuteAsync(SlashCommandContext ctx,
        [Parameter("value"), Description("The criteria to parse")]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var criteria = CriterionFactory.CreateMany(value);
        if (criteria.Count > 0)
        {
            var embed = _embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Tools, "Tools", culture)
                .WithTitle("Criteria parser");

            _embedBuilderService.AddCriteriaFields(embed, criteria, culture);

            await ctx.RespondAsync(embed);
            return;
        }

        await ctx.RespondAsync("Incorrect value");
    }

    [Command("effects"), Description("Parse the effects of an item")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task EffectsExecuteAsync(SlashCommandContext ctx,
        [Parameter("value"), Description("The effects to parse")]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var effects = EffectFactory.CreateMany(value);
        if (effects.Count > 0)
        {
            var embed = _embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Tools, "Tools", culture)
                .WithTitle("Item effects parser");

            _embedBuilderService.AddEffectFields(embed, "Effects :", effects, true, culture);

            await ctx.RespondAsync(embed);
            return;
        }

        await ctx.RespondAsync("Incorrect value");
    }

    [Command("description"), Description("Parse the description")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task CriteriaExecuteAsync(SlashCommandContext ctx,
        [Parameter("template"), Description("The template of the description")]
        string template,
        [Parameter("parameters"), Description("The parameters to add to the description, put an '_' for an empty value")]
        params string[] parameters)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        DescriptionString description = new(
            template,
            parameters.Select(x => x.Equals("_", StringComparison.Ordinal) ? string.Empty : x).ToArray());

        var embed = _embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Tools, "Tools", CultureInfo.DefaultThreadCurrentUICulture)
            .WithTitle("Description parser")
            .AddField("Template", template)
            .AddField("Parameters", string.Join(", ", parameters))
            .AddField("Result", description.ToString(x => Formatter.Bold(Formatter.Sanitize(x))));

        await ctx.RespondAsync(embed);
    }
}
