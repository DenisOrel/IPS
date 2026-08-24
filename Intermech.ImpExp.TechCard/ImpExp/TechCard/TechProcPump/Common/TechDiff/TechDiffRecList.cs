// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff.TechDiffRecList
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff;

internal class TechDiffRecList : Dictionary<int, TechDiffRec>
{
  public void Add(
    int key,
    int tpRecKey,
    int docTcKey,
    int artTcKey,
    string entity,
    int row,
    string strValue,
    double numValue,
    int entType)
  {
    TechDiffRec techDiffRec;
    if (!this.TryGetValue(tpRecKey, out techDiffRec))
    {
      techDiffRec = new TechDiffRec();
      this.Add(tpRecKey, techDiffRec);
    }
    techDiffRec.Add(key, tpRecKey, docTcKey, artTcKey, entity, row, strValue, numValue, entType);
  }

  public List<TechDiffElement> GetArtListByObjID(int tpRecKey)
  {
    List<TechDiffElement> artListByObjId = new List<TechDiffElement>();
    TechDiffRec techDiffRec;
    if (!this.TryGetValue(tpRecKey, out techDiffRec))
      return artListByObjId;
    artListByObjId.AddRange((IEnumerable<TechDiffElement>) techDiffRec.Diff);
    return artListByObjId;
  }
}
