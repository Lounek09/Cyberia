namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IDictionary{TKey, TValue}"/>.
/// </summary>
public static class IDictionaryExtensions
{
    extension<TKey, TValue>(IDictionary<TKey, TValue> source)
    {
        /// <summary>
        /// Removes the first key-value pair in the source dictionary that has the specified value.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        /// <returns><see langword="true"/> if at least one element was removed; otherwise, <see langword="false"/>.</returns>
        public bool RemoveByValue(TValue value)
        {
            foreach (var pair in source)
            {
                if (Equals(value, pair.Value))
                {
                    return source.Remove(pair.Key);
                }
            }

            return false;
        }

        /// <summary>
        /// Removes all key-value pairs in the source dictionary that has the specified value.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        /// <returns><see langword="true"/> if at least one element was removed; otherwise, <see langword="false"/>.</returns>
        public bool RemoveAllByValue(TValue value)
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
}
