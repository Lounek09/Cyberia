namespace Cyberia.Utils;

public static class ExtendDictionary
{
    public static bool RemoveByValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value, bool firstOnly = false)
    {
        var success = false;

        HashSet<TKey> keysToRemove = [];
        foreach (var item in source)
        {
            if (item.Value is not null && item.Value.Equals(value))
            {
                keysToRemove.Add(item.Key);
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
