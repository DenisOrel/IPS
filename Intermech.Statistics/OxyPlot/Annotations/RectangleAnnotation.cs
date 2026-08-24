// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.RectangleAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot.Annotations;

public class RectangleAnnotation : ShapeAnnotation
{
  private OxyRect screenRectangle;

  public RectangleAnnotation()
  {
    this.MinimumX = double.MinValue;
    this.MaximumX = double.MaxValue;
    this.MinimumY = double.MinValue;
    this.MaximumY = double.MaxValue;
    this.TextRotation = 0.0;
  }

  public double MinimumX { get; set; }

  public double MaximumX { get; set; }

  public double MinimumY { get; set; }

  public double MaximumY { get; set; }

  public override void Render(IRenderContext rc)
  {
    base.Render(rc);
    double x1 = double.IsNaN(this.MinimumX) || this.MinimumX.Equals(double.MinValue) ? this.XAxis.ActualMinimum : this.MinimumX;
    double x2 = double.IsNaN(this.MaximumX) || this.MaximumX.Equals(double.MaxValue) ? this.XAxis.ActualMaximum : this.MaximumX;
    double y1 = double.IsNaN(this.MinimumY) || this.MinimumY.Equals(double.MinValue) ? this.YAxis.ActualMinimum : this.MinimumY;
    double y2 = double.IsNaN(this.MaximumY) || this.MaximumY.Equals(double.MaxValue) ? this.YAxis.ActualMaximum : this.MaximumY;
    this.screenRectangle = new OxyRect(this.Transform(x1, y1), this.Transform(x2, y2));
    OxyRect clippingRect = this.GetClippingRect();
    rc.DrawClippedRectangle(clippingRect, this.screenRectangle, this.GetSelectableFillColor(this.Fill), this.GetSelectableColor(this.Stroke), this.StrokeThickness);
    if (string.IsNullOrEmpty(this.Text))
      return;
    ScreenPoint actualTextPosition = this.GetActualTextPosition((Func<ScreenPoint>) (() => this.screenRectangle.Center));
    rc.DrawClippedText(clippingRect, actualTextPosition, this.Text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.TextRotation, HorizontalAlignment.Center, VerticalAlignment.Middle);
  }

  protected override HitTestResult HitTestOverride(HitTestArguments args)
  {
    return this.screenRectangle.Contains(args.Point) ? new HitTestResult((UIElement) this, args.Point) : (HitTestResult) null;
  }
}
