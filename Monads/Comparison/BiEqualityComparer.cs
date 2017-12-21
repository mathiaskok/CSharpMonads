using System.Collections.Generic;

namespace Monads.Comparison
{
  public class BiEqualityComparer<T> : EqualityComparer<T>
  {
    private readonly IEqualityComparer<T> Comparer1;
    private readonly IEqualityComparer<T> Comparer2;

    public BiEqualityComparer(IEqualityComparer<T> comparer1, IEqualityComparer<T> comparer2)
    {
      Comparer1 = comparer1;
      Comparer2 = comparer2;
    }

    public override bool Equals(T x, T y) =>
      Comparer1.Equals(x, y) &&
      Comparer2.Equals(x, y);

    public override int GetHashCode(T obj) =>
      HashCodeExtensions.BaseHashCode
        .HashWith(Comparer1.GetHashCode(obj))
        .HashWith(Comparer2.GetHashCode(obj));
  }
}
