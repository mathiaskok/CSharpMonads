using System;
using System.Collections;
using System.Collections.Generic;

namespace Monads.Unit
{
  public struct UnitType : 
    IEquatable<UnitType>, 
    IComparable<UnitType>,
    IComparable,
    IEnumerable<UnitType>
  {
    public static readonly UnitType Value = new UnitType();

    public override string ToString() => "()";

    public override int GetHashCode() => 15485867;

    public override bool Equals(object obj) => obj is UnitType;

    bool IEquatable<UnitType>.Equals(UnitType other) => true;

    int IComparable<UnitType>.CompareTo(UnitType other) => 0;

    int IComparable.CompareTo(object obj) =>
      obj is UnitType ? 0 : -1;

    public IEnumerator<UnitType> GetEnumerator()
    {
      yield return Value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}
