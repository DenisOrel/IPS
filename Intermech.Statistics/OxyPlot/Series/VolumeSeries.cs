// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.VolumeSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class VolumeSeries : XYAxisSeries
{
  public new const string DefaultTrackerFormatString = "Time: {0}\nBuy Volume: {1}\nSell Volume: {2}";
  private List<OhlcvItem> data;
  private double minDx;
  private int winIndex;

  public VolumeSeries()
  {
    this.PositiveColor = OxyColors.DarkGreen;
    this.NegativeColor = OxyColors.Red;
    this.BarWidth = 0.0;
    this.StrokeThickness = 1.0;
    this.NegativeHollow = false;
    this.PositiveHollow = true;
    this.StrokeIntensity = 0.8;
    this.VolumeStyle = VolumeStyle.Combined;
    this.InterceptColor = OxyColors.Gray;
    this.InterceptLineStyle = LineStyle.Dash;
    this.InterceptStrokeThickness = 1.0;
    this.TrackerFormatString = "Time: {0}\nBuy Volume: {1}\nSell Volume: {2}";
  }

  public List<OhlcvItem> Items
  {
    get => this.data == null ? (this.data = new List<OhlcvItem>()) : this.data;
    set => this.data = value;
  }

  public VolumeStyle VolumeStyle { get; set; }

  public double StrokeThickness { get; set; }

  public double StrokeIntensity { get; set; }

  public OxyColor PositiveColor { get; set; }

  public OxyColor NegativeColor { get; set; }

  public OxyColor InterceptColor { get; set; }

  public double InterceptStrokeThickness { get; set; }

  public LineStyle InterceptLineStyle { get; set; }

  public bool PositiveHollow { get; set; }

  public bool NegativeHollow { get; set; }

  public double BarWidth { get; set; }

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
    OxyRect clippingRect = this.GetClippingRect();
    double num1 = this.BarWidth > 0.0 ? this.BarWidth : this.minDx * 0.8;
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
          double left = this.XAxis.Transform(ohlcvItem.X) - this.BarWidth / 2.0;
          double top1 = this.YAxis.Transform(0.0);
          switch (this.VolumeStyle)
          {
            case VolumeStyle.Combined:
              double top2 = this.YAxis.Transform(Math.Abs(ohlcvItem.BuyVolume - ohlcvItem.SellVolume));
              OxyColor fill = ohlcvItem.BuyVolume > ohlcvItem.SellVolume ? oxyColor1 : oxyColor2;
              OxyColor stroke = ohlcvItem.BuyVolume > ohlcvItem.SellVolume ? selectableColor1 : selectableColor2;
              OxyRect rect1 = new OxyRect(left, top2, width, Math.Abs(top2 - top1));
              rc.DrawClippedRectangleAsPolygon(clippingRect, rect1, fill, stroke, this.StrokeThickness);
              continue;
            case VolumeStyle.Stacked:
              if (ohlcvItem.BuyVolume > ohlcvItem.SellVolume)
              {
                double num2 = this.YAxis.Transform(ohlcvItem.BuyVolume);
                double top3 = this.YAxis.Transform(ohlcvItem.SellVolume);
                double num3 = top3 - top1;
                OxyRect rect2 = new OxyRect(left, top3, width, Math.Abs(top3 - top1));
                rc.DrawClippedRectangleAsPolygon(clippingRect, rect2, selectableFillColor2, selectableColor2, this.StrokeThickness);
                OxyRect rect3 = new OxyRect(left, num2 + num3, width, Math.Abs(num2 - top1));
                rc.DrawClippedRectangleAsPolygon(clippingRect, rect3, selectableFillColor1, selectableColor1, this.StrokeThickness);
                continue;
              }
              double top4 = this.YAxis.Transform(ohlcvItem.BuyVolume);
              double num4 = this.YAxis.Transform(ohlcvItem.SellVolume);
              double num5 = top4 - top1;
              OxyRect rect4 = new OxyRect(left, top4, width, Math.Abs(top4 - top1));
              rc.DrawClippedRectangleAsPolygon(clippingRect, rect4, selectableFillColor1, selectableColor1, this.StrokeThickness);
              OxyRect rect5 = new OxyRect(left, num4 + num5, width, Math.Abs(num4 - top1));
              rc.DrawClippedRectangleAsPolygon(clippingRect, rect5, selectableFillColor2, selectableColor2, this.StrokeThickness);
              continue;
            case VolumeStyle.PositiveNegative:
              double top5 = this.YAxis.Transform(ohlcvItem.BuyVolume);
              double num6 = this.YAxis.Transform(-ohlcvItem.SellVolume);
              OxyRect rect6 = new OxyRect(left, top5, width, Math.Abs(top5 - top1));
              rc.DrawClippedRectangleAsPolygon(clippingRect, rect6, selectableFillColor1, selectableColor1, this.StrokeThickness);
              OxyRect rect7 = new OxyRect(left, top1, width, Math.Abs(num6 - top1));
              rc.DrawClippedRectangleAsPolygon(clippingRect, rect7, selectableFillColor2, selectableColor2, this.StrokeThickness);
              continue;
            default:
              continue;
          }
        }
      }
      else
        break;
    }
    double y = this.YAxis.Transform(0.0);
    rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
    {
      new ScreenPoint(clippingRect.Left, y),
      new ScreenPoint(clippingRect.Right, y)
    }, 0.0, this.InterceptColor, this.InterceptStrokeThickness, this.InterceptLineStyle.GetDashArray(), LineJoin.Miter, true);
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double x = (legendBox.Left + legendBox.Right) / 2.0;
    double num1 = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.7;
    double top = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.3;
    double[] dashArray = LineStyle.Solid.GetDashArray();
    double num2 = this.BarWidth > 0.0 ? this.BarWidth : this.minDx * 0.8;
    OxyColor selectableFillColor = this.GetSelectableFillColor(this.PositiveColor);
    OxyColor selectableColor = this.GetSelectableColor(this.PositiveColor.ChangeIntensity(this.StrokeIntensity));
    double width = this.XAxis.Transform(this.data[0].X + num2) - this.XAxis.Transform(this.data[0].X);
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
      Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, (object) ohlcvItem, this.XAxis.GetValue(ohlcvItem.X), this.YAxis.GetValue(ohlcvItem.BuyVolume), this.YAxis.GetValue(ohlcvItem.SellVolume))
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

  protected internal override void UpdateAxisMaxMin()
  {
    this.XAxis.Include(this.MinX);
    this.XAxis.Include(this.MaxX);
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
    double num2 = Math.Max(this.YAxis.FilterMinValue, val2_1);
    double num3 = Math.Min(this.YAxis.FilterMaxValue, val2_2);
    this.YAxis.Include(num2);
    this.YAxis.Include(num3);
  }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    double num1 = double.MaxValue;
    double num2 = double.MinValue;
    double val1_1 = 0.0;
    double val1_2 = double.MinValue;
    double num3 = 0.0;
    double num4 = 0.0;
    foreach (OhlcvItem ohlcvItem in this.Items)
    {
      if (ohlcvItem.IsValid())
      {
        if (ohlcvItem.SellVolume > 0.0)
          ++num3;
        if (ohlcvItem.BuyVolume > 0.0)
          ++num3;
        num4 += ohlcvItem.BuyVolume;
        num4 += ohlcvItem.SellVolume;
        num1 = Math.Min(num1, ohlcvItem.X);
        num2 = Math.Max(num2, ohlcvItem.X);
        val1_1 = Math.Min(val1_1, -ohlcvItem.SellVolume);
        val1_2 = Math.Max(val1_2, ohlcvItem.BuyVolume);
      }
    }
    this.MinX = Math.Max(this.XAxis.FilterMinValue, num1);
    this.MaxX = Math.Min(this.XAxis.FilterMaxValue, num2);
    this.MinimumVolume = val1_1;
    this.MaximumVolume = val1_2;
    this.AverageVolume = num4 / num3;
  }
}
