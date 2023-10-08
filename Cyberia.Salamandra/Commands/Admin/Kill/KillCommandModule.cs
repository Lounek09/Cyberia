using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin
{
    public sealed class KillCommandModule : ApplicationCommandModule
    {
        [SlashCommand("kill", "[Owner] Coupe Salamandra")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(@"\💀");

            await Bot.Instance.Client.DisconnectAsync();
        }
    }
}
