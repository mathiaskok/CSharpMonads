using System.Collections.Generic;
using System.Linq;

namespace Monads.Comparison
{
  public class MultiEqualityComparer<T> : EqualityComparer<T>
  {
    private readonly IReadOnlyCollection<IEqualityComparer<T>> Comparers;

    public MultiEqualityComparer(params IEqualityComparer<T>[] comparers)
      : this((IReadOnlyCollection<IEqualityComparer<T>>) comparers){}

    public MultiEqualityComparer(IReadOnlyCollection<IEqualityComparer<T>> comparers)
    {
      Comparers = comparers;
    }

    public override bool Equals(T x, T y) =>
      Comparers.All(c => c.Equals(x, y));

    public override int GetHashCode(T obj) =>
      Comparers.SequenceHashCode(c => c.GetHashCode(obj));
  }
}
