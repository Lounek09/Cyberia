using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin
{
    public sealed class RestartCommandModule : ApplicationCommandModule
    {
        [SlashCommand("restart", "[Owner] Restart Salamandra")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync("🔃", true);

            await Bot.Instance.Client.DisconnectAsync();

            await Bot.Instance.Client.ConnectAsync(new("Dofus Retro", ActivityType.Playing));
        }
    }
}
