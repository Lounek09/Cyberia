using Cyberia.Api;

using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using System.Text;

namespace Cyberia.Salamandra.Commands.Admin;

public sealed class TestCommandModule : ApplicationCommandModule
{
    [SlashCommand("test", "[Owner] Command to test random stuff")]
    [SlashRequireOwner]
    public async Task Command(InteractionContext _)
    {
        await Task.Delay(0);

        throw new NotImplementedException();
    }
}
