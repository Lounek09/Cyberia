using Cyberia.Api;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.ContextChecks;
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
    [RequirePermissions(DiscordPermissions.UseApplicationCommands)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("value"), Description("The encoded IP")]
        [MinMaxLength(11, 11)]
        string value)
    {
        try
        {
            await ctx.RespondAsync(PatternDecoder.Ip(value));
        }
        catch (ArgumentException e)
        {
            await ctx.RespondAsync(e.Message);
        }
    }
}
