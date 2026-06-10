using Dapper;

using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Reflection;

namespace Cyberia.Database.Migrations;

/// <summary>
/// Represents a service that manages database migrations.
/// </summary>
internal interface IMigrationManager
{
    /// <summary>
    /// Ensures that the migration table exists in the database.
    /// </summary>
    void EnsureDatabaseInitialized();

    /// <summary>
    /// Gets the list of migrations that have already been applied to the database.
    /// </summary>
    /// <returns>The list of applied migrations.</returns>
    IEnumerable<Migration> GetAppliedMigrations();

    /// <summary>
    /// Gets the list of available migrations that can be applied to the database.
    /// </summary>
    /// <returns>The list of pending migrations.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    ReadOnlyCollection<PendingMigration> GetAvailableMigrations();

    /// <summary>
    /// Applies all pending migrations to the database.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    void ApplyMigrations();
}

internal sealed class MigrationManager : IMigrationManager
{
    private readonly IDbConnectionFactory<SQLiteConnection> _connectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationManager"/> class.
    /// </summary>
    /// <param name="connectionFactory">The connection factory.</param>
    public MigrationManager(IDbConnectionFactory<SQLiteConnection> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void EnsureDatabaseInitialized()
    {
        const string query =
        $"""
        CREATE TABLE IF NOT EXISTS {nameof(Migration)} (
            {nameof(Migration.Id)} INTEGER PRIMARY KEY AUTOINCREMENT,
            {nameof(Migration.Name)} TEXT NOT NULL UNIQUE,
            {nameof(Migration.AppliedAt)} DATETIME DEFAULT CURRENT_TIMESTAMP
        );
        """;

        using var connection = _connectionFactory.CreateConnection();
        connection.Execute(query);
    }

    public IEnumerable<Migration> GetAppliedMigrations()
    {
        const string query =
        $"""
        SELECT *
        FROM {nameof(Migration)};
        """;

        using var connection = _connectionFactory.CreateConnection();
        return connection.Query<Migration>(query);
    }

    public ReadOnlyCollection<PendingMigration> GetAvailableMigrations()
    {
        const string sqlExtension = ".sql";
        const string expectedFormat = "Expected format is '{order}-{name}.sql' where '{order}' is the date in format 'yyyyMMddHHmm'.";

        var appliedMigrations = GetAppliedMigrations();
        var appliedMigrationsLookup = appliedMigrations
            .Select(x => x.Name)
            .ToHashSet(StringComparer.Ordinal)
            .GetAlternateLookup<ReadOnlySpan<char>>();

        var assembly = typeof(IMigrationManager).Assembly;
        var resourceNames = assembly.GetManifestResourceNames();

        if (resourceNames.Length == 0)
        {
            Log.Warning("No resources found in assembly '{AssemblyName}'.", assembly.FullName);
            return ReadOnlyCollection<PendingMigration>.Empty;
        }

        List<PendingMigration> pendingMigrations = [];
        foreach (var resourceName in resourceNames)
        {
            if (!resourceName.EndsWith(sqlExtension, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var resourceNameWithoutExtension = resourceName.AsSpan(..^sqlExtension.Length);

            // Embedded resource name includes namespace that we don't want
            var lastDotIndex = resourceNameWithoutExtension.LastIndexOf('.');
            if (lastDotIndex <= 0)
            {
                throw new InvalidOperationException($"Invalid resource name format: {resourceName}. {expectedFormat}");
            }

            var name = resourceNameWithoutExtension[(lastDotIndex + 1)..];
            if (name.IsWhiteSpace())
            {
                throw new InvalidOperationException($"Invalid migration name extracted from resource: {resourceName}. {expectedFormat}");
            }

            var dashIndex = name.IndexOf('-');
            if (dashIndex == -1)
            {
                throw new InvalidOperationException($"Invalid resource name format: {resourceName}. {expectedFormat}");
            }

            if (!long.TryParse(name[..dashIndex], out var order))
            {
                throw new InvalidOperationException($"Invalid migration order extracted from resource: {resourceName}. {expectedFormat}");
            }

            if (appliedMigrationsLookup.Contains(name))
            {
                Log.Debug("Migration {Name} already applied. Skipping.", name.ToString());
                continue;
            }

            var script = GetResourceContent(assembly, resourceName);
            if (string.IsNullOrWhiteSpace(script))
            {
                throw new InvalidOperationException($"Migration script '{name}' is empty.");
            }

            pendingMigrations.Add(new PendingMigration(name.ToString(), order, script));
        }

        pendingMigrations.Sort((x, y) => x.Order.CompareTo(y.Order));

        return pendingMigrations.AsReadOnly();
    }

    public void ApplyMigrations()
    {
        const string query =
        $"""
        INSERT INTO {nameof(Migration)} ({nameof(Migration.Name)})
        VALUES (@Name);
        """;

        using var connection = _connectionFactory.CreateConnection();

        var availableMigrations = GetAvailableMigrations();
        foreach (var pendingMigration in availableMigrations)
        {
            using var transaction = connection.BeginTransaction();

            try
            {
                connection.Execute(pendingMigration.Script);
                connection.Execute(query, new { pendingMigration.Name });

                transaction.Commit();

                Log.Information("Applied migration {Name}.", pendingMigration.Name);
            }
            catch
            {
                Log.Error("Failed to apply migration {Name}.", pendingMigration.Name);

                transaction.Rollback();

                throw;
            }
        }
    }

    /// <summary>
    /// Gets the content of a resource embedded in the assembly.
    /// </summary>
    /// <param name="assembly">The assembly containing the resource.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns>The content of the resource as a string.</returns>
    private static string GetResourceContent(Assembly assembly, string resourceName)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Resource '{resourceName}' not found in assembly '{assembly.FullName}'.");
        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
