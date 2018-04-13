using System.Collections.Generic;
using System.Collections.Immutable;
using Monads.EnumerableExtensions;

namespace Monads.Collections.Immutable
{
  public static class ImmutableStackExtensions
  {
    public static void Deconstruct<T>(this IImmutableStack<T> src, out T head, out IImmutableStack<T> tail)
    {
      head = src.Peek();
      tail = src.Pop();
    }

    public static IImmutableStack<T> Append<T>(this IImmutableStack<T> snd, IEnumerable<T> fst) =>
      fst.FoldRight(snd, (v, s) => s.Push(v));

    public static IImmutableStack<T> Concat<T>(IEnumerable<T> fst, IImmutableStack<T> snd) =>
      snd.Append(fst);
  }
}
