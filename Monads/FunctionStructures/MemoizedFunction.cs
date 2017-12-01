using System;
using System.Collections.Generic;

namespace Monads.FunctionStructures
{
  public class MemoizedFunction<T, U>
  {
    private readonly Func<T, U> MemoizedFuntion;
    private readonly Dictionary<T, U> MemoizedResults;

    public MemoizedFunction(Func<T, U> func, IEqualityComparer<T> comparer)
    {
      MemoizedFuntion = func;
      MemoizedResults = new Dictionary<T, U>(comparer);
    }

    public MemoizedFunction(Func<T, U> func)
      : this(func, EqualityComparer<T>.Default) { }

    public U Apply(T t)
    {
      if (MemoizedResults.TryGetValue(t, out U u))
        return u;

      u = MemoizedFuntion(t);
      MemoizedResults.Add(t, u);
      return u;
    }
  }
}
