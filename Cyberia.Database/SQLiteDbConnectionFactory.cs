using System.Data.SQLite;

namespace Cyberia.Database;

/// <summary>
/// Represents a factory for creating SQLite database connections.
/// </summary>
public sealed class SQLiteDbConnectionFactory : IDbConnectionFactory<SQLiteConnection>
{
    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="SQLiteDbConnectionFactory"/> class.
    /// </summary>
    /// <param name="connectionString"></param>
    public SQLiteDbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<SQLiteConnection> CreateConnectionAsync()
    {
        SQLiteConnection connection = new(_connectionString);
        await connection.OpenAsync();

        return connection;
    }
}
