using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Salamandra.Bot.Commands.Admin
{
    public sealed class KillCommandModule : ApplicationCommandModule
    {
        [SlashCommand("kill", "[RequireOwner] Coupe Salamandra")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync("💀", true);

            DiscordActivity activity = new("se suicide", ActivityType.Playing);
            await DiscordBot.Instance.Client.UpdateStatusAsync(activity);

            await DiscordBot.Instance.Client.DisconnectAsync();
        }
    }
}
