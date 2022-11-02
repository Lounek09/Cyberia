using DSharpPlus.SlashCommands;

namespace Salamandra.Bot.Commands.Other
{
    public sealed class PingCommandModule : ApplicationCommandModule
    {
        [SlashCommand("ping", "Retourne Pong")]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync("Pong... " + DiscordBot.Instance.Client.Ping + "ms !", true);
        }
    }
}
