namespace Cyberia.Database.Models;

public interface IDatabaseEntity<T>
    where T : notnull
{
    /// <summary>
    /// Gets the ID of the entity.
    /// </summary>
    T Id { get; init; }
}
