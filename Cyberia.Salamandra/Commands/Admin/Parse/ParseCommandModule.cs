using Cyberia.Api.Factories;
using Cyberia.Salamandra.DSharpPlus;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

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
    [Command("effects"), Description("Parse the effects of an item")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task EffectsExecuteAsync(SlashCommandContext ctx,
        [Parameter("value"), Description("Effects of an item")]
        string value)
    {
        var effects = EffectFactory.CreateMany(value);

        if (effects.Count > 0)
        {
            var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Tools")
                .WithTitle("Item effects parser")
                .AddEffectFields("Effects :", effects);

            await ctx.RespondAsync(embed);
            return;
        }

        await ctx.RespondAsync("Incorrect value");
    }

    [Command("criteria"), Description("Parse the critera")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task CriteriaExecuteAsync(SlashCommandContext ctx,
        [Parameter("value"), Description("Criteria")]
        string value)
    {
        var criteria = CriterionFactory.CreateMany(value);

        if (criteria.Count > 0)
        {
            var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Tools")
                .WithTitle("Criteria parser")
                .AddCriteriaFields(criteria);

            await ctx.RespondAsync(embed);
            return;
        }

        await ctx.RespondAsync("Incorrect value");
    }
}
