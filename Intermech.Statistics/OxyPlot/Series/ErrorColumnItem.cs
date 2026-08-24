// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ErrorColumnItem
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Series;

public class ErrorColumnItem : ColumnItem
{
  public ErrorColumnItem() => this.Color = OxyColors.Undefined;

  public ErrorColumnItem(double value, double error, int categoryIndex = -1)
    : this()
  {
    this.Value = value;
    this.Error = error;
    this.CategoryIndex = categoryIndex;
  }

  public double Error { get; set; }

  public override string ToCode()
  {
    return !this.Color.IsUndefined() ? CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3}", (object) this.Value, (object) this.Error, (object) this.CategoryIndex, (object) OxyColorExtensions.ToCode(this.Color)) : (this.CategoryIndex != -1 ? CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2}", (object) this.Value, (object) this.Error, (object) this.CategoryIndex) : CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", (object) this.Value, (object) this.Error));
  }
}
