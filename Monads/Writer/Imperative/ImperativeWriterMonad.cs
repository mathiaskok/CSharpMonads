using System;
using System.Collections.Generic;

namespace Monads.Writer.Imperative
{
  public class ImperativeWriterMonad<T>
  {
    private Action<string> WriterAction { get; }
    public T Value { get; }

    private ImperativeWriterMonad(T value, Action<string> writerAction)
    {
      Value = value;
      WriterAction = writerAction;
    }

    public ImperativeWriterMonad<T> Return(T value, Action<string> writerAction)
    {
      return new ImperativeWriterMonad<T>(value, writerAction);
    }

    public ImperativeWriterMonad<U> Bind<U>(Func<T, (U, IEnumerable<string>)> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public ImperativeWriterMonad<U> Bind<U>(Func<T, (U, string)> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public ImperativeWriterMonad<U> Bind<U>(Func<T, Tuple<U, IEnumerable<string>>> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public ImperativeWriterMonad<U> Bind<U>(Func<T, Tuple<U, string>> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public ImperativeWriterMonad<U> Bind<U>(Func<T, KeyValuePair<U, IEnumerable<string>>> binder)
    {
      var kvp = binder(Value);
      return Next(kvp.Key, kvp.Value);
    }

    public ImperativeWriterMonad<U> Bind<U>(Func<T, KeyValuePair<U, string>> binder)
    {
      var kvp = binder(Value);
      return Next(kvp.Key, kvp.Value);
    }

    public ImperativeWriterMonad<U> Map<U>(Func<T, U> mapper)
    {
      return new ImperativeWriterMonad<U>(mapper(Value), WriterAction);
    }

    private ImperativeWriterMonad<U> Next<U>(U value, IEnumerable<string> messages)
    {
      foreach (string m in messages)
        WriterAction(m);

      return new ImperativeWriterMonad<U>(value, WriterAction);
    }

    private ImperativeWriterMonad<U> Next<U>(U value, string message)
    {
      WriterAction(message);
      return new ImperativeWriterMonad<U>(value, WriterAction);
    }
  }
}
