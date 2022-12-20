using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Salamandra.Bot.Commands.Admin
{
    public sealed class RestartCommandModule : ApplicationCommandModule
    {
        [SlashCommand("restart", "[RequireOwner] Restart Salamandra")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync("🔃", true);

            await DiscordBot.Instance.Client.DisconnectAsync();

            await DiscordBot.Instance.Client.ConnectAsync(new("Dofus Retro", ActivityType.Playing));
        }
    }
}
