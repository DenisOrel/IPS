// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump.TechProcessObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump;

internal class TechProcessObject : TechObjectRecordDynamic
{
  public TechProcessObject()
    : base("TC_ARCDOCS")
  {
  }

  protected override void AddFields(string fieldName, object value)
  {
    this._fields.Add(fieldName, value);
  }

  public override void Parse(IDataReader idr)
  {
    base.Parse(idr);
    this.Key = Convert.ToInt32(this.Fields["F_KEY"]);
    this.baseKey = this.Key;
  }

  internal enum TechProcess
  {
    OneTP = 1,
    TPOnType = 4,
    TPType = 6,
    GTP = 7,
  }
}
