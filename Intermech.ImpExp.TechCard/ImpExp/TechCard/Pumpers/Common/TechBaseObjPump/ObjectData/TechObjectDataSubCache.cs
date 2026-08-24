// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectData.TechObjectDataSubCache
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectData;

internal class TechObjectDataSubCache
{
  private readonly Dictionary<string, TechObjectDataSubList> _data = new Dictionary<string, TechObjectDataSubList>();

  public List<TechObjectDataSub> GetTechRecs(string dopType, int parentKey)
  {
    TechObjectDataSubList objectDataSubList;
    TechObjectDataSub techObjectDataSub;
    if (!this._data.TryGetValue(dopType, out objectDataSubList) || !objectDataSubList.TryGetValue(parentKey, out techObjectDataSub))
      return (List<TechObjectDataSub>) null;
    return new List<TechObjectDataSub>()
    {
      techObjectDataSub
    };
  }

  public void AddTechDataRec(string dopType, TechObjectRecordSub recDopBase)
  {
    if (recDopBase == null)
      return;
    TechObjectDataSubList objectDataSubList;
    if (!this._data.TryGetValue(dopType, out objectDataSubList))
    {
      objectDataSubList = new TechObjectDataSubList();
      this._data.Add(dopType, objectDataSubList);
    }
    TechObjectDataSub techObjectDataSub;
    if (!objectDataSubList.TryGetValue(recDopBase.ParentKey, out techObjectDataSub))
    {
      techObjectDataSub = new TechObjectDataSub(recDopBase.ParentKey);
      objectDataSubList.Add(recDopBase.ParentKey, techObjectDataSub);
    }
    techObjectDataSub.AddToList(recDopBase);
  }

  internal void RemoveTechRecs(int parentKey)
  {
    foreach (Dictionary<int, TechObjectDataSub> dictionary in this._data.Values)
      dictionary.Remove(parentKey);
  }

  internal void Clear() => this._data.Clear();
}
