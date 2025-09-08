namespace Cyberia.Database.Models;

/// <summary>
/// Represents a database entity.
/// </summary>
/// <typeparam name="T">The type of the entity's ID.</typeparam>
public interface IDatabaseEntity<T>
    where T : notnull
{
    /// <summary>
    /// Gets the ID of the entity.
    /// </summary>
    T Id { get; init; }
}
