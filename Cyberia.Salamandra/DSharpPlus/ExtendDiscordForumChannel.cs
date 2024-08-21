using DSharpPlus.Entities;

namespace Cyberia.Salamandra.DSharpPlus;

/// <summary>
/// Provides extension methods for <see cref="DiscordForumChannel"/>.
/// </summary>
public static class ExtendDiscordForumChannel
{
    public const string ManualDiffTagName = "Manual diff";

    /// <summary>
    /// Gets a <see cref="DiscordForumTag"/> by its name.
    /// </summary>
    /// <param name="forum">The forum.</param>
    /// <param name="name">The name of the tag.</param>
    /// <returns>Returns the tag with the specified name; if not found, <see langword="null"/>.</returns>
    public static DiscordForumTag? GetDiscordForumTagByName(this DiscordForumChannel forum, string name)
    {
        return forum.AvailableTags.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
