using System;
using System.Collections.Generic;
using System.Text;

namespace Monads.Lazy
{
  public static class LazyMonad
  {
    public static Lazy<T> AsLazy<T>(this T t) =>
      new Lazy<T>(() => t);

    public static Lazy<U> Apply<T, U>(this T t, Func<T, U> func) =>
      new Lazy<U>(() => func(t));

    public static Lazy<U> Map<T, U>(this Lazy<T> t, Func<T, U> mapper) =>
      new Lazy<U>(() => mapper(t.Value));

    public static Lazy<U> Bind<T, U>(this Lazy<T> t, Func<T, Lazy<U>> binder) =>
      new Lazy<U>(() => binder(t.Value).Value);

    public static Func<Lazy<T>, Lazy<U>> Lift1<T, U>(Func<T, U> func) =>
      t => new Lazy<U>(() => func(t.Value));

    public static Func<Lazy<T>, Lazy<U>, Lazy<V>> Lift2<T, U, V>(Func<T, U, V> func) =>
      (t, u) => new Lazy<V>(() => func(t.Value, u.Value));

    public static Func<Lazy<T>, Lazy<U>, Lazy<V>, Lazy<X>> Lift3<T, U, V, X>(Func<T, U, V, X> func) =>
      (t, u, v) => new Lazy<X>(() => func(t.Value, u.Value, v.Value));

    public static Lazy<V> Combine<T, U, V>(this Lazy<T> t, Lazy<U> u, Func<T, U, V> combiner) =>
      new Lazy<V>(() => combiner(t.Value, u.Value));
  }
}
