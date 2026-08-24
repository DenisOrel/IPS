// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords.TechObjectRecordSub_D2
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;

internal class TechObjectRecordSub_D2 : TechObjectRecordSub_D
{
  public TechObjectRecordSub_D2() => this.TablePrefix = "_D2";

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    this.Idx_F_KEY = schema["F_KEY"];
  }

  public override void Parse(IDataReader dataReader)
  {
    this.Key = dataReader.IsDBNull(this.Idx_F_KEY) ? 0 : BasePumpHelper.ToInt32(dataReader[this.Idx_F_KEY]);
    this.baseKey = this.Key;
    int num1 = 0;
    int num2 = 0;
    double num3 = 0.0;
    string str = string.Empty;
    for (int i = 0; i < dataReader.FieldCount; ++i)
    {
      switch (dataReader.GetName(i))
      {
        case "F_ENTITY":
          this.Entity = dataReader.IsDBNull(i) ? string.Empty : dataReader.GetString(i);
          break;
        case "F_FLOATVAL":
          num3 = dataReader.IsDBNull(i) ? 0.0 : BasePumpHelper.ToDouble(dataReader[i]);
          break;
        case "F_INT_VAL":
          num2 = dataReader.IsDBNull(i) ? 0 : BasePumpHelper.ToInt32(dataReader[i]);
          break;
        case "F_PARENTKEY":
          this.ParentKey = dataReader.IsDBNull(i) ? 0 : BasePumpHelper.ToInt32(dataReader[i]);
          break;
        case "F_ROW":
          this.Row = dataReader.IsDBNull(i) ? 0 : BasePumpHelper.ToInt32(dataReader[i]);
          break;
        case "F_STRVAL":
          str = dataReader.IsDBNull(i) ? string.Empty : dataReader.GetString(i);
          break;
        case "F_TCKEY":
          this.TcKey = dataReader.IsDBNull(i) ? 0 : BasePumpHelper.ToInt32(dataReader[i]);
          break;
        case "F_TYPE":
          num1 = dataReader.IsDBNull(i) ? 0 : BasePumpHelper.ToInt32(dataReader[i]);
          break;
      }
    }
    switch (num1)
    {
      case 0:
        this.Value = (object) num2;
        break;
      case 1:
        this.Value = (object) num3;
        break;
      case 2:
        this.Value = (object) str;
        break;
    }
  }
}
