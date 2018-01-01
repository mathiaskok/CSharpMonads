using System;
using System.Collections.Generic;
using System.Linq;
using Monads.Either.Structures;
using Monads.FunctionStructures;

namespace Monads.Either
{
  public static class Result
  {
    public static IResult<TSuccess, TFailure> Success<TSuccess, TFailure>(TSuccess result) =>
      new Success<TSuccess, TFailure>(result);

    public static IResult<TSuccess, TFailure> Failure<TSuccess, TFailure>(TFailure result) =>
      new Failure<TSuccess, TFailure>(result);

    public static bool TryGetSuccess<TSuccess, TFailure>(
      this IResult<TSuccess, TFailure> result,
      out TSuccess success)
    {
      bool succ = result.IsSuccess;
      if (succ)
        success = result.SuccessResult;
      else
        success = default(TSuccess);
      return succ;
    }

    public static bool TryGetFailure<TSuccess, TFailure>(
      this IResult<TSuccess, TFailure> result,
      out TFailure failure)
    {
      bool fail = result.IsFailure;
      if (fail)
        failure = result.FailureResult;
      else
        failure = default(TFailure);
      return fail;
    }

    public static bool IsSuccess<TSuccess, TFailure>(IResult<TSuccess, TFailure> res) => res.IsSuccess;

    public static bool IsFailure<TSuccess, TFailure>(IResult<TSuccess, TFailure> res) => res.IsFailure;

    public static TSuccess SuccessResult<TSuccess, TFailure>(IResult<TSuccess, TFailure> res) => res.SuccessResult;

    public static TFailure FailureResult<TSuccess, TFailure>(IResult<TSuccess, TFailure> res) => res.FailureResult;

    public static IResult<TSuccess, TFailure> Return<TSuccess, TFailure>(TSuccess result) =>
      Success<TSuccess, TFailure>(result);

    public static IResult<USuccess, TFailure> Select<TSuccess, USuccess, TFailure>(
      this IResult<TSuccess, TFailure> result,
      Func<TSuccess, USuccess> mapper)
    {
      return result.IsSuccess ?
        Success<USuccess, TFailure>(mapper(result.SuccessResult)) :
        Failure<USuccess, TFailure>(result.FailureResult);
    }

    public static IResult<TSuccess, UFailure> Transform<TSuccess, TFailure, UFailure>(
      this IResult<TSuccess, TFailure> resut,
      Func<TFailure, UFailure> transformer)
    {
      return resut.IsFailure ?
        Failure<TSuccess, UFailure>(transformer(resut.FailureResult)) :
        Success<TSuccess, UFailure>(resut.SuccessResult);
    }

    public static IResult<USuccess, TFailure> SelectMany<TSuccess, USuccess, TFailure>(
      this IResult<TSuccess, TFailure> result,
      Func<TSuccess, IResult<USuccess, TFailure>> binder)
    {
      return result.IsSuccess ?
        binder(result.SuccessResult) :
        Failure<USuccess, TFailure>(result.FailureResult);
    }

    public static IResult<VSuccess, TFailure> SelectMany<TSuccess, USuccess, VSuccess, TFailure>(
      this IResult<TSuccess, TFailure> result,
      Func<TSuccess, IResult<USuccess, TFailure>> binder,
      Func<TSuccess, USuccess, VSuccess> valueSelector)
    {
      if (result.IsSuccess)
      {
        TSuccess t = result.SuccessResult;
        return binder(t)
          .Select(u => valueSelector(t, u));
      }
      else
        return Failure<VSuccess, TFailure>(result.FailureResult);
    }

    public static IResult<TSuccess, TFailure> Combine<TSuccess, TFailure>(
      this IResult<TSuccess, TFailure> res1,
      IResult<TSuccess, TFailure> res2,
      Func<TSuccess, TSuccess, TSuccess> successCombiner,
      Func<TFailure, TFailure, TFailure> failureCombiner)
    {
      if (res1.IsSuccess)
      {
        if (res2.IsSuccess)
          return Success<TSuccess, TFailure>(successCombiner(res1.SuccessResult, res2.SuccessResult));
        else
          return Failure<TSuccess, TFailure>(res2.FailureResult);
      }
      else
      {
        if (res2.IsSuccess)
          return Failure<TSuccess, TFailure>(res1.FailureResult);
        else
          return Failure<TSuccess, TFailure>(failureCombiner(res1.FailureResult, res2.FailureResult));
      }
    }

    public static IResult<USuccess, TFailure> Apply<TSuccess, USuccess, TFailure>(
      this IResult<Func<TSuccess, USuccess>, TFailure> func,
      IResult<TSuccess, TFailure> result)
    {
      if (func.IsSuccess)
        return result.Select(func.SuccessResult);
      else
        return Failure<USuccess, TFailure>(func.FailureResult);
    }

    public static Func<IResult<TSuccess, TFailure>, IResult<USuccess, TFailure>> Lift<TSuccess, USuccess, TFailure>(
      Func<TSuccess, USuccess> func)
    {
      return r1 =>
      {
        if (r1.IsSuccess)
          return Success<USuccess, TFailure>(func(r1.SuccessResult));
        else
          return Failure<USuccess, TFailure>(r1.FailureResult);
      };
    }

    public static Func<IResult<TSuccess, TFailure>, IResult<USuccess, TFailure>, IResult<VSuccess, TFailure>> Lift<TSuccess, USuccess, VSuccess, TFailure>(
      Func<TSuccess, USuccess, VSuccess> func)
    {
      return (r1, r2) => r1.Select(func.Curry()).Apply(r2);
    }

    public static Func<IResult<TSuccess, TFailure>, IResult<USuccess, TFailure>, IResult<VSuccess, TFailure>, IResult<XSuccess, TFailure>> Lift<TSuccess, USuccess, VSuccess, XSuccess, TFailure>(
      Func<TSuccess, USuccess, VSuccess, XSuccess> func)
    {
      return (r1, r2, r3) => r1.Select(func.Curry()).Apply(r2).Apply(r3);
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
  }
}
