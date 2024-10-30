using Cyberia.Database.Models;

using Dapper;

namespace Cyberia.Database.Repositories;

/// <summary>
/// Represents a repository for Discord users.
/// </summary>
public sealed class DiscordCachedUserRepository : IDatabaseRepository
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

    /// <summary>
    /// Creates the table for the repository.
    /// </summary>
    /// <returns><see langword="true"/> if the table was created; otherwise, <see langword="false"/>.</returns>
    public async Task<bool> CreateTableAsync()
    {
        var query = @"
            CREATE TABLE IF NOT EXISTS DiscordCachedUser (
                Id INTEGER PRIMARY KEY,
                Locale TEXT
            );";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteAsync(query) > 0;
    }

    /// <summary>
    /// Gets a Discord user by their id.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns>The user if found; otherwise, <see langword="null"/>.</returns>
    public async Task<DiscordCachedUser?> GetAsync(ulong id)
    {
        var query = @"
            SELECT * FROM DiscordCachedUser
            WHERE Id = @Id";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<DiscordCachedUser>(query, new { Id = id });
    }

    /// <summary>
    /// Creates or updates a Discord user.
    /// </summary>
    /// <param name="user">The user to create or update.</param>
    /// <returns><see langword="true"/> if the user was created or updated; otherwise, <see langword="false"/>.</returns>
    public async Task<bool> CreateOrUpdateAsync(DiscordCachedUser user)
    {
        var query = @"
            INSERT INTO DiscordCachedUser (Id, Locale)
            VALUES (@Id, @Locale)
            ON CONFLICT(Id) DO UPDATE SET Locale = @Locale";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteAsync(query, user) > 0;
    }

    /// <summary>
    /// Deletes a Discord user by their id.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns><see langword="true"/> if the user was deleted; otherwise, <see langword="false"/>.</returns>
    public async Task<bool> DeleteAsync(ulong id)
    {
        var query = @"
            DELETE FROM DiscordCachedUser
            WHERE Id = @Id";

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteAsync(query, new { Id = id }) > 0;
    }
}
