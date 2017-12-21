using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monads.Comparison
{
  public class MultiEqualityComparer<T> : EqualityComparer<T>
  {
    private readonly IReadOnlyCollection<IEqualityComparer<T>> Comparers;

    public MultiEqualityComparer(IReadOnlyCollection<IEqualityComparer<T>> comparers)
    {
      Comparers = comparers;
    }

    public override bool Equals(T x, T y) =>
      Comparers.All(c => c.Equals(x, y));

    public override int GetHashCode(T obj) =>
      Comparers
        .Select(c => c.GetHashCode(obj))
        .SequenceHashCode();
  }
}
