namespace Cyberia.Utils;

/// <summary>
/// Provides extension methods for IDictionary.
/// </summary>
public static class ExtendDictionary
{
    /// <summary>
    /// Removes all (or the first) key-value pair(s) in the source dictionary that has the specified value.
    /// </summary>
    /// <param name="source">The source dictionary.</param>
    /// <param name="value">The value to remove.</param>
    /// <param name="firstOnly">If true, only the first key-value pair with the specified value is removed. Default is false.</param>
    /// <returns>True if at least one element was removed; otherwise, false.</returns>
    public static bool RemoveByValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value, bool firstOnly = false)
    {
        var success = false;

        List<TKey> keysToRemove = [];
        foreach (var pair in source)
        {
            if (Equals(value, pair.Value))
            {
                keysToRemove.Add(pair.Key);
                success = true;

                if (firstOnly)
                {
                    break;
                }
            }
        }

        foreach (var key in keysToRemove)
        {
            source.Remove(key);
        }

        return success;
    }
}
