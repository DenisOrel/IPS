// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.RectangleBarItem
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Series;

public class RectangleBarItem : ICodeGenerating
{
  public RectangleBarItem() => this.Color = OxyColors.Automatic;

  public RectangleBarItem(double x0, double y0, double x1, double y1)
    : this()
  {
    this.X0 = x0;
    this.Y0 = y0;
    this.X1 = x1;
    this.Y1 = y1;
  }

  public OxyColor Color { get; set; }

  public string Title { get; set; }

  public double X0 { get; set; }

  public double X1 { get; set; }

  public double Y0 { get; set; }

  public double Y1 { get; set; }

  public string ToCode()
  {
    return !this.Color.IsUndefined() ? CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3},{4},{5}", (object) this.X0, (object) this.Y0, (object) this.X1, (object) this.Y1, (object) this.Title, (object) OxyColorExtensions.ToCode(this.Color)) : (this.Title != null ? CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3},{4}", (object) this.X0, (object) this.Y0, (object) this.X1, (object) this.Y1, (object) this.Title) : CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3}", (object) this.X0, (object) this.Y0, (object) this.X1, (object) this.Y1));
  }
}
