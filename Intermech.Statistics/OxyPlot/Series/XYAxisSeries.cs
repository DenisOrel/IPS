// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.XYAxisSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public abstract class XYAxisSeries : ItemsSeries
{
  public const string DefaultTrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}";
  protected const string DefaultXAxisTitle = "X";
  protected const string DefaultYAxisTitle = "Y";

  protected XYAxisSeries() => this.TrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}";

  public double MaxX { get; protected set; }

  public double MaxY { get; protected set; }

  public double MinX { get; protected set; }

  public double MinY { get; protected set; }

  public Axis XAxis { get; private set; }

  public string XAxisKey { get; set; }

  public Axis YAxis { get; private set; }

  public string YAxisKey { get; set; }

  public OxyRect GetScreenRectangle() => this.GetClippingRect();

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
  }

  public DataPoint InverseTransform(ScreenPoint p)
  {
    return this.XAxis.InverseTransform(p.X, p.Y, this.YAxis);
  }

  public ScreenPoint Transform(double x, double y) => this.XAxis.Transform(x, y, this.YAxis);

  public ScreenPoint Transform(DataPoint p) => this.XAxis.Transform(p.X, p.Y, this.YAxis);

  protected internal override bool AreAxesRequired() => true;

  protected internal override void EnsureAxes()
  {
    this.XAxis = this.PlotModel.GetAxisOrDefault(this.XAxisKey, this.PlotModel.DefaultXAxis);
    this.YAxis = this.PlotModel.GetAxisOrDefault(this.YAxisKey, this.PlotModel.DefaultYAxis);
  }

  protected internal override bool IsUsing(Axis axis) => false;

  protected internal override void SetDefaultValues()
  {
  }

  protected internal override void UpdateAxisMaxMin()
  {
    this.XAxis.Include(this.MinX);
    this.XAxis.Include(this.MaxX);
    this.YAxis.Include(this.MinY);
    this.YAxis.Include(this.MaxY);
  }

  protected internal override void UpdateData()
  {
  }

  protected internal override void UpdateMaxMin()
  {
    this.MinX = this.MinY = this.MaxX = this.MaxY = double.NaN;
  }

  protected OxyRect GetClippingRect()
  {
    ScreenPoint screenPoint1 = this.XAxis.ScreenMin;
    double x1 = screenPoint1.X;
    screenPoint1 = this.XAxis.ScreenMax;
    double x2 = screenPoint1.X;
    double left = Math.Min(x1, x2);
    ScreenPoint screenPoint2 = this.YAxis.ScreenMin;
    double y1 = screenPoint2.Y;
    screenPoint2 = this.YAxis.ScreenMax;
    double y2 = screenPoint2.Y;
    double top = Math.Min(y1, y2);
    ScreenPoint screenPoint3 = this.XAxis.ScreenMin;
    double x3 = screenPoint3.X;
    screenPoint3 = this.XAxis.ScreenMax;
    double x4 = screenPoint3.X;
    double num1 = Math.Max(x3, x4);
    ScreenPoint screenPoint4 = this.YAxis.ScreenMin;
    double y3 = screenPoint4.Y;
    screenPoint4 = this.YAxis.ScreenMax;
    double y4 = screenPoint4.Y;
    double num2 = Math.Max(y3, y4);
    return new OxyRect(left, top, num1 - left, num2 - top);
  }

  protected TrackerHitResult GetNearestInterpolatedPointInternal(
    List<DataPoint> points,
    ScreenPoint point)
  {
    if (this.XAxis == null || this.YAxis == null || points == null)
      return (TrackerHitResult) null;
    ScreenPoint screenPoint = new ScreenPoint();
    DataPoint dataPoint = new DataPoint();
    double a = -1.0;
    double num1 = double.MaxValue;
    for (int index = 0; index + 1 < points.Count; ++index)
    {
      DataPoint point1 = points[index];
      DataPoint point2 = points[index + 1];
      if (this.IsValidPoint(point1) && this.IsValidPoint(point2))
      {
        ScreenPoint p1 = this.Transform(point1);
        ScreenPoint p2 = this.Transform(point2);
        ScreenPoint pointOnLine = ScreenPointHelper.FindPointOnLine(point, p1, p2);
        if (!ScreenPoint.IsUndefined(pointOnLine))
        {
          double lengthSquared = (point - pointOnLine).LengthSquared;
          if (lengthSquared < num1)
          {
            double length = (p2 - p1).Length;
            double num2 = length > 0.0 ? (pointOnLine - p1).Length / length : 0.0;
            dataPoint = this.InverseTransform(pointOnLine);
            screenPoint = pointOnLine;
            num1 = lengthSquared;
            a = (double) index + num2;
          }
        }
      }
    }
    if (num1 >= double.MaxValue)
      return (TrackerHitResult) null;
    object obj = this.GetItem((int) Math.Round(a));
    return new TrackerHitResult()
    {
      Series = (OxyPlot.Series.Series) this,
      DataPoint = dataPoint,
      Position = screenPoint,
      Item = obj,
      Index = a
    };
  }

  protected TrackerHitResult GetNearestPointInternal(
    IEnumerable<DataPoint> points,
    ScreenPoint point)
  {
    ScreenPoint screenPoint1 = new ScreenPoint();
    DataPoint dataPoint = new DataPoint();
    double a = -1.0;
    double num1 = double.MaxValue;
    int num2 = 0;
    foreach (DataPoint point1 in points)
    {
      if (!this.IsValidPoint(point1))
      {
        ++num2;
      }
      else
      {
        ScreenPoint screenPoint2 = this.XAxis.Transform(point1.x, point1.y, this.YAxis);
        double lengthSquared = (screenPoint2 - point).LengthSquared;
        if (lengthSquared < num1)
        {
          dataPoint = point1;
          screenPoint1 = screenPoint2;
          num1 = lengthSquared;
          a = (double) num2;
        }
        ++num2;
      }
    }
    if (num1 >= double.MaxValue)
      return (TrackerHitResult) null;
    object obj = this.GetItem((int) Math.Round(a));
    return new TrackerHitResult()
    {
      Series = (OxyPlot.Series.Series) this,
      DataPoint = dataPoint,
      Position = screenPoint1,
      Item = obj,
      Index = a
    };
  }

  protected virtual bool IsValidPoint(DataPoint pt)
  {
    return this.XAxis != null && this.XAxis.IsValidValue(pt.X) && this.YAxis != null && this.YAxis.IsValidValue(pt.Y);
  }

  protected bool IsValidPoint(double x, double y)
  {
    return this.XAxis != null && this.XAxis.IsValidValue(x) && this.YAxis != null && this.YAxis.IsValidValue(y);
  }

  protected void InternalUpdateMaxMin(List<DataPoint> points)
  {
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    if (points.Count == 0)
      return;
    double d1 = this.MinX;
    double d2 = this.MinY;
    double d3 = this.MaxX;
    double d4 = this.MaxY;
    if (double.IsNaN(d1))
      d1 = double.MaxValue;
    if (double.IsNaN(d2))
      d2 = double.MaxValue;
    if (double.IsNaN(d3))
      d3 = double.MinValue;
    if (double.IsNaN(d4))
      d4 = double.MinValue;
    foreach (DataPoint point in points)
    {
      double x = point.X;
      double y = point.Y;
      if (this.IsValidPoint(point))
      {
        if (x < d1)
          d1 = x;
        if (x > d3)
          d3 = x;
        if (y < d2)
          d2 = y;
        if (y > d4)
          d4 = y;
      }
    }
    if (d1 < double.MaxValue)
    {
      if (d1 < this.XAxis.FilterMinValue)
        d1 = this.XAxis.FilterMinValue;
      this.MinX = d1;
    }
    if (d2 < double.MaxValue)
    {
      if (d2 < this.YAxis.FilterMinValue)
        d2 = this.YAxis.FilterMinValue;
      this.MinY = d2;
    }
    if (d3 > double.MinValue)
    {
      if (d3 > this.XAxis.FilterMaxValue)
        d3 = this.XAxis.FilterMaxValue;
      this.MaxX = d3;
    }
    if (d4 <= double.MinValue)
      return;
    if (d4 > this.YAxis.FilterMaxValue)
      d4 = this.YAxis.FilterMaxValue;
    this.MaxY = d4;
  }

  protected void InternalUpdateMaxMin<T>(List<T> items, Func<T, double> xf, Func<T, double> yf)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (items.Count == 0)
      return;
    double d1 = this.MinX;
    double d2 = this.MinY;
    double d3 = this.MaxX;
    double d4 = this.MaxY;
    if (double.IsNaN(d1))
      d1 = double.MaxValue;
    if (double.IsNaN(d2))
      d2 = double.MaxValue;
    if (double.IsNaN(d3))
      d3 = double.MinValue;
    if (double.IsNaN(d4))
      d4 = double.MinValue;
    foreach (T obj in items)
    {
      double x = xf(obj);
      double y = yf(obj);
      if (this.IsValidPoint(x, y))
      {
        if (x < d1)
          d1 = x;
        if (x > d3)
          d3 = x;
        if (y < d2)
          d2 = y;
        if (y > d4)
          d4 = y;
      }
    }
    if (d1 < double.MaxValue)
      this.MinX = d1;
    if (d2 < double.MaxValue)
      this.MinY = d2;
    if (d3 > double.MinValue)
      this.MaxX = d3;
    if (d4 <= double.MinValue)
      return;
    this.MaxY = d4;
  }

  protected void InternalUpdateMaxMin<T>(
    List<T> items,
    Func<T, double> xmin,
    Func<T, double> xmax,
    Func<T, double> ymin,
    Func<T, double> ymax)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (items.Count == 0)
      return;
    double d1 = this.MinX;
    double d2 = this.MinY;
    double d3 = this.MaxX;
    double d4 = this.MaxY;
    if (double.IsNaN(d1))
      d1 = double.MaxValue;
    if (double.IsNaN(d2))
      d2 = double.MaxValue;
    if (double.IsNaN(d3))
      d3 = double.MinValue;
    if (double.IsNaN(d4))
      d4 = double.MinValue;
    foreach (T obj in items)
    {
      double x1 = xmin(obj);
      double x2 = xmax(obj);
      double y1 = ymin(obj);
      double y2 = ymax(obj);
      if (this.IsValidPoint(x1, y1) && this.IsValidPoint(x2, y2))
      {
        if (x1 < d1)
          d1 = x1;
        if (x2 > d3)
          d3 = x2;
        if (y1 < d2)
          d2 = y1;
        if (y2 > d4)
          d4 = y2;
      }
    }
    if (d1 < double.MaxValue)
      this.MinX = d1;
    if (d2 < double.MaxValue)
      this.MinY = d2;
    if (d3 > double.MinValue)
      this.MaxX = d3;
    if (d4 <= double.MinValue)
      return;
    this.MaxY = d4;
  }

  protected void VerifyAxes()
  {
    if (this.XAxis == null)
      throw new InvalidOperationException("XAxis not defined.");
    if (this.YAxis == null)
      throw new InvalidOperationException("YAxis not defined.");
  }
}
