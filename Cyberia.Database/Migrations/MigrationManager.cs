﻿using Dapper;

using System.Collections.ObjectModel;
using System.Reflection;

namespace Cyberia.Database.Migrations;

/// <summary>
/// Represents a pending migration that has not yet been applied to the database.
/// </summary>
internal readonly record struct PendingMigration(string Name, int Order, string Script);

/// <summary>
/// Represents a service that manages database migrations.
/// </summary>
internal interface IMigrationManager
{
    /// <summary>
    /// Ensures that the migration table exists in the database.
    /// </summary>
    Task EnsureMigrationTableExistsAsync();

    /// <summary>
    /// Gets the list of migrations that have already been applied to the database.
    /// </summary>
    /// <returns>The list of applied migrations.</returns>
    Task<IEnumerable<Migration>> GetAppliedMigrationsAsync();

    /// <summary>
    /// Gets the list of available migrations that can be applied to the database.
    /// </summary>
    /// <returns>The list of pending migrations.</returns>
    Task<ReadOnlyCollection<PendingMigration>> GetAvailableMigrationsAsync();

    /// <summary>
    /// Applies all pending migrations to the database.
    /// </summary>
    Task ApplyMigrationsAsync();
}

internal sealed class MigrationManager : IMigrationManager
{
    private readonly IDbConnectionFactory _connectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationManager"/> class.
    /// </summary>
    /// <param name="connectionFactory">The connection factory.</param>
    public MigrationManager(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task EnsureMigrationTableExistsAsync()
    {
        const string query =
        $"""
        CREATE TABLE IF NOT EXISTS {nameof(Migration)} (
            {nameof(Migration.Id)} INTEGER PRIMARY KEY AUTOINCREMENT,
            {nameof(Migration.Name)} TEXT NOT NULL UNIQUE,
            {nameof(Migration.AppliedAt)} DATETIME DEFAULT CURRENT_TIMESTAMP
        );
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(query);
    }

    public async Task<IEnumerable<Migration>> GetAppliedMigrationsAsync()
    {
        const string query =
        $"""
        SELECT * FROM {nameof(Migration)};
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<Migration>(query);
    }

    public async Task<ReadOnlyCollection<PendingMigration>> GetAvailableMigrationsAsync()
    {
        const string sqlExtension = ".sql";
        const string expectedFormat = "Expected format is '{order}-{name}.sql' where '{order}' is the migration sequence number.";

        var appliedMigrations = await GetAppliedMigrationsAsync();
        var appliedMigrationsLookup = appliedMigrations
            .ToDictionary(x => x.Name, x => x)
            .GetAlternateLookup<ReadOnlySpan<char>>();

        var assembly = typeof(MigrationManager).Assembly;
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

            var lastDotIndex = resourceNameWithoutExtension.LastIndexOf('.');
            if (lastDotIndex == -1)
            {
                throw new InvalidOperationException($"Invalid resource name format: {resourceName}. {expectedFormat}.");
            }

            var name = resourceNameWithoutExtension[(lastDotIndex + 1)..];
            if (name.IsEmpty || name.IsWhiteSpace())
            {
                throw new InvalidOperationException($"Invalid migration name extracted from resource: {resourceName}.");
            }

            var dashIndex = name.IndexOf('-');
            if (dashIndex == -1)
            {
                throw new InvalidOperationException($"Invalid resource name format: {resourceName}. {expectedFormat}");
            }

            if (!int.TryParse(name[..dashIndex], out var order))
            {
                throw new InvalidOperationException($"Invalid migration order extracted from resource: {resourceName}.");
            }

            if (appliedMigrationsLookup.ContainsKey(name))
            {
                Log.Debug("Migration {Name} already applied. Skipping.", name.ToString());
                continue;
            }

            var script = GetResourceContent(assembly, resourceName);
            if (string.IsNullOrWhiteSpace(script))
            {
                throw new InvalidOperationException($"Migration script for '{name}' is empty.");
            }

            pendingMigrations.Add(new PendingMigration(name.ToString(), order, script));
        }

        pendingMigrations.Sort((x, y) => x.Order.CompareTo(y.Order));

        return pendingMigrations.AsReadOnly();
    }

    public async Task ApplyMigrationsAsync()
    {
        const string insertQuery =
        $"""
        INSERT INTO {nameof(Migration)} ({nameof(Migration.Name)})
        VALUES (@Name);
        """;

        using var connection = await _connectionFactory.CreateConnectionAsync();

        var availableMigrations = await GetAvailableMigrationsAsync();
        foreach (var pendingMigration in availableMigrations)
        {
            using var transaction = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(pendingMigration.Script);
                await connection.ExecuteAsync(insertQuery, new { pendingMigration.Name });

                transaction.Commit();

                Log.Information("Applied migration {Name}.", pendingMigration.Name, pendingMigration.Order);
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
