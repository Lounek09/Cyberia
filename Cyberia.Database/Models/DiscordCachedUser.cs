namespace Cyberia.Database.Models;

/// <summary>
/// Represents a cached Discord user.
/// </summary>
public sealed class DiscordCachedUser : IDatabaseEntity<ulong>
{
    public required ulong Id { get; init; }

    /// <summary>
    /// Gets or sets the locale of the user.
    /// </summary>
    public string? Locale { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscordCachedUser"/> class.
    /// </summary>
    public DiscordCachedUser() { }
}
