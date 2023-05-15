using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Effects;

using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin
{
    public sealed class TestCommandModule : ApplicationCommandModule
    {
        [SlashCommand("test", "[RequireOwner] Commande pour tester des trucs")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(".");

            foreach (Spell spell in Bot.Instance.Api.Datacenter.SpellsData.Spells)
            {
                if (spell.SpellLevel1 is not null)
                {
                    foreach (IEffect effects in spell.SpellLevel1.Effects)
                    {
                        if (!string.IsNullOrEmpty(effects.Criteria))
                        {
                            await ctx.Channel.SendMessageAsync(spell.Name);
                            break;
                        }
                    }
                }

                if (spell.SpellLevel2 is not null)
                {
                    foreach (IEffect effects in spell.SpellLevel2.Effects)
                    {
                        if (!string.IsNullOrEmpty(effects.Criteria))
                        {
                            await ctx.Channel.SendMessageAsync(spell.Name);
                            break;
                        }
                    }
                }

                if (spell.SpellLevel3 is not null)
                {
                    foreach (IEffect effects in spell.SpellLevel3.Effects)
                    {
                        if (!string.IsNullOrEmpty(effects.Criteria))
                        {
                            await ctx.Channel.SendMessageAsync(spell.Name);
                            break;
                        }
                    }
                }

                if (spell.SpellLevel4 is not null)
                {
                    foreach (IEffect effects in spell.SpellLevel4.Effects)
                    {
                        if (!string.IsNullOrEmpty(effects.Criteria))
                        {
                            await ctx.Channel.SendMessageAsync(spell.Name);
                            break;
                        }
                    }
                }

                if (spell.SpellLevel5 is not null)
                {
                    foreach (IEffect effects in spell.SpellLevel5.Effects)
                    {
                        if (!string.IsNullOrEmpty(effects.Criteria))
                        {
                            await ctx.Channel.SendMessageAsync(spell.Name);
                            break;
                        }
                    }
                }

                if (spell.SpellLevel6 is not null)
                {
                    foreach (IEffect effects in spell.SpellLevel6.Effects)
                    {
                        if (!string.IsNullOrEmpty(effects.Criteria))
                        {
                            await ctx.Channel.SendMessageAsync(spell.Name);
                            break;
                        }
                    }
                }
            }
        }
    }
}
