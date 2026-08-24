// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.DraftPump.DraftOLEObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.DraftPump;

internal class DraftOLEObject(TechRecordParser parser) : TechObjectRecordDynamic("TP_OLE", parser)
{
  protected override void AddFields(string fieldName, object value)
  {
    this._fields.Add(fieldName, value);
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    if (!this.FieldExist("F_KEY"))
      return;
    this.Key = Convert.ToInt32(this.Fields["F_KEY"]);
    this.baseKey = this.Key;
  }
}
