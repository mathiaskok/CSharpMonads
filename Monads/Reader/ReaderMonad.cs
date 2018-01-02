using System;
using static Monads.FunctionStructures.StandardFunctions;

namespace Monads.Reader
{
  public static class ReaderMonad
  {
    public static Func<TState, UValue> Select<TValue, UValue, TState>(
      this Func<TState, TValue> reader,
      Func<TValue, UValue> mapper)
    {
      return s => mapper(reader(s));
    }

    public static Func<TState, VValue> SelectMany<TValue, UValue, VValue, TState>(
      this Func<TState, TValue> reader,
      Func<TValue, TState, UValue> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      return s =>
      {
        TValue t = reader(s);
        UValue u = binder(t, s);
        return valueSelector(t, u);
      };
    }

    public static Func<TState, UValue> SelectMany<TValue, UValue, TState>(
      this Func<TState, TValue> reader,
      Func<TValue, TState, UValue> binder)
    {
      return reader.SelectMany(binder, IdValueSelector);
    }

    public static Func<TState, VValue> SelectMany<TValue, UValue, VValue, TState>(
      this Func<TState, TValue> reader,
      Func<TValue, Func<TState, UValue>> binder,
      Func<TValue, UValue, VValue> valueSelector)
    {
      return s =>
      {
        TValue t = reader(s);
        UValue u = binder(t)(s);
        return valueSelector(t, u);
      };
    }

    public static Func<TState, UValue> SelectMany<TValue, UValue, TState>(
      this Func<TState, TValue> reader,
      Func<TValue, Func<TState, UValue>> binder)
    {
      return reader.SelectMany(binder, IdValueSelector);
    }
  }
}
