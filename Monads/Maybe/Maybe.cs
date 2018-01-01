using System;
using System.Collections.Generic;
using System.Linq;
using Monads.FunctionStructures;
using Monads.Maybe.Structures;

namespace Monads.Maybe
{
  public static class Maybe
  {
    public static IMaybe<T> Some<T>(T value) => new Some<T>(value);

    public static IMaybe<T> None<T>() => new None<T>();

    public static IMaybe<T> Return<T>(T value)
      where T : class
    {
      if (value != null)
        return Some(value);
      else
        return None<T>();
    }

    public static IMaybe<T> Return<T>(T? value)
      where T : struct
    {
      if (value.HasValue)
        return Some(value.Value);
      else
        return None<T>();
    }

    public static T Coerce<T>(this IMaybe<T> m, T value)
    {
      if (m.HasValue)
        return m.Value;
      else
        return value;
    }

    public static T Coerce<T>(this IMaybe<T> m, Func<T> func)
    {
      if (m.HasValue)
        return m.Value;
      else
        return func();
    }

    public static bool HasValue<T>(IMaybe<T> m) => m.HasValue;

    public static T Value<T>(IMaybe<T> m) => m.Value;

    public static IMaybe<U> Select<T, U>(this IMaybe<T> m, Func<T, U> mapper)
    {
      if (m.HasValue)
        return Some(mapper(m.Value));
      else
        return None<U>();
    }

    public static IMaybe<T> Where<T>(this IMaybe<T> m, Func<T, bool> predicate)
    {
      return m.SelectMany(t => predicate(t) ? m : None<T>());
    }

    public static IMaybe<U> SelectMany<T, U>(this IMaybe<T> m, Func<T, IMaybe<U>> binder)
    {
      if (m.HasValue)
        return binder(m.Value);
      else
        return None<U>();
    }

    public static IMaybe<V> SelectMany<T, U, V>(
      this IMaybe<T> m, 
      Func<T, IMaybe<U>> binder,
      Func<T, U, V> valueSelector)
    {
      if (m.HasValue)
      {
        T t = m.Value;
        return binder(t)
          .Select(u => valueSelector(t, u));
      }
      else
        return None<V>();
    }

    public static IMaybe<U> BindNullable<T, U>(this IMaybe<T> m, Func<T, U> binder)
      where U : class
    {
      if (m.HasValue)
      {
        U value = binder(m.Value);
        if (value != null)
          return Some(value);
      }

      return None<U>();
    }

    public static IMaybe<U> BindNullable<T, U>(this IMaybe<T> m, Func<T, U?> binder)
      where U : struct
    {
      if (m.HasValue)
      {
        U? value = binder(m.Value);
        if (value.HasValue)
          return Some(value.Value);
      }

      return None<U>();
    }

    public static IMaybe<U> Apply<T, U>(this IMaybe<Func<T, U>> func, IMaybe<T> m)
    {
      if (func.HasValue)
        return m.Select(func.Value);
      else
        return None<U>();
    }

    public static Func<IMaybe<T>, IMaybe<U>> Lift<T, U>(this Func<T, U> func)
    {
      return m => m.Select(func);
    }

    public static Func<IMaybe<T>, IMaybe<U>, IMaybe<V>> Lift<T, U, V>(this Func<T, U, V> func)
    {
      return (m1, m2) => m1.Select(func.Curry()).Apply(m2);
    }

    public static Func<IMaybe<T>, IMaybe<U>, IMaybe<V>, IMaybe<X>> Lift<T, U, V, X>(this Func<T, U, V, X> func)
    {
      return (m1, m2, m3) => m1.Select(func.Curry()).Apply(m2).Apply(m3);
    }

    public static IMaybe<V> Combine<T, U, V>(this IMaybe<T> t, IMaybe<U> u, Func<T, U, V> combiner)
    {
      if (t.HasValue && u.HasValue)
        return Some(combiner(t.Value, u.Value));
      else
        return None<V>();
    }

    public static IEnumerable<T> SelectSome<T>(this IEnumerable<IMaybe<T>> seq)
    {
      return seq
        .Where(HasValue)
        .Select(Value);
    }
  }
}
