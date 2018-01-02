using System;
using System.Collections.Generic;

namespace Monads.Comparison
{
  public class CombinedDelegateComparer<T> : IEqualityComparer<T>, IComparer<T>
  {
    private readonly Func<T, T, bool> EqualsFunc;
    private readonly Func<T, int> HashCodeFunc;
    private readonly Func<T, T, int> CompareFunc;

    public CombinedDelegateComparer(
      Func<T, T, bool> equalsFunc,
      Func<T, int> hashCodeFunc,
      Func<T, T, int> compareFunc)
    {
      EqualsFunc = equalsFunc;
      HashCodeFunc = hashCodeFunc;
      CompareFunc = compareFunc;
    }

    public CombinedDelegateComparer(
      IEqualityComparer<T> equality,
      IComparer<T> compare)
      : this(equality.Equals, equality.GetHashCode, compare.Compare) { }

    public int Compare(T x, T y) => CompareFunc(x, y);

    public bool Equals(T x, T y) => EqualsFunc(x, y);

    public int GetHashCode(T obj) => HashCodeFunc(obj);
  }
}
