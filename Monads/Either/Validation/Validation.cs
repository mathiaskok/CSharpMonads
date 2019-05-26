using System;
using System.Collections.Generic;
using System.Linq;
using Monads.FunctionStructures;

namespace Monads.Either.Validation
{
  public static class Validation
  {
    public static IResult<S, IEnumerable<F>> ValidationSuccess<S, F>(this S result)
    {
      return Result.Success<S, IEnumerable<F>>(result);
    }

    public static IResult<S, IEnumerable<F>> ValidationFailure<S, F>(this IEnumerable<F> validationErrors)
    {
      return Result.Failure<S, IEnumerable<F>>(validationErrors);
    }

    public static IResult<S2, IEnumerable<F>> ApplyValidation<S1, S2, F>(
      this IResult<Func<S1, S2>, IEnumerable<F>> func,
      IResult<S1, IEnumerable<F>> param)
    {
      if (func.IsFailure)
      {
        if (param.IsFailure)
          return func.FailureResult
            .Concat(param.FailureResult)
            .ValidationFailure<S2, F>();
        else
          return func.FailureResult
            .ValidationFailure<S2, F>();
      }
      else
      {
        if (param.IsFailure)
          return param.FailureResult
            .ValidationFailure<S2, F>();
        else
          return func.SuccessResult(param.SuccessResult)
            .ValidationSuccess<S2, F>();
      }
    }



    public static IResult<S2, IEnumerable<F>> PipeValidation<S1, S2, F>(
      this Func<S1, S2> func,
      IResult<S1, IEnumerable<F>> param)
    {
      return param.Select(func);
    }

    public static IResult<S3, IEnumerable<F>> PipeValidation<S1, S2, S3, F>(
      this Func<S1, S2, S3> func,
      IResult<S1, IEnumerable<F>> param1,
      IResult<S2, IEnumerable<F>> param2)
    {
      return param1.Select(func.Curry())
        .ApplyValidation(param2);
    }

    public static IResult<S4, IEnumerable<F>> PipeValidation<S1, S2, S3, S4, F>(
      this Func<S1, S2, S3, S4> func,
      IResult<S1, IEnumerable<F>> param1,
      IResult<S2, IEnumerable<F>> param2,
      IResult<S3, IEnumerable<F>> param3)
    {
      return param1.Select(func.Curry())
        .ApplyValidation(param2)
        .ApplyValidation(param3);
    }

    public static IResult<S2, IEnumerable<F>> WithValidation<S1, S2, F>(
      this Func<S1, IResult<S2, IEnumerable<F>>> func,
      IResult<S1, IEnumerable<F>> param)
    {
      return func.PipeValidation(param)
        .Join();
    }

    public static IResult<S3, IEnumerable<F>> WithValidation<S1, S2, S3, F>(
      this Func<S1, S2, IResult<S3, IEnumerable<F>>> func,
      IResult<S1, IEnumerable<F>> param1,
      IResult<S2, IEnumerable<F>> param2)
    {
      return func.PipeValidation(param1, param2)
        .Join();
    }

    public static IResult<S4, IEnumerable<F>> WithValidation<S1, S2, S3, S4, F>(
      this Func<S1, S2, S3, IResult<S4, IEnumerable<F>>> func,
      IResult<S1, IEnumerable<F>> param1,
      IResult<S2, IEnumerable<F>> param2,
      IResult<S3, IEnumerable<F>> param3)
    {
      return func.PipeValidation(param1, param2, param3)
        .Join();
    }
  }
}
