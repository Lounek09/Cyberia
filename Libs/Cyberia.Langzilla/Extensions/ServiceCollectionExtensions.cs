using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Langzilla.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds the Langzilla dependencies to the service collection.
        /// </summary>
        /// <returns>The updated service collection.</returns>
        public IServiceCollection AddLangzilla()
        {
            services.AddSingleton<ILangsWatcher, LangsWatcher>();
            services.AddSingleton<ILangsParser, LangsParser>();

            return services;
        }
    }
}
