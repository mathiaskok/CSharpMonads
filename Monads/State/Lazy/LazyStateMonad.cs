using System;
using System.Collections.Generic;
using System.Text;

namespace Monads.State.Lazy
{
  public class LazyStateMonad<TValue, TState>
  {
    public Func<TState, (TValue, TState)> StateFunction { get; }

    private LazyStateMonad(Func<TState, (TValue, TState)> stateFunction)
    {
      StateFunction = stateFunction;
    }

    public static LazyStateMonad<TValue, TState> Return(TValue value) =>
      new LazyStateMonad<TValue, TState>(s => (value, s));

    public LazyStateMonad<UValue, TState> Bind<UValue>(Func<TValue, Func<TState, (UValue, TState)>> binder)
    {
      return new LazyStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        return binder(tv)(ts);
      });
    }

    public LazyStateMonad<UValue, TState> Bind<UValue>(Func<TValue, TState, (UValue, TState)> binder)
    {
      return new LazyStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        return binder(tv, ts);
      });
    }

    public LazyStateMonad<UValue, TState> Bind<UValue>(Func<TValue, Func<TState, Tuple<UValue, TState>>> binder)
    {
      return new LazyStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        (var uv, var us) = binder(tv)(ts);
        return (uv, us);
      });
    }

    public LazyStateMonad<UValue, TState> Bind<UValue>(Func<TValue, TState, Tuple<UValue, TState>> binder)
    {
      return new LazyStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        (var uv, var us) = binder(tv, ts);
        return (uv, us);
      });
    }

    public LazyStateMonad<UValue, TState> Bind<UValue>(Func<TValue, Func<TState, KeyValuePair<UValue, TState>>> binder)
    {
      return new LazyStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        var kvp = binder(tv)(ts);
        return (kvp.Key, kvp.Value);
      });
    }

    public LazyStateMonad<UValue, TState> Bind<UValue>(Func<TValue, TState, KeyValuePair<UValue, TState>> binder)
    {
      return new LazyStateMonad<UValue, TState>(s =>
      {
        (var tv, var ts) = StateFunction(s);
        var kvp = binder(tv, ts);
        return (kvp.Key, kvp.Value);
      });
    }
  }
}
