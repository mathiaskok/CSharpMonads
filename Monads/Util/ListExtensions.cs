﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monads.Util
{
  public static class ListExtensions
  {
    public static T BinaryMappend<T>(this IList<T> list, Func<T, T, T> monoid)
    {
      if (!list.Any())
        throw new ArgumentException("list must not be empty");

      T Internal(int start, int end)
      {
        if (start == end)
          return list[start];

        int diff = end - start;

        if (diff == 1)
          return monoid(list[start], list[end]);

        int split = diff / 2;
        int startSplit = start + split;

        return monoid(
          Internal(start, startSplit),
          Internal(startSplit + 1, end));
      }

      return Internal(0, list.Count - 1);
    }
  }
}