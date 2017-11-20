using System;
using System.Collections.Generic;
using System.Linq;

namespace Monads.Writer.Functional
{
  public class EnumerableWriterMonad<T>
  {
    public T Value { get; }

    public IEnumerable<string> WriterState { get; }

    private EnumerableWriterMonad(T value, IEnumerable<string> writerState)
    {
      Value = value;
      WriterState = writerState;
    }

    public static EnumerableWriterMonad<T> Return(T value)
    {
      return new EnumerableWriterMonad<T>(value, Enumerable.Empty<string>());
    }

    public void Deconstruct(out T value, out IEnumerable<string> writerState)
    {
      value = Value;
      writerState = WriterState;
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, EnumerableWriterMonad<U>> binder)
    {
      var (u, state) = binder(Value);
      return new EnumerableWriterMonad<U>(u, state);
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, (U, IEnumerable<string>)> binder)
    {
      var (u, state) = binder(Value);
      return new EnumerableWriterMonad<U>(u, state);
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, (U, string)> binder)
    {
      var (u, state) = binder(Value);
      return new EnumerableWriterMonad<U>(u, Append(state));
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, Tuple<U, IEnumerable<string>>> binder)
    {
      var (u, state) = binder(Value);
      return new EnumerableWriterMonad<U>(u, state);
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, Tuple<U, string>> binder)
    {
      var (u, state) = binder(Value);
      return new EnumerableWriterMonad<U>(u, Append(state));
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, KeyValuePair<U, IEnumerable<string>>> binder)
    {
      var kvp = binder(Value);
      return new EnumerableWriterMonad<U>(kvp.Key, kvp.Value);
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, KeyValuePair<U, string>> binder)
    {
      var kvp = binder(Value);
      return new EnumerableWriterMonad<U>(kvp.Key, Append(kvp.Value));
    }

    public EnumerableWriterMonad<U> Map<U>(Func<T, U> mapper)
    {
      return new EnumerableWriterMonad<U>(mapper(Value), WriterState);
    }

    public EnumerableWriterMonad<V> Combine<U, V>(EnumerableWriterMonad<U> other, Func<T, U, V> combiner)
    {
      return new EnumerableWriterMonad<V>(
        combiner(Value, other.Value),
        WriterState.Concat(other.WriterState));
    }

    private IEnumerable<string> Append(string str)
    {
      foreach (string m in WriterState)
        yield return m;

      yield return str;
    }
  }
}
