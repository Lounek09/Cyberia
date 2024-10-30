using Cyberia.Database.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Database.Extentsions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the database dependencies to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="connectionString">The connection string to the SQLite database.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new SQLiteDbConnectionFactory(connectionString));

        var assembly = typeof(IDatabaseRepository).Assembly;
        var repositoryTypes = assembly.GetTypes().Where(x =>
        {
            return x.IsClass && !x.IsAbstract && typeof(IDatabaseRepository).IsAssignableFrom(x);
        });

        foreach (var repositoryType in repositoryTypes)
        {
            services.AddSingleton(repositoryType);
            services.AddSingleton(typeof(IDatabaseRepository), repositoryType);
        }

        return services;
    }
}
