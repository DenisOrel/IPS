// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechTypes.Settings.TechTypeConvertionRule
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechTypes.Settings;

public class TechTypeConvertionRule
{
  private readonly int _id;
  private readonly Guid _pumpTo;
  private TechTypeConvertionRuleMode _mode;

  public TechTypeConvertionRule(int id, Guid pumpTo)
  {
    this._id = id;
    this._pumpTo = pumpTo;
  }

  public int Id => this._id;

  public Guid PumpTo => this._pumpTo;

  public TechTypeConvertionRuleMode Mode
  {
    get => this._mode;
    set => this._mode = value;
  }
}
