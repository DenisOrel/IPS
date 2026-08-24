// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroup.TechMaterialGroupObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroup;

internal class TechMaterialGroupObject : TechObjectRecordDynamic
{
  public TechMaterialGroupObject()
    : base("TP_MAT_GR")
  {
  }

  protected override void AddFields(string fieldName, object value)
  {
    this._fields.Add(fieldName, value);
  }

  public override void Parse(IDataReader idr) => base.Parse(idr);
}
