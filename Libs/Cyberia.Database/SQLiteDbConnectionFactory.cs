using System.Data.SQLite;

namespace Cyberia.Database;

/// <summary>
/// Represents a factory for creating SQLite database connections.
/// </summary>
public sealed class SQLiteDbConnectionFactory : IDbConnectionFactory<SQLiteConnection>
{
    /// <summary>
    /// A lock used to avoid concurrent write on the SQLite Db.
    /// </summary>
    public static readonly Lock WriteLock = new();

    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="SQLiteDbConnectionFactory"/> class.
    /// </summary>
    /// <param name="connectionString">The connection string to the SQLite database.</param>
    public SQLiteDbConnectionFactory(string connectionString)
    {
        SQLiteConnectionStringBuilder builder = new(connectionString)
        {
            JournalMode = SQLiteJournalModeEnum.Wal,
            BusyTimeout = 5000
        };

        _connectionString = builder.ToString();
    }

    public SQLiteConnection CreateConnection()
    {
        SQLiteConnection connection = new(_connectionString);
        connection.Open();

        return connection;
    }
}
