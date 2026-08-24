// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Tp2ZagLink.TechTp2ZagLinkObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Tp2ZagLink;

internal class TechTp2ZagLinkObject : TechObjectRecord
{
  private static int _idxFldDocKey;
  private static int _idxFldZagotKey;
  private static int _idxFldOrder;
  private static int _idxFldArtTcKey;

  public TechTp2ZagLinkObject() => this.TableName = "TP_DOC_ZAG";

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    TechTp2ZagLinkObject._idxFldDocKey = schema["F_DOCTCKEY"];
    TechTp2ZagLinkObject._idxFldZagotKey = schema["F_ZAGOTKEY"];
    TechTp2ZagLinkObject._idxFldOrder = schema["F_ORDER"];
    TechTp2ZagLinkObject._idxFldArtTcKey = schema["F_ART_TCKEY"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this._fields.Add("F_DOCTCKEY", (object) (dataReader.IsDBNull(TechTp2ZagLinkObject._idxFldDocKey) ? 0 : BasePumpHelper.ToInt32(dataReader[TechTp2ZagLinkObject._idxFldDocKey])));
    this._fields.Add("F_ZAGOTKEY", (object) (dataReader.IsDBNull(TechTp2ZagLinkObject._idxFldZagotKey) ? 0 : BasePumpHelper.ToInt32(dataReader[TechTp2ZagLinkObject._idxFldZagotKey])));
    this._fields.Add("F_ORDER", (object) (dataReader.IsDBNull(TechTp2ZagLinkObject._idxFldOrder) ? 0 : BasePumpHelper.ToInt32(dataReader[TechTp2ZagLinkObject._idxFldOrder])));
    if (TechTp2ZagLinkObject._idxFldArtTcKey < 0)
      return;
    this._fields.Add("F_ART_TCKEY", (object) (dataReader.IsDBNull(TechTp2ZagLinkObject._idxFldArtTcKey) ? 0 : BasePumpHelper.ToInt32(dataReader[TechTp2ZagLinkObject._idxFldArtTcKey])));
  }
}
