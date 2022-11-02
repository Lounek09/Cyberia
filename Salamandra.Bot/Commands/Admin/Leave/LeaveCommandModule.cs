using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Salamandra.Bot.Commands.Admin
{
    public sealed class LeaveCommandModule : ApplicationCommandModule
    {
        [SlashCommand("leave", "[RequireOwner] Quitte un serveur via son id")]
        [SlashRequireOwner]
        public async Task LeaveCommand(InteractionContext ctx,
            [Option("id", "Id du serveur discord")]
            string guildId)
        {
            try
            {
                DiscordGuild guild = await DiscordBot.Instance.Client.GetGuildAsync(Convert.ToUInt64(guildId));
                await guild.LeaveAsync();

                if (guild.Id != ctx.Guild.Id)
                    await ctx.CreateResponseAsync("Bot kick du discord **" + guild.Name + "** (" + guild.Id + ") !");
            }
            catch
            {
                await ctx.CreateResponseAsync("Serveur introuvable");
            }
        }
    }
}
