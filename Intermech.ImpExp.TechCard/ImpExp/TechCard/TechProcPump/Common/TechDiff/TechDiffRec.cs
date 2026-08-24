// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff.TechDiffRec
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff;

internal class TechDiffRec
{
  private int _recKey;
  private readonly List<TechDiffElement> _list = new List<TechDiffElement>();

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
    this._list.Add(new TechDiffElement(key, docTcKey, artTcKey, entity, row, strValue, numValue, entType));
    this._recKey = tpRecKey;
  }

  public int RecKey => this._recKey;

  public List<TechDiffElement> Diff => this._list;
}
