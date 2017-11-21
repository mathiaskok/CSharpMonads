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
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, (U, IEnumerable<string>)> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, (U, string)> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, Tuple<U, IEnumerable<string>>> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, Tuple<U, string>> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, KeyValuePair<U, IEnumerable<string>>> binder)
    {
      var kvp = binder(Value);
      return Next(kvp.Key, kvp.Value);
    }

    public EnumerableWriterMonad<U> Bind<U>(Func<T, KeyValuePair<U, string>> binder)
    {
      var kvp = binder(Value);
      return Next(kvp.Key, kvp.Value);
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

    public StateWriterMonad<T, TState> Transform<TState>(
      Func<IEnumerable<string>, TState> stateTransformer,
      Func<TState, string, TState> stateAppender,
      Func<TState, string> stateStringifier)
    {
      return StateWriterMonad<T, TState>.Return(
        Value,
        stateTransformer(WriterState),
        stateAppender,
        stateStringifier);
    }

    private IEnumerable<string> Append(string str)
    {
      foreach (string m in WriterState)
        yield return m;

      yield return str;
    }

    private EnumerableWriterMonad<U> Next<U>(U value, IEnumerable<string> messages) =>
      new EnumerableWriterMonad<U>(value, WriterState.Concat(messages));

    private EnumerableWriterMonad<U> Next<U>(U value, string message) =>
      new EnumerableWriterMonad<U>(value, WriterState.Append(message));
  }
}
