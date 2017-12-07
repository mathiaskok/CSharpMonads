using System.Collections;
using System.Collections.Generic;

namespace Monads.Collections.ReadOnly
{
  public class ReadOnlyCollectionAdpater<T> : IReadOnlyCollection<T>
  {
    private readonly ICollection<T> Adaptee;

    public int Count => Adaptee.Count;

    public ReadOnlyCollectionAdpater(ICollection<T> adaptee)
    {
      Adaptee = adaptee;
    }

    public IEnumerator<T> GetEnumerator() => Adaptee.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}
