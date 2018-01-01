using System;
using System.Collections.Generic;

namespace Monads.State
{
  public class StateMonad<TValue, TState>
  {
    public Func<TState, (TValue, TState)> StateFunction { get; }

    private StateMonad(Func<TState, (TValue, TState)> stateFunction)
    {
      StateFunction = stateFunction;
    }

    public static StateMonad<TValue, TState> Return(TValue value) =>
      new StateMonad<TValue, TState>(s => (value, s));

    private UValue IdentityValueSelector<UValue>(TValue t, UValue u) => u;

    public StateMonad<UValue, TState> SelectMany<UValue>(
      Func<TValue, Func<TState, (UValue, TState)>> binder)
    {
      return SelectMany(binder, IdentityValueSelector);
    }

    public StateMonad<VValue, TState> SelectMany<UValue, VValue>(
      Func<TValue, Func<TState, (UValue, TState)>> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      return new StateMonad<VValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        (var uv, var us) = binder(tv)(ts);
        return (valueSelector(tv, uv), us);
      });
    }

    public StateMonad<UValue, TState> SelectMany<UValue>(
      Func<TValue, TState, (UValue, TState)> binder)
    {
      return SelectMany(binder, IdentityValueSelector);
    }

    public StateMonad<VValue, TState> SelectMany<UValue, VValue>(
      Func<TValue, TState, (UValue, TState)> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      return new StateMonad<VValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        (var uv, var us) = binder(tv, ts);
        return (valueSelector(tv, uv), us);
      });
    }

    public StateMonad<UValue, TState> SelectMany<UValue>(
      Func<TValue, Func<TState, Tuple<UValue, TState>>> binder)
    {
      return SelectMany(binder, IdentityValueSelector);
    }

    public StateMonad<VValue, TState> SelectMany<UValue, VValue>(
      Func<TValue, Func<TState, Tuple<UValue, TState>>> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      return new StateMonad<VValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        (var uv, var us) = binder(tv)(ts);
        return (valueSelector(tv, uv), us);
      });
    }

    public StateMonad<UValue, TState> SelectMany<UValue>(
      Func<TValue, TState, Tuple<UValue, TState>> binder)
    {
      return SelectMany(binder, IdentityValueSelector);
    }

    public StateMonad<VValue, TState> SelectMany<UValue, VValue>(
      Func<TValue, TState, Tuple<UValue, TState>> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      return new StateMonad<VValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        (var uv, var us) = binder(tv, ts);
        return (valueSelector(tv, uv), us);
      });
    }

    public StateMonad<UValue, TState> SelectMany<UValue>(
      Func<TValue, Func<TState, KeyValuePair<UValue, TState>>> binder)
    {
      return SelectMany(binder, IdentityValueSelector);
    }

    public StateMonad<VValue, TState> SelectMany<UValue, VValue>(
      Func<TValue, Func<TState, KeyValuePair<UValue, TState>>> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      return new StateMonad<VValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        var kvp = binder(tv)(ts);
        return (valueSelector(tv, kvp.Key), kvp.Value);
      });
    }

    public StateMonad<UValue, TState> SelectMany<UValue>(
      Func<TValue, TState, KeyValuePair<UValue, TState>> binder)
    {
      return SelectMany(binder, IdentityValueSelector);
    }

    public StateMonad<VValue, TState> SelectMany<UValue, VValue>(
      Func<TValue, TState, KeyValuePair<UValue, TState>> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      return new StateMonad<VValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        var kvp = binder(tv, ts);
        return (valueSelector(tv, kvp.Key), kvp.Value);
      });
    }

    public StateMonad<UValue, TState> SelectMany<UValue>(
      Func<TValue, StateMonad<UValue, TState>> binder)
    {
      return SelectMany(binder, IdentityValueSelector);
    }

    public StateMonad<VValue, TState> SelectMany<UValue, VValue>(
      Func<TValue, StateMonad<UValue, TState>> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      return new StateMonad<VValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        (var uv, var us) = binder(tv).StateFunction(ts);
        return (valueSelector(tv, uv), us);
      });
    }

    public StateMonad<VValue, TState> Combine<UValue, VValue>(
      StateMonad<UValue, TState> other,
      Func<TValue, UValue, VValue> valueCombiner,
      Func<TState, TState, TState> stateCombiner)
    {
      return new StateMonad<VValue, TState>(s =>
      {
        (var v1, var s1) = StateFunction(s);
        (var v2, var s2) = other.StateFunction(s);

        return (valueCombiner(v1, v2), stateCombiner(s1, s2));
      });
    }

    public StateMonad<UValue, TState> Select<UValue>(Func<TValue, UValue> mapper)
    {
      return new StateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        return (mapper(tv), ts);
      });
    }

    public StateMonad<TValue, TState> Transform(Func<TState, TState> stateTransformer)
    {
      return new StateMonad<TValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        return (tv, stateTransformer(ts));
      });
    }
  }
}
