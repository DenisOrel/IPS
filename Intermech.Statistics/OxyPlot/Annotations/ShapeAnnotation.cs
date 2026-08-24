// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.ShapeAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Annotations;

public abstract class ShapeAnnotation : TextualAnnotation
{
  protected ShapeAnnotation()
  {
    this.Stroke = OxyColors.Black;
    this.Fill = OxyColors.LightBlue;
  }

  public OxyColor Fill { get; set; }

  public OxyColor Stroke { get; set; }

  public double StrokeThickness { get; set; }
}
