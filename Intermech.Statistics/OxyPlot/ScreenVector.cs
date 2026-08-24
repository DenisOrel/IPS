// Decompiled with JetBrains decompiler
// Type: OxyPlot.ScreenVector
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public struct ScreenVector(double x, double y) : IEquatable<ScreenVector>
{
  internal double x = x;
  internal double y = y;

  public double Length => Math.Sqrt(this.x * this.x + this.y * this.y);

  public double LengthSquared => this.x * this.x + this.y * this.y;

  public double X => this.x;

  public double Y => this.y;

  public static ScreenVector operator *(ScreenVector v, double d)
  {
    return new ScreenVector(v.x * d, v.y * d);
  }

  public static ScreenVector operator +(ScreenVector v, ScreenVector d)
  {
    return new ScreenVector(v.x + d.x, v.y + d.y);
  }

  public static ScreenVector operator -(ScreenVector v, ScreenVector d)
  {
    return new ScreenVector(v.x - d.x, v.y - d.y);
  }

  public static ScreenVector operator -(ScreenVector v) => new ScreenVector(-v.x, -v.y);

  public void Normalize()
  {
    double num = Math.Sqrt(this.x * this.x + this.y * this.y);
    if (num <= 0.0)
      return;
    this.x /= num;
    this.y /= num;
  }

  public override string ToString() => $"{(object) this.x} {(object) this.y}";

  public bool Equals(ScreenVector other) => this.x.Equals(other.x) && this.y.Equals(other.y);
}
