// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.TextAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Annotations;

public class TextAnnotation : TextualAnnotation
{
  private IList<ScreenPoint> actualBounds;

  public TextAnnotation()
  {
    this.Stroke = OxyColors.Black;
    this.Background = OxyColors.Undefined;
    this.StrokeThickness = 1.0;
    this.TextVerticalAlignment = VerticalAlignment.Bottom;
    this.Padding = new OxyThickness(4.0);
  }

  public OxyColor Background { get; set; }

  public ScreenVector Offset { get; set; }

  public OxyThickness Padding { get; set; }

  public OxyColor Stroke { get; set; }

  public double StrokeThickness { get; set; }

  public override void Render(IRenderContext rc)
  {
    base.Render(rc);
    ScreenPoint screenPoint = this.Transform(this.TextPosition) + this.Offset;
    OxyRect clippingRect = this.GetClippingRect();
    OxySize size = rc.MeasureText(this.Text, this.ActualFont, this.ActualFontSize, this.ActualFontWeight);
    rc.SetClip(clippingRect);
    this.actualBounds = TextAnnotation.GetTextBounds(screenPoint, size, this.Padding, this.TextRotation, this.TextHorizontalAlignment, this.TextVerticalAlignment);
    if ((this.TextRotation % 90.0).Equals(0.0))
    {
      OxyRect rectangle = new OxyRect(this.actualBounds[0], this.actualBounds[2]);
      rc.DrawRectangle(rectangle, this.Background, this.Stroke, this.StrokeThickness);
    }
    else
      rc.DrawPolygon(this.actualBounds, this.Background, this.Stroke, this.StrokeThickness);
    rc.DrawMathText(screenPoint, this.Text, this.GetSelectableFillColor(this.ActualTextColor), this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.TextRotation, this.TextHorizontalAlignment, this.TextVerticalAlignment);
    rc.ResetClip();
  }

  protected override HitTestResult HitTestOverride(HitTestArguments args)
  {
    if (this.actualBounds == null)
      return (HitTestResult) null;
    return !ScreenPointHelper.IsPointInPolygon(args.Point, this.actualBounds) ? (HitTestResult) null : new HitTestResult((UIElement) this, args.Point);
  }

  private static IList<ScreenPoint> GetTextBounds(
    ScreenPoint position,
    OxySize size,
    OxyThickness padding,
    double rotation,
    HorizontalAlignment horizontalAlignment,
    VerticalAlignment verticalAlignment)
  {
    double num1;
    double num2;
    switch (horizontalAlignment)
    {
      case HorizontalAlignment.Center:
        num1 = -size.Width * 0.5;
        num2 = -num1;
        break;
      case HorizontalAlignment.Right:
        num1 = -size.Width;
        num2 = 0.0;
        break;
      default:
        num1 = 0.0;
        num2 = size.Width;
        break;
    }
    double num3;
    double num4;
    switch (verticalAlignment)
    {
      case VerticalAlignment.Middle:
        num3 = -size.Height * 0.5;
        num4 = -num3;
        break;
      case VerticalAlignment.Bottom:
        num3 = -size.Height;
        num4 = 0.0;
        break;
      default:
        num3 = 0.0;
        num4 = size.Height;
        break;
    }
    double num5 = Math.Cos(rotation / 180.0 * Math.PI);
    double y = Math.Sin(rotation / 180.0 * Math.PI);
    ScreenVector screenVector1 = new ScreenVector(num5, y);
    ScreenVector screenVector2 = new ScreenVector(-y, num5);
    return (IList<ScreenPoint>) new ScreenPoint[4]
    {
      position + screenVector1 * (num1 - padding.Left) + screenVector2 * (num3 - padding.Top),
      position + screenVector1 * (num2 + padding.Right) + screenVector2 * (num3 - padding.Top),
      position + screenVector1 * (num2 + padding.Right) + screenVector2 * (num4 + padding.Bottom),
      position + screenVector1 * (num1 - padding.Left) + screenVector2 * (num4 + padding.Bottom)
    };
  }
}
