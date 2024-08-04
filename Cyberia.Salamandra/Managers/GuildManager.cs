using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Managers;

public static class GuildManager
{
    public static async Task OnGuildCreated(DiscordClient _, GuildCreatedEventArgs args)
    {
        await MessageManager.SendLogMessage(
            $"""
            [NEW] {Formatter.Bold(Formatter.Sanitize(args.Guild.Name))} ({args.Guild.Id})
            created on : {args.Guild.CreationTimestamp}
            Owner : {Formatter.Sanitize(args.Guild.Owner.Username)} ({args.Guild.Owner.Mention})
            """);
    }

    public static async Task OnGuildDeleted(DiscordClient _, GuildDeletedEventArgs args)
    {
        await MessageManager.SendLogMessage($"[LOSE] {Formatter.Bold(Formatter.Sanitize(args.Guild.Name))} ({args.Guild.Id})");
    }
}
