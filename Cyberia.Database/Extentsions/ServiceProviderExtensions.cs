using Cyberia.Database.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Database.Extentsions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceProvider"/> interface.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Creates the tables for the database.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <returns>The service provider.</returns>
    public static async Task<IServiceProvider> CreateTablesAsync(this IServiceProvider provider)
    {
        var scope = provider.CreateScope();

        await scope.ServiceProvider.GetRequiredService<DiscordCachedUserRepository>().CreateTableAsync();

        return provider;
    }
}
