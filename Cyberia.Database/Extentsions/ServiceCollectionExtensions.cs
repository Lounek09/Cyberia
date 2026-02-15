using Cyberia.Database.Migrations;
using Cyberia.Database.Repositories;
using Cyberia.Database.TypeHandlers;

using Dapper;

using Microsoft.Extensions.DependencyInjection;

using System.Data.SQLite;

namespace Cyberia.Database.Extentsions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds the database dependencies to the service collection.
        /// </summary>
        /// <param name="connectionString">The connection string to the SQLite database.</param>
        /// <returns>The updated service collection.</returns>
        public IServiceCollection AddDatabase(string connectionString)
        {
            SqlMapper.AddTypeHandler(new DateTimeHandler());

            services.AddSingleton<IDbConnectionFactory<SQLiteConnection>, SQLiteDbConnectionFactory>(
                _ => new SQLiteDbConnectionFactory(connectionString));
            services.AddSingleton<IMigrationManager, MigrationManager>();

            var repositories = typeof(IDatabaseRepository).Assembly.GetTypes()
                .Where(x => x.IsAssignableTo(typeof(IDatabaseRepository)) && !x.IsAbstract);

            foreach (var repository in repositories)
            {
                services.AddSingleton(repository);
            }

            return services;
        }
    }
}
