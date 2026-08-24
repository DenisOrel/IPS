// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyRect
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace OxyPlot;

public struct OxyRect : IFormattable, IEquatable<OxyRect>
{
  private readonly double height;
  private readonly double left;
  private readonly double top;
  private readonly double width;

  public OxyRect(double left, double top, double width, double height)
  {
    if (width < 0.0)
      throw new ArgumentOutOfRangeException(nameof (width), "The width should not be negative.");
    if (height < 0.0)
      throw new ArgumentOutOfRangeException(nameof (height), "The height should not be negative.");
    this.left = left;
    this.top = top;
    this.width = width;
    this.height = height;
  }

  public OxyRect(ScreenPoint p0, ScreenPoint p1)
    : this(Math.Min(p0.X, p1.X), Math.Min(p0.Y, p1.Y), Math.Abs(p1.X - p0.X), Math.Abs(p1.Y - p0.Y))
  {
  }

  public OxyRect(ScreenPoint p0, OxySize size)
    : this(p0.X, p0.Y, size.Width, size.Height)
  {
  }

  public double Bottom => this.top + this.height;

  public double Height => this.height;

  public double Left => this.left;

  public double Right => this.left + this.width;

  public double Top => this.top;

  public double Width => this.width;

  public ScreenPoint Center
  {
    get => new ScreenPoint(this.left + this.width * 0.5, this.top + this.height * 0.5);
  }

  public static OxyRect Create(double x0, double y0, double x1, double y1)
  {
    return new OxyRect(Math.Min(x0, x1), Math.Min(y0, y1), Math.Abs(x1 - x0), Math.Abs(y1 - y0));
  }

  public bool Contains(double x, double y)
  {
    return x >= this.Left && x <= this.Right && y >= this.Top && y <= this.Bottom;
  }

  public bool Contains(ScreenPoint p) => this.Contains(p.x, p.y);

  public override string ToString()
  {
    return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", (object) this.left, (object) this.top, (object) this.width, (object) this.height);
  }

  public string ToString(string format, IFormatProvider formatProvider)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("(");
    stringBuilder.Append(this.Left.ToString(format, formatProvider));
    stringBuilder.Append(", ");
    stringBuilder.Append(this.Top.ToString(format, formatProvider));
    stringBuilder.Append(", ");
    stringBuilder.Append(this.Width.ToString(format, formatProvider));
    stringBuilder.Append(", ");
    stringBuilder.Append(this.Height.ToString(format, formatProvider));
    stringBuilder.Append(")");
    return stringBuilder.ToString();
  }

  public bool Equals(OxyRect other)
  {
    return this.Left.Equals(other.Left) && this.Top.Equals(other.Top) && this.Width.Equals(other.Width) && this.Height.Equals(other.Height);
  }

  public OxyRect Inflate(double dx, double dy)
  {
    return new OxyRect(this.left - dx, this.top - dy, this.width + dx * 2.0, this.height + dy * 2.0);
  }

  public OxyRect Inflate(OxyThickness t)
  {
    return new OxyRect(this.left - t.Left, this.top - t.Top, this.width + t.Left + t.Right, this.height + t.Top + t.Bottom);
  }

  public OxyRect Deflate(OxyThickness t)
  {
    return new OxyRect(this.left + t.Left, this.top + t.Top, this.width - t.Left - t.Right, this.height - t.Top - t.Bottom);
  }

  public OxyRect Offset(double offsetX, double offsetY)
  {
    return new OxyRect(this.left + offsetX, this.top + offsetY, this.width, this.height);
  }
}
