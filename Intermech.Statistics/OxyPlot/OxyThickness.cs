// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyThickness
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Globalization;

#nullable disable
namespace OxyPlot;

public struct OxyThickness(double left, double top, double right, double bottom) : ICodeGenerating
{
  private readonly double bottom = bottom;
  private readonly double left = left;
  private readonly double right = right;
  private readonly double top = top;

  public OxyThickness(double thickness)
    : this(thickness, thickness, thickness, thickness)
  {
  }

  public double Bottom => this.bottom;

  public double Height => this.Bottom - this.Top;

  public double Left => this.left;

  public double Right => this.right;

  public double Top => this.top;

  public double Width => this.Right - this.Left;

  public string ToCode()
  {
    return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "new OxyThickness({0},{1},{2},{3})", (object) this.Left, (object) this.Top, (object) this.Right, (object) this.Bottom);
  }

  public override string ToString()
  {
    return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", (object) this.left, (object) this.top, (object) this.right, (object) this.bottom);
  }

  public bool Equals(OxyThickness other)
  {
    return this.Left.Equals(other.Left) && this.Top.Equals(other.Top) && this.Width.Equals(other.Width) && this.Height.Equals(other.Height);
  }
}
