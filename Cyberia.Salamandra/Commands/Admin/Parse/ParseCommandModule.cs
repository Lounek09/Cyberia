using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Admin
{
    public sealed class ParseCommandModule : ApplicationCommandModule
    {
        [SlashCommand("parse", "Parse les stats d'un item")]
        public async Task ItemParserCommand(InteractionContext ctx,
            [Option("stats", "Stats de l'item")]
                string value)
        {
            List<IEffect> effects = EffectFactory.GetEffectsParseFromItem(value).ToList();

            if (effects.Count > 0)
            {
                DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Inventory, "Inventaire")
                    .WithTitle("Item stats parser")
                    .AddEffectFields("Effets :", effects);

                await ctx.CreateResponseAsync(embed);
            }
            else
                await ctx.CreateResponseAsync("Valeur incorrect !");
        }
    }
}
