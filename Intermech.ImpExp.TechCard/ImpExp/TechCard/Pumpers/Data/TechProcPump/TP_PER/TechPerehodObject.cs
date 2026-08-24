// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_PER.TechPerehodObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_PER;

internal class TechPerehodObject : TechObjectRecord
{
  private static int idx_F_DOCTCKEY;
  private static int idx_F_OPERKEY;
  private static int idx_F_RECKEY;
  private static int idx_F_ORDER;
  private static int idx_F_FLAGS;
  private static int idx_F_DATE;
  private static int idx_F_NUMBER;
  private static int idx_F_PNVM;
  private static int idx_F_REFPER;
  private static int idx_F_VAR;

  public TechPerehodObject() => this.TableName = "TP_PER";

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechPerehodObject.idx_F_DOCTCKEY = schema["F_DOCTCKEY"];
    TechPerehodObject.idx_F_OPERKEY = schema["F_OPERKEY"];
    TechPerehodObject.idx_F_RECKEY = schema["F_RECKEY"];
    TechPerehodObject.idx_F_ORDER = schema["F_ORDER"];
    TechPerehodObject.idx_F_FLAGS = schema["F_FLAGS"];
    TechPerehodObject.idx_F_DATE = schema["F_DATE"];
    TechPerehodObject.idx_F_NUMBER = schema["F_NUMBER"];
    TechPerehodObject.idx_F_PNVM = schema["F_PNVM"];
    TechPerehodObject.idx_F_REFPER = schema["F_REFPER"];
    TechPerehodObject.idx_F_VAR = schema["F_VAR"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_DOCTCKEY", (object) (dataReader.IsDBNull(TechPerehodObject.idx_F_DOCTCKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechPerehodObject.idx_F_DOCTCKEY])));
    this._fields.Add("F_OPERKEY", (object) (dataReader.IsDBNull(TechPerehodObject.idx_F_OPERKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechPerehodObject.idx_F_OPERKEY])));
    this._fields.Add("F_NUMBER", (object) (dataReader.IsDBNull(TechPerehodObject.idx_F_NUMBER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechPerehodObject.idx_F_NUMBER])));
    this._fields.Add("F_PNVM", (object) (dataReader.IsDBNull(TechPerehodObject.idx_F_PNVM) ? 0 : BasePumpHelper.ToInt32(dataReader[TechPerehodObject.idx_F_PNVM])));
    this._fields.Add("F_REFPER", (object) (dataReader.IsDBNull(TechPerehodObject.idx_F_REFPER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechPerehodObject.idx_F_REFPER])));
    this._fields.Add("F_RECKEY", (object) (dataReader.IsDBNull(TechPerehodObject.idx_F_RECKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechPerehodObject.idx_F_RECKEY])));
    this._fields.Add("F_ORDER", (object) (dataReader.IsDBNull(TechPerehodObject.idx_F_ORDER) ? 0 : BasePumpHelper.ToInt32(dataReader[TechPerehodObject.idx_F_ORDER])));
    this._fields.Add("F_FLAGS", (object) (dataReader.IsDBNull(TechPerehodObject.idx_F_FLAGS) ? 0 : BasePumpHelper.ToInt32(dataReader[TechPerehodObject.idx_F_FLAGS])));
    this._fields.Add("F_DATE", (object) (dataReader.IsDBNull(TechPerehodObject.idx_F_DATE) ? DateTime.Now : dataReader.GetDateTime(TechPerehodObject.idx_F_DATE)));
    this._fields.Add("F_VAR", (object) (dataReader.IsDBNull(TechPerehodObject.idx_F_VAR) ? 0 : BasePumpHelper.ToInt32(dataReader[TechPerehodObject.idx_F_VAR])));
  }
}
