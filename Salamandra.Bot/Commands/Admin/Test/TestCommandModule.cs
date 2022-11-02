using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Salamandra.Bot.Commands.Admin
{
    public sealed class TestCommandModule : ApplicationCommandModule
    {
        [SlashCommand("test", "[RequireOwner] Commande pour tester des trucs")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(".l.");
        }
    }
}
