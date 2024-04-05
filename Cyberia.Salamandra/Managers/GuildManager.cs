using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Managers;

public static class GuildManager
{
    public static async Task OnGuildCreated(DiscordClient _, GuildCreateEventArgs args)
    {
        await MessageManager.SendLogMessage(
            $"""
            [NEW] {Formatter.Bold(args.Guild.Name)} ({args.Guild.Id})
            créé le : {args.Guild.CreationTimestamp:dd/MM/yyyy hh:mm}
            Propriétaire : {Formatter.Sanitize(args.Guild.Owner.Username)} ({args.Guild.Owner.Mention})
            """);
    }

    public static async Task OnGuildDeleted(DiscordClient _, GuildDeleteEventArgs args)
    {
        await MessageManager.SendLogMessage($"[LOSE] {Formatter.Bold(args.Guild.Name)} ({args.Guild.Id})");
    }
}
