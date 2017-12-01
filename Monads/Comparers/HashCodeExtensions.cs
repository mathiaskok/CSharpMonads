using System.Collections.Generic;
using System.Linq;

namespace Monads.Comparers
{
  public static class HashCodeExtensions
  {
    public const int BaseHashCode = 10007; //Large Prime
    public const int StepHashCode = 486187739; //Large Prime

    public static int HashWith<T>(this int i, T t)
    {
      return (i * StepHashCode) + t.GetHashCode();
    }

    public static int HashWith<T>(this int i, int o)
    {
      return (i * StepHashCode) + o;
    }

    public static int SequenceHashCode(this IEnumerable<int> seq)
    {
      return seq.Aggregate(BaseHashCode, HashWith);
    }

    public static int SequenceHashCode<T>(this IEnumerable<T> seq, IEqualityComparer<T> comparer)
    {
      return seq
        .Select(comparer.GetHashCode)
        .SequenceHashCode();
    }

    public static int SequenceHashCode<T>(this IEnumerable<T> seq)
    {
      return seq.SequenceHashCode(EqualityComparer<T>.Default);
    }
  }
}
