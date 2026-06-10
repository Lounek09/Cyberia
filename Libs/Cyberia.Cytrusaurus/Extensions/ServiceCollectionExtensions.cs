using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Cytrusaurus.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds the Cytrusaurus dependencies to the service collection.
        /// </summary>
        /// <returns>The updated service collection.</returns>
        public IServiceCollection AddCytrusaurus()
        {
            services.AddSingleton<ICytrusWatcher, CytrusWatcher>();
            services.AddSingleton<ICytrusManifestFetcher, CytrusManifestFetcher>();

            return services;
        }
    }
}
