// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.PieSlice
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Series;

public class PieSlice : ICodeGenerating
{
  public PieSlice(string label, double value)
  {
    this.Fill = OxyColors.Automatic;
    this.Label = label;
    this.Value = value;
  }

  public OxyColor Fill { get; set; }

  public OxyColor ActualFillColor => this.Fill.GetActualColor(this.DefaultFillColor);

  public bool IsExploded { get; set; }

  public string Label { get; private set; }

  public double Value { get; private set; }

  internal OxyColor DefaultFillColor { get; set; }

  public string ToCode()
  {
    return CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}", (object) this.Label, (object) this.Value);
  }
}
