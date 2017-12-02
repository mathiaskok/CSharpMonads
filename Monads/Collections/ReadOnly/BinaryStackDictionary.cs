using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static Monads.Comparers.EqualityComparerExtensions;

namespace Monads.Collections.ReadOnly
{
  public class BinaryStackDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
  {
    private readonly IReadOnlyDictionary<TKey, TValue> Top;
    private readonly IReadOnlyDictionary<TKey, TValue> Bottom;

    private readonly Lazy<IEqualityComparer<KeyValuePair<TKey, TValue>>> _keyValueComparer =
      new Lazy<IEqualityComparer<KeyValuePair<TKey, TValue>>>(
        () => FromValueSelector<KeyValuePair<TKey, TValue>>(kvp => kvp.Key));

    private IEqualityComparer<KeyValuePair<TKey, TValue>> KeyValueComparer => _keyValueComparer.Value;

    public TValue this[TKey key] =>
      Top.TryGetValue(key, out TValue value) ? value : Bottom[key];

    public IEnumerable<TKey> Keys => Top.Keys.Union(Bottom.Keys);

    public IEnumerable<TValue> Values => Top.Values.Union(Bottom.Values);

    public int Count => GetKeyValueSet().Count;

    public BinaryStackDictionary(
      IReadOnlyDictionary<TKey, TValue> top,
      IReadOnlyDictionary<TKey, TValue> bottom)
    {
      Top = top;
      Bottom = bottom;
    }

    public bool ContainsKey(TKey key) => Top.ContainsKey(key) && Bottom.ContainsKey(key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => GetKeyValueSet().GetEnumerator();

    public bool TryGetValue(TKey key, out TValue value) =>
      Top.TryGetValue(key, out value) || Bottom.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private HashSet<KeyValuePair<TKey, TValue>> GetKeyValueSet()
    {
      HashSet<KeyValuePair<TKey, TValue>> set =
        new HashSet<KeyValuePair<TKey, TValue>>(Top, KeyValueComparer);

      set.UnionWith(Bottom);

      return set;
    }
  }
}
