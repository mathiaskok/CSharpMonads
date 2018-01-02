using System;
using System.Collections.Generic;
using System.Text;

namespace Monads.FunctionStructures
{
  public static class StandardFunctions
  {
    public static T IdFunc<T>(T t) => t;

    public static UValue IdValueSelector<TValue, UValue>(TValue t, UValue u) => u;
  }
}
