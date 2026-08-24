// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2Tp.TechRoute2TpObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2Tp;

internal class TechRoute2TpObject : TechObjectRecord
{
  private static int idx_F_ROUTE_KEY;
  private static int idx_F_TP_KEY;

  public TechRoute2TpObject() => this.TableName = "TC_TECH_LINKS";

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechRoute2TpObject.idx_F_ROUTE_KEY = schema["F_SOURCE_KEY"];
    TechRoute2TpObject.idx_F_TP_KEY = schema["F_TARGET_KEY"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_SOURCE_KEY", (object) (dataReader.IsDBNull(TechRoute2TpObject.idx_F_ROUTE_KEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoute2TpObject.idx_F_ROUTE_KEY])));
    this._fields.Add("F_TARGET_KEY", (object) (dataReader.IsDBNull(TechRoute2TpObject.idx_F_TP_KEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoute2TpObject.idx_F_TP_KEY])));
  }
}
