// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_SKETCH.TechSketchObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_SKETCH;

internal class TechSketchObject(TechRecordParser parser) : TechObjectRecordDynamic("TP_SKETCH", parser)
{
  public override void Parse(IDataReader idr)
  {
    base.Parse(idr);
    this.SketchType = (TechSketchType) Convert.ToInt32(this.Fields["F_TYPE"]);
  }

  protected override void AddFields(string fieldName, object value)
  {
    this._fields.Add(fieldName, value);
  }

  public override void Assign(object source)
  {
    base.Assign(source);
    if (!(source is TechSketchObject techSketchObject))
      return;
    this.SketchType = techSketchObject.SketchType;
  }

  public override void Clear()
  {
    base.Clear();
    this.SketchType = TechSketchType.None;
  }

  public TechSketchType SketchType { get; private set; }
}
