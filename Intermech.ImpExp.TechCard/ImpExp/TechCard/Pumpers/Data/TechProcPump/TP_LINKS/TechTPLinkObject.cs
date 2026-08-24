// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_LINKS.TechTPLinkObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_LINKS;

internal class TechTPLinkObject : TechObjectRecord
{
  private static int _idxFldDocTcKey;
  private static int _idxFldSourceKey;
  private static int _idxFldSourceType;
  private static int _idxFldTargetKey;
  private static int _idxFldTargetType;

  public TechTPLinkObject() => this.TableName = "TP_LINKS";

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechTPLinkObject._idxFldDocTcKey = schema["F_DOC_TCKEY"];
    TechTPLinkObject._idxFldSourceKey = schema["F_SOURCE_KEY"];
    TechTPLinkObject._idxFldSourceType = schema["F_SOURCE_TYPE"];
    TechTPLinkObject._idxFldTargetKey = schema["F_TARGET_KEY"];
    TechTPLinkObject._idxFldTargetType = schema["F_TARGET_TYPE"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_DOC_TCKEY", (object) (dataReader.IsDBNull(TechTPLinkObject._idxFldDocTcKey) ? 0 : BasePumpHelper.ToInt32(dataReader[TechTPLinkObject._idxFldDocTcKey])));
    this._fields.Add("F_SOURCE_KEY", (object) (dataReader.IsDBNull(TechTPLinkObject._idxFldSourceKey) ? 0 : BasePumpHelper.ToInt32(dataReader[TechTPLinkObject._idxFldSourceKey])));
    this._fields.Add("F_SOURCE_TYPE", (object) (dataReader.IsDBNull(TechTPLinkObject._idxFldSourceType) ? 0 : BasePumpHelper.ToInt32(dataReader[TechTPLinkObject._idxFldSourceType])));
    this._fields.Add("F_TARGET_KEY", (object) (dataReader.IsDBNull(TechTPLinkObject._idxFldTargetKey) ? 0 : BasePumpHelper.ToInt32(dataReader[TechTPLinkObject._idxFldTargetKey])));
    this._fields.Add("F_TARGET_TYPE", (object) (dataReader.IsDBNull(TechTPLinkObject._idxFldTargetType) ? 0 : BasePumpHelper.ToInt32(dataReader[TechTPLinkObject._idxFldTargetType])));
  }
}
