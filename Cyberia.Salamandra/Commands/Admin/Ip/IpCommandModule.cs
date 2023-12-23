using Cyberia.Api;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Admin;

public sealed class IpCommandModule : ApplicationCommandModule
{
    [SlashCommand("ip", "Decodes IPs sent via packets")]
    public async Task Command(InteractionContext ctx,
        [Option("ip", "Encoded IP")]
        string value)
    {
        try
        {
            await ctx.CreateResponseAsync(PatternDecoder.Ip(value));
        }
        catch (ArgumentException e)
        {
            await ctx.CreateResponseAsync(e.Message);
        }
    }
}
