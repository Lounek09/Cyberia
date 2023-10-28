using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Admin
{
    public sealed class LeaveCommandModule : ApplicationCommandModule
    {
        [SlashCommand("leave", "[Owner] Quitte un serveur via son id")]
        [SlashRequireOwner]
        public async Task LeaveCommand(InteractionContext ctx,
            [Option("id", "Id du serveur discord")]
            string guildId)
        {
            try
            {
                DiscordGuild guild = await Bot.Client.GetGuildAsync(Convert.ToUInt64(guildId));
                await guild.LeaveAsync();

                if (guild.Id != ctx.Guild.Id)
                {
                    await ctx.CreateResponseAsync($"Bot kick du discord {Formatter.Bold(guild.Name)} ({guild.Id}) !");
                }
            }
            catch
            {
                await ctx.CreateResponseAsync("Serveur introuvable");
            }
        }
    }
}
