// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ScatterErrorPoint
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Series;

public class ScatterErrorPoint : ScatterPoint
{
  public ScatterErrorPoint(
    double x,
    double y,
    double errorX,
    double errorY,
    double size = double.NaN,
    double value = double.NaN,
    object tag = null)
    : base(x, y, size, value, tag)
  {
    this.ErrorX = errorX;
    this.ErrorY = errorY;
  }

  public double ErrorX { get; private set; }

  public double ErrorY { get; private set; }

  public override string ToCode()
  {
    return double.IsNaN(this.Size) && double.IsNaN(this.Value) ? CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}, {2}, {3}", (object) this.X, (object) this.Y, (object) this.ErrorX, (object) this.ErrorY) : (double.IsNaN(this.Value) ? CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}, {2}, {3}, {4}", (object) this.X, (object) this.Y, (object) this.ErrorX, (object) this.ErrorY, (object) this.Size) : CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}, {2}, {3}, {3}, {4}, {5}", (object) this.X, (object) this.Y, (object) this.ErrorX, (object) this.ErrorY, (object) this.Size, (object) this.Value));
  }
}
