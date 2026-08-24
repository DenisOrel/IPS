// Decompiled with JetBrains decompiler
// Type: OxyPlot.ScreenPoint
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public struct ScreenPoint(double x, double y) : IEquatable<ScreenPoint>
{
  public static readonly ScreenPoint Undefined = new ScreenPoint(double.NaN, double.NaN);
  internal double x = x;
  internal double y = y;

  public double X => this.x;

  public double Y => this.y;

  public static bool IsUndefined(ScreenPoint point)
  {
    return double.IsNaN(point.x) && double.IsNaN(point.y);
  }

  public static ScreenPoint operator +(ScreenPoint p1, ScreenVector p2)
  {
    return new ScreenPoint(p1.x + p2.x, p1.y + p2.y);
  }

  public static ScreenVector operator -(ScreenPoint p1, ScreenPoint p2)
  {
    return new ScreenVector(p1.x - p2.x, p1.y - p2.y);
  }

  public static ScreenPoint operator -(ScreenPoint point, ScreenVector vector)
  {
    return new ScreenPoint(point.x - vector.x, point.y - vector.y);
  }

  public double DistanceTo(ScreenPoint point)
  {
    double num1 = point.x - this.x;
    double num2 = point.y - this.y;
    return Math.Sqrt(num1 * num1 + num2 * num2);
  }

  public double DistanceToSquared(ScreenPoint point)
  {
    double num1 = point.x - this.x;
    double num2 = point.y - this.y;
    return num1 * num1 + num2 * num2;
  }

  public override string ToString() => $"{(object) this.x} {(object) this.y}";

  public bool Equals(ScreenPoint other) => this.x.Equals(other.x) && this.y.Equals(other.y);
}
