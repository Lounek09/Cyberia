using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Api.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds the Dofus API dependencies to the service collection.
        /// </summary>
        /// <returns>The updated service collection.</returns>
        public IServiceCollection AddDofusApi(DofusApiConfig config)
        {
            DofusApi.Config = config;
            DofusApi.Datacenter = new();
            DofusApi.HttpClient = new();

            services.AddSingleton(config);
            services.AddSingleton(DofusApi.Datacenter);

            return services;
        }
    }
}
