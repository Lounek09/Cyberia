using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class RestartCommandModule : ApplicationCommandModule
    {
        [SlashCommand("restart", "[RequireOwner] Restart Salamandra")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync("🔃", true);

            await Bot.Instance.Client.DisconnectAsync();

            await Bot.Instance.Client.ConnectAsync(new("Dofus Retro", ActivityType.Playing));
        }
    }
}
