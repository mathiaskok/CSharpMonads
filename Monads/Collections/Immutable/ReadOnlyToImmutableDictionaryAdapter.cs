using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Monads.Comparison;

namespace Monads.Collections.Immutable
{
  public class ReadOnlyToImmutableDictionaryAdapter<K, V> : IImmutableDictionary<K, V>
  {
    private IReadOnlyDictionary<K, V> Adaptee;
    private IImmutableDictionary<K, V> Immutable;

    public V this[K key]
    {
      get
      {
        if (Immutable.TryGetValue(key, out V value))
          return value;
        else
          return Adaptee[key];
      }
    }

    public IEnumerable<K> Keys => this.Select(kvp => kvp.Key);

    public IEnumerable<V> Values => this.Select(kvp => kvp.Value);

    public int Count => ((IEnumerable<KeyValuePair<K, V>>)this).Count();

    private ReadOnlyToImmutableDictionaryAdapter(
      IReadOnlyDictionary<K, V> adaptee,
      IImmutableDictionary<K, V> immutable)
    {
      Adaptee = adaptee;
      Immutable = immutable;
    }

    public IImmutableDictionary<K, V> Add(K key, V value) =>
      CreateNew(Immutable.Add(key, value));

    public IImmutableDictionary<K, V> AddRange(IEnumerable<KeyValuePair<K, V>> pairs) =>
      CreateNew(Immutable.AddRange(pairs));

    public IImmutableDictionary<K, V> Clear() =>
      CreateNew(Immutable.Clear());

    public bool Contains(KeyValuePair<K, V> pair)
    {
      if (Immutable.TryGetValue(pair.Key, out V value))
        return Equals(pair.Value, value);
      else if (Adaptee.TryGetValue(pair.Key, out value))
        return Equals(pair.Value, value);
      else
        return false;
    }

    public bool ContainsKey(K key) =>
      Immutable.ContainsKey(key) || Adaptee.ContainsKey(key);

    public IImmutableDictionary<K, V> Remove(K key) =>
      CreateNew(Immutable.Remove(key));

    public IImmutableDictionary<K, V> RemoveRange(IEnumerable<K> keys) =>
      CreateNew(Immutable.RemoveRange(keys));

    public IImmutableDictionary<K, V> SetItem(K key, V value) =>
      CreateNew(Immutable.SetItem(key, value));

    public IImmutableDictionary<K, V> SetItems(IEnumerable<KeyValuePair<K, V>> items) =>
      CreateNew(Immutable.SetItems(items));

    public bool TryGetKey(K equalKey, out K actualKey)
    {
      if (Immutable.TryGetKey(equalKey, out actualKey))
        return true;
      else if (Adaptee.ContainsKey(equalKey))
      {
        actualKey = equalKey;
        return true;
      }
      else
        return false;
    }

    public bool TryGetValue(K key, out V value) =>
      Adaptee.TryGetValue(key, out value) ||
      Immutable.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
    {
      HashSet<KeyValuePair<K, V>> dupSet = new HashSet<KeyValuePair<K, V>>(
        EqualityComparerExtensions.FromValueSelector<KeyValuePair<K, V>>(kvp => kvp.Key));

      IEnumerable<KeyValuePair<K, V>> elements = Immutable
        .Concat(Adaptee);

      foreach (KeyValuePair<K, V> kvp in elements)
      {
        if (dupSet.Add(kvp))
          yield return kvp;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private IImmutableDictionary<K, V> CreateNew(IImmutableDictionary<K, V> newImmutable)
    {
      return new ReadOnlyToImmutableDictionaryAdapter<K, V>(
        Adaptee,
        newImmutable);
    }
  }
}
