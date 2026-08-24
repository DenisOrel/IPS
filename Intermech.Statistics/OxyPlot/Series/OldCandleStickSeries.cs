// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.OldCandleStickSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

[Obsolete("use replacement CandleStickSeries instead")]
public class OldCandleStickSeries : HighLowSeries
{
  public OldCandleStickSeries()
  {
    this.CandleWidth = 10.0;
    this.IncreasingFill = OxyColors.Automatic;
    this.DecreasingFill = OxyColors.Undefined;
    this.ShadowEndColor = OxyColors.Undefined;
    this.ShadowEndLength = 1.0;
  }

  public double CandleWidth { get; set; }

  public OxyColor IncreasingFill { get; set; }

  public OxyColor DecreasingFill { get; set; }

  public OxyColor ShadowEndColor { get; set; }

  public double ShadowEndLength { get; set; }

  public OxyColor ActualIncreasingFill => this.IncreasingFill.GetActualColor(this.ActualColor);

  public override void Render(IRenderContext rc)
  {
    if (this.Items.Count == 0)
      return;
    this.VerifyAxes();
    OxyRect clippingRect = this.GetClippingRect();
    double[] dashArray = this.LineStyle.GetDashArray();
    OxyColor selectableColor1 = this.GetSelectableColor(this.ActualColor);
    OxyColor selectableColor2 = this.GetSelectableColor(this.ShadowEndColor);
    if (this.StrokeThickness <= 0.0 || this.LineStyle == LineStyle.None)
      return;
    foreach (HighLowItem pt in this.Items)
    {
      if (this.IsValidItem(pt, this.XAxis, this.YAxis) && pt.X > this.XAxis.ActualMinimum && pt.X < this.XAxis.ActualMaximum)
      {
        ScreenPoint screenPoint1 = this.Transform(pt.X, pt.High);
        ScreenPoint screenPoint2 = this.Transform(pt.X, pt.Low);
        if (double.IsNaN(pt.Open) || double.IsNaN(pt.Close))
        {
          rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
          {
            screenPoint2,
            screenPoint1
          }, 0.0, selectableColor1, this.StrokeThickness, dashArray, this.LineJoin, false);
        }
        else
        {
          ScreenPoint screenPoint3 = this.Transform(pt.X, pt.Open);
          ScreenPoint screenPoint4 = this.Transform(pt.X, pt.Close);
          ScreenPoint screenPoint5 = new ScreenPoint(screenPoint3.X, Math.Max(screenPoint3.Y, screenPoint4.Y));
          ScreenPoint screenPoint6 = new ScreenPoint(screenPoint3.X, Math.Min(screenPoint3.Y, screenPoint4.Y));
          rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
          {
            screenPoint1,
            screenPoint6
          }, 0.0, selectableColor1, this.StrokeThickness, dashArray, this.LineJoin, true);
          rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
          {
            screenPoint5,
            screenPoint2
          }, 0.0, selectableColor1, this.StrokeThickness, dashArray, this.LineJoin, true);
          if (this.ShadowEndColor.IsVisible() && this.ShadowEndLength > 0.0)
          {
            ScreenPoint screenPoint7 = new ScreenPoint(screenPoint1.X - this.CandleWidth * 0.5 * this.ShadowEndLength - 1.0, screenPoint1.Y);
            ScreenPoint screenPoint8 = new ScreenPoint(screenPoint1.X + this.CandleWidth * 0.5 * this.ShadowEndLength, screenPoint1.Y);
            rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
            {
              screenPoint7,
              screenPoint8
            }, 0.0, selectableColor2, this.StrokeThickness, dashArray, this.LineJoin, true);
            ScreenPoint screenPoint9 = new ScreenPoint(screenPoint2.X - this.CandleWidth * 0.5 * this.ShadowEndLength - 1.0, screenPoint2.Y);
            ScreenPoint screenPoint10 = new ScreenPoint(screenPoint2.X + this.CandleWidth * 0.5 * this.ShadowEndLength, screenPoint2.Y);
            rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
            {
              screenPoint9,
              screenPoint10
            }, 0.0, selectableColor2, this.StrokeThickness, dashArray, this.LineJoin, true);
          }
          OxyRect rect = new OxyRect((screenPoint3 + new ScreenVector(-this.CandleWidth * 0.5, 0.0)).X, screenPoint6.Y, this.CandleWidth, screenPoint5.Y - screenPoint6.Y);
          OxyColor fill = pt.Close > pt.Open ? this.GetSelectableFillColor(this.ActualIncreasingFill) : this.GetSelectableFillColor(this.DecreasingFill);
          rc.DrawClippedRectangleAsPolygon(clippingRect, rect, fill, selectableColor1, this.StrokeThickness);
        }
      }
    }
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double x = (legendBox.Left + legendBox.Right) / 2.0;
    double num = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.7;
    double top = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.3;
    double[] dashArray = this.LineStyle.GetDashArray();
    rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
    {
      new ScreenPoint(x, legendBox.Top),
      new ScreenPoint(x, legendBox.Bottom)
    }, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, dashArray, aliased: true);
    if (this.ShadowEndColor.IsVisible() && this.ShadowEndLength > 0.0)
    {
      ScreenPoint screenPoint1 = new ScreenPoint(x - this.CandleWidth * 0.5 * this.ShadowEndLength - 1.0, legendBox.Top);
      ScreenPoint screenPoint2 = new ScreenPoint(x + this.CandleWidth * 0.5 * this.ShadowEndLength, legendBox.Top);
      rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
      {
        screenPoint1,
        screenPoint2
      }, this.GetSelectableColor(this.ShadowEndColor), this.StrokeThickness, dashArray, this.LineJoin, true);
      ScreenPoint screenPoint3 = new ScreenPoint(x - this.CandleWidth * 0.5 * this.ShadowEndLength - 1.0, legendBox.Bottom);
      ScreenPoint screenPoint4 = new ScreenPoint(x + this.CandleWidth * 0.5 * this.ShadowEndLength, legendBox.Bottom);
      rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
      {
        screenPoint3,
        screenPoint4
      }, this.GetSelectableColor(this.ShadowEndColor), this.StrokeThickness, dashArray, this.LineJoin, true);
    }
    rc.DrawRectangleAsPolygon(new OxyRect(x - this.CandleWidth * 0.5, top, this.CandleWidth, num - top), this.GetSelectableFillColor(this.ActualIncreasingFill), this.GetSelectableColor(this.ActualColor), this.StrokeThickness);
  }
}
