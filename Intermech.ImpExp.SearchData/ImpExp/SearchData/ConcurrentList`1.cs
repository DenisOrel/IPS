// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ConcurrentList`1
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class ConcurrentList<T> : List<T>
{
  private readonly ReaderWriterLockSlim _listLock = new ReaderWriterLockSlim();
  private readonly List<T> _holder = new List<T>();

  public new void Add(T item)
  {
    this._listLock.EnterWriteLock();
    try
    {
      this._holder.Add(item);
    }
    finally
    {
      this._listLock.ExitWriteLock();
    }
  }

  public new int Count
  {
    get
    {
      this._listLock.EnterReadLock();
      try
      {
        return this._holder.Count;
      }
      finally
      {
        this._listLock.ExitReadLock();
      }
    }
  }

  public new T Find(Predicate<T> match)
  {
    this._listLock.EnterReadLock();
    try
    {
      return this._holder.Find(match);
    }
    finally
    {
      this._listLock.ExitReadLock();
    }
  }

  public void RemoveAll(Predicate<T> match)
  {
    this._listLock.EnterWriteLock();
    try
    {
      this._holder.RemoveAll(match);
    }
    finally
    {
      this._listLock.ExitWriteLock();
    }
  }

  public new List<T> GetRange(int index, int count)
  {
    this._listLock.EnterReadLock();
    try
    {
      return this._holder.GetRange(index, count);
    }
    finally
    {
      this._listLock.ExitReadLock();
    }
  }

  public new void RemoveRange(int index, int count)
  {
    this._listLock.EnterWriteLock();
    try
    {
      this._holder.RemoveRange(index, count);
    }
    finally
    {
      this._listLock.ExitWriteLock();
    }
  }

  public new bool Exists(Predicate<T> match)
  {
    this._listLock.EnterReadLock();
    try
    {
      return this._holder.Exists(match);
    }
    finally
    {
      this._listLock.ExitReadLock();
    }
  }
}
