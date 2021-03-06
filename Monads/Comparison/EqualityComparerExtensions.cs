﻿using System;
using System.Collections.Generic;

namespace Monads.Comparison
{
  public static class EqualityComparerExtensions
  {
    public static IEqualityComparer<(T, U)> AsTupleComparer<T, U>(
      IEqualityComparer<T> t,
      IEqualityComparer<U> u)
    {
      return new DelegateEqualityComparer<(T, U)>(
        (x, y) =>
          t.Equals(x.Item1, y.Item1) &&
          u.Equals(x.Item2, y.Item2),
        (o) => HashCodeExtensions.BaseHashCode
          .HashWith(t.GetHashCode(o.Item1))
          .HashWith(u.GetHashCode(o.Item2)));
    }

    public static IEqualityComparer<(T, U, V)> AsTupleComparer<T, U, V>(
      IEqualityComparer<T> t,
      IEqualityComparer<U> u,
      IEqualityComparer<V> v)
    {
      return new DelegateEqualityComparer<(T, U, V)>(
        (x, y) =>
          t.Equals(x.Item1, y.Item1) &&
          u.Equals(x.Item2, y.Item2) &&
          v.Equals(x.Item3, y.Item3),
        (o) => HashCodeExtensions.BaseHashCode
          .HashWith(t.GetHashCode(o.Item1))
          .HashWith(u.GetHashCode(o.Item2))
          .HashWith(v.GetHashCode(o.Item3)));
    }

    public static IEqualityComparer<(T, U, V, X)> AsTupleComparer<T, U, V, X>(
      IEqualityComparer<T> t,
      IEqualityComparer<U> u,
      IEqualityComparer<V> v,
      IEqualityComparer<X> x)
    {
      return new DelegateEqualityComparer<(T, U, V, X)>(
        (v1, v2) =>
          t.Equals(v1.Item1, v2.Item1) &&
          u.Equals(v1.Item2, v2.Item2) &&
          v.Equals(v1.Item3, v2.Item3) &&
          x.Equals(v1.Item4, v2.Item4),
        (o) => HashCodeExtensions.BaseHashCode
          .HashWith(t.GetHashCode(o.Item1))
          .HashWith(u.GetHashCode(o.Item2))
          .HashWith(v.GetHashCode(o.Item3))
          .HashWith(x.GetHashCode(o.Item4)));
    }

    public static IEqualityComparer<T> FromValueSelector<T>(Func<T, object> valueSelector)
    {
      return new DelegateEqualityComparer<T>(
        (x, y) => Equals(valueSelector(x), valueSelector(y)),
        o => valueSelector(o).GetHashCode());
    }

    public static IEqualityComparer<T> FromValueSelector<T, U>(
      Func<T, U> valueSelector,
      IEqualityComparer<U> valueComparer)
    {
      return new DelegateEqualityComparer<T>(
        (x, y) => valueComparer.Equals(valueSelector(x), valueSelector(y)),
        o => valueComparer.GetHashCode(valueSelector(o)));
    }

    public static IEqualityComparer<T> CombineWith<T>(
      this IEqualityComparer<T> fst,
      IEqualityComparer<T> snd)
    {
      return new BiEqualityComparer<T>(fst, snd);
    }

    public static IEqualityComparer<T> FromComparers<T>(
      IReadOnlyCollection<IEqualityComparer<T>> comparers)
    {
      return new MultiEqualityComparer<T>(comparers);
    }

    public static IEqualityComparer<T> FromComparers<T>(
      params IEqualityComparer<T>[] comparers)
    {
      return FromComparers(comparers);
    }

    public static IEqualityComparer<T> NullToValueObjEqualityComparer<T>(T t)
      where T : class
    {
      return new DelegateEqualityComparer<T>(
        (x, y) => Equals(x ?? t, y ?? t),
        o => o != null ?
          o.GetHashCode() :
          t.GetHashCode());
    }

    public static IEqualityComparer<T> NullToValueObjEqualityComparer<T>(
      T t,
      IEqualityComparer<T> comparer)
      where T : class
    {
      return new DelegateEqualityComparer<T>(
        (x, y) => comparer.Equals(x ?? t, y ?? t),
        o => o != null ?
          comparer.GetHashCode(o) :
          comparer.GetHashCode(t));
    }

    public static CombinedDelegateComparer<T> NullToValueObjComparer<T>(T t)
      where T : class, IComparable<T>
    {
      return new CombinedDelegateComparer<T>(
        (x, y) => Equals(x ?? t, y ?? t),
        o => o != null ?
          o.GetHashCode() :
          t.GetHashCode(),
        (x, y) => (x ?? t).CompareTo(y ?? t));
    }

    public static CombinedDelegateComparer<T> NullToValueObjComparer<T>(
      T t,
      IEqualityComparer<T> eqComparer,
      IComparer<T> comparer)
      where T : class
    {
      return new CombinedDelegateComparer<T>(
        (x, y) => eqComparer.Equals(x ?? t, y ?? t),
        o => o != null ?
          eqComparer.GetHashCode(o) :
          eqComparer.GetHashCode(t),
        (x, y) => comparer.Compare(x ?? t, y ?? t));
    }

    public static IEqualityComparer<T?> NullToValueStructEqualityComparer<T>(T t)
      where T : struct
    {
      return new DelegateEqualityComparer<T?>(
        (x, y) => Equals(x ?? t, y ?? t),
        o => o.HasValue ?
          o.Value.GetHashCode() :
          t.GetHashCode());
    }

    public static IEqualityComparer<T?> NullToValueStructEqualityComparer<T>(
      T t,
      IEqualityComparer<T> comparer)
      where T : struct
    {
      return new DelegateEqualityComparer<T?>(
        (x, y) => Equals(x ?? t, y ?? t),
        o => o.HasValue ?
          comparer.GetHashCode(o.Value) :
          comparer.GetHashCode(t));
    }

    public static CombinedDelegateComparer<T?> NullToValueStructComparer<T>(T t)
      where T : struct, IComparable<T>
    {
      return new CombinedDelegateComparer<T?>(
        (x, y) => Equals(x ?? t, y ?? t),
        o => o.HasValue ?
          o.Value.GetHashCode() :
          t.GetHashCode(),
        (x, y) => (x ?? t).CompareTo(y ?? t));
    }

    public static CombinedDelegateComparer<T?> NullToValueStructComparer<T>(
      T t,
      IEqualityComparer<T> eqComparer,
      IComparer<T> comparer)
      where T : struct
    {
      return new CombinedDelegateComparer<T?>(
        (x, y) => eqComparer.Equals(x ?? t, y ?? t),
        o => o.HasValue ?
          eqComparer.GetHashCode(o.Value) :
          eqComparer.GetHashCode(t),
        (x, y) => comparer.Compare(x ?? t, y ?? t));
    }
  }
}
