// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.EllipseAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot.Annotations;

public class EllipseAnnotation : ShapeAnnotation
{
  private OxyRect screenRectangle;

  public EllipseAnnotation()
  {
    this.Width = double.NaN;
    this.Height = double.NaN;
  }

  public double X { get; set; }

  public double Y { get; set; }

  public double Width { get; set; }

  public double Height { get; set; }

  public override void Render(IRenderContext rc)
  {
    base.Render(rc);
    this.screenRectangle = new OxyRect(this.Transform(this.X - this.Width / 2.0, this.Y - this.Height / 2.0), this.Transform(this.X + this.Width / 2.0, this.Y + this.Height / 2.0));
    OxyRect clippingRect = this.GetClippingRect();
    rc.DrawClippedEllipse(clippingRect, this.screenRectangle, this.GetSelectableFillColor(this.Fill), this.GetSelectableColor(this.Stroke), this.StrokeThickness);
    if (string.IsNullOrEmpty(this.Text))
      return;
    ScreenPoint actualTextPosition = this.GetActualTextPosition((Func<ScreenPoint>) (() => this.screenRectangle.Center));
    rc.DrawClippedText(clippingRect, actualTextPosition, this.Text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.TextRotation, this.TextHorizontalAlignment, this.TextVerticalAlignment);
  }

  protected override HitTestResult HitTestOverride(HitTestArguments args)
  {
    return this.screenRectangle.Contains(args.Point) ? new HitTestResult((UIElement) this, args.Point) : (HitTestResult) null;
  }
}
