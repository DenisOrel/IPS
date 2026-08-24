// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.TechRouteElem.TechRouteElemObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.TechRouteElem;

internal class TechRouteElemObject : TechObjectRecord
{
  private static int idx_F_TEMPLATE_ID;
  private static int idx_F_CEH_ID;
  private static int idx_F_VID_ID;
  private static int idx_F_ORDER;
  private static int idx_F_PRIM;

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechRouteElemObject.idx_F_TEMPLATE_ID = schema["F_TEMPLATE_ID"];
    TechRouteElemObject.idx_F_CEH_ID = schema["F_CEH_ID"];
    TechRouteElemObject.idx_F_VID_ID = schema["F_VID_ID"];
    TechRouteElemObject.idx_F_ORDER = schema["F_ORDER"];
    TechRouteElemObject.idx_F_PRIM = schema["F_PRIM"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_TEMPLATE_ID", (object) (dataReader.IsDBNull(TechRouteElemObject.idx_F_TEMPLATE_ID) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRouteElemObject.idx_F_TEMPLATE_ID])));
    this._fields.Add("F_CEH_ID", (object) (dataReader.IsDBNull(TechRouteElemObject.idx_F_CEH_ID) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRouteElemObject.idx_F_CEH_ID])));
    this._fields.Add("F_VID_ID", (object) (dataReader.IsDBNull(TechRouteElemObject.idx_F_VID_ID) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRouteElemObject.idx_F_VID_ID])));
    this._fields.Add("F_ORDER", (object) (dataReader.IsDBNull(TechRouteElemObject.idx_F_ORDER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRouteElemObject.idx_F_ORDER])));
    this._fields.Add("F_PRIM", dataReader.IsDBNull(TechRouteElemObject.idx_F_PRIM) ? (object) string.Empty : (object) dataReader.GetString(TechRouteElemObject.idx_F_PRIM));
  }

  public TechRouteElemObject() => this.TableName = "TC_NROUTE_STRINGS";
}
