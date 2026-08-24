// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.IntervalBarItem
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Series;

public class IntervalBarItem : CategorizedItem, ICodeGenerating
{
  public IntervalBarItem() => this.Color = OxyColors.Automatic;

  public IntervalBarItem(double start, double end, string title = null)
    : this()
  {
    this.Start = start;
    this.End = end;
    this.Title = title;
  }

  public OxyColor Color { get; set; }

  public double End { get; set; }

  public double Start { get; set; }

  public string Title { get; set; }

  public string ToCode()
  {
    return this.Color.IsUndefined() ? CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3}", (object) this.Start, (object) this.End, (object) this.Title, (object) OxyColorExtensions.ToCode(this.Color)) : (this.Title != null ? CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2}", (object) this.Start, (object) this.End, (object) this.Title) : CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", (object) this.Start, (object) this.End));
  }
}
