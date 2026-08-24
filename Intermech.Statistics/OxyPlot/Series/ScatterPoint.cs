// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ScatterPoint
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Series;

public class ScatterPoint : ICodeGenerating
{
  public ScatterPoint(double x, double y, double size = double.NaN, double value = double.NaN, object tag = null)
  {
    this.X = x;
    this.Y = y;
    this.Size = size;
    this.Value = value;
    this.Tag = tag;
  }

  public double X { get; private set; }

  public double Y { get; private set; }

  public double Size { get; set; }

  public double Value { get; set; }

  public object Tag { get; set; }

  public virtual string ToCode()
  {
    return double.IsNaN(this.Size) && double.IsNaN(this.Value) ? CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}", (object) this.X, (object) this.Y) : (double.IsNaN(this.Value) ? CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}, {2}", (object) this.X, (object) this.Y, (object) this.Size) : CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}, {2}, {3}", (object) this.X, (object) this.Y, (object) this.Size, (object) this.Value));
  }

  public override string ToString() => $"{(object) this.X} {(object) this.Y}";
}
