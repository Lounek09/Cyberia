namespace Cyberia.Utils;

public static class ExtendDictionary
{
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
