using System;

namespace Monads.Maybe.Structures
{
  internal class None<T> : IMaybe<T>
  {
    public bool HasValue => false;

    public T Value => throw new InvalidOperationException();
  }
}
