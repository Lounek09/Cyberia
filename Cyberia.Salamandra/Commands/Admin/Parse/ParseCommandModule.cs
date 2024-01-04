using Cyberia.Api.Factories;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Admin;

public sealed class ParseCommandModule : ApplicationCommandModule
{
    [SlashCommand("parse", "Parse an item's stats")]
    public async Task ItemParserCommand(InteractionContext ctx,
        [Option("stats", "Item's stats")]
        string value)
    {
        var effects = EffectFactory.CreateMany(value);

        if (effects.Any())
        {
            var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Inventory, "Inventaire")
                .WithTitle("Item stats parser")
                .AddEffectFields("Effets :", effects);

            await ctx.CreateResponseAsync(embed);
            return;
        }

        await ctx.CreateResponseAsync("Incorrect value");
    }
}
