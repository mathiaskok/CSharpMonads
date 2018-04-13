using System;
using System.Collections.Generic;
using System.Linq;
using Monads.Collections.ReadOnly;

namespace Monads.Collections
{
  public static class DictionaryExtensions
  {
    public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
      this IDictionary<TKey, TValue> dict)
    {
      return new ReadOnlyDictionaryAdapter<TKey, TValue>(dict);
    }

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
      this IEnumerable<(TKey, TValue)> seq,
      IEqualityComparer<TKey> keyComparer)
    {
      return seq.ToDictionary(t => t.Item1, t => t.Item2, keyComparer);
    }

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
      this IEnumerable<(TKey, TValue)> seq)
    {
      return seq.ToDictionary(EqualityComparer<TKey>.Default);
    }

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
      this IEnumerable<Tuple<TKey, TValue>> seq,
      IEqualityComparer<TKey> keyComparer)
    {
      return seq.ToDictionary(t => t.Item1, t => t.Item2, keyComparer);
    }

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
      this IEnumerable<Tuple<TKey, TValue>> seq)
    {
      return seq.ToDictionary(EqualityComparer<TKey>.Default);
    }

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
      this IEnumerable<KeyValuePair<TKey, TValue>> seq,
      IEqualityComparer<TKey> keyComparer)
    {
      return seq.ToDictionary(t => t.Key, t => t.Value, keyComparer);
    }

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
      this IEnumerable<KeyValuePair<TKey, TValue>> seq)
    {
      return seq.ToDictionary(EqualityComparer<TKey>.Default);
    }

    public static void Deconstruct<K, V>(this KeyValuePair<K, V> kvp, out K key, out V value)
    {
      key = kvp.Key;
      value = kvp.Value;
    }
  }
}
