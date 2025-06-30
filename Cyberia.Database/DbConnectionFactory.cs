using Cyberia.Database.TypeHandlers;

using Dapper;

using System.Data;
using System.Data.SQLite;

namespace Cyberia.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}

public sealed class SQLiteDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    static SQLiteDbConnectionFactory()
    {
        SqlMapper.AddTypeHandler(new DateTimeHandler());
    }

    public SQLiteDbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        SQLiteConnection connection = new(_connectionString);
        await connection.OpenAsync();

        return connection;
    }
}
