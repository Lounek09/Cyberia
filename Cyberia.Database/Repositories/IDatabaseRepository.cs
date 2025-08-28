using Cyberia.Database.Models;

namespace Cyberia.Database.Repositories;

public interface IDatabaseRepository
{

}

/// <summary>
/// Represents a generic repository for database entities.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the entity's ID.</typeparam>
public interface IDatabaseRepository<T, TId> : IDatabaseRepository
    where T : IDatabaseEntity
    where TId : notnull
{
    /// <summary>
    /// Gets a <typeparamref name="T"/> by their ID.
    /// </summary>
    /// <param name="id">The ID of the <typeparamref name="T"/>.</param>
    /// <returns>The <typeparamref name="T"/> if found; otherwise, <see langword="null"/>.</returns>
    Task<T?> GetAsync(TId id);

    /// <summary>
    /// Gets many <typeparamref name="T"/> by their IDs.
    /// </summary>
    /// <param name="ids">The IDs of the <typeparamref name="T"/>.</param>
    /// <returns>The <see cref="IEnumerable{T}"/> of <typeparamref name="T"/> found; otherwise, an empty collection.</returns>
    Task<IEnumerable<T>> GetManyAsync(params IReadOnlyCollection<TId> ids);

    /// <summary>
    /// Creates or updates a <typeparamref name="T"/>.
    /// </summary>
    /// <param name="user">The <typeparamref name="T"/> to create or update.</param>
    /// <returns><see langword="true"/> if the <typeparamref name="T"/> was created or updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpsertAsync(T user);

    /// <summary>
    /// Deletes a <typeparamref name="T"/> by their ID.
    /// </summary>
    /// <param name="id">The ID of the <typeparamref name="T"/>.</param>
    /// <returns><see langword="true"/> if the <typeparamref name="T"/> was deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(TId id);
}
