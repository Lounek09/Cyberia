using Cyberia.Api.Data;
using Cyberia.Langzilla.Enums;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Api.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceProvider"/> interface.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Loads the data of the Dofus datacenter from the service provider.
    /// </summary>
    /// <param name="provider">The service provider to get the datacenter from.</param>
    /// <param name="type">The type of the lang data to load.</param>
    /// <returns>The service provider.</returns>
    public static IServiceProvider LoadDofusDatacenter(this IServiceProvider provider, LangType type)
    {
        var datacenter = provider.GetRequiredService<DofusDatacenter>();

        datacenter.Load(type);

        return provider;
    }
}
