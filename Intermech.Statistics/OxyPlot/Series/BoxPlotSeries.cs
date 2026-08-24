// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.BoxPlotSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class BoxPlotSeries : XYAxisSeries
{
  public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\nUpper Whisker: {3:N2}\nThird Quartil: {4:N2}\nMedian: {5:N2}\nFirst Quartil: {6:N2}\nLower Whisker: {7:N2}\nMean: {8:N2}";
  private List<BoxPlotItem> itemsSourceItems;
  private bool ownsItemsSourceItems;

  public BoxPlotSeries()
  {
    this.Items = (IList<BoxPlotItem>) new List<BoxPlotItem>();
    this.TrackerFormatString = "{0}\n{1}: {2}\nUpper Whisker: {3:N2}\nThird Quartil: {4:N2}\nMedian: {5:N2}\nFirst Quartil: {6:N2}\nLower Whisker: {7:N2}\nMean: {8:N2}";
    this.OutlierTrackerFormatString = "{0}\n{1}: {2}\nY: {3:0.00}";
    this.Title = (string) null;
    this.Fill = OxyColors.Automatic;
    this.Stroke = OxyColors.Black;
    this.BoxWidth = 0.3;
    this.StrokeThickness = 1.0;
    this.MedianThickness = 2.0;
    this.MeanThickness = 2.0;
    this.OutlierSize = 2.0;
    this.OutlierType = MarkerType.Circle;
    this.MedianPointSize = 2.0;
    this.MeanPointSize = 2.0;
    this.WhiskerWidth = 0.5;
    this.LineStyle = LineStyle.Solid;
    this.ShowMedianAsDot = false;
    this.ShowMeanAsDot = false;
    this.ShowBox = true;
  }

  public double BoxWidth { get; set; }

  public OxyColor Fill { get; set; }

  public IList<BoxPlotItem> Items { get; set; }

  public LineStyle LineStyle { get; set; }

  public double MedianPointSize { get; set; }

  public double MedianThickness { get; set; }

  public double MeanPointSize { get; set; }

  public double MeanThickness { get; set; }

  public double OutlierSize { get; set; }

  public string OutlierTrackerFormatString { get; set; }

  public MarkerType OutlierType { get; set; }

  public ScreenPoint[] OutlierOutline { get; set; }

  public bool ShowBox { get; set; }

  public bool ShowMedianAsDot { get; set; }

  public bool ShowMeanAsDot { get; set; }

  public OxyColor Stroke { get; set; }

  public double StrokeThickness { get; set; }

  public double WhiskerWidth { get; set; }

  protected IList<BoxPlotItem> ActualItems
  {
    get => this.ItemsSource == null ? this.Items : (IList<BoxPlotItem>) this.itemsSourceItems;
  }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (this.XAxis == null || this.YAxis == null)
      return (TrackerHitResult) null;
    double num = double.MaxValue;
    TrackerHitResult trackerHitResult = (TrackerHitResult) null;
    foreach (BoxPlotItem actualItem in (IEnumerable<BoxPlotItem>) this.ActualItems)
    {
      ScreenVector screenVector;
      foreach (double outlier in (IEnumerable<double>) actualItem.Outliers)
      {
        ScreenPoint screenPoint = this.Transform(actualItem.X, outlier);
        screenVector = screenPoint - point;
        double lengthSquared = screenVector.LengthSquared;
        if (lengthSquared < num)
        {
          trackerHitResult = new TrackerHitResult()
          {
            Series = (OxyPlot.Series.Series) this,
            DataPoint = new DataPoint(actualItem.X, outlier),
            Position = screenPoint,
            Item = (object) actualItem,
            Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.OutlierTrackerFormatString, (object) actualItem, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(actualItem.X), (object) outlier)
          };
          num = lengthSquared;
        }
      }
      DataPoint p = DataPoint.Undefined;
      if (this.GetBoxRect(actualItem).Contains(point))
      {
        p = new DataPoint(actualItem.X, this.YAxis.InverseTransform(point.Y));
        num = 0.0;
      }
      ScreenPoint p1 = this.Transform(actualItem.X, actualItem.UpperWhisker);
      ScreenPoint p2 = this.Transform(actualItem.X, actualItem.LowerWhisker);
      ScreenPoint pointOnLine = ScreenPointHelper.FindPointOnLine(point, p1, p2);
      screenVector = pointOnLine - point;
      double lengthSquared1 = screenVector.LengthSquared;
      if (lengthSquared1 < num)
      {
        p = this.InverseTransform(pointOnLine);
        num = lengthSquared1;
      }
      if (p.IsDefined())
        trackerHitResult = new TrackerHitResult()
        {
          Series = (OxyPlot.Series.Series) this,
          DataPoint = p,
          Position = this.Transform(p),
          Item = (object) actualItem,
          Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, (object) actualItem, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(actualItem.X), this.YAxis.GetValue(actualItem.UpperWhisker), this.YAxis.GetValue(actualItem.BoxTop), this.YAxis.GetValue(actualItem.Median), this.YAxis.GetValue(actualItem.BoxBottom), this.YAxis.GetValue(actualItem.LowerWhisker), this.YAxis.GetValue(actualItem.Mean))
        };
    }
    return num < double.MaxValue ? trackerHitResult : (TrackerHitResult) null;
  }

  public virtual bool IsValidPoint(BoxPlotItem item, Axis xaxis, Axis yaxis)
  {
    return !double.IsNaN(item.X) && !double.IsInfinity(item.X) && !item.Values.Any<double>(new Func<double, bool>(double.IsNaN)) && !item.Values.Any<double>(new Func<double, bool>(double.IsInfinity)) && xaxis != null && xaxis.IsValidValue(item.X) && yaxis != null && item.Values.All<double>(new Func<double, bool>(yaxis.IsValidValue));
  }

  public override void Render(IRenderContext rc)
  {
    if (this.ActualItems.Count == 0)
      return;
    OxyRect clippingRect = this.GetClippingRect();
    List<ScreenPoint> screenPointList = new List<ScreenPoint>();
    double num1 = this.BoxWidth * 0.5;
    double num2 = num1 * this.WhiskerWidth;
    OxyColor selectableColor = this.GetSelectableColor(this.Stroke);
    OxyColor selectableFillColor = this.GetSelectableFillColor(this.Fill);
    double[] dashArray = this.LineStyle.GetDashArray();
    foreach (BoxPlotItem actualItem in (IEnumerable<BoxPlotItem>) this.ActualItems)
    {
      BoxPlotItem item = actualItem;
      screenPointList.AddRange(item.Outliers.Select<double, ScreenPoint>((Func<double, ScreenPoint>) (outlier => this.Transform(item.X, outlier))));
      ScreenPoint screenPoint1 = this.Transform(item.X, item.UpperWhisker);
      ScreenPoint screenPoint2 = this.Transform(item.X, item.BoxTop);
      ScreenPoint screenPoint3 = this.Transform(item.X, item.BoxBottom);
      ScreenPoint screenPoint4 = this.Transform(item.X, item.LowerWhisker);
      rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
      {
        screenPoint1,
        screenPoint2
      }, 0.0, selectableColor, this.StrokeThickness, dashArray, LineJoin.Miter, true);
      rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
      {
        screenPoint3,
        screenPoint4
      }, 0.0, selectableColor, this.StrokeThickness, dashArray, LineJoin.Miter, true);
      if (this.WhiskerWidth > 0.0)
      {
        ScreenPoint screenPoint5 = this.Transform(item.X - num2, item.UpperWhisker);
        ScreenPoint screenPoint6 = this.Transform(item.X + num2, item.UpperWhisker);
        ScreenPoint screenPoint7 = this.Transform(item.X - num2, item.LowerWhisker);
        ScreenPoint screenPoint8 = this.Transform(item.X + num2, item.LowerWhisker);
        rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
        {
          screenPoint5,
          screenPoint6
        }, 0.0, selectableColor, this.StrokeThickness, (double[]) null, LineJoin.Miter, true);
        rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
        {
          screenPoint7,
          screenPoint8
        }, 0.0, selectableColor, this.StrokeThickness, (double[]) null, LineJoin.Miter, true);
      }
      if (this.ShowBox)
      {
        OxyRect boxRect = this.GetBoxRect(item);
        rc.DrawClippedRectangleAsPolygon(clippingRect, boxRect, selectableFillColor, selectableColor, this.StrokeThickness);
      }
      if (!this.ShowMedianAsDot)
      {
        ScreenPoint screenPoint9 = this.Transform(item.X - num1, item.Median);
        ScreenPoint screenPoint10 = this.Transform(item.X + num1, item.Median);
        rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
        {
          screenPoint9,
          screenPoint10
        }, 0.0, selectableColor, this.StrokeThickness * this.MedianThickness, (double[]) null, LineJoin.Miter, true);
      }
      else
      {
        ScreenPoint p = this.Transform(item.X, item.Median);
        if (clippingRect.Contains(p))
        {
          OxyRect extents = new OxyRect(p.X - this.MedianPointSize, p.Y - this.MedianPointSize, this.MedianPointSize * 2.0, this.MedianPointSize * 2.0);
          rc.DrawEllipse(extents, selectableFillColor, OxyColors.Undefined, 0.0);
        }
      }
      if (!this.ShowMeanAsDot && !double.IsNaN(item.Mean))
      {
        ScreenPoint screenPoint11 = this.Transform(item.X - num1, item.Mean);
        ScreenPoint screenPoint12 = this.Transform(item.X + num1, item.Mean);
        rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) new ScreenPoint[2]
        {
          screenPoint11,
          screenPoint12
        }, 0.0, selectableColor, this.StrokeThickness * this.MeanThickness, LineStyle.Dash.GetDashArray(), LineJoin.Miter, true);
      }
      else if (!double.IsNaN(item.Mean))
      {
        ScreenPoint p = this.Transform(item.X, item.Mean);
        if (clippingRect.Contains(p))
        {
          OxyRect extents = new OxyRect(p.X - this.MeanPointSize, p.Y - this.MeanPointSize, this.MeanPointSize * 2.0, this.MeanPointSize * 2.0);
          rc.DrawEllipse(extents, selectableFillColor, OxyColors.Undefined, 0.0);
        }
      }
    }
    if (this.OutlierType == MarkerType.None)
      return;
    List<double> list = screenPointList.Select<ScreenPoint, double>((Func<ScreenPoint, double>) (o => this.OutlierSize)).ToList<double>();
    rc.DrawMarkers(clippingRect, (IList<ScreenPoint>) screenPointList, this.OutlierType, (IList<ScreenPoint>) this.OutlierOutline, (IList<double>) list, selectableFillColor, selectableColor, this.StrokeThickness);
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double x = (legendBox.Left + legendBox.Right) / 2.0;
    double y1 = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.7;
    double num1 = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.3;
    double y2 = (y1 + num1) * 0.5;
    double num2 = legendBox.Width * 0.24;
    double num3 = num2 * this.WhiskerWidth;
    OxyColor selectableColor = this.GetSelectableColor(this.Stroke);
    OxyColor selectableFillColor = this.GetSelectableFillColor(this.Fill);
    rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
    {
      new ScreenPoint(x, legendBox.Top),
      new ScreenPoint(x, num1)
    }, selectableColor, dashArray: LineStyle.Solid.GetDashArray(), aliased: true);
    rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
    {
      new ScreenPoint(x, y1),
      new ScreenPoint(x, legendBox.Bottom)
    }, selectableColor, dashArray: LineStyle.Solid.GetDashArray(), aliased: true);
    if (this.WhiskerWidth > 0.0)
    {
      rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
      {
        new ScreenPoint(x - num3 - 1.0, legendBox.Bottom),
        new ScreenPoint(x + num3, legendBox.Bottom)
      }, selectableColor, dashArray: LineStyle.Solid.GetDashArray(), aliased: true);
      rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
      {
        new ScreenPoint(x - num3 - 1.0, legendBox.Top),
        new ScreenPoint(x + num3, legendBox.Top)
      }, selectableColor, dashArray: LineStyle.Solid.GetDashArray(), aliased: true);
    }
    if (this.ShowBox)
      rc.DrawRectangleAsPolygon(new OxyRect(x - num2, num1, 2.0 * num2, y1 - num1), selectableFillColor, selectableColor, 1.0);
    if (!this.ShowMedianAsDot)
    {
      rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
      {
        new ScreenPoint(x - num2, y2),
        new ScreenPoint(x + num2, y2)
      }, selectableColor, 1.0 * this.MedianThickness, LineStyle.Solid.GetDashArray(), aliased: true);
    }
    else
    {
      OxyRect extents = new OxyRect(x - this.MedianPointSize, y2 - this.MedianPointSize, this.MedianPointSize * 2.0, this.MedianPointSize * 2.0);
      rc.DrawEllipse(extents, selectableFillColor, OxyColors.Undefined);
    }
  }

  protected internal override void UpdateData()
  {
    if (this.ItemsSource == null)
      return;
    if (this.ItemsSource is IEnumerable<BoxPlotItem> itemsSource)
    {
      this.itemsSourceItems = itemsSource.ToList<BoxPlotItem>();
      this.ownsItemsSourceItems = false;
    }
    else
    {
      this.ClearItemsSourceItems();
      this.itemsSourceItems.AddRange(this.ItemsSource.OfType<BoxPlotItem>());
    }
  }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    this.InternalUpdateMaxMin(this.ActualItems);
  }

  protected void InternalUpdateMaxMin(IList<BoxPlotItem> items)
  {
    if (items == null || items.Count == 0)
      return;
    double d1 = this.MinX;
    double d2 = this.MinY;
    double d3 = this.MaxX;
    double d4 = this.MaxY;
    foreach (BoxPlotItem boxPlotItem in (IEnumerable<BoxPlotItem>) items)
    {
      if (this.IsValidPoint(boxPlotItem, this.XAxis, this.YAxis))
      {
        double x = boxPlotItem.X;
        if (x < d1 || double.IsNaN(d1))
          d1 = x;
        if (x > d3 || double.IsNaN(d3))
          d3 = x;
        foreach (double num in (IEnumerable<double>) boxPlotItem.Values)
        {
          if (num < d2 || double.IsNaN(d2))
            d2 = num;
          if (num > d4 || double.IsNaN(d4))
            d4 = num;
        }
      }
    }
    this.MinX = d1;
    this.MinY = d2;
    this.MaxX = d3;
    this.MaxY = d4;
  }

  protected override object GetItem(int i)
  {
    return this.ItemsSource != null || this.ActualItems == null || this.ActualItems.Count == 0 ? base.GetItem(i) : (object) this.ActualItems[i];
  }

  private OxyRect GetBoxRect(BoxPlotItem item)
  {
    double num = this.BoxWidth * 0.5;
    ScreenPoint screenPoint1 = this.Transform(item.X - num, item.BoxTop);
    ScreenPoint screenPoint2 = this.Transform(item.X + num, item.BoxBottom);
    return new OxyRect(screenPoint1.X, screenPoint1.Y, screenPoint2.X - screenPoint1.X, screenPoint2.Y - screenPoint1.Y);
  }

  private void ClearItemsSourceItems()
  {
    if (!this.ownsItemsSourceItems || this.itemsSourceItems == null)
      this.itemsSourceItems = new List<BoxPlotItem>();
    else
      this.itemsSourceItems.Clear();
    this.ownsItemsSourceItems = true;
  }
}
