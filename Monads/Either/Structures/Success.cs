using System;

namespace Monads.Either.Structures
{
  public class Success<TSuccess, TFailure> : IResult<TSuccess, TFailure>
  {
    public TSuccess Result { get; }

    public bool IsSuccess => true;

    public bool IsFailure => false;

    public Success(TSuccess result)
    {
      Result = result;
    }

    public TSuccess SuccessResult => Result;

    public TFailure FailureResult => throw new InvalidOperationException();
  }
}
