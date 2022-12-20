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

            await DiscordBot.Instance.Client.DisconnectAsync();
        }
    }
}
