using System.Collections;
using System.Collections.Generic;

namespace Monads.Collections.ReadOnly
{
  public class ReadOnlyListAdapter<T> : IReadOnlyList<T>
  {
    private IList<T> Adaptee { get; }

    public T this[int index] => Adaptee[index];

    public int Count => Adaptee.Count;

    public ReadOnlyListAdapter(IList<T> adaptee)
    {
      Adaptee = adaptee;
    }

    public IEnumerator<T> GetEnumerator() => Adaptee.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}
