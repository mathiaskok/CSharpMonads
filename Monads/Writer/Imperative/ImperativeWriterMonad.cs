using System;
using System.Collections.Generic;
using System.Text;

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
      foreach (string m in w)
        WriterAction(m);

      return new ImperativeWriterMonad<U>(u, WriterAction);
    }

    public ImperativeWriterMonad<U> Bind<U>(Func<T, (U, string)> binder)
    {
      var (u, w) = binder(Value);
      WriterAction(w);

      return new ImperativeWriterMonad<U>(u, WriterAction);
    }

    public ImperativeWriterMonad<U> Bind<U>(Func<T, Tuple<U, IEnumerable<string>>> binder)
    {
      var (u, w) = binder(Value);
      foreach (string m in w)
        WriterAction(m);

      return new ImperativeWriterMonad<U>(u, WriterAction);
    }

    public ImperativeWriterMonad<U> Bind<U>(Func<T, Tuple<U, string>> binder)
    {
      var (u, w) = binder(Value);
      WriterAction(w);

      return new ImperativeWriterMonad<U>(u, WriterAction);
    }

    public ImperativeWriterMonad<U> Bind<U>(Func<T, KeyValuePair<U, IEnumerable<string>>> binder)
    {
      var kvp = binder(Value);
      foreach (string m in kvp.Value)
        WriterAction(m);

      return new ImperativeWriterMonad<U>(kvp.Key, WriterAction);
    }

    public ImperativeWriterMonad<U> Bind<U>(Func<T, KeyValuePair<U, string>> binder)
    {
      var kvp = binder(Value);
      WriterAction(kvp.Value);

      return new ImperativeWriterMonad<U>(kvp.Key, WriterAction);
    }

    public ImperativeWriterMonad<U> Map<U>(Func<T, U> mapper)
    {
      return new ImperativeWriterMonad<U>(mapper(Value), WriterAction);
    }
  }
}
