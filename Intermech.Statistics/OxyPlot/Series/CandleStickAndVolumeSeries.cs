// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.CandleStickAndVolumeSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class CandleStickAndVolumeSeries : XYAxisSeries
{
  public new const string DefaultTrackerFormatString = "Time: {0}\nHigh: {1}\nLow: {2}\nOpen: {3}\nClose: {4}\nBuy Volume: {5}\nSell Volume: {6}";
  private List<OhlcvItem> data;
  private double minDx;
  private int winIndex;

  public CandleStickAndVolumeSeries()
  {
    this.PositiveColor = OxyColors.DarkGreen;
    this.NegativeColor = OxyColors.Red;
    this.SeparatorColor = OxyColors.Black;
    this.CandleWidth = 0.0;
    this.SeparatorStrokeThickness = 1.0;
    this.SeparatorLineStyle = LineStyle.Dash;
    this.StrokeThickness = 1.0;
    this.NegativeHollow = false;
    this.PositiveHollow = true;
    this.StrokeIntensity = 0.8;
    this.VolumeStyle = VolumeStyle.Combined;
    this.VolumeAxisKey = "Volume";
    this.BarAxisKey = (string) null;
    this.TrackerFormatString = "Time: {0}\nHigh: {1}\nLow: {2}\nOpen: {3}\nClose: {4}\nBuy Volume: {5}\nSell Volume: {6}";
  }

  public List<OhlcvItem> Items
  {
    get => this.data ?? (this.data = new List<OhlcvItem>());
    set => this.data = value;
  }

  public LinearAxis BarAxis => (LinearAxis) this.YAxis;

  public LinearAxis VolumeAxis { get; private set; }

  public string VolumeAxisKey { get; set; }

  public string BarAxisKey { get; set; }

  public VolumeStyle VolumeStyle { get; set; }

  public double StrokeThickness { get; set; }

  public double StrokeIntensity { get; set; }

  public double SeparatorStrokeThickness { get; set; }

  public LineStyle SeparatorLineStyle { get; set; }

  public OxyColor PositiveColor { get; set; }

  public OxyColor NegativeColor { get; set; }

  public OxyColor SeparatorColor { get; set; }

  public bool PositiveHollow { get; set; }

  public bool NegativeHollow { get; set; }

  public double CandleWidth { get; set; }

  public double MinimumVolume { get; protected set; }

  public double MaximumVolume { get; protected set; }

  public double AverageVolume { get; protected set; }

  public void Append(OhlcvItem bar)
  {
    if (this.data == null)
      this.data = new List<OhlcvItem>();
    if (this.data.Count > 0 && this.data[this.data.Count - 1].X > bar.X)
      throw new ArgumentException("cannot append bar out of order, must be sequential in X");
    this.data.Add(bar);
  }

  public int FindByX(double x, int startingIndex = -1)
  {
    if (startingIndex < 0)
      startingIndex = this.winIndex;
    return OhlcvItem.FindIndex(this.data, x, startingIndex);
  }

  public override void Render(IRenderContext rc)
  {
    if (this.data == null || this.data.Count == 0)
      return;
    List<OhlcvItem> data = this.data;
    int count = this.data.Count;
    this.VerifyAxes();
    OxyRect clippingRect1 = this.GetClippingRect((Axis) this.BarAxis);
    OxyRect separationClippingRect = this.GetSeparationClippingRect();
    OxyRect clippingRect2 = this.GetClippingRect((Axis) this.VolumeAxis);
    double num1 = this.CandleWidth > 0.0 ? this.CandleWidth : this.minDx * 0.8;
    double width = this.XAxis.Transform(data[0].X + num1) - this.XAxis.Transform(data[0].X) - this.StrokeThickness;
    OxyColor selectableFillColor1 = this.GetSelectableFillColor(this.PositiveColor);
    OxyColor selectableFillColor2 = this.GetSelectableFillColor(this.NegativeColor);
    OxyColor oxyColor1 = this.PositiveHollow ? OxyColors.Transparent : selectableFillColor1;
    OxyColor oxyColor2 = this.NegativeHollow ? OxyColors.Transparent : selectableFillColor2;
    OxyColor selectableColor1 = this.GetSelectableColor(this.PositiveColor.ChangeIntensity(this.StrokeIntensity));
    OxyColor selectableColor2 = this.GetSelectableColor(this.NegativeColor.ChangeIntensity(this.StrokeIntensity));
    double actualMinimum = this.XAxis.ActualMinimum;
    double actualMaximum = this.XAxis.ActualMaximum;
    this.winIndex = OhlcvItem.FindIndex(data, actualMinimum, this.winIndex);
    for (int winIndex = this.winIndex; winIndex < count; ++winIndex)
    {
      OhlcvItem ohlcvItem = data[winIndex];
      if (ohlcvItem.X <= actualMaximum)
      {
        if (ohlcvItem.IsValid())
        {
          OxyColor fill1 = ohlcvItem.Close > ohlcvItem.Open ? oxyColor1 : oxyColor2;
          OxyColor stroke1 = ohlcvItem.Close > ohlcvItem.Open ? selectableColor1 : selectableColor2;
          ScreenPoint screenPoint1 = this.Transform(ohlcvItem.X, ohlcvItem.High);
          ScreenPoint screenPoint2 = this.Transform(ohlcvItem.X, ohlcvItem.Low);
          ScreenPoint screenPoint3 = this.Transform(ohlcvItem.X, ohlcvItem.Open);
          ScreenPoint screenPoint4 = this.Transform(ohlcvItem.X, ohlcvItem.Close);
          ScreenPoint screenPoint5 = new ScreenPoint(screenPoint3.X, Math.Max(screenPoint3.Y, screenPoint4.Y));
          ScreenPoint screenPoint6 = new ScreenPoint(screenPoint3.X, Math.Min(screenPoint3.Y, screenPoint4.Y));
          rc.DrawClippedLine(clippingRect1, (IList<ScreenPoint>) new ScreenPoint[2]
          {
            screenPoint1,
            screenPoint6
          }, 0.0, stroke1, this.StrokeThickness, (double[]) null, LineJoin.Miter, true);
          rc.DrawClippedLine(clippingRect1, (IList<ScreenPoint>) new ScreenPoint[2]
          {
            screenPoint5,
            screenPoint2
          }, 0.0, stroke1, this.StrokeThickness, (double[]) null, LineJoin.Miter, true);
          ScreenPoint screenPoint7 = screenPoint3 + new ScreenVector(-width * 0.5, 0.0);
          OxyRect rect1 = new OxyRect(screenPoint7.X, screenPoint6.Y, width, screenPoint5.Y - screenPoint6.Y);
          rc.DrawClippedRectangleAsPolygon(clippingRect1, rect1, fill1, stroke1, this.StrokeThickness);
          if (this.VolumeAxis != null && this.VolumeStyle != VolumeStyle.None)
          {
            double top1 = this.VolumeAxis.Transform(0.0);
            switch (this.VolumeStyle)
            {
              case VolumeStyle.Combined:
                double top2 = this.VolumeAxis.Transform(Math.Abs(ohlcvItem.BuyVolume - ohlcvItem.SellVolume));
                OxyColor fill2 = ohlcvItem.BuyVolume > ohlcvItem.SellVolume ? oxyColor1 : oxyColor2;
                OxyColor stroke2 = ohlcvItem.BuyVolume > ohlcvItem.SellVolume ? selectableColor1 : selectableColor2;
                OxyRect rect2 = new OxyRect(screenPoint7.X, top2, width, Math.Abs(top2 - top1));
                rc.DrawClippedRectangleAsPolygon(clippingRect2, rect2, fill2, stroke2, this.StrokeThickness);
                continue;
              case VolumeStyle.Stacked:
                if (ohlcvItem.BuyVolume > ohlcvItem.SellVolume)
                {
                  double num2 = this.VolumeAxis.Transform(ohlcvItem.BuyVolume);
                  double top3 = this.VolumeAxis.Transform(ohlcvItem.SellVolume);
                  double num3 = top3 - top1;
                  OxyRect rect3 = new OxyRect(screenPoint7.X, top3, width, Math.Abs(top3 - top1));
                  rc.DrawClippedRectangleAsPolygon(clippingRect2, rect3, selectableFillColor2, selectableColor2, this.StrokeThickness);
                  OxyRect rect4 = new OxyRect(screenPoint7.X, num2 + num3, width, Math.Abs(num2 - top1));
                  rc.DrawClippedRectangleAsPolygon(clippingRect2, rect4, selectableFillColor1, selectableColor1, this.StrokeThickness);
                  continue;
                }
                double top4 = this.VolumeAxis.Transform(ohlcvItem.BuyVolume);
                double num4 = this.VolumeAxis.Transform(ohlcvItem.SellVolume);
                double num5 = top4 - top1;
                OxyRect rect5 = new OxyRect(screenPoint7.X, top4, width, Math.Abs(top4 - top1));
                rc.DrawClippedRectangleAsPolygon(clippingRect2, rect5, selectableFillColor1, selectableColor1, this.StrokeThickness);
                OxyRect rect6 = new OxyRect(screenPoint7.X, num4 + num5, width, Math.Abs(num4 - top1));
                rc.DrawClippedRectangleAsPolygon(clippingRect2, rect6, selectableFillColor2, selectableColor2, this.StrokeThickness);
                continue;
              case VolumeStyle.PositiveNegative:
                double top5 = this.VolumeAxis.Transform(ohlcvItem.BuyVolume);
                double num6 = this.VolumeAxis.Transform(-ohlcvItem.SellVolume);
                OxyRect rect7 = new OxyRect(screenPoint7.X, top5, width, Math.Abs(top5 - top1));
                rc.DrawClippedRectangleAsPolygon(clippingRect2, rect7, selectableFillColor1, selectableColor1, this.StrokeThickness);
                OxyRect rect8 = new OxyRect(screenPoint7.X, top1, width, Math.Abs(num6 - top1));
                rc.DrawClippedRectangleAsPolygon(clippingRect2, rect8, selectableFillColor2, selectableColor2, this.StrokeThickness);
                continue;
              default:
                continue;
            }
          }
        }
      }
      else
        break;
    }
    if (this.VolumeStyle != VolumeStyle.None)
    {
      double y = (separationClippingRect.Bottom + separationClippingRect.Top) / 2.0;
      rc.DrawClippedLine(separationClippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
      {
        new ScreenPoint(separationClippingRect.Left, y),
        new ScreenPoint(separationClippingRect.Right, y)
      }, 0.0, this.SeparatorColor, this.SeparatorStrokeThickness, this.SeparatorLineStyle.GetDashArray(), LineJoin.Miter, true);
    }
    if (this.VolumeAxis == null || this.VolumeStyle != VolumeStyle.PositiveNegative)
      return;
    double y1 = this.VolumeAxis.Transform(0.0);
    rc.DrawClippedLine(clippingRect2, (IList<ScreenPoint>) new ScreenPoint[2]
    {
      new ScreenPoint(clippingRect2.Left, y1),
      new ScreenPoint(clippingRect2.Right, y1)
    }, 0.0, OxyColors.Goldenrod, this.SeparatorStrokeThickness, this.SeparatorLineStyle.GetDashArray(), LineJoin.Miter, true);
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double x = (legendBox.Left + legendBox.Right) / 2.0;
    double num1 = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.7;
    double top = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.3;
    double[] dashArray = LineStyle.Solid.GetDashArray();
    double num2 = this.CandleWidth > 0.0 ? this.CandleWidth : this.minDx * 0.8;
    OxyColor selectableFillColor = this.GetSelectableFillColor(this.PositiveColor);
    OxyColor selectableColor = this.GetSelectableColor(this.PositiveColor.ChangeIntensity(0.7));
    double width = Math.Min(legendBox.Width, this.XAxis.Transform(this.data[0].X + num2) - this.XAxis.Transform(this.data[0].X));
    rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
    {
      new ScreenPoint(x, legendBox.Top),
      new ScreenPoint(x, legendBox.Bottom)
    }, selectableColor, this.StrokeThickness, dashArray, aliased: true);
    rc.DrawRectangleAsPolygon(new OxyRect(x - width * 0.5, top, width, num1 - top), selectableFillColor, selectableColor, this.StrokeThickness);
  }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (((this.XAxis == null ? 1 : (this.YAxis == null ? 1 : 0)) | (interpolate ? 1 : 0)) != 0 || this.data.Count == 0)
      return (TrackerHitResult) null;
    int count = this.data.Count;
    DataPoint xy = this.InverseTransform(point);
    double x = xy.X;
    if (x > this.data[count - 1].X + this.minDx)
      return (TrackerHitResult) null;
    if (x < this.data[0].X - this.minDx)
      return (TrackerHitResult) null;
    int index1 = OhlcvItem.FindIndex(this.data, x, this.winIndex);
    int index2 = index1 + 1 < this.data.Count ? index1 + 1 : index1;
    Func<OhlcvItem, double> func = (Func<OhlcvItem, double>) (bar =>
    {
      double num = bar.X - xy.X;
      return num * num;
    });
    int index3 = func(this.data[index1]) <= func(this.data[index2]) ? index1 : index2;
    OhlcvItem ohlcvItem = this.data[index3];
    DataPoint p = new DataPoint(ohlcvItem.X, ohlcvItem.Close);
    return new TrackerHitResult()
    {
      Series = (OxyPlot.Series.Series) this,
      DataPoint = p,
      Position = this.Transform(p),
      Item = (object) ohlcvItem,
      Index = (double) index3,
      Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, (object) ohlcvItem, this.XAxis.GetValue(ohlcvItem.X), this.YAxis.GetValue(ohlcvItem.High), this.YAxis.GetValue(ohlcvItem.Low), this.YAxis.GetValue(ohlcvItem.Open), this.YAxis.GetValue(ohlcvItem.Close), this.YAxis.GetValue(ohlcvItem.BuyVolume), this.YAxis.GetValue(ohlcvItem.SellVolume))
    };
  }

  protected internal override void UpdateData()
  {
    base.UpdateData();
    this.winIndex = 0;
    List<OhlcvItem> data = this.data;
    int count = data.Count;
    this.minDx = double.MaxValue;
    for (int index = 1; index < count; ++index)
    {
      this.minDx = Math.Min(this.minDx, data[index].X - data[index - 1].X);
      if (this.minDx < 0.0)
        throw new ArgumentException("bars are out of order, must be sequential in x");
    }
    if (count > 1)
      return;
    this.minDx = 1.0;
  }

  protected internal override void EnsureAxes()
  {
    this.VolumeAxis = (LinearAxis) this.PlotModel.Axes.FirstOrDefault<Axis>((Func<Axis, bool>) (a => a.Key == this.VolumeAxisKey));
    this.YAxisKey = this.BarAxisKey;
    base.EnsureAxes();
  }

  protected internal override void UpdateAxisMaxMin()
  {
    this.XAxis.Include(this.MinX);
    this.XAxis.Include(this.MaxX);
    this.YAxis.Include(this.MinY);
    this.YAxis.Include(this.MaxY);
    if (this.VolumeAxis == null)
      return;
    double minimumVolume = this.MinimumVolume;
    double maximumVolume = this.MaximumVolume;
    double averageVolume = this.AverageVolume;
    double num1 = (maximumVolume - minimumVolume) / 4.0;
    double val2_1;
    double val2_2;
    switch (this.VolumeStyle)
    {
      case VolumeStyle.Stacked:
        val2_2 = averageVolume + num1;
        val2_1 = 0.0;
        break;
      case VolumeStyle.PositiveNegative:
        val2_1 = -(averageVolume + num1 / 2.0);
        val2_2 = averageVolume + num1 / 2.0;
        break;
      default:
        val2_2 = averageVolume + num1 / 2.0;
        val2_1 = 0.0;
        break;
    }
    double num2 = Math.Max(this.VolumeAxis.FilterMinValue, val2_1);
    double num3 = Math.Min(this.VolumeAxis.FilterMaxValue, val2_2);
    this.VolumeAxis.Include(num2);
    this.VolumeAxis.Include(num3);
  }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    double num1 = double.MaxValue;
    double num2 = double.MinValue;
    double num3 = double.MaxValue;
    double num4 = double.MinValue;
    double val1_1 = double.MaxValue;
    double val1_2 = double.MinValue;
    double num5 = 0.0;
    double num6 = 0.0;
    foreach (OhlcvItem ohlcvItem in this.Items)
    {
      if (ohlcvItem.IsValid())
      {
        if (ohlcvItem.SellVolume > 0.0)
          ++num5;
        if (ohlcvItem.BuyVolume > 0.0)
          ++num5;
        num6 += ohlcvItem.BuyVolume;
        num6 += ohlcvItem.SellVolume;
        num1 = Math.Min(num1, ohlcvItem.X);
        num2 = Math.Max(num2, ohlcvItem.X);
        num3 = Math.Min(num3, ohlcvItem.Low);
        num4 = Math.Max(num4, ohlcvItem.High);
        val1_1 = Math.Min(val1_1, -ohlcvItem.SellVolume);
        val1_2 = Math.Max(val1_2, ohlcvItem.BuyVolume);
      }
    }
    this.MinX = Math.Max(this.XAxis.FilterMinValue, num1);
    this.MaxX = Math.Min(this.XAxis.FilterMaxValue, num2);
    this.MinY = Math.Max(this.YAxis.FilterMinValue, num3);
    this.MaxY = Math.Min(this.YAxis.FilterMaxValue, num4);
    this.MinimumVolume = val1_1;
    this.MaximumVolume = val1_2;
    this.AverageVolume = num6 / num5;
  }

  protected OxyRect GetClippingRect(Axis yaxis)
  {
    if (yaxis == null)
      return new OxyRect();
    double x1 = this.XAxis.ScreenMin.X;
    ScreenPoint screenPoint1 = this.XAxis.ScreenMax;
    double x2 = screenPoint1.X;
    double left = Math.Min(x1, x2);
    screenPoint1 = yaxis.ScreenMin;
    double y1 = screenPoint1.Y;
    ScreenPoint screenPoint2 = yaxis.ScreenMax;
    double y2 = screenPoint2.Y;
    double top = Math.Min(y1, y2);
    screenPoint2 = this.XAxis.ScreenMin;
    double x3 = screenPoint2.X;
    screenPoint2 = this.XAxis.ScreenMax;
    double x4 = screenPoint2.X;
    double num1 = Math.Max(x3, x4);
    screenPoint2 = yaxis.ScreenMin;
    double y3 = screenPoint2.Y;
    screenPoint2 = yaxis.ScreenMax;
    double y4 = screenPoint2.Y;
    double num2 = Math.Max(y3, y4);
    return new OxyRect(left, top, num1 - left, num2 - top);
  }

  protected OxyRect GetSeparationClippingRect()
  {
    if (this.VolumeAxis == null)
      return new OxyRect();
    ScreenPoint screenPoint = this.XAxis.ScreenMin;
    double x1 = screenPoint.X;
    screenPoint = this.XAxis.ScreenMax;
    double x2 = screenPoint.X;
    double left = Math.Min(x1, x2);
    screenPoint = this.XAxis.ScreenMin;
    double x3 = screenPoint.X;
    screenPoint = this.XAxis.ScreenMax;
    double x4 = screenPoint.X;
    double num = Math.Max(x3, x4);
    screenPoint = this.VolumeAxis.ScreenMax;
    double y1 = screenPoint.Y;
    screenPoint = this.BarAxis.ScreenMin;
    double y2 = screenPoint.Y;
    double y3;
    double y4;
    if (y1 < y2)
    {
      screenPoint = this.BarAxis.ScreenMin;
      y3 = screenPoint.Y;
      screenPoint = this.VolumeAxis.ScreenMax;
      y4 = screenPoint.Y;
    }
    else
    {
      screenPoint = this.VolumeAxis.ScreenMin;
      y3 = screenPoint.Y;
      screenPoint = this.BarAxis.ScreenMax;
      y4 = screenPoint.Y;
    }
    return new OxyRect(left, y4, num - left, y3 - y4);
  }
}
