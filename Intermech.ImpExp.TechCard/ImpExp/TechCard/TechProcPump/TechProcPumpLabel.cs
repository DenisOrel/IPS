// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TechProcPumpLabel
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump;

[Serializable]
internal class TechProcPumpLabel
{
  private long _artID = -1;
  private string _tpType = string.Empty;
  private long _tpTypeID = -1;

  public long ArtID
  {
    get => this._artID;
    set => this._artID = value;
  }

  public string TpType
  {
    get => this._tpType;
    set => this._tpType = value;
  }

  public long TpTypeID
  {
    get => this._tpTypeID;
    set => this._tpTypeID = value;
  }
}
