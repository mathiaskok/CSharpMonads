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
  }
}
