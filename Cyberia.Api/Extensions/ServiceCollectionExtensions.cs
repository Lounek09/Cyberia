using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Api.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Dofus API dependencies to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDofusApi(this IServiceCollection services, DofusApiConfig config)
    {
        DofusApi.Config = config;
        DofusApi.Datacenter = new();
        DofusApi.HttpClient = new();

        services.AddSingleton(config);
        services.AddSingleton(DofusApi.Datacenter);

        return services;
    }
}
