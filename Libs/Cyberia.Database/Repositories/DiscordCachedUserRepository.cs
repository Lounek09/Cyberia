using Cyberia.Database.Models;

using Dapper;

using System.Data.SQLite;

namespace Cyberia.Database.Repositories;

/// <summary>
/// Represents a repository for <see cref="DiscordCachedUser"/>.
/// </summary>
public interface IDiscordCachedUserRepository : IDatabaseRepository<DiscordCachedUser, ulong>
{
    /// <summary>
    /// Gets the locale of a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The locale of the user if found; otherwise, <see langword="null"/>.</returns>
    string? GetLocaleById(ulong id);
}

/// <inheritdoc cref="IDiscordCachedUserRepository"/>
public sealed class DiscordCachedUserRepository : IDiscordCachedUserRepository
{
    private readonly IDbConnectionFactory<SQLiteConnection> _connectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscordCachedUserRepository"/> class.
    /// </summary>
    public DiscordCachedUserRepository(IDbConnectionFactory<SQLiteConnection> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public DiscordCachedUser? Get(ulong id)
    {
        const string query =
        $"""
        SELECT * FROM {nameof(DiscordCachedUser)}
        WHERE {nameof(DiscordCachedUser.Id)} = @{nameof(id)}
        """;

        using var connection = _connectionFactory.CreateConnection();
        return connection.QueryFirstOrDefault<DiscordCachedUser>(query, new { id });
    }

    public IEnumerable<DiscordCachedUser> GetMany(params IEnumerable<ulong> ids)
    {
        const string query =
        $"""
        SELECT * 
        FROM {nameof(DiscordCachedUser)}
        WHERE {nameof(DiscordCachedUser.Id)} IN @{nameof(ids)}
        """;

        if (!ids.Any())
        {
            return Enumerable.Empty<DiscordCachedUser>();
        }

        using var connection = _connectionFactory.CreateConnection();
        return connection.Query<DiscordCachedUser>(query, new { ids });
    }

    public bool Upsert(DiscordCachedUser entity)
    {
        const string query =
        $"""
        INSERT INTO {nameof(DiscordCachedUser)} ({nameof(DiscordCachedUser.Id)}, {nameof(DiscordCachedUser.Locale)})
        VALUES (@{nameof(DiscordCachedUser.Id)}, @{nameof(DiscordCachedUser.Locale)})
        ON CONFLICT({nameof(DiscordCachedUser.Id)})
        DO UPDATE SET
            {nameof(DiscordCachedUser.Locale)} = excluded.{nameof(DiscordCachedUser.Locale)}
        """;

        lock (SQLiteDbConnectionFactory.WriteLock)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Execute(query, entity) > 0;
        }
    }

    public int UpsertMany(params IEnumerable<DiscordCachedUser> entities)
    {
        const string query =
        $"""
        INSERT INTO {nameof(DiscordCachedUser)} ({nameof(DiscordCachedUser.Id)}, {nameof(DiscordCachedUser.Locale)})
        VALUES (@{nameof(DiscordCachedUser.Id)}, @{nameof(DiscordCachedUser.Locale)})
        ON CONFLICT({nameof(DiscordCachedUser.Id)})
        DO UPDATE SET
            {nameof(DiscordCachedUser.Locale)} = excluded.{nameof(DiscordCachedUser.Locale)}
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

    public bool Delete(ulong id)
    {
        const string query =
        $"""
        DELETE FROM {nameof(DiscordCachedUser)}
        WHERE {nameof(DiscordCachedUser.Id)} = @{nameof(id)}
        """;

        lock (SQLiteDbConnectionFactory.WriteLock)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Execute(query, new { id }) > 0;
        }
    }

    public int DeleteMany(params IEnumerable<ulong> ids)
    {
        const string query =
        $"""
        DELETE FROM {nameof(DiscordCachedUser)}
        WHERE {nameof(DiscordCachedUser.Id)} IN @{nameof(ids)}
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

    public string? GetLocaleById(ulong id)
    {
        const string query =
        $"""
        SELECT {nameof(DiscordCachedUser.Locale)}
        FROM {nameof(DiscordCachedUser)}
        WHERE {nameof(DiscordCachedUser.Id)} = @{nameof(id)}
        """;

        using var connection = _connectionFactory.CreateConnection();
        return connection.QueryFirstOrDefault<string>(query, new { id });
    }
}
