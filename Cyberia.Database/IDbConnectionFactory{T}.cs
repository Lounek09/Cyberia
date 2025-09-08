using System.Data;

namespace Cyberia.Database;

/// <summary>
/// Represents a factory for creating database connections.
/// </summary>
/// <typeparam name="T">The type of the database connection.</typeparam>
public interface IDbConnectionFactory<T>
    where T : IDbConnection
{
    /// <summary>
    /// Creates and opens a new database connection asynchronously.
    /// </summary>
    /// <returns>The opened database connection.</returns>
    Task<T> CreateConnectionAsync();
}
