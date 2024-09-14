using Cyberia.Database.Models;

using Dapper;

using System.Data;

namespace Cyberia.Database.Repositories;

/// <summary>
/// Represents a repository for Discord users.
/// </summary>
public sealed class UserRepository : IDatabaseRepository
{
    private readonly IDbConnection _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    public UserRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Creates the table for the repository.
    /// </summary>
    /// <returns><see langword="true"/> if the table was created; otherwise, <see langword="false"/>.</returns>
    public async Task<bool> CreateTableAsync()
    {
        var query = @"
            CREATE TABLE IF NOT EXISTS User (
                Id INTEGER PRIMARY KEY,
                Locale TEXT
            );";

        return await _connection.ExecuteAsync(query) > 0;
    }

    /// <summary>
    /// Gets a Discord user by their id.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns>The user if found; otherwise, <see langword="null"/>.</returns>
    public async Task<User?> GetAsync(ulong id)
    {
        var query = @"
            SELECT * FROM User
            WHERE Id = @Id";

        return await _connection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
    }

    /// <summary>
    /// Creates or updates a Discord user.
    /// </summary>
    /// <param name="user">The user to create or update.</param>
    /// <returns><see langword="true"/> if the user was created or updated; otherwise, <see langword="false"/>.</returns>
    public async Task<bool> CreateOrUpdateAsync(User user)
    {
        var query = @"
            INSERT INTO User (Id, Locale)
            VALUES (@Id, @Locale)
            ON CONFLICT(Id) DO UPDATE SET Locale = @Locale";

        return await _connection.ExecuteAsync(query, user) > 0;
    }

    /// <summary>
    /// Deletes a Discord user by their id.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns><see langword="true"/> if the user was deleted; otherwise, <see langword="false"/>.</returns>
    public async Task<bool> DeleteAsync(ulong id)
    {
        var query = @"
            DELETE FROM User
            WHERE Id = @Id";

        return await _connection.ExecuteAsync(query, new { Id = id }) > 0;
    }
}
