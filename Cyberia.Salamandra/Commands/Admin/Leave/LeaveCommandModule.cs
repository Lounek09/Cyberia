using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Admin;

public sealed class LeaveCommandModule
{
    [Command("leave"), Description("[Owner] Leave a guild")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [RequireApplicationOwner]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("id"), Description("Guild's id")]
        [SlashMinMaxValue(MinValue = 0)]
        long guildId)
    {
        try
        {
            var guild = await ctx.Client.GetGuildAsync((ulong)guildId);
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
