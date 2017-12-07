using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Monads.Collections.ReadOnly;

namespace Monads.Collections
{
  public static class CollectionExtensions
  {
    public static IReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> seq)
    {
      if (seq is IReadOnlyCollection<T> rc)
        return rc;
      else if (seq is ICollection<T> c)
        return new ReadOnlyCollectionAdpater<T>(c);
      else if (seq is IList<T> l)
        return new ReadOnlyCollection<T>(l);
      else
        return seq.ToList().AsReadOnly();
    }
  }
}
