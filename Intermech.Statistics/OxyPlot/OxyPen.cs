// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyPen
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public class OxyPen
{
  public OxyPen(OxyColor color, double thickness = 1.0, LineStyle lineStyle = LineStyle.Solid, LineJoin lineJoin = LineJoin.Miter)
  {
    this.Color = color;
    this.Thickness = thickness;
    this.DashArray = lineStyle.GetDashArray();
    this.LineStyle = lineStyle;
    this.LineJoin = lineJoin;
  }

  public OxyColor Color { get; set; }

  public double[] DashArray { get; set; }

  public LineJoin LineJoin { get; set; }

  public LineStyle LineStyle { get; set; }

  public double Thickness { get; set; }

  public double[] ActualDashArray => this.DashArray ?? this.LineStyle.GetDashArray();

  public static OxyPen Create(
    OxyColor color,
    double thickness,
    LineStyle lineStyle = LineStyle.Solid,
    LineJoin lineJoin = LineJoin.Miter)
  {
    return color.IsInvisible() || lineStyle == LineStyle.None || Math.Abs(thickness) < double.Epsilon ? (OxyPen) null : new OxyPen(color, thickness, lineStyle, lineJoin);
  }

  public override int GetHashCode()
  {
    return ((this.Color.GetHashCode() * 397 ^ this.Thickness.GetHashCode()) * 397 ^ this.LineStyle.GetHashCode()) * 397 ^ this.LineJoin.GetHashCode();
  }
}
