namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IDictionary{TKey, TValue}"/>.
/// </summary>
public static class IDictionaryExtensions
{
    /// <summary>
    /// Removes the first key-value pair in the source dictionary that has the specified value.
    /// </summary>
    /// <param name="source">The source dictionary.</param>
    /// <param name="value">The value to remove.</param>
    /// <returns><see langword="true"/> if at least one element was removed; otherwise, <see langword="false"/>.</returns>
    public static bool RemoveByValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value)
    {
        var keyToRemove = default(TKey);
        var found = false;

        foreach (var pair in source)
        {
            if (Equals(value, pair.Value))
            {
                keyToRemove = pair.Key;
                found = true;
                break;
            }
        }

        if (found && keyToRemove is not null)
        {
            source.Remove(keyToRemove);
        }

        return found;
    }

    /// <summary>
    /// Removes all key-value pairs in the source dictionary that has the specified value.
    /// </summary>
    /// <param name="source">The source dictionary.</param>
    /// <param name="value">The value to remove.</param>
    /// <returns><see langword="true"/> if at least one element was removed; otherwise, <see langword="false"/>.</returns>
    public static bool RemoveAllByValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value)
    {
        List<TKey> keysToRemove = [];

        foreach (var pair in source)
        {
            if (Equals(value, pair.Value))
            {
                keysToRemove.Add(pair.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            source.Remove(key);
        }

        return keysToRemove.Count > 0;
    }
}
