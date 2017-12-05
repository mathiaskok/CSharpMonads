namespace Monads.Maybe
{
  public interface IMaybe<T>
  {
    bool HasValue { get; }

    T Value { get; }
  }
}
