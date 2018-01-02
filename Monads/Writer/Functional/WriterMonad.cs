using System;
using System.Collections.Generic;
using static Monads.FunctionStructures.StandardFunctions;

namespace Monads.Writer.Functional
{
  public class WriterMonad<TValue, TState>
  {
    private readonly Func<TState, TState, TState> StateMonoid;

    public TValue Value { get; }
    public TState State { get; }

    public WriterMonad(TValue value, TState state, Func<TState, TState, TState> stateMonoid)
    {
      Value = value;
      State = state;
      StateMonoid = stateMonoid;
    }

    private WriterMonad<UValue, TState> NextWriter<UValue>(UValue newValue)
    {
      return new WriterMonad<UValue, TState>(newValue, State, StateMonoid);
    }

    private WriterMonad<UValue, TState> NextWriter<UValue>(UValue newValue, TState newState)
    {
      return new WriterMonad<UValue, TState>(newValue, StateMonoid(State, newState), StateMonoid);
    }

    public WriterMonad<UValue, TState> Select<UValue>(Func<TValue, UValue> mapper)
    {
      return NextWriter(mapper(Value));
    }

    public WriterMonad<UValue, TState> SelectMany<UValue>(
      Func<TValue, (UValue, TState)> binder)
    {
      return SelectMany(binder, IdValueSelector);
    }

    public WriterMonad<UValue, TState> SelectMany<UValue>(
      Func<TValue, Tuple<UValue, TState>> binder)
    {
      return SelectMany(binder, IdValueSelector);
    }

    public WriterMonad<UValue, TState> SelectMany<UValue>(
      Func<TValue, KeyValuePair<UValue, TState>> binder)
    {
      return SelectMany(binder, IdValueSelector);
    }

    public WriterMonad<VValue, TState> SelectMany<UValue, VValue>(
      Func<TValue, (UValue, TState)> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      var (v, s) = binder(Value);
      return NextWriter(valueSelector(Value, v), s);
    }

    public WriterMonad<VValue, TState> SelectMany<UValue, VValue>(
      Func<TValue, Tuple<UValue, TState>> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      var (v, s) = binder(Value);
      return NextWriter(valueSelector(Value, v), s);
    }

    public WriterMonad<VValue, TState> SelectMany<UValue, VValue>(
      Func<TValue, KeyValuePair<UValue, TState>> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      var kvp = binder(Value);
      return NextWriter(valueSelector(Value, kvp.Key), kvp.Value);
    }
  }
}
