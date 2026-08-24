// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.TwoColorLineSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class TwoColorLineSeries : LineSeries
{
  private OxyColor defaultColor2;

  public TwoColorLineSeries()
  {
    this.Limit = 0.0;
    this.Color2 = OxyColors.Blue;
    this.LineStyle2 = LineStyle.Solid;
  }

  public OxyColor Color2 { get; set; }

  public OxyColor ActualColor2 => this.Color2.GetActualColor(this.defaultColor2);

  public double Limit { get; set; }

  public double[] Dashes2 { get; set; }

  public LineStyle LineStyle2 { get; set; }

  public LineStyle ActualLineStyle2
  {
    get => this.LineStyle2 == LineStyle.Automatic ? LineStyle.Solid : this.LineStyle2;
  }

  protected double[] ActualDashArray2 => this.Dashes2 ?? this.ActualLineStyle2.GetDashArray();

  protected internal override void SetDefaultValues()
  {
    base.SetDefaultValues();
    if (this.Color2.IsAutomatic())
      this.defaultColor2 = this.PlotModel.GetDefaultColor();
    if (this.LineStyle2 != LineStyle.Automatic)
      return;
    this.LineStyle2 = this.PlotModel.GetDefaultLineStyle();
  }

  protected override void RenderLine(
    IRenderContext rc,
    OxyRect clippingRect,
    IList<ScreenPoint> pointsToRender)
  {
    double bottom = clippingRect.Bottom;
    double top = this.YAxis.Transform(this.Limit);
    if (top < clippingRect.Top)
      top = clippingRect.Top;
    if (top > clippingRect.Bottom)
      top = clippingRect.Bottom;
    double[] actualDashArray = this.ActualDashArray;
    double[] actualDashArray2 = this.ActualDashArray2;
    clippingRect = new OxyRect(clippingRect.Left, clippingRect.Top, clippingRect.Width, top - clippingRect.Top);
    rc.DrawClippedLine(clippingRect, pointsToRender, this.MinimumSegmentLength * this.MinimumSegmentLength, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, actualDashArray, this.LineJoin, false);
    clippingRect = new OxyRect(clippingRect.Left, top, clippingRect.Width, bottom - top);
    rc.DrawClippedLine(clippingRect, pointsToRender, this.MinimumSegmentLength * this.MinimumSegmentLength, this.GetSelectableColor(this.ActualColor2), this.StrokeThickness, actualDashArray2, this.LineJoin, false);
  }
}
