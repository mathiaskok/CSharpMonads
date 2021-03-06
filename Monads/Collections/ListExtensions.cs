﻿using System;
using System.Collections.Generic;
using System.Linq;
using Monads.Collections.Immutable;
using Monads.Collections.ReadOnly;

namespace Monads.Collections
{
  public static class ListExtensions
  {
    public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> list)
    {
      if (list is IReadOnlyList<T> rList)
        return rList;
      else
        return new ReadOnlyListAdapter<T>(list);
    }

    public static T BinaryMappend<T>(this IReadOnlyList<T> list, Func<T, T, T> semiGroup)
    {
      return list.Any() ?
         list.InternalBinaryMappend(semiGroup) :
         throw new ArgumentException("list must not be empty");
    }

    public static T BinaryMappend<T>(this IReadOnlyList<T> list, T defaultValue, Func<T, T, T> monoid)
    {
      return list.Any() ?
        list.InternalBinaryMappend(monoid) :
        defaultValue;
    }

    private static T InternalBinaryMappend<T>(this IReadOnlyList<T> list, Func<T, T, T> semiGroup)
    {
      T Internal(int start, int end)
      {
        if (start == end)
          return list[start];

        int diff = end - start;

        if (diff == 1)
          return semiGroup(list[start], list[end]);

        int split = diff / 2;
        int startSplit = start + split;

        return semiGroup(
          Internal(start, startSplit),
          Internal(startSplit + 1, end));
      }

      return Internal(0, list.Count - 1);
    }

    private static IReadOnlyList<T> Cache<T>(this IEnumerable<T> src) => new LazyCachingList<T>(src);
  }
}
