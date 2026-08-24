// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.LinearAxis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot.Axes;

public class LinearAxis : Axis
{
  public LinearAxis()
  {
    this.FractionUnit = 1.0;
    this.FractionUnitSymbol = (string) null;
    this.FormatAsFractions = false;
  }

  public bool FormatAsFractions { get; set; }

  public double FractionUnit { get; set; }

  public string FractionUnitSymbol { get; set; }

  public override bool IsXyAxis() => true;

  protected override string FormatValueOverride(double x)
  {
    return this.FormatAsFractions ? FractionHelper.ConvertToFractionString(x, this.FractionUnit, this.FractionUnitSymbol, formatProvider: (IFormatProvider) this.ActualCulture, formatString: this.StringFormat) : base.FormatValueOverride(x);
  }
}
