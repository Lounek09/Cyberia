using Cyberia.Database.Models;
using Cyberia.Langzilla.Primitives;

using Dapper;

using System.Data.SQLite;

using static Dapper.SqlMapper;

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

    public async Task<Lang?> GetAsync(int id)
    {
        const string query =
        $"""
        SELECT * FROM {nameof(Lang)}
        WHERE {nameof(Lang.Id)} = @id
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<Lang>(query, new { id });
    }

    public async Task<IEnumerable<Lang>> GetManyAsync(params IEnumerable<int> ids)
    {
        const string query =
        $"""
        SELECT * 
        FROM {nameof(Lang)}
        WHERE {nameof(Lang.Id)} IN @ids
        """;

        if (!ids.Any())
        {
            return Enumerable.Empty<Lang>();
        }

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<Lang>(query, new { ids });
    }

    public async Task<bool> UpsertAsync(Lang entity)
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

        await SQLiteDbConnectionFactory.WriteLock.WaitAsync();
        try
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.ExecuteAsync(query, entity) > 0;
        }
        finally
        {
            SQLiteDbConnectionFactory.WriteLock.Release();
        }
    }

    public async Task<int> UpsertManyAsync(params IEnumerable<Lang> entities)
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

        await SQLiteDbConnectionFactory.WriteLock.WaitAsync();
        try
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var transaction = await connection.BeginTransactionAsync();

            var affectedRows = await connection.ExecuteAsync(query, entities, transaction);

            await transaction.CommitAsync();

            return affectedRows;
        }
        finally
        {
            SQLiteDbConnectionFactory.WriteLock.Release();
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string query =
        $"""
        DELETE FROM {nameof(Lang)}
        WHERE {nameof(Lang.Id)} = @id
        """;

        await SQLiteDbConnectionFactory.WriteLock.WaitAsync();
        try
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.ExecuteAsync(query, new { id }) > 0;
        }
        finally
        {
            SQLiteDbConnectionFactory.WriteLock.Release();
        }
    }

    public async Task<int> DeleteManyAsync(params IEnumerable<int> ids)
    {
        const string query =
        $"""
        DELETE FROM {nameof(Lang)}
        WHERE {nameof(Lang.Id)} IN @Ids
        """;

        if (!ids.Any())
        {
            return 0;
        }

        await SQLiteDbConnectionFactory.WriteLock.WaitAsync();
        try
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.ExecuteAsync(query, new { Ids = ids });
        }
        finally
        {
            SQLiteDbConnectionFactory.WriteLock.Release();
        }
    }

    public async Task<IEnumerable<Lang>> GetManyByIdentifierAsync(LangsIdentifier identifier)
    {
        const string query =
        $"""
        SELECT * 
        FROM {nameof(Lang)}
        WHERE {nameof(Lang.Type)} = @{nameof(LangsIdentifier.Type)}
        AND {nameof(Lang.Language)} = @{nameof(LangsIdentifier.Language)}
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<Lang>(query, new { identifier.Type, identifier.Language });
    }

    public async Task<Lang?> GetByIdentifierAndNameAsync(LangsIdentifier identifier, string name)
    {
        const string query =
        $"""
        SELECT * 
        FROM {nameof(Lang)}
        WHERE {nameof(Lang.Type)} = @{nameof(LangsIdentifier.Type)}
        AND {nameof(Lang.Language)} = @{nameof(LangsIdentifier.Language)}
        AND {nameof(Lang.Name)} = @name
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<Lang>(query, new { identifier.Type, identifier.Language, name });
    }

    public async Task<IEnumerable<Lang>> SearchByIdentifierAndNameAsync(LangsIdentifier identifier, string name, int limit = -1, int offset = 0)
    {
        const string query =
        $"""
        SELECT * 
        FROM {nameof(Lang)}
        WHERE {nameof(Lang.Type)} = @{nameof(LangsIdentifier.Type)}
        AND {nameof(Lang.Language)} = @{nameof(LangsIdentifier.Language)}
        AND (@name = '' OR {nameof(Lang.Name)} LIKE '%' || @name || '%')
        ORDER BY {nameof(Lang.Name)}
        LIMIT @limit OFFSET @offset
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<Lang>(query, new { identifier.Type, identifier.Language, name, limit, offset });
    }
}
