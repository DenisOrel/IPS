// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.HighLowSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class HighLowSeries : XYAxisSeries
{
  public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\nHigh: {3:0.###}\nLow: {4:0.###}\nOpen: {5:0.###}\nClose: {6:0.###}";
  private readonly List<HighLowItem> items = new List<HighLowItem>();
  private OxyColor defaultColor;

  public HighLowSeries()
  {
    this.Color = OxyColors.Automatic;
    this.TickLength = 4.0;
    this.StrokeThickness = 1.0;
    this.TrackerFormatString = "{0}\n{1}: {2}\nHigh: {3:0.###}\nLow: {4:0.###}\nOpen: {5:0.###}\nClose: {6:0.###}";
  }

  public OxyColor Color { get; set; }

  public OxyColor ActualColor => this.Color.GetActualColor(this.defaultColor);

  public double[] Dashes { get; set; }

  public string DataFieldClose { get; set; }

  public string DataFieldHigh { get; set; }

  public string DataFieldLow { get; set; }

  public string DataFieldOpen { get; set; }

  public string DataFieldX { get; set; }

  public List<HighLowItem> Items => this.items;

  public LineJoin LineJoin { get; set; }

  public LineStyle LineStyle { get; set; }

  public Func<object, HighLowItem> Mapping { get; set; }

  public double StrokeThickness { get; set; }

  public double TickLength { get; set; }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (this.XAxis == null || this.YAxis == null)
      return (TrackerHitResult) null;
    if (interpolate)
      return (TrackerHitResult) null;
    double minimumDistance = double.MaxValue;
    TrackerHitResult result = (TrackerHitResult) null;
    Action<DataPoint, HighLowItem, int> action = (Action<DataPoint, HighLowItem, int>) ((p, item, index) =>
    {
      ScreenPoint screenPoint = this.Transform(p);
      double num1 = screenPoint.x - point.x;
      double num2 = screenPoint.y - point.y;
      double num3 = num1 * num1 + num2 * num2;
      if (num3 >= minimumDistance)
        return;
      result = new TrackerHitResult()
      {
        Series = (OxyPlot.Series.Series) this,
        DataPoint = p,
        Position = screenPoint,
        Item = (object) item,
        Index = (double) index,
        Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, (object) item, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(p.X), this.YAxis.GetValue(item.High), this.YAxis.GetValue(item.Low), this.YAxis.GetValue(item.Open), this.YAxis.GetValue(item.Close))
      };
      minimumDistance = num3;
    });
    int num = 0;
    foreach (HighLowItem highLowItem in this.items)
    {
      action(new DataPoint(highLowItem.X, highLowItem.High), highLowItem, num);
      action(new DataPoint(highLowItem.X, highLowItem.Low), highLowItem, num);
      action(new DataPoint(highLowItem.X, highLowItem.Open), highLowItem, num);
      action(new DataPoint(highLowItem.X, highLowItem.Close), highLowItem, num++);
    }
    return minimumDistance < double.MaxValue ? result : (TrackerHitResult) null;
  }

  public virtual bool IsValidItem(HighLowItem pt, Axis xaxis, Axis yaxis)
  {
    return !double.IsNaN(pt.X) && !double.IsInfinity(pt.X) && !double.IsNaN(pt.High) && !double.IsInfinity(pt.High) && !double.IsNaN(pt.Low) && !double.IsInfinity(pt.Low);
  }

  public override void Render(IRenderContext rc)
  {
    if (this.items.Count == 0)
      return;
    this.VerifyAxes();
    OxyRect clippingRect = this.GetClippingRect();
    double[] dashArray = this.LineStyle.GetDashArray();
    OxyColor selectableColor = this.GetSelectableColor(this.ActualColor);
    foreach (HighLowItem pt in this.items)
    {
      if (this.IsValidItem(pt, this.XAxis, this.YAxis) && this.StrokeThickness > 0.0 && this.LineStyle != LineStyle.None)
      {
        ScreenPoint screenPoint1 = this.Transform(pt.X, pt.High);
        ScreenPoint screenPoint2 = this.Transform(pt.X, pt.Low);
        rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
        {
          screenPoint2,
          screenPoint1
        }, 0.0, selectableColor, this.StrokeThickness, dashArray, this.LineJoin, true);
        if (!double.IsNaN(pt.Open))
        {
          ScreenPoint screenPoint3 = this.Transform(pt.X, pt.Open);
          ScreenPoint screenPoint4 = screenPoint3 + new ScreenVector(-this.TickLength, 0.0);
          rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
          {
            screenPoint3,
            screenPoint4
          }, 0.0, selectableColor, this.StrokeThickness, dashArray, this.LineJoin, true);
        }
        if (!double.IsNaN(pt.Close))
        {
          ScreenPoint screenPoint5 = this.Transform(pt.X, pt.Close);
          ScreenPoint screenPoint6 = screenPoint5 + new ScreenVector(this.TickLength, 0.0);
          rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
          {
            screenPoint5,
            screenPoint6
          }, 0.0, selectableColor, this.StrokeThickness, dashArray, this.LineJoin, true);
        }
      }
    }
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double x = (legendBox.Left + legendBox.Right) / 2.0;
    double y1 = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.7;
    double y2 = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.3;
    double[] dashArray = this.LineStyle.GetDashArray();
    OxyColor selectableColor = this.GetSelectableColor(this.ActualColor);
    rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
    {
      new ScreenPoint(x, legendBox.Top),
      new ScreenPoint(x, legendBox.Bottom)
    }, selectableColor, this.StrokeThickness, dashArray, aliased: true);
    rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
    {
      new ScreenPoint(x - this.TickLength, y1),
      new ScreenPoint(x, y1)
    }, selectableColor, this.StrokeThickness, dashArray, aliased: true);
    rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
    {
      new ScreenPoint(x + this.TickLength, y2),
      new ScreenPoint(x, y2)
    }, selectableColor, this.StrokeThickness, dashArray, aliased: true);
  }

  protected internal override void SetDefaultValues()
  {
    if (!this.Color.IsAutomatic())
      return;
    this.LineStyle = this.PlotModel.GetDefaultLineStyle();
    this.defaultColor = this.PlotModel.GetDefaultColor();
  }

  protected internal override void UpdateData()
  {
    if (this.ItemsSource == null)
      return;
    this.items.Clear();
    if (this.Mapping != null)
    {
      foreach (object obj in this.ItemsSource)
        this.items.Add(this.Mapping(obj));
    }
    else
    {
      OxyPlot.ListBuilder<HighLowItem> listBuilder = new OxyPlot.ListBuilder<HighLowItem>();
      listBuilder.Add<double>(this.DataFieldX, double.NaN);
      listBuilder.Add<double>(this.DataFieldHigh, double.NaN);
      listBuilder.Add<double>(this.DataFieldLow, double.NaN);
      listBuilder.Add<double>(this.DataFieldOpen, double.NaN);
      listBuilder.Add<double>(this.DataFieldClose, double.NaN);
      listBuilder.FillT((IList<HighLowItem>) this.items, this.ItemsSource, (Func<IList<object>, HighLowItem>) (args => new HighLowItem(Convert.ToDouble(args[0]), Convert.ToDouble(args[1]), Convert.ToDouble(args[2]), Convert.ToDouble(args[3]), Convert.ToDouble(args[4]))));
    }
  }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    this.InternalUpdateMaxMin<HighLowItem>(this.items, (Func<HighLowItem, double>) (i => i.X), (Func<HighLowItem, double>) (i => i.X), (Func<HighLowItem, double>) (i => i.Low), (Func<HighLowItem, double>) (i => i.High));
  }
}
