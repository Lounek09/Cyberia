namespace Salamandra.Utils
{
    public static class ExtendDictionary
    {
        public static bool RemoveByValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value)
        {
            bool success = false;

            foreach (KeyValuePair<TKey, TValue> item in source.Where(x => x.Value!.Equals(value)).ToList())
                success = source.Remove(item.Key);

            return success;
        }
    }
}
