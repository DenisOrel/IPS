// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ThreeColorLineSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class ThreeColorLineSeries : LineSeries
{
  private OxyColor defaultColorLo;
  private OxyColor defaultColorHi;

  public ThreeColorLineSeries()
  {
    this.LimitLo = -5.0;
    this.ColorLo = OxyColors.Blue;
    this.LineStyleLo = LineStyle.Solid;
    this.LimitHi = 5.0;
    this.ColorHi = OxyColors.Red;
    this.LineStyleHi = LineStyle.Solid;
  }

  public OxyColor ColorLo { get; set; }

  public OxyColor ColorHi { get; set; }

  public OxyColor ActualColorLo => this.ColorLo.GetActualColor(this.defaultColorLo);

  public OxyColor ActualColorHi => this.ColorHi.GetActualColor(this.defaultColorHi);

  public double LimitHi { get; set; }

  public double LimitLo { get; set; }

  public double[] DashesHi { get; set; }

  public double[] DashesLo { get; set; }

  public LineStyle LineStyleHi { get; set; }

  public LineStyle LineStyleLo { get; set; }

  public LineStyle ActualLineStyleHi
  {
    get => this.LineStyleHi == LineStyle.Automatic ? LineStyle.Solid : this.LineStyleHi;
  }

  public LineStyle ActualLineStyleLo
  {
    get => this.LineStyleLo == LineStyle.Automatic ? LineStyle.Solid : this.LineStyleLo;
  }

  protected double[] ActualDashArrayHi => this.DashesHi ?? this.ActualLineStyleHi.GetDashArray();

  protected double[] ActualDashArrayLo => this.DashesLo ?? this.ActualLineStyleLo.GetDashArray();

  protected internal override void SetDefaultValues()
  {
    base.SetDefaultValues();
    if (this.ColorLo.IsAutomatic())
      this.defaultColorLo = this.PlotModel.GetDefaultColor();
    if (this.LineStyleLo == LineStyle.Automatic)
      this.LineStyleLo = this.PlotModel.GetDefaultLineStyle();
    if (this.ColorHi.IsAutomatic())
      this.defaultColorHi = this.PlotModel.GetDefaultColor();
    if (this.LineStyleHi != LineStyle.Automatic)
      return;
    this.LineStyleHi = this.PlotModel.GetDefaultLineStyle();
  }

  protected override void RenderLine(
    IRenderContext rc,
    OxyRect clippingRect,
    IList<ScreenPoint> pointsToRender)
  {
    double bottom = clippingRect.Bottom;
    double top1 = clippingRect.Top;
    double top2 = this.YAxis.Transform(this.LimitLo);
    double top3 = this.YAxis.Transform(this.LimitHi);
    if (top2 < clippingRect.Top)
      top2 = clippingRect.Top;
    if (top2 > clippingRect.Bottom)
      top2 = clippingRect.Bottom;
    if (top3 < clippingRect.Top)
      top3 = clippingRect.Top;
    if (top3 > clippingRect.Bottom)
      top3 = clippingRect.Bottom;
    clippingRect = new OxyRect(clippingRect.Left, top3, clippingRect.Width, top2 - top3);
    rc.DrawClippedLine(clippingRect, pointsToRender, this.MinimumSegmentLength * this.MinimumSegmentLength, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, this.ActualDashArray, this.LineJoin, false);
    clippingRect = new OxyRect(clippingRect.Left, top2, clippingRect.Width, bottom - top2);
    rc.DrawClippedLine(clippingRect, pointsToRender, this.MinimumSegmentLength * this.MinimumSegmentLength, this.GetSelectableColor(this.ActualColorLo), this.StrokeThickness, this.ActualDashArrayLo, this.LineJoin, false);
    clippingRect = new OxyRect(clippingRect.Left, top1, clippingRect.Width, top3 - top1);
    rc.DrawClippedLine(clippingRect, pointsToRender, this.MinimumSegmentLength * this.MinimumSegmentLength, this.GetSelectableColor(this.ActualColorHi), this.StrokeThickness, this.ActualDashArrayHi, this.LineJoin, false);
  }
}
