// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRouteTemplate.TechRouteTemplateObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRouteTemplate;

internal class TechRouteTemplateObject : TechObjectRecord
{
  private static int idx_F_NAME;
  private static int idx_F_TYPE;
  private static int idx_F_VID;
  private static int idx_F_DATE;
  private static int idx_F_RECORD_STATE;
  private static int idx_F_USER;
  private static int idx_F_WORKCOPY_FOR;

  public TechRouteTemplateObject() => this.TableName = "TC_NROUTE_TEMPLATES";

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechRouteTemplateObject.idx_F_NAME = schema["F_NAME"];
    TechRouteTemplateObject.idx_F_TYPE = schema["F_TYPE"];
    TechRouteTemplateObject.idx_F_VID = schema["F_VID"];
    TechRouteTemplateObject.idx_F_DATE = schema["F_DATE"];
    TechRouteTemplateObject.idx_F_RECORD_STATE = schema["F_RECORD_STATE"];
    TechRouteTemplateObject.idx_F_USER = schema["F_USER"];
    TechRouteTemplateObject.idx_F_WORKCOPY_FOR = schema["F_WORKCOPY_FOR"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_NAME", dataReader.IsDBNull(TechRouteTemplateObject.idx_F_NAME) ? (object) string.Empty : (object) dataReader.GetString(TechRouteTemplateObject.idx_F_NAME));
    this._fields.Add("F_TYPE", (object) (dataReader.IsDBNull(TechRouteTemplateObject.idx_F_TYPE) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRouteTemplateObject.idx_F_TYPE])));
    this._fields.Add("F_VID", (object) (dataReader.IsDBNull(TechRouteTemplateObject.idx_F_VID) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRouteTemplateObject.idx_F_VID])));
    this._fields.Add("F_DATE", (object) (dataReader.IsDBNull(TechRouteTemplateObject.idx_F_DATE) ? DateTime.MinValue : dataReader.GetDateTime(TechRouteTemplateObject.idx_F_DATE)));
    this._fields.Add("F_RECORD_STATE", (object) (dataReader.IsDBNull(TechRouteTemplateObject.idx_F_RECORD_STATE) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRouteTemplateObject.idx_F_RECORD_STATE])));
    this._fields.Add("F_USER", (object) (dataReader.IsDBNull(TechRouteTemplateObject.idx_F_USER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRouteTemplateObject.idx_F_USER])));
    this._fields.Add("F_WORKCOPY_FOR", (object) (dataReader.IsDBNull(TechRouteTemplateObject.idx_F_WORKCOPY_FOR) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRouteTemplateObject.idx_F_WORKCOPY_FOR])));
  }
}
