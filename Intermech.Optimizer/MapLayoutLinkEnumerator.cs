// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutLinkEnumerator
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;
using System.Collections;

#nullable disable
namespace Intermech.Map.Layout;

public struct MapLayoutLinkEnumerator : IEnumerator, IEnumerable
{
  private ArrayList myArray;
  private int myIndex;

  internal MapLayoutLinkEnumerator(ArrayList a)
  {
    this.myArray = a;
    this.myIndex = -1;
    this.Reset();
  }

  private MapLayoutNetworkLink GetCurrent()
  {
    if (this.myIndex >= 0 && this.myIndex < this.myArray.Count)
      return (MapLayoutNetworkLink) this.myArray[this.myIndex];
    throw new InvalidOperationException("MapLayoutNetworkLink.Enumerator is not at a valid position for the ArrayList");
  }

  public MapLayoutLinkEnumerator GetEnumerator()
  {
    MapLayoutLinkEnumerator enumerator = this;
    enumerator.Reset();
    return enumerator;
  }

  public bool MoveNext()
  {
    if (this.myIndex + 1 >= this.myArray.Count)
      return false;
    ++this.myIndex;
    return true;
  }

  public void Reset() => this.myIndex = -1;

  IEnumerator IEnumerable.GetEnumerator()
  {
    MapLayoutLinkEnumerator enumerator = this;
    enumerator.Reset();
    return (IEnumerator) enumerator;
  }

  object IEnumerator.Current => (object) this.GetCurrent();

  public MapLayoutNetworkLink Current => this.GetCurrent();
}
