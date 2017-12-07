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
    public static IEnumerable<T> SelectSome<T>(this IEnumerable<IMaybe<T>> seq)
    {
      return seq
        .Where(HasValue)
        .Select(Value);
    }

    public static IEnumerable<TSuccess> SelectSuccess<TSuccess, TFailure>(
      this IEnumerable<IResult<TSuccess, TFailure>> seq)
    {
      return seq
        .Where(IsSuccess)
        .Select(SuccessResult);
    }

    public static IEnumerable<TFailure> SelectFailure<TSuccess, TFailure>(
      this IEnumerable<IResult<TSuccess, TFailure>> seq)
    {
      return seq
        .Where(IsFailure)
        .Select(FailureResult);
    }
    
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
  }
}
