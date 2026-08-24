// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords.TechObjectRecord
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Diagnostics;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;

internal class TechObjectRecord : TechObjectRecordBase
{
  protected TechObjectRecord.PumpMode _recMode;

  public TechObjectRecord.PumpMode RecMode
  {
    [DebuggerStepThrough] get => this._recMode;
    set => this._recMode = value;
  }

  public override void Clear()
  {
    base.Clear();
    this._recMode = TechObjectRecord.PumpMode.Unknown;
  }

  public override void Assign(object source)
  {
    base.Assign(source);
    if (!(source is TechObjectRecord techObjectRecord))
      return;
    this._recMode = techObjectRecord.RecMode;
  }

  public enum PumpMode
  {
    Unknown,
    NotPump,
    ObjectAndLinks,
    LinkOnly,
    ObjectOnly,
  }
}
