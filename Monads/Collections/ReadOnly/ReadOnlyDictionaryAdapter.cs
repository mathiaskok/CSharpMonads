using System.Collections;
using System.Collections.Generic;

namespace Monads.Collections.ReadOnly
{
  public class ReadOnlyDictionaryAdapter<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
  {
    private readonly IDictionary<TKey, TValue> Adaptee;

    public TValue this[TKey key] => Adaptee[key];

    public IEnumerable<TKey> Keys => Adaptee.Keys;

    public IEnumerable<TValue> Values => Adaptee.Values;

    public int Count => Adaptee.Count;

    public ReadOnlyDictionaryAdapter(IDictionary<TKey, TValue> adaptee)
    {
      Adaptee = adaptee;
    }

    public bool ContainsKey(TKey key) => Adaptee.ContainsKey(key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Adaptee.GetEnumerator();

    public bool TryGetValue(TKey key, out TValue value) => Adaptee.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}
