using Cyberia.Api;
using Cyberia.Salamandra.Managers;

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
        var builder = new StringBuilder();

        builder.Append("Items:\n");
        foreach (var itemStats in DofusApi.Datacenter.ItemsStatsData.ItemsStats)
        {
            await Task.Delay(0);

            throw new NotImplementedException();
        }
    }
}
