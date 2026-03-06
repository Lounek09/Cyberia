using Cyberia.Database.Models;

using Dapper;

using System.Data.SQLite;

using static Dapper.SqlMapper;

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

    public async Task<MonitoredFile?> GetAsync(string id)
    {
        const string query =
        $"""
        SELECT * FROM {nameof(MonitoredFile)}
        WHERE {nameof(MonitoredFile.Id)} = @id
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<MonitoredFile>(query, new { id });
    }

    public async Task<IEnumerable<MonitoredFile>> GetManyAsync(params IEnumerable<string> ids)
    {
        const string query =
        $"""
        SELECT * 
        FROM {nameof(MonitoredFile)}
        WHERE {nameof(MonitoredFile.Id)} IN @ids
        """;

        if (!ids.Any())
        {
            return Enumerable.Empty<MonitoredFile>();
        }

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<MonitoredFile>(query, new { ids });
    }

    public async Task<bool> UpsertAsync(MonitoredFile file)
    {
        const string query =
        $"""
        INSERT INTO {nameof(MonitoredFile)} ({nameof(MonitoredFile.Id)}, {nameof(MonitoredFile.LastModified)})
        VALUES (@{nameof(MonitoredFile.Id)}, @{nameof(MonitoredFile.LastModified)})
        ON CONFLICT({nameof(MonitoredFile.Id)})
        DO UPDATE SET
            {nameof(MonitoredFile.LastModified)} = excluded.{nameof(MonitoredFile.LastModified)}
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();

        await SQLiteDbConnectionFactory.WriteLock.WaitAsync();
        try
        {
            return await connection.ExecuteAsync(query, file) > 0;
        }
        finally
        {
            SQLiteDbConnectionFactory.WriteLock.Release();
        }
    }

    public async Task<int> UpsertManyAsync(params IEnumerable<MonitoredFile> files)
    {
        const string query =
        $"""
        INSERT INTO {nameof(MonitoredFile)} ({nameof(MonitoredFile.Id)}, {nameof(MonitoredFile.LastModified)})
        VALUES (@{nameof(MonitoredFile.Id)}, @{nameof(MonitoredFile.LastModified)})
        ON CONFLICT({nameof(MonitoredFile.Id)})
        DO UPDATE SET
            {nameof(MonitoredFile.LastModified)} = excluded.{nameof(MonitoredFile.LastModified)}
        """;

        if (!files.Any())
        {
            return 0;
        }

        using var connection = await _connectionFactory.CreateConnectionAsync();

        await SQLiteDbConnectionFactory.WriteLock.WaitAsync();
        try
        {
            using var transaction = await connection.BeginTransactionAsync();

            var affectedRows = await connection.ExecuteAsync(query, files, transaction);

            await transaction.CommitAsync();

            return affectedRows;
        }
        finally
        {
            SQLiteDbConnectionFactory.WriteLock.Release();
        }
    }

    public async Task<bool> DeleteAsync(string id)
    {
        const string query =
        $"""
        DELETE FROM {nameof(MonitoredFile)}
        WHERE {nameof(MonitoredFile.Id)} = @id
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();

        await SQLiteDbConnectionFactory.WriteLock.WaitAsync();
        try
        {
            return await connection.ExecuteAsync(query, new { id }) > 0;
        }
        finally
        {
            SQLiteDbConnectionFactory.WriteLock.Release();
        }

    }

    public async Task<int> DeleteManyAsync(params IEnumerable<string> ids)
    {
        const string query =
        $"""
        DELETE FROM {nameof(MonitoredFile)}
        WHERE {nameof(MonitoredFile.Id)} IN @ids
        """;

        if (!ids.Any())
        {
            return 0;
        }

        using var connection = await _connectionFactory.CreateConnectionAsync();

        await SQLiteDbConnectionFactory.WriteLock.WaitAsync();
        try
        {
            return await connection.ExecuteAsync(query, new { ids });
        }
        finally
        {
            SQLiteDbConnectionFactory.WriteLock.Release();
        }
    }

    /// <summary>
    /// Gets the last modified date of a <see cref="MonitoredFile"/> by its ID.
    /// </summary>
    /// <param name="id">The ID of the <see cref="MonitoredFile"/>.</param>
    /// <returns>The last modified date of the <see cref="MonitoredFile"/>, or <see cref="DateTime.MinValue"/> if not found.</returns>
    public async Task<DateTime> GetLastModifiedByIdAsync(string id)
    {
        const string query =
        $"""
        SELECT {nameof(MonitoredFile.LastModified)}
        FROM {nameof(MonitoredFile)}
        WHERE {nameof(MonitoredFile.Id)} = @id
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<DateTime>(query, new { id });
    }
}
