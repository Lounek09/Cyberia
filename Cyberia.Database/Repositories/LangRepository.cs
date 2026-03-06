using Cyberia.Database.Models;
using Cyberia.Langzilla.Primitives;

using Dapper;

using System.Data.SQLite;

namespace Cyberia.Database.Repositories;

/// <summary>
/// Represents a repository for <see cref="Lang"/>.
/// </summary>
public sealed class LangRepository : IDatabaseRepository<Lang, int>
{
    private readonly IDbConnectionFactory<SQLiteConnection> _connectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="LangRepository"/> class.
    /// </summary>
    /// <param name="connectionFactory">The connection factory.</param>
    public LangRepository(IDbConnectionFactory<SQLiteConnection> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Lang? Get(int id)
    {
        const string query =
        $"""
        SELECT * FROM {nameof(Lang)}
        WHERE {nameof(Lang.Id)} = @{nameof(id)}
        """;

        using var connection = _connectionFactory.CreateConnection();
        return connection.QueryFirstOrDefault<Lang>(query, new { id });
    }

    public IEnumerable<Lang> GetMany(params IEnumerable<int> ids)
    {
        const string query =
        $"""
        SELECT * 
        FROM {nameof(Lang)}
        WHERE {nameof(Lang.Id)} IN @{nameof(ids)}
        """;

        if (!ids.Any())
        {
            return Enumerable.Empty<Lang>();
        }

        using var connection = _connectionFactory.CreateConnection();
        return connection.Query<Lang>(query, new { ids });
    }

    public bool Upsert(Lang entity)
    {
        const string query =
        $"""
        INSERT INTO {nameof(Lang)} ({nameof(Lang.Type)}, {nameof(Lang.Language)}, {nameof(Lang.Name)}, {nameof(Lang.Version)})
        VALUES (@{nameof(Lang.Type)}, @{nameof(Lang.Language)}, @{nameof(Lang.Name)}, @{nameof(Lang.Version)})
        ON CONFLICT({nameof(Lang.Type)}, {nameof(Lang.Language)}, {nameof(Lang.Name)})
        DO UPDATE SET
            {nameof(Lang.Version)} = excluded.{nameof(Lang.Version)},
            {nameof(Lang.IsNew)} = excluded.{nameof(Lang.IsNew)}
        """;

        lock (SQLiteDbConnectionFactory.WriteLock)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Execute(query, entity) > 0;
        }
    }

    public int UpsertMany(params IEnumerable<Lang> entities)
    {
        const string query =
        $"""
        INSERT INTO {nameof(Lang)} ({nameof(Lang.Type)}, {nameof(Lang.Language)}, {nameof(Lang.Name)}, {nameof(Lang.Version)})
        VALUES (@{nameof(Lang.Type)}, @{nameof(Lang.Language)}, @{nameof(Lang.Name)}, @{nameof(Lang.Version)})
        ON CONFLICT({nameof(Lang.Type)}, {nameof(Lang.Language)}, {nameof(Lang.Name)})
        DO UPDATE SET
            {nameof(Lang.Version)} = excluded.{nameof(Lang.Version)},
            {nameof(Lang.IsNew)} = excluded.{nameof(Lang.IsNew)}
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

    public bool Delete(int id)
    {
        const string query =
        $"""
        DELETE FROM {nameof(Lang)}
        WHERE {nameof(Lang.Id)} = @{nameof(id)}
        """;

        lock (SQLiteDbConnectionFactory.WriteLock)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Execute(query, new { id }) > 0;
        }
    }

    public int DeleteMany(params IEnumerable<int> ids)
    {
        const string query =
        $"""
        DELETE FROM {nameof(Lang)}
        WHERE {nameof(Lang.Id)} IN @{nameof(ids)}
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

    public IEnumerable<Lang> GetManyByIdentifier(LangsIdentifier identifier)
    {
        const string query =
        $"""
        SELECT * 
        FROM {nameof(Lang)}
        WHERE {nameof(Lang.Type)} = @{nameof(LangsIdentifier.Type)}
        AND {nameof(Lang.Language)} = @{nameof(LangsIdentifier.Language)}
        """;

        using var connection = _connectionFactory.CreateConnection();
        return connection.Query<Lang>(query, new { identifier.Type, identifier.Language });
    }

    public Lang? GetByIdentifierAndName(LangsIdentifier identifier, string name)
    {
        const string query =
        $"""
        SELECT * 
        FROM {nameof(Lang)}
        WHERE {nameof(Lang.Type)} = @{nameof(LangsIdentifier.Type)}
        AND {nameof(Lang.Language)} = @{nameof(LangsIdentifier.Language)}
        AND {nameof(Lang.Name)} = @{nameof(name)}
        """;

        using var connection = _connectionFactory.CreateConnection();
        return connection.QueryFirstOrDefault<Lang>(query, new { identifier.Type, identifier.Language, name });
    }

    public IEnumerable<Lang> SearchByIdentifierAndName(LangsIdentifier identifier, string name, int limit = -1, int offset = 0)
    {
        const string query =
        $"""
        SELECT * 
        FROM {nameof(Lang)}
        WHERE {nameof(Lang.Type)} = @{nameof(LangsIdentifier.Type)}
        AND {nameof(Lang.Language)} = @{nameof(LangsIdentifier.Language)}
        AND (@name = '' OR {nameof(Lang.Name)} LIKE '%' || @{nameof(name)} || '%')
        ORDER BY {nameof(Lang.Name)}
        LIMIT @{nameof(limit)} OFFSET @{nameof(offset)}
        """;

        using var connection = _connectionFactory.CreateConnection();
        return connection.Query<Lang>(query, new { identifier.Type, identifier.Language, name, limit, offset });
    }
}
