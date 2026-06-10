using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Extensions.DSharpPlus;

/// <summary>
/// Provides extension methods for <see cref="DiscordForumChannel"/>.
/// </summary>
public static class DiscordForumChannelExtensions
{
    public const string ManualDiffTagName = "Manual diff";

    extension(DiscordForumChannel forumChannel)
    {
        /// <summary>
        /// Gets a <see cref="DiscordForumTag"/> by its name.
        /// </summary>
        /// <param name="name">The name of the tag.</param>
        /// <returns>Returns the tag with the specified name; if not found, <see langword="null"/>.</returns>
        public DiscordForumTag? GetDiscordForumTagByName(string name)
        {
            return forumChannel.AvailableTags.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
