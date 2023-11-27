using DSharpPlus.SlashCommands;
namespace Cyberia.Salamandra.Commands.Other;

public sealed class PingCommandModule : ApplicationCommandModule
{
    [SlashCommand("ping", "Retourne Pong")]
    public async Task Command(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync("Pong... " + Bot.Client.Ping + "ms !", true);
    }
}
