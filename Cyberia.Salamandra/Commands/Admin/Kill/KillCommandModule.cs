using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin;

public sealed class KillCommandModule : ApplicationCommandModule
{
    [SlashCommand("kill", "[Owner] Kill Salamandra")]
    [SlashRequireOwner]
    public async Task Command(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync(@"\💀");

        await Bot.Client.DisconnectAsync();
    }
}
