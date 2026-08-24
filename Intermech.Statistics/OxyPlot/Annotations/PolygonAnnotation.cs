// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.PolygonAnnotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Annotations;

public class PolygonAnnotation : ShapeAnnotation
{
  private IList<ScreenPoint> screenPoints;

  public PolygonAnnotation()
  {
    this.LineStyle = LineStyle.Solid;
    this.LineJoin = LineJoin.Miter;
    this.Points = new List<DataPoint>();
  }

  public LineJoin LineJoin { get; set; }

  public LineStyle LineStyle { get; set; }

  public List<DataPoint> Points { get; private set; }

  public override void Render(IRenderContext rc)
  {
    base.Render(rc);
    if (this.Points == null)
      return;
    this.screenPoints = (IList<ScreenPoint>) this.Points.Select<DataPoint, ScreenPoint>(new Func<DataPoint, ScreenPoint>(((Annotation) this).Transform)).ToList<ScreenPoint>();
    if (this.screenPoints.Count == 0)
      return;
    OxyRect clippingRect = this.GetClippingRect();
    rc.DrawClippedPolygon(clippingRect, this.screenPoints, 16.0, this.GetSelectableFillColor(this.Fill), this.GetSelectableColor(this.Stroke), this.StrokeThickness, this.LineStyle, this.LineJoin);
    if (string.IsNullOrEmpty(this.Text))
      return;
    ScreenPoint actualTextPosition = this.GetActualTextPosition((Func<ScreenPoint>) (() => ScreenPointHelper.GetCentroid(this.screenPoints)));
    rc.DrawClippedText(clippingRect, actualTextPosition, this.Text, this.ActualTextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.TextRotation, this.TextHorizontalAlignment, this.TextVerticalAlignment);
  }

  protected override HitTestResult HitTestOverride(HitTestArguments args)
  {
    if (this.screenPoints == null)
      return (HitTestResult) null;
    return !ScreenPointHelper.IsPointInPolygon(args.Point, this.screenPoints) ? (HitTestResult) null : new HitTestResult((UIElement) this, args.Point);
  }
}
