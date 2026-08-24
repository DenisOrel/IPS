// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_TOOL.TechToolsObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_TOOL;

internal class TechToolsObject : TechObjectRecord
{
  private static int idx_F_RECKEY;
  private static int idx_F_TBLKEY;
  private static int idx_F_ORDER;
  private static int idx_F_FLAGS;
  private static int idx_F_DATE;
  private static int idx_F_VAR;
  private static int idx_F_DOCTCKEY;
  private static int idx_F_OPERKEY;
  private static int idx_F_PEREHKEY;
  private static int idx_F_KIND;
  private static int idx_F_COUNT;
  public string UniqueRecordHash;

  public TechToolsObject() => this.TableName = "TP_TOOL";

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechToolsObject.idx_F_RECKEY = schema["F_RECKEY"];
    TechToolsObject.idx_F_TBLKEY = schema["F_TBLKEY"];
    TechToolsObject.idx_F_ORDER = schema["F_ORDER"];
    TechToolsObject.idx_F_FLAGS = schema["F_FLAGS"];
    TechToolsObject.idx_F_DATE = schema["F_DATE"];
    TechToolsObject.idx_F_VAR = schema["F_VAR"];
    TechToolsObject.idx_F_PEREHKEY = schema["F_PEREHKEY"];
    TechToolsObject.idx_F_DOCTCKEY = schema["F_DOCTCKEY"];
    TechToolsObject.idx_F_OPERKEY = schema["F_OPERKEY"];
    TechToolsObject.idx_F_KIND = schema["F_KIND"];
    TechToolsObject.idx_F_COUNT = schema["F_COUNT"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_RECKEY", (object) (dataReader.IsDBNull(TechToolsObject.idx_F_RECKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechToolsObject.idx_F_RECKEY])));
    this._fields.Add("F_TBLKEY", (object) (dataReader.IsDBNull(TechToolsObject.idx_F_TBLKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechToolsObject.idx_F_TBLKEY])));
    this._fields.Add("F_ORDER", (object) (dataReader.IsDBNull(TechToolsObject.idx_F_ORDER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechToolsObject.idx_F_ORDER])));
    this._fields.Add("F_FLAGS", (object) (dataReader.IsDBNull(TechToolsObject.idx_F_FLAGS) ? 0 : BasePumpHelper.ToInt32(dataReader[TechToolsObject.idx_F_FLAGS])));
    this._fields.Add("F_DATE", (object) (dataReader.IsDBNull(TechToolsObject.idx_F_DATE) ? DateTime.Now : dataReader.GetDateTime(TechToolsObject.idx_F_DATE)));
    this._fields.Add("F_VAR", (object) (dataReader.IsDBNull(TechToolsObject.idx_F_VAR) ? 0 : BasePumpHelper.ToInt32(dataReader[TechToolsObject.idx_F_VAR])));
    this._fields.Add("F_OPERKEY", (object) (dataReader.IsDBNull(TechToolsObject.idx_F_OPERKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechToolsObject.idx_F_OPERKEY])));
    this._fields.Add("F_KIND", (object) (dataReader.IsDBNull(TechToolsObject.idx_F_KIND) ? 0 : BasePumpHelper.ToInt32(dataReader[TechToolsObject.idx_F_KIND])));
    this._fields.Add("F_COUNT", (object) (dataReader.IsDBNull(TechToolsObject.idx_F_COUNT) ? 0 : BasePumpHelper.ToInt32(dataReader[TechToolsObject.idx_F_COUNT])));
    this._fields.Add("F_PEREHKEY", (object) (dataReader.IsDBNull(TechToolsObject.idx_F_PEREHKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechToolsObject.idx_F_PEREHKEY])));
    this._fields.Add("F_DOCTCKEY", (object) (dataReader.IsDBNull(TechToolsObject.idx_F_DOCTCKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechToolsObject.idx_F_DOCTCKEY])));
  }

  public override void Clear()
  {
    base.Clear();
    this.UniqueRecordHash = string.Empty;
  }

  public override void Assign(object source) => base.Assign(source);
}
