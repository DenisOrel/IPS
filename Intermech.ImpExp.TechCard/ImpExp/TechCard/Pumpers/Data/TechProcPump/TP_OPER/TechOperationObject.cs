// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_OPER.TechOperationObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_OPER;

internal class TechOperationObject : TechObjectRecord
{
  private static int idx_F_DOCTCKEY;
  private static int idx_F_RECKEY;
  private static int idx_F_ORDER;
  private static int idx_F_FLAGS;
  private static int idx_F_DATE;
  private static int idx_F_NUMBER;
  private static int idx_F_PLACE;
  private static int idx_F_REFOPER;
  private static int idx_F_USERGROUP;
  private static int idx_F_USER;
  private static int idx_F_PRODUCTION;
  private static int idx_F_TPKEY;
  private static int idx_F_PROTECTED;
  private static int idx_F_VAR;
  private static int idx_F_OPERDOCKEY;

  public TechOperationObject() => this.TableName = "TP_OPER";

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechOperationObject.idx_F_DOCTCKEY = schema["F_DOCTCKEY"];
    TechOperationObject.idx_F_RECKEY = schema["F_RECKEY"];
    TechOperationObject.idx_F_ORDER = schema["F_ORDER"];
    TechOperationObject.idx_F_FLAGS = schema["F_FLAGS"];
    TechOperationObject.idx_F_DATE = schema["F_DATE"];
    TechOperationObject.idx_F_NUMBER = schema["F_NUMBER"];
    TechOperationObject.idx_F_PLACE = schema["F_PLACE"];
    TechOperationObject.idx_F_REFOPER = schema["F_REFOPER"];
    TechOperationObject.idx_F_USERGROUP = schema["F_USERGROUP"];
    TechOperationObject.idx_F_USER = schema["F_USER"];
    TechOperationObject.idx_F_PRODUCTION = schema["F_PRODUCTION"];
    TechOperationObject.idx_F_TPKEY = schema["F_TPKEY"];
    TechOperationObject.idx_F_PROTECTED = schema["F_PROTECTED"];
    TechOperationObject.idx_F_VAR = schema["F_VAR"];
    TechOperationObject.idx_F_OPERDOCKEY = schema["F_OPERDOCKEY"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_DOCTCKEY", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_DOCTCKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_DOCTCKEY])));
    this._fields.Add("F_RECKEY", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_RECKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_RECKEY])));
    this._fields.Add("F_ORDER", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_ORDER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_ORDER])));
    this._fields.Add("F_FLAGS", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_FLAGS) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_FLAGS])));
    this._fields.Add("F_DATE", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_DATE) ? DateTime.Now : dataReader.GetDateTime(TechOperationObject.idx_F_DATE)));
    this._fields.Add("F_NUMBER", dataReader.IsDBNull(TechOperationObject.idx_F_NUMBER) ? (object) string.Empty : (object) dataReader.GetString(TechOperationObject.idx_F_NUMBER));
    this._fields.Add("F_PLACE", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_PLACE) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_PLACE])));
    this._fields.Add("F_REFOPER", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_REFOPER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_REFOPER])));
    this._fields.Add("F_USERGROUP", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_USERGROUP) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_USERGROUP])));
    this._fields.Add("F_USER", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_USER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_USER])));
    this._fields.Add("F_PRODUCTION", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_PRODUCTION) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_PRODUCTION])));
    this._fields.Add("F_TPKEY", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_TPKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_TPKEY])));
    this._fields.Add("F_PROTECTED", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_PROTECTED) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_PROTECTED])));
    this._fields.Add("F_VAR", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_VAR) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_VAR])));
    this._fields.Add("F_OPERDOCKEY", (object) (dataReader.IsDBNull(TechOperationObject.idx_F_OPERDOCKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechOperationObject.idx_F_OPERDOCKEY])));
  }
}
