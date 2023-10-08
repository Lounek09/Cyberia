using Cyberia.Api.DatacenterNS;

using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin
{
    public sealed class TestCommandModule : ApplicationCommandModule
    {
        [SlashCommand("test", "[Owner] Commande pour tester des trucs")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(".");

            foreach (SpellData spell in Bot.Instance.Api.Datacenter.SpellsData.Spells)
            {
                if (spell.SpellLevelData1?.Effects.Find(x => x.EffectId == 111) is not null)
                    await ctx.Channel.SendMessageAsync(spell.Name);
            }
        }
    }
}
