using Cyberia.Database.Models;

using Dapper;

namespace Cyberia.Database.Repositories;

/// <summary>
/// Represents a repository for <see cref="DiscordCachedUser"/>.
/// </summary>
public sealed class DiscordCachedUserRepository : IDatabaseRepository<DiscordCachedUser, ulong>
{
    private readonly IDbConnectionFactory _connectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscordCachedUserRepository"/> class.
    /// </summary>
    /// <param name="connectionFactory">The connection factory.</param>
    public DiscordCachedUserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<DiscordCachedUser?> GetAsync(ulong id)
    {
        const string query =
        $"""
        SELECT * FROM {nameof(DiscordCachedUser)}
        WHERE {nameof(DiscordCachedUser.Id)} = @Id
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<DiscordCachedUser>(query, new { Id = id });
    }

    public async Task<bool> UpsertAsync(DiscordCachedUser user)
    {
        const string query =
        $"""
        INSERT INTO {nameof(DiscordCachedUser)} ({nameof(DiscordCachedUser.Id)}, {nameof(DiscordCachedUser.Locale)})
        VALUES (@{nameof(DiscordCachedUser.Id)}, @{nameof(DiscordCachedUser.Locale)})
        ON CONFLICT({nameof(DiscordCachedUser.Id)})
        DO UPDATE SET
            {nameof(DiscordCachedUser.Locale)} = excluded.{nameof(DiscordCachedUser.Locale)}
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteAsync(query, user) > 0;
    }

    public async Task<bool> DeleteAsync(ulong id)
    {
        const string query =
        $"""
        DELETE FROM {nameof(DiscordCachedUser)}
        WHERE {nameof(DiscordCachedUser.Id)} = @Id
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteAsync(query, new { Id = id }) > 0;
    }
}
