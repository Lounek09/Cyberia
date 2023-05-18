using Cyberia.Api;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Admin
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class IpCommandModule : ApplicationCommandModule
    {

        [SlashCommand("ip", "Décode les ips envoyées via les paquets")]
        public async Task Command(InteractionContext ctx,
            [Option("ip", "Ip encodée")]
            string value)
        {
            try
            {
                await ctx.CreateResponseAsync(PatternDecoder.DecodeIp(value));
            }
            catch (ArgumentException e)
            {
                await ctx.CreateResponseAsync(e.Message);
            }
        }
    }
}
