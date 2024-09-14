using Cyberia.Database.Repositories;

using Microsoft.Extensions.DependencyInjection;

using System.Data;
using System.Data.SQLite;

namespace Cyberia.Database;

public static class Database
{
    /// <summary>
    /// Adds the database dependencies to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="connectionString">The connection string to the SQLite database.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDbConnection>(x =>
        {
            SQLiteConnection connection = new(connectionString);
            connection.Open();
            return connection;
        });

        services.AddScoped<UserRepository>();

        return services;
    }

    /// <summary>
    /// Creates the tables for the database.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <returns>The service provider.</returns>
    public static async Task<IServiceProvider> CreateTablesAsync(this IServiceProvider provider)
    {
        var scope = provider.CreateScope();
        var repositories = scope.ServiceProvider.GetServices<IDatabaseRepository>();

        foreach (var repository in repositories)
        {
            await repository.CreateTableAsync();
        }

        return provider;
    }
}
