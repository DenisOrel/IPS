// Decompiled with JetBrains decompiler
// Type: OxyPlot.ElementCollectionChangedEventArgs`1
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot;

public class ElementCollectionChangedEventArgs<T> : EventArgs
{
  public ElementCollectionChangedEventArgs(IEnumerable<T> addedItems, IEnumerable<T> removedItems)
  {
    this.AddedItems = new List<T>((IEnumerable<T>) ((object) addedItems ?? (object) new T[0]));
    this.RemovedItems = new List<T>((IEnumerable<T>) ((object) removedItems ?? (object) new T[0]));
  }

  public List<T> AddedItems { get; private set; }

  public List<T> RemovedItems { get; private set; }
}
