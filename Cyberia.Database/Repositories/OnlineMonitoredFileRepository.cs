using Cyberia.Database.Models;

using Dapper;

namespace Cyberia.Database.Repositories;

/// <summary>
/// Represents a repository for <see cref="OnlineMonitoredFile"/>.
/// </summary>
public sealed class OnlineMonitoredFileRepository : IDatabaseRepository<OnlineMonitoredFile, string>
{
    private readonly IDbConnectionFactory _connectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="OnlineMonitoredFileRepository"/> class.
    /// </summary>
    /// <param name="connectionFactory">The connection factory.</param>
    public OnlineMonitoredFileRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<OnlineMonitoredFile?> GetAsync(string id)
    {
        const string query =
        $"""
        SELECT * FROM {nameof(OnlineMonitoredFile)}
        WHERE {nameof(OnlineMonitoredFile.Id)} = @Id
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<OnlineMonitoredFile>(query, new { Id = id });
    }

    public async Task<bool> UpsertAsync(OnlineMonitoredFile file)
    {
        const string query =
        $"""
        INSERT INTO {nameof(OnlineMonitoredFile)} ({nameof(OnlineMonitoredFile.Id)}, {nameof(OnlineMonitoredFile.LastModified)})
        VALUES (@{nameof(OnlineMonitoredFile.Id)}, @{nameof(OnlineMonitoredFile.LastModified)})
        ON CONFLICT({nameof(OnlineMonitoredFile.Id)})
        DO UPDATE SET
            {nameof(OnlineMonitoredFile.LastModified)} = excluded.{nameof(OnlineMonitoredFile.LastModified)}
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteAsync(query, file) > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        const string query =
        $"""
        DELETE FROM {nameof(OnlineMonitoredFile)}
        WHERE {nameof(OnlineMonitoredFile.Id)} = @Id
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteAsync(query, new { Id = id }) > 0;
    }
}
