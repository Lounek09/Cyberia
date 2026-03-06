using Cyberia.Database.Models;

using Dapper;

using System.Data.SQLite;

namespace Cyberia.Database.Repositories;

/// <summary>
/// Represents a repository for <see cref="MonitoredFile"/>.
/// </summary>
public sealed class MonitoredFileRepository : IDatabaseRepository<MonitoredFile, string>
{
    private readonly IDbConnectionFactory<SQLiteConnection> _connectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonitoredFileRepository"/> class.
    /// </summary>
    /// <param name="connectionFactory">The connection factory.</param>
    public MonitoredFileRepository(IDbConnectionFactory<SQLiteConnection> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public MonitoredFile? Get(string id)
    {
        const string query =
        $"""
        SELECT * FROM {nameof(MonitoredFile)}
        WHERE {nameof(MonitoredFile.Id)} = @{nameof(id)}
        """;

        using var connection = _connectionFactory.CreateConnection();
        return connection.QueryFirstOrDefault<MonitoredFile>(query, new { id });
    }

    public IEnumerable<MonitoredFile> GetMany(params IEnumerable<string> ids)
    {
        const string query =
        $"""
        SELECT * 
        FROM {nameof(MonitoredFile)}
        WHERE {nameof(MonitoredFile.Id)} IN @{nameof(ids)}
        """;

        if (!ids.Any())
        {
            return Enumerable.Empty<MonitoredFile>();
        }

        using var connection = _connectionFactory.CreateConnection();
        return connection.Query<MonitoredFile>(query, new { ids });
    }

    public bool Upsert(MonitoredFile entity)
    {
        const string query =
        $"""
        INSERT INTO {nameof(MonitoredFile)} ({nameof(MonitoredFile.Id)}, {nameof(MonitoredFile.LastModified)})
        VALUES (@{nameof(MonitoredFile.Id)}, @{nameof(MonitoredFile.LastModified)})
        ON CONFLICT({nameof(MonitoredFile.Id)})
        DO UPDATE SET
            {nameof(MonitoredFile.LastModified)} = excluded.{nameof(MonitoredFile.LastModified)}
        """;

        lock (SQLiteDbConnectionFactory.WriteLock)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Execute(query, entity) > 0;
        }
    }

    public int UpsertMany(params IEnumerable<MonitoredFile> entities)
    {
        const string query =
        $"""
        INSERT INTO {nameof(MonitoredFile)} ({nameof(MonitoredFile.Id)}, {nameof(MonitoredFile.LastModified)})
        VALUES (@{nameof(MonitoredFile.Id)}, @{nameof(MonitoredFile.LastModified)})
        ON CONFLICT({nameof(MonitoredFile.Id)})
        DO UPDATE SET
            {nameof(MonitoredFile.LastModified)} = excluded.{nameof(MonitoredFile.LastModified)}
        """;

        if (!entities.Any())
        {
            return 0;
        }

        lock (SQLiteDbConnectionFactory.WriteLock)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var transaction = connection.BeginTransaction();

            var affectedRows = connection.Execute(query, entities, transaction);

            transaction.Commit();

            return affectedRows;
        }
    }

    public bool Delete(string id)
    {
        const string query =
        $"""
        DELETE FROM {nameof(MonitoredFile)}
        WHERE {nameof(MonitoredFile.Id)} = @{nameof(id)}
        """;

        lock (SQLiteDbConnectionFactory.WriteLock)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Execute(query, new { id }) > 0;
        }
    }

    public int DeleteMany(params IEnumerable<string> ids)
    {
        const string query =
        $"""
        DELETE FROM {nameof(MonitoredFile)}
        WHERE {nameof(MonitoredFile.Id)} IN @{nameof(ids)}
        """;

        if (!ids.Any())
        {
            return 0;
        }

        lock (SQLiteDbConnectionFactory.WriteLock)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Execute(query, new { ids });
        }
    }

    /// <summary>
    /// Gets the last modified date of a <see cref="MonitoredFile"/> by its ID.
    /// </summary>
    /// <param name="id">The ID of the <see cref="MonitoredFile"/>.</param>
    /// <returns>The last modified date of the <see cref="MonitoredFile"/>, or <see cref="DateTime.MinValue"/> if not found.</returns>
    public DateTime GetLastModifiedById(string id)
    {
        const string query =
        $"""
        SELECT {nameof(MonitoredFile.LastModified)}
        FROM {nameof(MonitoredFile)}
        WHERE {nameof(MonitoredFile.Id)} = @{nameof(id)}
        """;

        using var connection = _connectionFactory.CreateConnection();
        return connection.QueryFirstOrDefault<DateTime>(query, new { id });
    }
}
