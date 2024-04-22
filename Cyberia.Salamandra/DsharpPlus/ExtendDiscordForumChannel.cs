using DSharpPlus.Entities;

namespace Cyberia.Salamandra.DsharpPlus;

public static class ExtendDiscordForumChannel
{
    public static DiscordForumTag? GetDiscordForumTagByName(this DiscordForumChannel forum, string name)
    {
        return forum.AvailableTags.FirstOrDefault(x => x.Name.Equals(name));
    }
}
