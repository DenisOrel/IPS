// Decompiled with JetBrains decompiler
// Type: OxyPlot.DataPoint
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public struct DataPoint(double x, double y) : ICodeGenerating, IEquatable<DataPoint>
{
  public static readonly DataPoint Undefined = new DataPoint(double.NaN, double.NaN);
  internal readonly double x = x;
  internal readonly double y = y;

  public double X => this.x;

  public double Y => this.y;

  public string ToCode()
  {
    return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", (object) this.x, (object) this.y);
  }

  public bool Equals(DataPoint other) => this.x.Equals(other.x) && this.y.Equals(other.y);

  public override string ToString() => $"{(object) this.x} {(object) this.y}";

  public bool IsDefined() => !double.IsNaN(this.x) && !double.IsNaN(this.y);
}
