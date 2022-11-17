namespace Salamandra.Utils
{
    public static class ExtendDictionary
    {
        public static bool RemoveByValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value, bool firstOnly = false)
        {
            bool success = false;

            foreach (KeyValuePair<TKey, TValue> item in source.Where(x => x.Value!.Equals(value)).ToList())
            {
                success = source.Remove(item.Key);

                if (firstOnly)
                    break;
            }

            return success;
        }
    }
}
