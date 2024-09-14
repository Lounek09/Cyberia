using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Cytrusaurus.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCytrusaurus(this IServiceCollection services)
    {
        services.AddSingleton<CytrusWatcher>();
        services.AddSingleton<CytrusManifestFetcher>();

        return services;
    }
}
