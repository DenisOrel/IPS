// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_MAT.TechMaterialObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_MAT;

internal class TechMaterialObject : TechObjectRecordUniqueDynamic
{
  public TechMaterialObject()
    : base("TP_MAT")
  {
  }

  protected override void AddFields(string fieldName, object value)
  {
    this._fields.Add(fieldName, value);
  }

  public override void Parse(IDataReader idr) => base.Parse(idr);
}
