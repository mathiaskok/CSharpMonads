using System;
using Monads.Either.Structures;

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

    public static IResult<TSuccess, TFailure> Return<TSuccess, TFailure>(TSuccess result) =>
      Success<TSuccess, TFailure>(result);

    public static IResult<USuccess, TFailure> Map<TSuccess, USuccess, TFailure>(
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

    public static IResult<USuccess, TFailure> Bind<TSuccess, USuccess, TFailure>(
      this IResult<TSuccess, TFailure> result,
      Func<TSuccess, IResult<USuccess, TFailure>> binder)
    {
      return result.IsSuccess ?
        binder(result.SuccessResult) :
        Failure<USuccess, TFailure>(result.FailureResult);
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
  }
}
