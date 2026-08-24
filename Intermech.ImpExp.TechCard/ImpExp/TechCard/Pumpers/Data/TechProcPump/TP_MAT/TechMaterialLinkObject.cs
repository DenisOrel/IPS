// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.TechMaterialLinkObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT;

internal class TechMaterialLinkObject : TechObjectRecord
{
  private static int idx_F_PARENTTYPE;
  private static int idx_F_PARENTKEY;
  private static int idx_F_CHILDTYPE;
  private static int idx_F_CHILDKEY;

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechMaterialLinkObject.idx_F_PARENTTYPE = schema["F_PARENTTYPE"];
    TechMaterialLinkObject.idx_F_PARENTKEY = schema["F_PARENTKEY"];
    TechMaterialLinkObject.idx_F_CHILDTYPE = schema["F_CHILDTYPE"];
    TechMaterialLinkObject.idx_F_CHILDKEY = schema["F_CHILDKEY"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_PARENTTYPE", (object) (dataReader.IsDBNull(TechMaterialLinkObject.idx_F_PARENTTYPE) ? 0 : BasePumpHelper.ToInt32(dataReader[TechMaterialLinkObject.idx_F_PARENTTYPE])));
    this._fields.Add("F_PARENTKEY", (object) (dataReader.IsDBNull(TechMaterialLinkObject.idx_F_PARENTKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechMaterialLinkObject.idx_F_PARENTKEY])));
    this._fields.Add("F_CHILDTYPE", (object) (dataReader.IsDBNull(TechMaterialLinkObject.idx_F_CHILDTYPE) ? 0 : BasePumpHelper.ToInt32(dataReader[TechMaterialLinkObject.idx_F_CHILDTYPE])));
    this._fields.Add("F_CHILDKEY", (object) (dataReader.IsDBNull(TechMaterialLinkObject.idx_F_CHILDKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[TechMaterialLinkObject.idx_F_CHILDKEY])));
  }

  public TechMaterialLinkObject() => this.TableName = "TP_MAT_LINKS";
}
