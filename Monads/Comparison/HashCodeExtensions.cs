using System;
using System.Collections.Generic;
using System.Linq;

namespace Monads.Comparison
{
  public static class HashCodeExtensions
  {
    public const int BaseHashCode = 10007; //Large Prime
    public const int StepHashCode = 486187739; //Large Prime

    public static int HashWith<T>(this int i, T t) =>
      (i * StepHashCode) + (t?.GetHashCode() ?? 0);

    public static int HashWith(this int i, int o) =>
      (i * StepHashCode) + o;

    public static int ArrayHashCode<T>(this T[] arr)
    {
      int l = arr.Length;
      int hash = BaseHashCode;

      for (int i = 0; i < l; i++)
        hash = (hash * StepHashCode) + (arr[i]?.GetHashCode() ?? 0);

      return hash;
    }

    public static int SequenceHashCode(this IEnumerable<int> seq) =>
      seq.Aggregate(BaseHashCode, HashWith);

    public static int SequenceHashCode<T>(this IEnumerable<T> seq, Func<T, int> hashFunc) =>
      seq
        .Select(hashFunc)
        .SequenceHashCode();

    public static int SequenceHashCode<T>(this IEnumerable<T> seq) =>
      seq.SequenceHashCode(EqualityComparer<T>.Default.GetHashCode);
  }
}
