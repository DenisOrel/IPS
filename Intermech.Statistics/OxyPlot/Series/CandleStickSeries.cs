// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.CandleStickSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace OxyPlot.Series;

public class CandleStickSeries : HighLowSeries
{
  private double minDx;
  private int winIndex;

  public CandleStickSeries()
  {
    this.IncreasingColor = OxyColors.DarkGreen;
    this.DecreasingColor = OxyColors.Red;
    this.CandleWidth = 0.0;
  }

  public OxyColor IncreasingColor { get; set; }

  public OxyColor DecreasingColor { get; set; }

  public double CandleWidth { get; set; }

  public void Append(object bar)
  {
    HighLowItem nativeBar = this.ToNativeBar(bar);
    List<HighLowItem> items = this.Items;
    if (items.Count > 0 && items[items.Count - 1].X > nativeBar.X)
      throw new ArgumentException("cannot append bar out of order, must be sequential in X");
    items.Add(nativeBar);
  }

  public int FindByX(double x, int startIndex = -1)
  {
    if (startIndex < 0)
      startIndex = this.winIndex;
    return HighLowItem.FindIndex(this.Items, x, startIndex);
  }

  public override void Render(IRenderContext rc)
  {
    int count = this.Items.Count;
    List<HighLowItem> items = this.Items;
    if (count == 0 || this.StrokeThickness <= 0.0 || this.LineStyle == LineStyle.None)
      return;
    this.VerifyAxes();
    OxyRect clippingRect = this.GetClippingRect();
    double[] dashArray = this.LineStyle.GetDashArray();
    double num = this.CandleWidth > 0.0 ? this.CandleWidth : this.minDx * 0.8;
    double width = this.XAxis.Transform(items[0].X + num) - this.XAxis.Transform(items[0].X);
    OxyColor selectableFillColor1 = this.GetSelectableFillColor(this.IncreasingColor);
    OxyColor selectableFillColor2 = this.GetSelectableFillColor(this.DecreasingColor);
    OxyColor selectableColor1 = this.GetSelectableColor(this.IncreasingColor.ChangeIntensity(0.7));
    OxyColor selectableColor2 = this.GetSelectableColor(this.DecreasingColor.ChangeIntensity(0.7));
    double actualMinimum = this.XAxis.ActualMinimum;
    double actualMaximum = this.XAxis.ActualMaximum;
    this.winIndex = HighLowItem.FindIndex(items, actualMinimum, this.winIndex);
    for (int winIndex = this.winIndex; winIndex < count; ++winIndex)
    {
      HighLowItem pt = items[winIndex];
      if (pt.X > actualMaximum)
        break;
      if (this.IsValidItem(pt, this.XAxis, this.YAxis))
      {
        OxyColor fill = pt.Close > pt.Open ? selectableFillColor1 : selectableFillColor2;
        OxyColor stroke = pt.Close > pt.Open ? selectableColor1 : selectableColor2;
        ScreenPoint screenPoint1 = this.Transform(pt.X, pt.High);
        ScreenPoint screenPoint2 = this.Transform(pt.X, pt.Low);
        ScreenPoint screenPoint3 = this.Transform(pt.X, pt.Open);
        ScreenPoint screenPoint4 = this.Transform(pt.X, pt.Close);
        ScreenPoint screenPoint5 = new ScreenPoint(screenPoint3.X, Math.Max(screenPoint3.Y, screenPoint4.Y));
        ScreenPoint screenPoint6 = new ScreenPoint(screenPoint3.X, Math.Min(screenPoint3.Y, screenPoint4.Y));
        rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
        {
          screenPoint1,
          screenPoint6
        }, 0.0, stroke, this.StrokeThickness, dashArray, this.LineJoin, true);
        rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
        {
          screenPoint5,
          screenPoint2
        }, 0.0, stroke, this.StrokeThickness, dashArray, this.LineJoin, true);
        OxyRect rect = new OxyRect((screenPoint3 + new ScreenVector(-width * 0.5, 0.0)).X, screenPoint6.Y, width, screenPoint5.Y - screenPoint6.Y);
        rc.DrawClippedRectangleAsPolygon(clippingRect, rect, fill, stroke, this.StrokeThickness);
      }
    }
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double x = (legendBox.Left + legendBox.Right) / 2.0;
    double num1 = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.7;
    double top = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.3;
    double[] dashArray = this.LineStyle.GetDashArray();
    double num2 = this.CandleWidth > 0.0 ? this.CandleWidth : this.minDx * 0.8;
    double width = Math.Min(legendBox.Width, this.XAxis.Transform(this.Items[0].X + num2) - this.XAxis.Transform(this.Items[0].X));
    rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
    {
      new ScreenPoint(x, legendBox.Top),
      new ScreenPoint(x, legendBox.Bottom)
    }, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, dashArray, aliased: true);
    rc.DrawRectangleAsPolygon(new OxyRect(x - width * 0.5, top, width, num1 - top), this.GetSelectableFillColor(this.IncreasingColor), this.GetSelectableColor(this.ActualColor), this.StrokeThickness);
  }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (((this.XAxis == null ? 1 : (this.YAxis == null ? 1 : 0)) | (interpolate ? 1 : 0)) != 0 || this.Items.Count == 0)
      return (TrackerHitResult) null;
    int count = this.Items.Count;
    DataPoint xy = this.InverseTransform(point);
    double x = xy.X;
    if (x > this.Items[count - 1].X + this.minDx || x < this.Items[0].X - this.minDx)
      return (TrackerHitResult) null;
    int index1 = HighLowItem.FindIndex(this.Items, x, this.winIndex);
    int index2 = index1 + 1 < this.Items.Count ? index1 + 1 : index1;
    Func<HighLowItem, double> func = (Func<HighLowItem, double>) (bar =>
    {
      double num1 = bar.X - xy.X;
      double num2 = bar.Open - xy.Y;
      double num3 = bar.High - xy.Y;
      double num4 = bar.Low - xy.Y;
      double num5 = bar.Close - xy.Y;
      return Math.Min(num1 * num1 + num2 * num2, Math.Min(num1 * num1 + num3 * num3, Math.Min(num1 * num1 + num4 * num4, num1 * num1 + num5 * num5)));
    });
    int index3 = func(this.Items[index1]) <= func(this.Items[index2]) ? index1 : index2;
    HighLowItem highLowItem = this.Items[index3];
    DataPoint p = new DataPoint(highLowItem.X, highLowItem.Close);
    return new TrackerHitResult()
    {
      Series = (OxyPlot.Series.Series) this,
      DataPoint = p,
      Position = this.Transform(p),
      Item = (object) highLowItem,
      Index = (double) index3,
      Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, (object) highLowItem, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(highLowItem.X), this.YAxis.GetValue(highLowItem.High), this.YAxis.GetValue(highLowItem.Low), this.YAxis.GetValue(highLowItem.Open), this.YAxis.GetValue(highLowItem.Close))
    };
  }

  protected internal override void UpdateData()
  {
    base.UpdateData();
    this.winIndex = 0;
    List<HighLowItem> items = this.Items;
    int count = items.Count;
    this.minDx = double.MaxValue;
    for (int index = 1; index < count; ++index)
    {
      this.minDx = Math.Min(this.minDx, items[index].X - items[index - 1].X);
      if (this.minDx < 0.0)
        throw new ArgumentException("bars are out of order, must be sequential in x");
    }
    if (count > 1)
      return;
    this.minDx = 1.0;
  }

  private HighLowItem ToNativeBar(object bar)
  {
    if (bar is HighLowItem nativeBar)
      return nativeBar;
    double x = this.FieldValueOf(bar, this.DataFieldX);
    double num1 = this.FieldValueOf(bar, this.DataFieldOpen);
    double num2 = this.FieldValueOf(bar, this.DataFieldHigh);
    double num3 = this.FieldValueOf(bar, this.DataFieldLow);
    double num4 = this.FieldValueOf(bar, this.DataFieldClose);
    double high = num2;
    double low = num3;
    double open = num1;
    double close = num4;
    return new HighLowItem(x, high, low, open, close);
  }

  private double FieldValueOf(object item, string propertyName)
  {
    return propertyName != null ? Axis.ToDouble(RuntimeReflectionExtensions.GetRuntimeProperty(item.GetType(), propertyName).GetValue(item, (object[]) null)) : double.NaN;
  }
}
