using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Cytrusaurus.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Cytrusaurus dependencies to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCytrusaurus(this IServiceCollection services)
    {
        services.AddSingleton<ICytrusWatcher, CytrusWatcher>();
        services.AddSingleton<ICytrusManifestFetcher, CytrusManifestFetcher>();

        return services;
    }
}
