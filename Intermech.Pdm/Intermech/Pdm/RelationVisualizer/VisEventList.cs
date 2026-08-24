// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisEventList
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisEventList : 
  IList<VisDBEvent>,
  ICollection<VisDBEvent>,
  IEnumerable<VisDBEvent>,
  IEnumerable
{
  internal Dictionary<long, List<int>> _index;
  internal List<VisDBEvent> _list;

  public HashSet<long> ObjIdents { get; private set; }

  public HashSet<long> RelIdents { get; private set; }

  public IEnumerable<VisDBEvent> GetEventsForId(long Id)
  {
    List<int> source;
    return this._index.TryGetValue(Id, out source) ? source.Select<int, VisDBEvent>((Func<int, VisDBEvent>) (index => this._list[index])) : (IEnumerable<VisDBEvent>) null;
  }

  public static bool WasDeleted(IEnumerable<VisDBEvent> eventList)
  {
    return eventList.Any<VisDBEvent>((Func<VisDBEvent, bool>) (ev => (ev.EventCode & EvCode.Deleted) != 0));
  }

  public static bool WasModified(IEnumerable<VisDBEvent> eventList)
  {
    return eventList.Any<VisDBEvent>((Func<VisDBEvent, bool>) (ev => (ev.EventCode & EvCode.Modified) != 0));
  }

  public static bool WasIdChanged(IEnumerable<VisDBEvent> eventList)
  {
    return eventList.Any<VisDBEvent>((Func<VisDBEvent, bool>) (ev => (ev.EventCode & EvCode.IdChanged) != 0));
  }

  public static bool WasRelationCreated(IEnumerable<VisDBEvent> eventList, long relId)
  {
    return eventList.Any<VisDBEvent>((Func<VisDBEvent, bool>) (ev => (ev.EventCode & EvCode.Created) != EvCode.NoEvent && ev.RelId == relId));
  }

  public bool WasDeleted()
  {
    return this._list.Any<VisDBEvent>((Func<VisDBEvent, bool>) (ev => (ev.EventCode & EvCode.Deleted) != 0));
  }

  public bool WasModified()
  {
    return this._list.Any<VisDBEvent>((Func<VisDBEvent, bool>) (ev => (ev.EventCode & EvCode.Modified) != 0));
  }

  public bool WasRelationCreated()
  {
    return this._list.Any<VisDBEvent>((Func<VisDBEvent, bool>) (ev => (ev.EventCode & EvCode.Created) != 0));
  }

  public VisEventList()
  {
    this.ObjIdents = (HashSet<long>) null;
    this.RelIdents = (HashSet<long>) null;
    this._list = new List<VisDBEvent>();
    this._index = new Dictionary<long, List<int>>();
  }

  public VisEventList(HashSet<long> objIdents, HashSet<long> relIdents)
    : this()
  {
    this.InitIdents(objIdents, relIdents);
  }

  public void InitIdents(HashSet<long> objIdents, HashSet<long> relIdents)
  {
    this.ObjIdents = objIdents;
    this.RelIdents = relIdents;
  }

  public VisEventList(MapDocument md)
    : this()
  {
    this.InitIdents(md);
  }

  public void InitIdents(MapDocument md)
  {
    if (this.ObjIdents == null)
      this.ObjIdents = new HashSet<long>();
    else
      this.ObjIdents.Clear();
    if (this.RelIdents == null)
      this.RelIdents = new HashSet<long>();
    else
      this.RelIdents.Clear();
    foreach (MapObject mapObject in md.GetEnumerator())
    {
      if (mapObject is VisNode)
        this.ObjIdents.Add(Math.Abs(((VisNode) mapObject).ObjId));
      if (mapObject is VisLink)
        this.RelIdents.Add(Math.Abs(((VisLink) mapObject).RelId));
    }
  }

  internal void _RegisterIndexForId(long Id, int index)
  {
    List<int> intList1;
    if (!this._index.TryGetValue(Id, out intList1))
    {
      List<int> intList2 = new List<int>() { index };
      this._index[Id] = intList2;
    }
    else
    {
      if (intList1.Contains(index))
        return;
      intList1.Add(index);
    }
  }

  internal void _RegisterEvent(VisDBEvent ev, int index)
  {
    if (ev.ObjId != 0L)
      this._RegisterIndexForId(ev.ObjId, index);
    if (ev.RelId == 0L)
      return;
    this._RegisterIndexForId(ev.RelId, index);
  }

  internal void _UnregisterEvent(int index)
  {
    VisDBEvent visDbEvent = this._list[index];
    foreach (KeyValuePair<long, List<int>> keyValuePair in this._index)
    {
      if (keyValuePair.Value.Contains(index))
        keyValuePair.Value.Remove(index);
    }
  }

  internal bool MyEvent(VisDBEvent ev)
  {
    return this.ObjIdents.Contains(Math.Abs(ev.ObjId)) || this.RelIdents.Contains(Math.Abs(ev.RelId));
  }

  public int Count => this._list.Count;

  public bool IsReadOnly => false;

  public VisDBEvent this[int index]
  {
    get => this._list[index];
    set
    {
      if (!this.MyEvent(value))
        return;
      this._UnregisterEvent(index);
      this._list[index] = value;
      this._RegisterEvent(value, index);
    }
  }

  public int IndexOf(VisDBEvent item) => this._list.IndexOf(item);

  public void Insert(int index, VisDBEvent item)
  {
  }

  public void RemoveAt(int index) => this._list.RemoveAt(index);

  public void Add(VisDBEvent item)
  {
    if (!this.MyEvent(item))
      return;
    this._list.Add(item);
    this._RegisterEvent(item, this._list.Count - 1);
  }

  public void Clear()
  {
    this._list.Clear();
    this._index.Clear();
  }

  public bool Contains(VisDBEvent item) => this._list.Contains(item);

  public void CopyTo(VisDBEvent[] array, int arrayIndex) => this._list.CopyTo(array, arrayIndex);

  public bool Remove(VisDBEvent item) => false;

  public IEnumerator<VisDBEvent> GetEnumerator()
  {
    return (IEnumerator<VisDBEvent>) this._list.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();
}
