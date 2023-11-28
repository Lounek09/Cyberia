using Cyberia.Api.Factories;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Admin;

public sealed class ParseCommandModule : ApplicationCommandModule
{
    [SlashCommand("parse", "Parse les stats d'un item")]
    public async Task ItemParserCommand(InteractionContext ctx,
        [Option("stats", "Stats de l'item")]
            string value)
    {
        var effects = EffectFactory.GetEffectsParseFromItem(value);

        if (effects.Any())
        {
            var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Inventory, "Inventaire")
                .WithTitle("Item stats parser")
                .AddEffectFields("Effets :", effects);

            await ctx.CreateResponseAsync(embed);
            return;
        }

        await ctx.CreateResponseAsync("Valeur incorrect !");
    }
}
