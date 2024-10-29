using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Admin.Leave;

public sealed class LeaveCommandModule
{
    [Command("leave"), Description("[Owner] Leave a guild")]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequirePermissions(DiscordPermissions.UseApplicationCommands)]
    [RequireApplicationOwner]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("id"), Description("Guild's id")]
        [MinMaxValue(0)]
        string guildId)
    {
        try
        {
            var guild = await ctx.Client.GetGuildAsync(ulong.Parse(guildId));
            await guild.LeaveAsync();

            if (ctx.Guild is null || guild.Id != ctx.Guild.Id)
            {
                await ctx.RespondAsync($"Bot kick from the guild {Formatter.Bold(guild.Name)} ({guild.Id}) !");
            }
        }
        catch
        {
            await ctx.RespondAsync("Guild not found");
        }
    }
}
