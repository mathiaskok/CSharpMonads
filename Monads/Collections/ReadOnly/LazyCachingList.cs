using System.Collections;
using System.Collections.Generic;

namespace Monads.Collections.Immutable
{
  public class LazyCachingList<T> : IReadOnlyList<T>
  {
    private List<T> _list = new List<T>();
    private IEnumerator<T> _enumerator;

    public T this[int index] => Index(index);

    public int Count
    {
      get
      {
        ActualizeAll();
        return _list.Count;
      }
    }

    public LazyCachingList(IEnumerable<T> src)
    {
      _enumerator = src.GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
      return new LazyCachingEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private bool ActualizeNext()
    {
      if (_enumerator != null && _enumerator.MoveNext())
      {
        _list.Add(_enumerator.Current);
        return true;
      }
      else
      {
        _enumerator = null;
        return false;
      }
    }

    private void ActualizeAll()
    {
      while (ActualizeNext()) { }
    }

    private T Index(int i)
    {
      if (_enumerator == null || i < _list.Count)
        return _list[i];
      else
      {
        ActualizeNext();
        return Index(i);
      }
    }

    private class LazyCachingEnumerator : IEnumerator<T>
    {
      private int _i = -1;
      private LazyCachingList<T> _lazyList;

      public T Current
      {
        get
        {
          return _lazyList._list[_i];
        }
      }

      object IEnumerator.Current => Current;

      public LazyCachingEnumerator(LazyCachingList<T> lazy)
      {
        _lazyList = lazy;
      }

      public void Dispose() { }

      public bool MoveNext()
      {
        if (++_i >= _lazyList._list.Count)
          return _lazyList.ActualizeNext();
        else
          return true;
      }

      public void Reset()
      {
        _i = -1;
      }
    }
  }
}
