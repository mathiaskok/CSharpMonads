using System;
using System.Collections.Generic;

namespace Monads.State.Functional
{
  public class FunctionalStateMonad<TValue, TState>
  {
    public Func<TState, (TValue, TState)> StateFunction { get; }

    private FunctionalStateMonad(Func<TState, (TValue, TState)> stateFunction)
    {
      StateFunction = stateFunction;
    }

    public static FunctionalStateMonad<TValue, TState> Return(TValue value) =>
      new FunctionalStateMonad<TValue, TState>(s => (value, s));

    public FunctionalStateMonad<UValue, TState> Bind<UValue>(Func<TValue, Func<TState, (UValue, TState)>> binder)
    {
      return new FunctionalStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        return binder(tv)(ts);
      });
    }

    public FunctionalStateMonad<UValue, TState> Bind<UValue>(Func<TValue, TState, (UValue, TState)> binder)
    {
      return new FunctionalStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        return binder(tv, ts);
      });
    }

    public FunctionalStateMonad<UValue, TState> Bind<UValue>(Func<TValue, Func<TState, Tuple<UValue, TState>>> binder)
    {
      return new FunctionalStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        (var uv, var us) = binder(tv)(ts);
        return (uv, us);
      });
    }

    public FunctionalStateMonad<UValue, TState> Bind<UValue>(Func<TValue, TState, Tuple<UValue, TState>> binder)
    {
      return new FunctionalStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        (var uv, var us) = binder(tv, ts);
        return (uv, us);
      });
    }

    public FunctionalStateMonad<UValue, TState> Bind<UValue>(Func<TValue, Func<TState, KeyValuePair<UValue, TState>>> binder)
    {
      return new FunctionalStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        var kvp = binder(tv)(ts);
        return (kvp.Key, kvp.Value);
      });
    }

    public FunctionalStateMonad<UValue, TState> Bind<UValue>(Func<TValue, TState, KeyValuePair<UValue, TState>> binder)
    {
      return new FunctionalStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        var kvp = binder(tv, ts);
        return (kvp.Key, kvp.Value);
      });
    }

    public FunctionalStateMonad<UValue, TState> Bind<UValue>(Func<TValue, FunctionalStateMonad<UValue, TState>> binder)
    {
      return new FunctionalStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        return binder(tv).StateFunction(ts);
      });
    }

    public FunctionalStateMonad<VValue, TState> Combine<UValue, VValue>(
      FunctionalStateMonad<UValue, TState> other,
      Func<TValue, UValue, VValue> valueCombiner,
      Func<TState, TState, TState> stateCombiner)
    {
      return new FunctionalStateMonad<VValue, TState>(s =>
      {
        (var v1, var s1) = StateFunction(s);
        (var v2, var s2) = other.StateFunction(s);

        return (valueCombiner(v1, v2), stateCombiner(s1, s2));
      });
    }

    public FunctionalStateMonad<UValue, TState> Map<UValue>(Func<TValue, UValue> mapper)
    {
      return new FunctionalStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        return (mapper(tv), ts);
      });
    }

    public FunctionalStateMonad<TValue, TState> Transform(Func<TState, TState> stateTransformer)
    {
      return new FunctionalStateMonad<TValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        return (tv, stateTransformer(ts));
      });
    }
  }
}
