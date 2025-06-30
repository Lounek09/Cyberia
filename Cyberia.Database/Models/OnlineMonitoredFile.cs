namespace Cyberia.Database.Models;

/// <summary>
/// Represents an online file that is being monitored for changes.
/// </summary>
public sealed class OnlineMonitoredFile : IDatabaseEntity
{
    /// <summary>
    /// Gets the ID of the file.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets or sets when the file was last modified.
    /// </summary>
    public required DateTime LastModified { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MonitoredFile"/> class.
    /// </summary>
    public OnlineMonitoredFile()
    {
        
    }
}
