using DSharpPlus.SlashCommands;

using Salamandra.Api;

namespace Salamandra.Bot.Commands.Admin
{
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
