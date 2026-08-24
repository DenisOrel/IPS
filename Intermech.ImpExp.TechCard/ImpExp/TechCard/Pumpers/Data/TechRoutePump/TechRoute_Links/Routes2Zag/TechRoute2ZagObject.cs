// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2Zag.TechRoute2ZagObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2Zag;

internal class TechRoute2ZagObject : TechObjectRecord
{
  private static int idx_F_ROUTE_ID;
  private static int idx_F_ZAG_ID;

  public TechRoute2ZagObject() => this.TableName = "TC_NROUTE_TO_ZAG";

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechRoute2ZagObject.idx_F_ROUTE_ID = schema["F_ROUTE_ID"];
    TechRoute2ZagObject.idx_F_ZAG_ID = schema["F_ZAG_ID"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_ROUTE_ID", (object) (dataReader.IsDBNull(TechRoute2ZagObject.idx_F_ROUTE_ID) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoute2ZagObject.idx_F_ROUTE_ID])));
    this._fields.Add("F_ZAG_ID", (object) (dataReader.IsDBNull(TechRoute2ZagObject.idx_F_ZAG_ID) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoute2ZagObject.idx_F_ZAG_ID])));
  }
}
