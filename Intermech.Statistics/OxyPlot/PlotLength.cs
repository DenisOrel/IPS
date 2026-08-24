// Decompiled with JetBrains decompiler
// Type: OxyPlot.PlotLength
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public struct PlotLength(double value, PlotLengthUnit unit) : IEquatable<PlotLength>
{
  private readonly PlotLengthUnit unit = unit;
  private readonly double value = value;

  public double Value => this.value;

  public PlotLengthUnit Unit => this.unit;

  public bool Equals(PlotLength other)
  {
    return this.value.Equals(other.value) && this.unit.Equals((object) other.unit);
  }
}
