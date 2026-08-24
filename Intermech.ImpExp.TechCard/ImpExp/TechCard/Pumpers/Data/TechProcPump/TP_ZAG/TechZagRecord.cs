// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_ZAG.TechZagRecord
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_ZAG;

internal class TechZagRecord : TechObjectRecord
{
  public static int idx_F_ARTTCKEY;
  public static int idx_F_RECKEY;
  public static int idx_F_TBLKEY;
  public static int idx_F_ORDER;
  public static int idx_F_FLAGS;
  public static int idx_F_DATE;
  public static int idx_F_VAR;
  public static int idx_F_NAME;
  public static int idx_F_DESCR;
  public static int idx_F_ZAGARTKEY;
  public static int idx_F_CTLKEY;
  public static int idx_F_VERSION;
  public static int idx_F_PARENTKEY;
  public static int idx_F_STATUS;
  public static int idx_F_USERID;
  public static int idx_F_PRODUCTION;
  public static int idx_F_OWNER;
  public static int idx_F_DATA_AKTUAL;
  public static int idx_F_USER_CREATOR;
  public static int idx_F_OSN_VVODA;
  public static int idx_F_LEVEL;
  public static int idx_F_GROUPZAG_KEY;

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechZagRecord.idx_F_ARTTCKEY = schema["F_ARTTCKEY"];
    TechZagRecord.idx_F_RECKEY = schema["F_RECKEY"];
    TechZagRecord.idx_F_TBLKEY = schema["F_TBLKEY"];
    TechZagRecord.idx_F_ORDER = schema["F_ORDER"];
    TechZagRecord.idx_F_FLAGS = schema["F_FLAGS"];
    TechZagRecord.idx_F_DATE = schema["F_DATE"];
    TechZagRecord.idx_F_VAR = schema["F_VAR"];
    TechZagRecord.idx_F_NAME = schema["F_NAME"];
    TechZagRecord.idx_F_DESCR = schema["F_DESCR"];
    TechZagRecord.idx_F_ZAGARTKEY = schema["F_ZAGARTKEY"];
    TechZagRecord.idx_F_CTLKEY = schema["F_CTLKEY"];
    TechZagRecord.idx_F_VERSION = schema["F_VERSION"];
    TechZagRecord.idx_F_PARENTKEY = schema["F_PARENTKEY"];
    TechZagRecord.idx_F_STATUS = schema["F_STATUS"];
    TechZagRecord.idx_F_USERID = schema["F_USERID"];
    TechZagRecord.idx_F_PRODUCTION = schema["F_PRODUCTION"];
    TechZagRecord.idx_F_OWNER = schema["F_OWNER"];
    TechZagRecord.idx_F_DATA_AKTUAL = schema["F_DATA_AKTUAL"];
    TechZagRecord.idx_F_USER_CREATOR = schema["F_USER_CREATOR"];
    TechZagRecord.idx_F_OSN_VVODA = schema["F_OSN_VVODA"];
    TechZagRecord.idx_F_GROUPZAG_KEY = schema["F_GROUPZAG_KEY"];
    if (!schema.ContainsKey("F_LEVEL"))
      return;
    TechZagRecord.idx_F_LEVEL = schema["F_LEVEL"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_ARTTCKEY", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_ARTTCKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_ARTTCKEY])));
    this._fields.Add("F_RECKEY", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_RECKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_RECKEY])));
    this._fields.Add("F_TBLKEY", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_TBLKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_TBLKEY])));
    this._fields.Add("F_ORDER", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_ORDER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_ORDER])));
    this._fields.Add("F_FLAGS", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_FLAGS) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_FLAGS])));
    this._fields.Add("F_DATE", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_DATE) ? DateTime.Now : dataReader.GetDateTime(TechZagRecord.idx_F_DATE)));
    this._fields.Add("F_VAR", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_VAR) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_VAR])));
    this._fields.Add("F_NAME", dataReader.IsDBNull(TechZagRecord.idx_F_NAME) ? (object) string.Empty : (object) dataReader.GetString(TechZagRecord.idx_F_NAME));
    this._fields.Add("F_DESCR", dataReader.IsDBNull(TechZagRecord.idx_F_DESCR) ? (object) string.Empty : (object) dataReader.GetString(TechZagRecord.idx_F_DESCR));
    this._fields.Add("F_ZAGARTKEY", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_ZAGARTKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_ZAGARTKEY])));
    this._fields.Add("F_CTLKEY", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_CTLKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_CTLKEY])));
    this._fields.Add("F_VERSION", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_VERSION) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_VERSION])));
    this._fields.Add("F_PARENTKEY", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_PARENTKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_PARENTKEY])));
    this._fields.Add("F_STATUS", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_STATUS) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_STATUS])));
    this._fields.Add("F_USERID", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_USERID) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_USERID])));
    this._fields.Add("F_PRODUCTION", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_PRODUCTION) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_PRODUCTION])));
    this._fields.Add("F_OWNER", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_OWNER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_OWNER])));
    this._fields.Add("F_DATA_AKTUAL", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_DATA_AKTUAL) ? DateTime.Now : dataReader.GetDateTime(TechZagRecord.idx_F_DATA_AKTUAL)));
    this._fields.Add("F_USER_CREATOR", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_USER_CREATOR) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_USER_CREATOR])));
    this._fields.Add("F_OSN_VVODA", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_OSN_VVODA) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_OSN_VVODA])));
    this._fields.Add("F_LEVEL", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_LEVEL) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_LEVEL])));
    this._fields.Add("F_GROUPZAG_KEY", (object) (dataReader.IsDBNull(TechZagRecord.idx_F_GROUPZAG_KEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechZagRecord.idx_F_GROUPZAG_KEY])));
  }

  public TechZagRecord() => this.TableName = "TP_ZAG";
}
