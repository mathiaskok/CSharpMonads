using System;
using System.Collections.Generic;

namespace Monads.Comparers
{
  public class DelegateEqualityComparer<T> : EqualityComparer<T>
  {
    private readonly Func<T, T, bool> EqualsFunc;
    private readonly Func<T, int> HashCodeFunc;

    public DelegateEqualityComparer(Func<T, T, bool> equalsFunc, Func<T, int> hashCodeFunc)
    {
      EqualsFunc = equalsFunc;
      HashCodeFunc = hashCodeFunc;
    }

    public override bool Equals(T x, T y) => EqualsFunc(x, y);

    public override int GetHashCode(T obj) => HashCodeFunc(obj);
  }
}
