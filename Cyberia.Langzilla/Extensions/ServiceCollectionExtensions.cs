using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Langzilla.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Langzilla dependencies to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddLangzilla(this IServiceCollection services)
    {
        services.AddSingleton<LangsWatcher>();

        return services;
    }
}
