using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class KillCommandModule : ApplicationCommandModule
    {
        [SlashCommand("kill", "[Owner] Coupe Salamandra")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync("💀", true);

            await Bot.Instance.Client.DisconnectAsync();
        }
    }
}
