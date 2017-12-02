namespace Monads.Either
{
  public interface IResult<out TSuccess, out TFailure>
  {
    bool IsSuccess { get; }
    bool IsFailure { get; }

    TSuccess SuccessResult { get; }
    TFailure FailureResult { get; }
  }
}
