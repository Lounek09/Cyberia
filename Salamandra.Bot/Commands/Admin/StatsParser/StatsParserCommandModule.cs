using DSharpPlus.SlashCommands;

namespace Salamandra.Bot.Commands.Admin
{
    [SlashCommandGroup("statsparser", "Parser de stats")]
    public sealed class StatsParserCommandModule : ApplicationCommandModule
    {
        [SlashCommand("item", "Parser de stats d'item")]
        public async Task ItemParserCommand(InteractionContext ctx,
            [Option("stats", "Stats de l'item")]
                string value)
        {
            //TODO
            /*List<AbstractEffect> effects = DiscordBot.Instance.Api.Parser.EffectParser.GetEffectsParseFromItem(value);

            if (effects.Count > 0)
            {
                DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Inventory, "Inventaire")
                                            .WithTitle("Stats Parser")
                                            .AddEffectFields("Effets :", effects);

                await ctx.CreateResponseAsync(embed);
            }
            else
                await ctx.CreateResponseAsync("Valeur incorrect !");*/
        }


        [SlashCommand("spell_level", "Parser de spell level")]
        public async Task SpellParserCommand(InteractionContext ctx,
            [Option("stats", "Stats du spell level")]
                string value)
        {
            //TODO
            /*List<object>? spellLevel;
            try
            {
                spellLevel = JsonSerializer.Deserialize<List<object>>(value);
            }
            catch
            {
                spellLevel = null;
            }

            if (spellLevel is not null)
            {
                Spell spell = new(-1, "SpellLevel Parser", "-", spellLevel, null, null, null, null, null);

                await new SpellMessageManager(spell, ctx.User).SendInteractionResponse(ctx.Interaction);
            }
            else
                await ctx.CreateResponseAsync("Valeur incorrect !");*/
        }
    }
}
