// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord.ImbaseObjectRecordSub_Rec
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord;

internal class ImbaseObjectRecordSub_Rec : TechObjectRecordSubDynamic
{
  protected int _level;
  private static int idx_F_LEVEL;

  public ImbaseObjectRecordSub_Rec()
    : base()
  {
    this.TablePrefix = "_REC";
  }

  public int Level
  {
    get => this._level;
    set => this._level = value;
  }

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    ImbaseObjectRecordSub_Rec.idx_F_LEVEL = schema["F_LEVEL"];
  }

  public override void Parse(IDataReader idr)
  {
    base.Parse(idr);
    this._level = idr.IsDBNull(ImbaseObjectRecordSub_Rec.idx_F_LEVEL) ? 0 : BasePumpHelper.ToInt32(idr[ImbaseObjectRecordSub_Rec.idx_F_LEVEL]);
    this.Row = Math.Abs(this.Key);
  }
}
