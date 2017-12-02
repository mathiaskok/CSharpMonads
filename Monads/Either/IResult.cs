namespace Monads.Either
{
  public interface IResult<TSuccess, TFailure>
  {
    bool IsSuccess { get; }
    bool IsFailure { get; }

    TSuccess SuccessResult { get; }
    TFailure FailureResult { get; }
  }
}
