using Cyberia.Api.DatacenterNS;

using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin
{
    public sealed class TestCommandModule : ApplicationCommandModule
    {
        [SlashCommand("test", "[RequireOwner] Commande pour tester des trucs")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext ctx)
        {
            await Task.Delay(1000);
        }
    }
}
