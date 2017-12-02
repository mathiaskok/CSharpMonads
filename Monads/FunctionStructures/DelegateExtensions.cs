using System;
using System.Collections.Generic;
using static Monads.Comparison.EqualityComparerExtensions;

namespace Monads.FunctionStructures
{
  public static class DelegateExtensions
  {
    public static Func<T, U> Memoize<T, U>(this Func<T, U> func, IEqualityComparer<T> comparer)
    {
      MemoizedFunction<T, U> memoized = new MemoizedFunction<T, U>(func, comparer);
      return memoized.Apply;
    }

    public static Func<T, U> Memoize<T, U>(this Func<T, U> func)
    {
      return func.Memoize(EqualityComparer<T>.Default);
    }

    public static Func<T, U, V> Memoize<T, U, V>(
      this Func<T, U, V> func,
      IEqualityComparer<T> comparerT,
      IEqualityComparer<U> comparerU)
    {
      return func.Memoize(AsTupleComparer(comparerT, comparerU));
    }

    public static Func<T, U, V> Memoize<T, U, V>(this Func<T, U, V> func)
    {
      return func.Memoize(EqualityComparer<(T, U)>.Default);
    }

    private static Func<T, U, V> Memoize<T, U, V>(
      this Func<T, U, V> func,
      IEqualityComparer<(T, U)> comparer)
    {
      MemoizedFunction<(T, U), V> memoized = new MemoizedFunction<(T, U), V>(
        t => func(t.Item1, t.Item2),
        comparer);

      return (t, u) => memoized.Apply((t, u));
    }

    public static Func<T, U, V, X> Memoize<T, U, V, X>(
      this Func<T, U, V, X> func,
      IEqualityComparer<T> comparerT,
      IEqualityComparer<U> comparerU,
      IEqualityComparer<V> comparerV)
    {
      return func.Memoize(AsTupleComparer(comparerT, comparerU, comparerV));
    }

    public static Func<T, U, V, X> Memoize<T, U, V, X>(this Func<T, U, V, X> func)
    {
      return func.Memoize(EqualityComparer<(T, U, V)>.Default);
    }

    private static Func<T, U, V, X> Memoize<T, U, V, X>(
      this Func<T, U, V, X> func,
      IEqualityComparer<(T, U, V)> comparer)
    {
      MemoizedFunction<(T, U, V), X> memoized = new MemoizedFunction<(T, U, V), X>(
        t => func(t.Item1, t.Item2, t.Item3),
        comparer);

      return (t, u, v) => memoized.Apply((t, u, v));
    }

    public static Func<T, Func<U, V>> Curry<T, U, V>(
      this Func<T, U, V> func)
    {
      return
        t =>
          u => func(t, u);
    }

    public static Func<T, Func<U, Func<V, X>>> Curry<T, U, V, X>(
      this Func<T, U, V, X> func)
    {
      return
        t =>
          u =>
            v => func(t, u, v);
    }
  }
}
