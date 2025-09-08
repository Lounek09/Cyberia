using System.Data.SQLite;

namespace Cyberia.Database;

public sealed class SQLiteDbConnectionFactory : IDbConnectionFactory<SQLiteConnection>
{
    private readonly string _connectionString;

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
