using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Managers
{
    public static class GuildManager
    {
        public static async Task OnGuildCreated(DiscordClient _, GuildCreateEventArgs e)
        {
            await MessageManager.SendLogMessage($"[NEW] **{e.Guild.Name}** ({e.Guild.Id})\ncréé le : {e.Guild.CreationTimestamp:dd/MM/yyyy hh:mm}\nPropriétaire : {e.Guild.Owner.Username} ({e.Guild.Owner.Mention})");
        }

        public static async Task OnGuildDeleted(DiscordClient _, GuildDeleteEventArgs e)
        {
            await MessageManager.SendLogMessage($"[LOSE] **{e.Guild.Name}** ({e.Guild.Id})");
        }
    }
}
