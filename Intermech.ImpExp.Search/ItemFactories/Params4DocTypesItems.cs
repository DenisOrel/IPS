// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.Params4DocTypesItems
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class Params4DocTypesItems : IParams4DocTypesItems
{
  private Dictionary<int, List<int>> params4docTypes;

  public Params4DocTypesItems() => this.params4docTypes = new Dictionary<int, List<int>>();

  public void AddItem(int docTypeID, int groupID)
  {
    if (!this.params4docTypes.ContainsKey(docTypeID))
      this.params4docTypes.Add(docTypeID, new List<int>());
    if (this.params4docTypes[docTypeID].Contains(groupID))
      return;
    this.params4docTypes[docTypeID].Add(groupID);
  }

  public List<int> GetGroups(int docTypeID)
  {
    return this.params4docTypes.ContainsKey(docTypeID) ? this.params4docTypes[docTypeID] : (List<int>) null;
  }
}
