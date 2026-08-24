// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxySize
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace OxyPlot;

public struct OxySize(double width, double height) : IFormattable, IEquatable<OxySize>
{
  public static readonly OxySize Empty = new OxySize(0.0, 0.0);
  private readonly double height = height;
  private readonly double width = width;

  public double Height => this.height;

  public double Width => this.width;

  public override string ToString()
  {
    return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "({0}, {1})", (object) this.Width, (object) this.Height);
  }

  public string ToString(string format, IFormatProvider formatProvider)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("(");
    stringBuilder.Append(this.Width.ToString(format, formatProvider));
    stringBuilder.Append(",");
    stringBuilder.Append(" ");
    stringBuilder.Append(this.Height.ToString(format, formatProvider));
    stringBuilder.Append(")");
    return stringBuilder.ToString();
  }

  public bool Equals(OxySize other)
  {
    return this.Width.Equals(other.Width) && this.Height.Equals(other.Height);
  }
}
