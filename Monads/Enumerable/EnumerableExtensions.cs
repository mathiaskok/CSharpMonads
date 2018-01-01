using System;
using System.Collections.Generic;
using System.Linq;
using Monads.Collections;

namespace Monads.Enumerable
{
  public static class EnumerableExtensions
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

    public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> seq, Func<T, bool> predicate) =>
      seq.Where(t => !predicate(t));

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

    public static bool SequenceDistinct<T>(this IEnumerable<T> seq, IEqualityComparer<T> comparer)
    {
      HashSet<T> set = new HashSet<T>(comparer);
      return seq.All(set.Add);
    }

    public static bool SequenceDistinct<T>(this IEnumerable<T> seq) =>
      seq.SequenceDistinct(EqualityComparer<T>.Default);

    public static IReadOnlyCollection<T> NonDistinct<T>(this IEnumerable<T> seq, IEqualityComparer<T> comparer)
    {
      HashSet<T> dis = new HashSet<T>(comparer);
      return seq
        .WhereNot(dis.Add)
        .ToList();
    }

    public static IReadOnlyCollection<T> NonDistinct<T>(this IEnumerable<T> seq) =>
      seq.NonDistinct(EqualityComparer<T>.Default);
  }
}
