// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.TornadoBarItem
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Series;

public class TornadoBarItem : CategorizedItem, ICodeGenerating
{
  public TornadoBarItem()
  {
    this.Minimum = double.NaN;
    this.Maximum = double.NaN;
    this.BaseValue = double.NaN;
    this.MinimumColor = OxyColors.Automatic;
    this.MaximumColor = OxyColors.Automatic;
  }

  public double BaseValue { get; set; }

  public double Maximum { get; set; }

  public OxyColor MaximumColor { get; set; }

  public double Minimum { get; set; }

  public OxyColor MinimumColor { get; set; }

  public string ToCode()
  {
    return !this.MaximumColor.IsUndefined() ? CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3},{4}", (object) this.Minimum, (object) this.Maximum, (object) this.BaseValue, (object) OxyColorExtensions.ToCode(this.MinimumColor), (object) OxyColorExtensions.ToCode(this.MaximumColor)) : (!this.MinimumColor.IsUndefined() ? CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3}", (object) this.Minimum, (object) this.Maximum, (object) this.BaseValue, (object) OxyColorExtensions.ToCode(this.MinimumColor)) : (!double.IsNaN(this.BaseValue) ? CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2}", (object) this.Minimum, (object) this.Maximum, (object) this.BaseValue) : CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", (object) this.Minimum, (object) this.Maximum)));
  }
}
