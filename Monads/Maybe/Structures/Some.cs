namespace Monads.Maybe.Structures
{
  internal class Some<T> : IMaybe<T>
  {
    public bool HasValue => true;

    public T Value { get; }

    public Some(T value)
    {
      Value = value;
    }
  }
}
