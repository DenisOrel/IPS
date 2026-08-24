// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.MadeTypesItem
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class MadeTypesItem : IMadeTypesItem
{
  private Dictionary<int, List<int>> _docTypes;

  public MadeTypesItem() => this._docTypes = new Dictionary<int, List<int>>();

  public void AddItem(int docTypeID, int articleTypeID)
  {
    if (!this._docTypes.ContainsKey(docTypeID))
      this._docTypes.Add(docTypeID, new List<int>());
    if (this._docTypes[docTypeID].Contains(articleTypeID))
      return;
    this._docTypes[docTypeID].Add(articleTypeID);
  }

  public List<int> GetObjectTypes(int docTypeID)
  {
    return this._docTypes.ContainsKey(docTypeID) ? this._docTypes[docTypeID] : (List<int>) null;
  }
}
