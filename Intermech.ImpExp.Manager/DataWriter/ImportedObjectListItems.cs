// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ImportedObjectListItems
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

public class ImportedObjectListItems : IImportedObjectListItems
{
  private List<ImportingObject> _items = new List<ImportingObject>();
  private int _currentIndex = -1;

  public ImportingObject this[int index]
  {
    get => this._items[index];
    set => this._items[index] = value;
  }

  public void Clear()
  {
    this._items.Clear();
    this._currentIndex = -1;
  }

  public int Count => this._items.Count;

  public void Add(ImportingObject io)
  {
    this._items.Add(io);
    this._currentIndex = this._items.Count - 1;
  }

  public bool UseObject(long objectID)
  {
    for (int index = 0; index < this._items.Count; ++index)
    {
      if (this._items[index].Object.Object_id == objectID)
      {
        this._currentIndex = index;
        return true;
      }
    }
    return false;
  }

  public bool UseObject(Guid objectGuid)
  {
    for (int index = 0; index < this._items.Count; ++index)
    {
      object objectGuid1 = this._items[index].Object.ObjectGuid;
      if (objectGuid1 != null && objectGuid1.Equals((object) objectGuid))
      {
        this._currentIndex = index;
        return true;
      }
    }
    return false;
  }

  public ImportingObject[] ToArray() => this._items.ToArray();

  public int CurrentIndex => this._currentIndex;
}
