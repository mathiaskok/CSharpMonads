using System;
using System.Collections.Generic;
using System.Linq;
using Monads.Collections;
using Monads.Either;
using Monads.Maybe;
using static Monads.Either.Result;
using static Monads.Maybe.Maybe;

namespace Monads.Linq
{
  public static class LinqExtensions
  {
    public static IEnumerable<T> Intersperse<T>(this IEnumerable<T> seq, T sep)
    {
      if (!seq.Any())
        yield break;

      IEnumerator<T> iter = seq.GetEnumerator();
      iter.MoveNext();
      yield return iter.Current;

      while (iter.MoveNext())
      {
        yield return sep;
        yield return iter.Current;
      }
    }

    public static IEnumerable<T> Intercalate<T>(this IEnumerable<T> seq, IEnumerable<T> sep)
    {
      IReadOnlyCollection<T> roSep = sep.AsReadOnly();

      if (!seq.Any())
        yield break;

      IEnumerator<T> iter = seq.GetEnumerator();
      iter.MoveNext();
      yield return iter.Current;

      while (iter.MoveNext())
      {
        foreach (T t in roSep)
          yield return t;

        yield return iter.Current;
      }
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> seq)
      where T : class
    {
      return seq.Where(t => t != null);
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> seq)
      where T : struct
    {
      return seq
        .Where(t => t.HasValue)
        .Select(t => t.Value);
    }

    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> seq)
    {
      return seq.SelectMany(t => t);
    }

    public static T Mappend<T>(this IEnumerable<T> seq, T defaultValue, Func<T, T, T> monoid)
    {
      if (seq is IReadOnlyList<T> l)
        return l.BinaryMappend(defaultValue, monoid);
      else
        return seq.Aggregate(defaultValue, monoid);
    }

    public static T Mappend<T>(this IEnumerable<T> seq, Func<T, T, T> semiGroup)
    {
      if (!seq.Any())
        throw new ArgumentException("seq must not be empty");

      if (seq is IReadOnlyList<T> l)
        return l.BinaryMappend(semiGroup);
      else
        return seq.Aggregate(semiGroup);
    }
  }
}
