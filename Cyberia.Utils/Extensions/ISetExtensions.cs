using System.Collections.ObjectModel;

namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ISet{T}"/>.
/// </summary>
public static class ISetExtensions
{
    /// <summary>
    /// Returns a read-only <see cref="ReadOnlySet{T}"/> wrapper for the current collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="set">The collection to wrap.</param>
    /// <returns>An object that act as a read-only wrapper around the current <see cref="ISet{T}"/></returns>
    public static ReadOnlySet<T> AsReadOnly<T>(this ISet<T> set)
    {
        return new ReadOnlySet<T>(set);
    }
}
