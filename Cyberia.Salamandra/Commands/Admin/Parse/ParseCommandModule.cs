using Cyberia.Api.Factories;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Admin;

[SlashCommandGroup("parse", "PArse")]
public sealed class ParseCommandModule : ApplicationCommandModule
{
    [SlashCommand("effects", "Parse the effects of an item")]
    public async Task ItemParserCommand(InteractionContext ctx,
        [Option("value", "Effects of an item")]
        string value)
    {
        var effects = EffectFactory.CreateMany(value);

        if (effects.Any())
        {
            var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Tools")
                .WithTitle("Item effects parser")
                .AddEffectFields("Effets :", effects);

            await ctx.CreateResponseAsync(embed);
            return;
        }

        await ctx.CreateResponseAsync("Incorrect value");
    }

    [SlashCommand("criteria", "Parse the critera")]
    public async Task CriteriaParserCommand(InteractionContext ctx,
        [Option("value", "Criteria")]
        string value)
    {
        var criteria = CriterionFactory.CreateMany(value);

        if (criteria.Count > 0)
        {
            var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Tools")
                .WithTitle("Criteria parser")
                .AddCriteriaFields("Criteria :", criteria);

            await ctx.CreateResponseAsync(embed);
            return;
        }

        await ctx.CreateResponseAsync("Incorrect value");
    }
}
