namespace TouchGame
{
	public static class DictionaryExtensions
	{
        /// <summary>
        /// Removes all entries from the dictionary that match the given predicate.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary from which entries will be removed.</param>
        /// <param name="predicate">The predicate to determine which entries to remove.</param>
        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(dictionary);
            ArgumentNullException.ThrowIfNull(predicate);

            var keysToRemove = new List<TKey>();

            foreach (var kvp in dictionary)
            {
                if (predicate(kvp))
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                dictionary.Remove(key);
            }
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Action<TKey, TValue> action)
        {
            foreach (var item in dictionary)
            {
                action.Invoke(item.Key, item.Value);
            }
        }
    }
}

