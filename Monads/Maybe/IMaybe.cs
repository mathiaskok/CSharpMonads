namespace Monads.Maybe
{
  public interface IMaybe<out T>
  {
    bool HasValue { get; }

    T Value { get; }
  }
}
