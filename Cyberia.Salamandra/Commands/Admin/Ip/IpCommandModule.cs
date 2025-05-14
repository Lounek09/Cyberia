using Cyberia.Api.Utils;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Admin.Ip;

public sealed class IpCommandModule
{
    [Command("ip"), Description("Decodes IPs sent from game packets")]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("value"), Description("The encoded IP")]
        [MinMaxLength(11, 11)]
        string value)
    {
        try
        {
            await ctx.RespondAsync(PatternDecoder.DecodeIp(value));
        }
        catch (ArgumentException e)
        {
            await ctx.RespondAsync(e.Message);
        }
    }
}
