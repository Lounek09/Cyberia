using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin
{
#pragma warning disable CA1822 // Mark members as static
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
                    if (spell.SpellLevel1.SpellLevelCategoryId > 4)
                        await ctx.Channel.SendMessageAsync(spell.Name + ":" + spell.SpellLevel1.SpellLevelCategoryId);
                }
            }

            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("done"));
        }
    }
}
