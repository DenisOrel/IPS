// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.PointAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Annotations;

public class PointAnnotation : ShapeAnnotation
{
  private ScreenPoint screenPosition;

  public PointAnnotation()
  {
    this.Size = 4.0;
    this.TextMargin = 2.0;
    this.Shape = MarkerType.Circle;
    this.TextVerticalAlignment = VerticalAlignment.Top;
  }

  public double X { get; set; }

  public double Y { get; set; }

  public double Size { get; set; }

  public double TextMargin { get; set; }

  public MarkerType Shape { get; set; }

  public ScreenPoint[] CustomOutline { get; set; }

  public override void Render(IRenderContext rc)
  {
    base.Render(rc);
    this.screenPosition = this.Transform(this.X, this.Y);
    OxyRect clippingRect = this.GetClippingRect();
    rc.DrawMarker(clippingRect, this.screenPosition, this.Shape, (IList<ScreenPoint>) this.CustomOutline, this.Size, this.Fill, this.Stroke, this.StrokeThickness);
    if (string.IsNullOrEmpty(this.Text))
      return;
    ScreenPoint p = this.screenPosition + new ScreenVector((double) -(int) this.TextHorizontalAlignment * (this.Size + this.TextMargin), (double) -(int) this.TextVerticalAlignment * (this.Size + this.TextMargin));
    rc.DrawClippedText(clippingRect, p, this.Text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.TextRotation, this.TextHorizontalAlignment, this.TextVerticalAlignment);
  }

  protected override HitTestResult HitTestOverride(HitTestArguments args)
  {
    return this.screenPosition.DistanceTo(args.Point) < this.Size ? new HitTestResult((UIElement) this, this.screenPosition) : (HitTestResult) null;
  }
}
