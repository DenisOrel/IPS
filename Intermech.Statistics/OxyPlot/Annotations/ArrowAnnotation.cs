// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.ArrowAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Annotations;

public class ArrowAnnotation : TextualAnnotation
{
  private ScreenPoint screenEndPoint;
  private ScreenPoint screenStartPoint;

  public ArrowAnnotation()
  {
    this.HeadLength = 10.0;
    this.HeadWidth = 3.0;
    this.Color = OxyColors.Blue;
    this.StrokeThickness = 2.0;
    this.LineStyle = LineStyle.Solid;
    this.LineJoin = LineJoin.Miter;
  }

  public ScreenVector ArrowDirection { get; set; }

  public OxyColor Color { get; set; }

  public DataPoint EndPoint { get; set; }

  public double HeadLength { get; set; }

  public double HeadWidth { get; set; }

  public LineJoin LineJoin { get; set; }

  public LineStyle LineStyle { get; set; }

  public DataPoint StartPoint { get; set; }

  public double StrokeThickness { get; set; }

  public double Veeness { get; set; }

  public override void Render(IRenderContext rc)
  {
    base.Render(rc);
    this.screenEndPoint = this.Transform(this.EndPoint);
    this.screenStartPoint = this.ArrowDirection.LengthSquared <= 0.0 ? this.Transform(this.StartPoint) : this.screenEndPoint - this.ArrowDirection;
    ScreenVector screenVector1 = this.screenEndPoint - this.screenStartPoint;
    screenVector1.Normalize();
    ScreenVector screenVector2 = new ScreenVector(screenVector1.Y, -screenVector1.X);
    ScreenPoint screenPoint1 = this.screenEndPoint - screenVector1 * this.HeadLength * this.StrokeThickness;
    ScreenPoint screenPoint2 = screenPoint1 + screenVector2 * this.HeadWidth * this.StrokeThickness;
    ScreenPoint screenPoint3 = screenPoint1 - screenVector2 * this.HeadWidth * this.StrokeThickness;
    ScreenPoint screenPoint4 = screenPoint1 + screenVector1 * this.Veeness * this.StrokeThickness;
    OxyRect clippingRect = this.GetClippingRect();
    double[] dashArray = this.LineStyle.GetDashArray();
    rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
    {
      this.screenStartPoint,
      screenPoint4
    }, 16.0, this.GetSelectableColor(this.Color), this.StrokeThickness, dashArray, this.LineJoin, false);
    rc.DrawClippedPolygon(clippingRect, (IList<ScreenPoint>) new ScreenPoint[4]
    {
      screenPoint3,
      this.screenEndPoint,
      screenPoint2,
      screenPoint4
    }, 16.0, this.GetSelectableColor(this.Color), OxyColors.Undefined);
    if (string.IsNullOrEmpty(this.Text))
      return;
    HorizontalAlignment horizontalAlignment = this.TextHorizontalAlignment;
    VerticalAlignment verticalAlignment = this.TextVerticalAlignment;
    if (!this.TextPosition.IsDefined())
    {
      horizontalAlignment = screenVector1.X < 0.0 ? HorizontalAlignment.Left : HorizontalAlignment.Right;
      verticalAlignment = screenVector1.Y < 0.0 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
    }
    ScreenPoint actualTextPosition = this.GetActualTextPosition((Func<ScreenPoint>) (() => this.screenStartPoint));
    rc.DrawClippedText(clippingRect, actualTextPosition, this.Text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.TextRotation, horizontalAlignment, verticalAlignment);
  }

  protected override HitTestResult HitTestOverride(HitTestArguments args)
  {
    if ((args.Point - this.screenStartPoint).Length < args.Tolerance)
      return new HitTestResult((UIElement) this, this.screenStartPoint, index: 1.0);
    if ((args.Point - this.screenEndPoint).Length < args.Tolerance)
      return new HitTestResult((UIElement) this, this.screenEndPoint, index: 2.0);
    ScreenPoint pointOnLine = ScreenPointHelper.FindPointOnLine(args.Point, this.screenStartPoint, this.screenEndPoint);
    return (pointOnLine - args.Point).Length < args.Tolerance ? new HitTestResult((UIElement) this, pointOnLine) : (HitTestResult) null;
  }
}
