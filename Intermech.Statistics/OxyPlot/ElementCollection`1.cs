// Decompiled with JetBrains decompiler
// Type: OxyPlot.ElementCollection`1
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot;

public class ElementCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : Element
{
  private readonly Model parent;
  private readonly List<T> internalList = new List<T>();

  public ElementCollection(Model parent) => this.parent = parent;

  public event EventHandler<ElementCollectionChangedEventArgs<T>> CollectionChanged;

  public int Count => this.internalList.Count;

  public bool IsReadOnly => false;

  public T this[int index]
  {
    get => this.internalList[index];
    set
    {
      value.Parent = this.parent;
      this.internalList[index] = value;
    }
  }

  public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.internalList.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public void Add(T item)
  {
    item.Parent = item.Parent == null ? this.parent : throw new InvalidOperationException("The element cannot be added, it already belongs to a PlotModel.");
    this.internalList.Add(item);
    this.RaiseCollectionChanged((IEnumerable<T>) new T[1]
    {
      item
    });
  }

  public void Clear()
  {
    List<T> removedItems = new List<T>();
    foreach (T obj in this.internalList)
    {
      obj.Parent = (Model) null;
      removedItems.Add(obj);
    }
    this.internalList.Clear();
    this.RaiseCollectionChanged(removedItems: (IEnumerable<T>) removedItems);
  }

  public bool Contains(T item) => this.internalList.Contains(item);

  public void CopyTo(T[] array, int arrayIndex) => this.internalList.CopyTo(array, arrayIndex);

  public bool Remove(T item)
  {
    item.Parent = (Model) null;
    bool flag = this.internalList.Remove(item);
    if (flag)
      this.RaiseCollectionChanged(removedItems: (IEnumerable<T>) new T[1]
      {
        item
      });
    return flag;
  }

  public int IndexOf(T item) => this.internalList.IndexOf(item);

  public void Insert(int index, T item)
  {
    item.Parent = item.Parent == null ? this.parent : throw new InvalidOperationException("The element cannot be inserted, it already belongs to a PlotModel.");
    this.internalList.Insert(index, item);
    this.RaiseCollectionChanged((IEnumerable<T>) new T[1]
    {
      item
    });
  }

  public void RemoveAt(int index)
  {
    T obj = this[index];
    obj.Parent = (Model) null;
    this.internalList.RemoveAt(index);
    this.RaiseCollectionChanged(removedItems: (IEnumerable<T>) new T[1]
    {
      obj
    });
  }

  private void RaiseCollectionChanged(IEnumerable<T> addedItems = null, IEnumerable<T> removedItems = null)
  {
    EventHandler<ElementCollectionChangedEventArgs<T>> collectionChanged = this.CollectionChanged;
    if (collectionChanged == null)
      return;
    collectionChanged((object) this, new ElementCollectionChangedEventArgs<T>(addedItems, removedItems));
  }
}
