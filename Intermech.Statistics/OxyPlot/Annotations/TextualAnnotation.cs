// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.TextualAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot.Annotations;

public abstract class TextualAnnotation : Annotation
{
  protected TextualAnnotation()
  {
    this.TextHorizontalAlignment = HorizontalAlignment.Center;
    this.TextVerticalAlignment = VerticalAlignment.Middle;
    this.TextPosition = DataPoint.Undefined;
    this.TextRotation = 0.0;
  }

  public string Text { get; set; }

  public DataPoint TextPosition { get; set; }

  public HorizontalAlignment TextHorizontalAlignment { get; set; }

  public VerticalAlignment TextVerticalAlignment { get; set; }

  public double TextRotation { get; set; }

  protected ScreenPoint GetActualTextPosition(Func<ScreenPoint> defaultPosition)
  {
    return !this.TextPosition.IsDefined() ? defaultPosition() : this.Transform(this.TextPosition);
  }
}
