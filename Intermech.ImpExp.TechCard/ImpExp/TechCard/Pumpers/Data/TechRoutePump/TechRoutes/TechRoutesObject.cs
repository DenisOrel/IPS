// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoutes.TechRoutesObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoutes;

internal class TechRoutesObject : TechObjectRecord
{
  private static int idx_F_NAME;
  private static int idx_F_TMP_OBRABOTKI;
  private static int idx_F_TMP_SBORKI;
  private static int idx_F_VER;
  private static int idx_F_VERSION_FOR;
  private static int idx_F_NAZN;
  private static int idx_F_TIP;
  private static int idx_F_VID;
  private static int idx_F_STATUS;
  private static int idx_F_WORKCOPY_FOR;
  private static int idx_F_SOST;
  private static int idx_F_USER;
  private static int idx_F_RECORD_STATE;
  private static int idx_F_DATA_VVODA;
  private static int idx_F_DATA_ANUL;
  private static int idx_F_DATA_PRODUCTION;
  private static int idx_F_OSN_VVODA;
  private static int idx_F_OSN_ANUL;
  private static int idx_F_PRIM;
  private static int idx_F_ABS_OR_PERCENT;
  private static int idx_F_KOLVO;
  private static int idx_F_IS_ACTUAL;
  private static int idx_F_MODIFIED;
  private static int idx_F_ART_TCKEY;
  private static int idx_F_DATA_AKTUAL;
  private static int idx_F_ISDEFAULT;

  public TechRoutesObject() => this.TableName = "TC_NROUTES";

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechRoutesObject.idx_F_NAME = schema["F_NAME"];
    TechRoutesObject.idx_F_TMP_OBRABOTKI = schema["F_TMP_OBRABOTKI"];
    TechRoutesObject.idx_F_TMP_SBORKI = schema["F_TMP_SBORKI"];
    TechRoutesObject.idx_F_VER = schema["F_VER"];
    TechRoutesObject.idx_F_VERSION_FOR = schema["F_VERSION_FOR"];
    TechRoutesObject.idx_F_NAZN = schema["F_NAZN"];
    TechRoutesObject.idx_F_TIP = schema["F_TIP"];
    TechRoutesObject.idx_F_VID = schema["F_VID"];
    TechRoutesObject.idx_F_STATUS = schema["F_STATUS"];
    TechRoutesObject.idx_F_WORKCOPY_FOR = schema["F_WORKCOPY_FOR"];
    TechRoutesObject.idx_F_SOST = schema["F_SOST"];
    TechRoutesObject.idx_F_USER = schema["F_USER"];
    TechRoutesObject.idx_F_RECORD_STATE = schema["F_RECORD_STATE"];
    TechRoutesObject.idx_F_DATA_VVODA = schema["F_DATA_VVODA"];
    TechRoutesObject.idx_F_DATA_ANUL = schema["F_DATA_ANUL"];
    TechRoutesObject.idx_F_DATA_PRODUCTION = schema["F_DATA_PRODUCTION"];
    TechRoutesObject.idx_F_OSN_VVODA = schema["F_OSN_VVODA"];
    TechRoutesObject.idx_F_OSN_ANUL = schema["F_OSN_ANUL"];
    TechRoutesObject.idx_F_PRIM = schema["F_PRIM"];
    TechRoutesObject.idx_F_ABS_OR_PERCENT = schema["F_ABS_OR_PERCENT"];
    TechRoutesObject.idx_F_KOLVO = schema["F_KOLVO"];
    TechRoutesObject.idx_F_IS_ACTUAL = schema["F_IS_ACTUAL"];
    TechRoutesObject.idx_F_MODIFIED = schema["F_MODIFIED"];
    TechRoutesObject.idx_F_ART_TCKEY = schema["F_ART_TCKEY"];
    TechRoutesObject.idx_F_DATA_AKTUAL = schema["F_DATA_AKTUAL"];
    TechRoutesObject.idx_F_ISDEFAULT = schema["F_ISDEFAULT"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_NAME", dataReader.IsDBNull(TechRoutesObject.idx_F_NAME) ? (object) string.Empty : (object) dataReader.GetString(TechRoutesObject.idx_F_NAME));
    this._fields.Add("F_TMP_OBRABOTKI", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_TMP_OBRABOTKI) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_TMP_OBRABOTKI])));
    this._fields.Add("F_TMP_SBORKI", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_TMP_SBORKI) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_TMP_SBORKI])));
    this._fields.Add("F_VER", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_VER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_VER])));
    this._fields.Add("F_VERSION_FOR", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_VERSION_FOR) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_VERSION_FOR])));
    this._fields.Add("F_NAZN", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_NAZN) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_NAZN])));
    this._fields.Add("F_TIP", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_TIP) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_TIP])));
    this._fields.Add("F_VID", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_VID) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_VID])));
    this._fields.Add("F_STATUS", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_STATUS) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_STATUS])));
    this._fields.Add("F_WORKCOPY_FOR", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_WORKCOPY_FOR) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_WORKCOPY_FOR])));
    this._fields.Add("F_SOST", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_SOST) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_SOST])));
    this._fields.Add("F_USER", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_USER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_USER])));
    this._fields.Add("F_RECORD_STATE", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_RECORD_STATE) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_RECORD_STATE])));
    this._fields.Add("F_DATA_VVODA", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_DATA_VVODA) ? DateTime.MinValue : dataReader.GetDateTime(TechRoutesObject.idx_F_DATA_VVODA)));
    this._fields.Add("F_DATA_ANUL", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_DATA_ANUL) ? DateTime.MinValue : dataReader.GetDateTime(TechRoutesObject.idx_F_DATA_ANUL)));
    this._fields.Add("F_DATA_PRODUCTION", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_DATA_PRODUCTION) ? DateTime.MinValue : dataReader.GetDateTime(TechRoutesObject.idx_F_DATA_PRODUCTION)));
    this._fields.Add("F_OSN_VVODA", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_OSN_VVODA) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_OSN_VVODA])));
    this._fields.Add("F_OSN_ANUL", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_OSN_ANUL) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_OSN_ANUL])));
    this._fields.Add("F_PRIM", dataReader.IsDBNull(TechRoutesObject.idx_F_PRIM) ? (object) string.Empty : (object) dataReader.GetString(TechRoutesObject.idx_F_PRIM));
    this._fields.Add("F_ABS_OR_PERCENT", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_ABS_OR_PERCENT) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_ABS_OR_PERCENT])));
    this._fields.Add("F_KOLVO", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_KOLVO) ? 0.0 : BasePumpHelper.ToDouble(dataReader[TechRoutesObject.idx_F_KOLVO])));
    this._fields.Add("F_IS_ACTUAL", dataReader.IsDBNull(TechRoutesObject.idx_F_IS_ACTUAL) ? (object) string.Empty : (object) dataReader.GetString(TechRoutesObject.idx_F_IS_ACTUAL));
    this._fields.Add("F_MODIFIED", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_MODIFIED) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_MODIFIED])));
    this._fields.Add("F_ART_TCKEY", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_ART_TCKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_ART_TCKEY])));
    this._fields.Add("F_DATA_AKTUAL", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_DATA_AKTUAL) ? DateTime.MinValue : dataReader.GetDateTime(TechRoutesObject.idx_F_DATA_AKTUAL)));
    this._fields.Add("F_ISDEFAULT", (object) (dataReader.IsDBNull(TechRoutesObject.idx_F_ISDEFAULT) ? 0 : BasePumpHelper.ToInt32(dataReader[TechRoutesObject.idx_F_ISDEFAULT])));
  }
}
