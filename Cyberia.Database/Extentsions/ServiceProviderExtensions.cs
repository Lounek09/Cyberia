using Cyberia.Database.Migrations;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Database.Extentsions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceProvider"/> interface.
/// </summary>
public static class ServiceProviderExtensions
{
    extension(IServiceProvider provider)
    {
        /// <summary>
        /// Applies all pending database migrations.
        /// </summary>
        /// <returns>The service provider.</returns>
        public async Task ApplyDatabaseMigrationsAsync()
        {
            var migrationManager = provider.GetRequiredService<IMigrationManager>();

            await migrationManager.EnsureMigrationTableExistsAsync();
            await migrationManager.ApplyMigrationsAsync();
        }
    }
}
