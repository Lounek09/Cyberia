using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin;

public sealed class LeaveCommandModule : ApplicationCommandModule
{
    [SlashCommand("leave", "[Owner] Leave a guild")]
    [SlashRequireOwner]
    public async Task LeaveCommand(InteractionContext ctx,
        [Option("id", "Guild's id")]
        string guildId)
    {
        try
        {
            var guild = await Bot.Client.GetGuildAsync(Convert.ToUInt64(guildId));
            await guild.LeaveAsync();

            if (guild.Id != ctx.Guild.Id)
            {
                await ctx.CreateResponseAsync($"Bot kick from the guild {Formatter.Bold(guild.Name)} ({guild.Id}) !");
            }
        }
        catch
        {
            await ctx.CreateResponseAsync("Guild not found");
        }
    }
}
