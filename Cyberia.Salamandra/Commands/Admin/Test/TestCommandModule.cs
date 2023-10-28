using Cyberia.Api;
using Cyberia.Api.Data;

using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using System.Reflection.Metadata;

namespace Cyberia.Salamandra.Commands.Admin
{
    public sealed class TestCommandModule : ApplicationCommandModule
    {
        [SlashCommand("test", "[Owner] Commande pour tester des trucs")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext _)
        {
            await Task.Delay(0);

            throw new NotImplementedException();
        }
    }
}
