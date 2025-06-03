using Cyberia.Database.Models;

namespace Cyberia.Database.Migrations;

/// <summary>
/// Represents a database migration.
/// </summary>
internal sealed class Migration : IDatabaseEntity
{
    /// <summary>
    /// Gets the ID of the migration.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets the name of the migration.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the date and time when the migration was applied.
    /// </summary>
    public DateTime AppliedAt { get; init; }
}
