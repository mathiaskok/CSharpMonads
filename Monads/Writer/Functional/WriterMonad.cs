using System;
using System.Collections.Generic;
using System.Linq;

namespace Monads.Writer.Functional
{
  public class WriterMonad<T>
  {
    public T Value { get; }

    public IEnumerable<string> WriterState { get; }

    private WriterMonad(T value, IEnumerable<string> writerState)
    {
      Value = value;
      WriterState = writerState;
    }

    public static WriterMonad<T> Return(T value)
    {
      return new WriterMonad<T>(value, Enumerable.Empty<string>());
    }

    public void Deconstruct(out T value, out IEnumerable<string> writerState)
    {
      value = Value;
      writerState = WriterState;
    }

    public WriterMonad<U> Bind<U>(Func<T, WriterMonad<U>> binder)
    {
      var (u, state) = binder(Value);
      return new WriterMonad<U>(u, state);
    }

    public WriterMonad<U> Bind<U>(Func<T, (U, IEnumerable<string>)> binder)
    {
      var (u, state) = binder(Value);
      return new WriterMonad<U>(u, state);
    }

    public WriterMonad<U> Bind<U>(Func<T, Tuple<U, IEnumerable<string>>> binder)
    {
      var (u, state) = binder(Value);
      return new WriterMonad<U>(u, state);
    }

    public WriterMonad<U> Bind<U>(Func<T, KeyValuePair<U, IEnumerable<string>>> binder)
    {
      var kvp = binder(Value);
      return new WriterMonad<U>(kvp.Key, kvp.Value);
    }

    public WriterMonad<V> Combine<U, V>(WriterMonad<U> other, Func<T, U, V> combiner)
    {
      return new WriterMonad<V>(
        combiner(Value, other.Value),
        WriterState.Concat(other.WriterState));
    }
  }
}
