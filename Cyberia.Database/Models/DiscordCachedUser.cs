namespace Cyberia.Database.Models;

public sealed class DiscordCachedUser : IDatabaseEntity
{
    /// <summary>
    /// Gets the ID of the user.
    /// </summary>
    public required ulong Id { get; init; }

    /// <summary>
    /// Gets or sets the locale of the user.
    /// </summary>
    public string? Locale { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscordCachedUser"/> class.
    /// </summary>
    public DiscordCachedUser()
    {
        
    }
}
