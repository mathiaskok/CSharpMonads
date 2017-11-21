using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monads.Writer.Functional
{
  public class StateWriterMonad<T, TState>
  {
    public T Value { get; }
    private TState State { get; }
    private StateWriterState<TState> StateOps { get; }

    private StateWriterMonad(T value, TState state, StateWriterState<TState> stateOps)
    {
      Value = value;
      StateOps = stateOps;
    }

    public static StateWriterMonad<T, TState> Return(
      T value,
      TState state,
      Func<TState, string, TState> stateAppender,
      Func<TState, string> stateStringifier)
    {
      return new StateWriterMonad<T, TState>(value, state, new StateWriterState<TState>(stateAppender, stateStringifier));
    }

    public StateWriterMonad<U, TState> Bind<U>(Func<T, (U, IEnumerable<string>)> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public StateWriterMonad<U, TState> Bind<U>(Func<T, (U, string)> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public StateWriterMonad<U, TState> Bind<U>(Func<T, Tuple<U, IEnumerable<string>>> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public StateWriterMonad<U, TState> Bind<U>(Func<T, Tuple<U, string>> binder)
    {
      var (u, w) = binder(Value);
      return Next(u, w);
    }

    public StateWriterMonad<U, TState> Bind<U>(Func<T, KeyValuePair<U, IEnumerable<string>>> binder)
    {
      var kvp = binder(Value);
      return Next(kvp.Key, kvp.Value);
    }

    public StateWriterMonad<U, TState> Bind<U>(Func<T, KeyValuePair<U, string>> binder)
    {
      var kvp = binder(Value);
      return Next(kvp.Key, kvp.Value);
    }

    public StateWriterMonad<U, TState> Map<U>(Func<T, U> mapper) =>
      new StateWriterMonad<U, TState>(mapper(Value), State, StateOps);

    public StateWriterMonad<T, TState> Combine(
      StateWriterMonad<T, TState> other,
      Func<T,T,T> valueCombiner,
      Func<TState, TState, TState> stateCombiner)
    {
      return new StateWriterMonad<T, TState>(
        valueCombiner(Value, other.Value),
        stateCombiner(State, other.State),
        StateOps /* Consider allowing to choose StateOps */);
    }

    public StateWriterMonad<T, UState> Transform<UState>(
      Func<TState, UState> stateTransformer,
      Func<UState, string, UState> stateAppender,
      Func<UState, string> stateStringifier)
    {
      return new StateWriterMonad<T, UState>(
        Value,
        stateTransformer(State),
        new StateWriterState<UState>(stateAppender, stateStringifier));
    }


    private StateWriterMonad<U, TState> Next<U>(U value, IEnumerable<string> messages) =>
      new StateWriterMonad<U, TState>(value, messages.Aggregate(State, StateOps.StateAppender), StateOps);

    private StateWriterMonad<U, TState> Next<U>(U value, string message) =>
      new StateWriterMonad<U, TState>(value, StateOps.StateAppender(State, message), StateOps);
  }

  class StateWriterState<T>
  {
    public Func<T, string, T> StateAppender { get; }
    public Func<T, string> StateStringifier { get; }

    public StateWriterState(Func<T, string, T> stateAppender, Func<T, string> stateStringifier)
    {
      StateAppender = stateAppender;
      StateStringifier = stateStringifier;
    }
  }
}
