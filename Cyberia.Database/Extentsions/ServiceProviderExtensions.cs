using Cyberia.Database.Migrations;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Database.Extentsions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceProvider"/> interface.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Applies all pending database migrations.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>The service provider.</returns>
    public static async Task ApplyDatabaseMigrationsAsync(this IServiceProvider serviceProvider)
    {
        var migrationManager = serviceProvider.GetRequiredService<IMigrationManager>();

        await migrationManager.EnsureMigrationTableExistsAsync();
        await migrationManager.ApplyMigrationsAsync();
    }
}
