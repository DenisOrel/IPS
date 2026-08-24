// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectData.TechObjectDataSub
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectData;

internal class TechObjectDataSub
{
  protected int _parentKey;
  protected readonly List<TechObjectRecordSub> _recordList;

  protected void InitData()
  {
  }

  public TechObjectDataSub(int parentKey)
  {
    this._recordList = new List<TechObjectRecordSub>();
    this.InitData();
    this._parentKey = parentKey;
  }

  public void AddToList(TechObjectRecordSub recordSub)
  {
    if (recordSub == null)
      return;
    this._recordList.Add(recordSub);
  }

  public int ParentKey => this._parentKey;

  public List<TechObjectRecordSub> Records => this._recordList;
}
