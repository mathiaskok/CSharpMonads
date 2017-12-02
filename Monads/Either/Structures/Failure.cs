using System;

namespace Monads.Either.Structures
{
  public class Failure<TSuccess, TFailure> : IResult<TSuccess, TFailure>
  {
    public TFailure Result { get; }

    public bool IsSuccess => false;

    public bool IsFailure => true;

    public Failure(TFailure result)
    {
      Result = result;
    }

    public TSuccess SuccessResult => throw new InvalidOperationException();

    public TFailure FailureResult => Result;
  }
}
