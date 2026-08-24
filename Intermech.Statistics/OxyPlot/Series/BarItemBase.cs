// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.BarItemBase
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Series;

public abstract class BarItemBase : CategorizedItem, ICodeGenerating
{
  protected BarItemBase()
  {
    this.Value = double.NaN;
    this.Color = OxyColors.Automatic;
  }

  public OxyColor Color { get; set; }

  public double Value { get; set; }

  public virtual string ToCode()
  {
    return !this.Color.IsUndefined() ? CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2}", (object) this.Value, (object) this.CategoryIndex, (object) OxyColorExtensions.ToCode(this.Color)) : (this.CategoryIndex != -1 ? CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", (object) this.Value, (object) this.CategoryIndex) : CodeGenerator.FormatConstructor(this.GetType(), "{0}", (object) this.Value));
  }
}
