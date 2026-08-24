// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ImportedRelationListItems
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

public class ImportedRelationListItems : IImportedRelationListItems
{
  private List<ImportingRelation> _items = new List<ImportingRelation>();
  private int _currentIndex = -1;

  public ImportingRelation this[int index]
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

  public void Add(ImportingRelation io)
  {
    this._items.Add(io);
    this._currentIndex = this._items.Count - 1;
  }

  public ImportingRelation[] ToArray()
  {
    if (this._items.Count <= 0)
      return this._items.ToArray();
    List<ImportingRelation> importingRelationList1 = new List<ImportingRelation>();
    foreach (ImportingRelation importingRelation1 in this._items)
    {
      ImportingRelation importingRelation2;
      if (importingRelation1 != null && (importingRelation2 = importingRelation1) != null)
      {
        List<ImportingRelation> importingRelationList2 = importingRelationList1;
        ImportingRelation importingRelation3 = new ImportingRelation(importingRelation2.Relation);
        importingRelation3.Attributes = importingRelation2.Attributes;
        importingRelationList2.Add(importingRelation3);
      }
    }
    return importingRelationList1.ToArray();
  }

  public int CurrentIndex => this._currentIndex;

  public bool UseRelation(long prjLinkID)
  {
    for (int index = 0; index < this._items.Count; ++index)
    {
      if (this._items[index].Relation.PrjLinkId == prjLinkID)
      {
        this._currentIndex = index;
        return true;
      }
    }
    return false;
  }

  public bool UseRelation(long projID, long partID)
  {
    for (int index = 0; index < this._items.Count; ++index)
    {
      if (Convert.ToInt64(this._items[index].Relation.ProjId) == projID && Convert.ToInt64(this._items[index].Relation.PartId) == partID)
      {
        this._currentIndex = index;
        return true;
      }
    }
    return false;
  }
}
