using System.Data;

namespace Cyberia.Database;

public interface IDbConnectionFactory<T>
    where T : IDbConnection
{
    Task<T> CreateConnectionAsync();
}
