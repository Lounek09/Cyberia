namespace Cyberia.Database.Repositories;

public interface IDatabaseRepository
{
    Task<bool> CreateTableAsync();
}
