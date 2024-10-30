using Cyberia.Database.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Database.Extentsions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceProvider"/> interface.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Creates the tables of the database.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <returns>The service provider.</returns>
    public static async Task<IServiceProvider> CreateDatabaseTablesAsync(this IServiceProvider provider)
    {
        var repositories = provider.GetServices<IDatabaseRepository>();
        foreach (var repository in repositories)
        {
            await repository.CreateTableAsync();
        }

        return provider;
    }
}
