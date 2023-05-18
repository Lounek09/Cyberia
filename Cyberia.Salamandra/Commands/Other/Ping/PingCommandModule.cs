using DSharpPlus.SlashCommands;
namespace Cyberia.Salamandra.Commands.Other
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class PingCommandModule : ApplicationCommandModule
    {
        [SlashCommand("ping", "Retourne Pong")]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync("Pong... " + Bot.Instance.Client.Ping + "ms !", true);
        }
    }
}
