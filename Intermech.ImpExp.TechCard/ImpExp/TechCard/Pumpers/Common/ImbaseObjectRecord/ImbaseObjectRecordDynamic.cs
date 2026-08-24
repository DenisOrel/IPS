// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord.ImbaseObjectRecordDynamic
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord;

internal class ImbaseObjectRecordDynamic(string tableName) : TechObjectRecordDynamic(tableName)
{
  protected override void AddFields(string fieldName, object value)
  {
    base.AddFields(fieldName, value);
  }

  public override void Parse(IDataReader dataReader) => base.Parse(dataReader);

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    this.Idx_F_KEY = schema["F_LEVEL"];
  }
}
