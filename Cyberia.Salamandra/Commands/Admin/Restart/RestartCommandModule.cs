using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin;

public sealed class RestartCommandModule : ApplicationCommandModule
{
    [SlashCommand("restart", "[Owner] Restart Salamandra")]
    [SlashRequireOwner]
    public async Task Command(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync("🔃", true);

        await Bot.Client.DisconnectAsync();

        await Bot.Client.ConnectAsync(new DiscordActivity("Dofus Retro", ActivityType.Playing));
    }
}
