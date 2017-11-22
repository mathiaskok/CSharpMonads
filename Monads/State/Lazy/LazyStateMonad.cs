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

    public LazyStateMonad<UValue, TState> Bind<UValue>(Func<TValue, Func<TState, (UValue, TState)>> binder)
    {
      Func<TState, (UValue, TState)> func = s =>
      {
        (var tv, var ts) = StateFunction(s);
        return binder(tv)(ts);
      };

      return new LazyStateMonad<UValue, TState>(func);
    }
  }
}
