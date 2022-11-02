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

            DiscordActivity activity = new("redémarrage en cours", ActivityType.Playing);
            await DiscordBot.Instance.Client.UpdateStatusAsync(activity);

            ExecuteCmd.ExecuteCommand("sudo", "systemctl restart salamandra", out _);
        }
    }
}
