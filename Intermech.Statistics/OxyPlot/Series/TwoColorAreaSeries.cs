// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.TwoColorAreaSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class TwoColorAreaSeries : TwoColorLineSeries
{
  private readonly List<DataPoint> points2 = new List<DataPoint>();

  public TwoColorAreaSeries()
  {
    this.Fill = OxyColors.Automatic;
    this.Fill2 = OxyColors.Automatic;
    this.MarkerFill2 = OxyColors.Automatic;
    this.MarkerStroke2 = OxyColors.Automatic;
  }

  public OxyColor Fill { get; set; }

  public OxyColor Fill2 { get; set; }

  public OxyColor ActualFill
  {
    get => this.Fill.GetActualColor(OxyColor.FromAColor((byte) 100, this.ActualColor));
  }

  public OxyColor ActuallFill2
  {
    get => this.Fill2.GetActualColor(OxyColor.FromAColor((byte) 100, this.ActualColor2));
  }

  public OxyColor MarkerFill2 { get; set; }

  public OxyColor MarkerStroke2 { get; set; }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    TrackerHitResult nearestPoint = !interpolate || !this.CanTrackerInterpolatePoints ? this.GetNearestPointInternal((IEnumerable<DataPoint>) this.ActualPoints, point) : this.GetNearestInterpolatedPointInternal(this.ActualPoints, point);
    if (nearestPoint != null)
      nearestPoint.Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, nearestPoint.Item, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(nearestPoint.DataPoint.X), (object) (this.YAxis.Title ?? "Y"), this.YAxis.GetValue(nearestPoint.DataPoint.Y));
    return nearestPoint;
  }

  public override void Render(IRenderContext rc)
  {
    List<DataPoint> actualPoints = this.ActualPoints;
    List<DataPoint> points2 = this.points2;
    int count1 = actualPoints.Count;
    if (count1 == 0)
      return;
    this.VerifyAxes();
    double minDistSquared = this.MinimumSegmentLength * this.MinimumSegmentLength;
    OxyRect clippingRectangle = this.GetClippingRect();
    rc.SetClip(clippingRectangle);
    IList<ScreenPoint> screenPointList1 = (IList<ScreenPoint>) new List<ScreenPoint>();
    DataPoint dataPoint;
    for (int index = 0; index < count1; ++index)
    {
      IList<ScreenPoint> screenPointList2 = screenPointList1;
      Axis xaxis = this.XAxis;
      dataPoint = actualPoints[index];
      double x = dataPoint.X;
      dataPoint = actualPoints[index];
      double y = dataPoint.Y;
      Axis yaxis = this.YAxis;
      ScreenPoint screenPoint = xaxis.Transform(x, y, yaxis);
      screenPointList2.Add(screenPoint);
    }
    int count2 = points2.Count;
    IList<ScreenPoint> collection = (IList<ScreenPoint>) new ScreenPoint[count2];
    for (int index1 = 0; index1 < count2; ++index1)
    {
      int num = count2 - 1 - index1;
      IList<ScreenPoint> screenPointList3 = collection;
      int index2 = num;
      Axis xaxis = this.XAxis;
      dataPoint = points2[index1];
      double x = dataPoint.X;
      dataPoint = points2[index1];
      double y = dataPoint.Y;
      Axis yaxis = this.YAxis;
      ScreenPoint screenPoint = xaxis.Transform(x, y, yaxis);
      screenPointList3[index2] = screenPoint;
    }
    if (this.Smooth)
      screenPointList1 = (IList<ScreenPoint>) CanonicalSplineHelper.CreateSpline(ScreenPointHelper.ResamplePoints(screenPointList1, this.MinimumSegmentLength), 0.5, (IList<double>) null, false, 0.25);
    double[] actualDashArray = this.ActualDashArray;
    double[] actualDashArray2 = this.ActualDashArray2;
    double top1 = this.YAxis.Transform(this.Limit);
    if (top1 < clippingRectangle.Top)
      top1 = clippingRectangle.Top;
    if (top1 > clippingRectangle.Bottom)
      top1 = clippingRectangle.Bottom;
    double[] markerSize = new double[1]{ this.MarkerSize };
    double bottom = clippingRectangle.Bottom;
    double top2 = clippingRectangle.Top;
    clippingRectangle = new OxyRect(clippingRectangle.Left, top1, clippingRectangle.Width, bottom - top1);
    rc.DrawClippedLine(clippingRectangle, screenPointList1, minDistSquared, this.GetSelectableColor(this.ActualColor2), this.StrokeThickness, actualDashArray2, this.LineJoin, false);
    List<ScreenPoint> points = new List<ScreenPoint>();
    points.AddRange((IEnumerable<ScreenPoint>) collection);
    points.AddRange((IEnumerable<ScreenPoint>) screenPointList1);
    rc.DrawClippedPolygon(clippingRectangle, (IList<ScreenPoint>) points, minDistSquared, this.GetSelectableFillColor(this.ActuallFill2), OxyColors.Undefined);
    rc.DrawMarkers(clippingRectangle, screenPointList1, this.MarkerType, (IList<ScreenPoint>) null, (IList<double>) markerSize, this.MarkerFill2, this.MarkerStroke2, this.MarkerStrokeThickness, 1);
    clippingRectangle = new OxyRect(clippingRectangle.Left, top2, clippingRectangle.Width, top1 - top2);
    rc.DrawClippedLine(clippingRectangle, screenPointList1, minDistSquared, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, actualDashArray, this.LineJoin, false);
    rc.DrawClippedPolygon(clippingRectangle, (IList<ScreenPoint>) points, minDistSquared, this.GetSelectableFillColor(this.ActualFill), OxyColors.Undefined);
    rc.DrawMarkers(clippingRectangle, screenPointList1, this.MarkerType, (IList<ScreenPoint>) null, (IList<double>) markerSize, this.MarkerFill, this.MarkerStroke, this.MarkerStrokeThickness, 1);
    rc.ResetClip();
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double y1 = legendBox.Top * 0.2 + legendBox.Bottom * 0.8;
    double y2 = legendBox.Top * 0.4 + legendBox.Bottom * 0.6;
    double y3 = legendBox.Top * 0.8 + legendBox.Bottom * 0.2;
    ScreenPoint[] screenPointArray1 = new ScreenPoint[2]
    {
      new ScreenPoint(legendBox.Left, y1),
      new ScreenPoint(legendBox.Right, y1)
    };
    ScreenPoint[] screenPointArray2 = new ScreenPoint[2]
    {
      new ScreenPoint(legendBox.Right, y3),
      new ScreenPoint(legendBox.Left, y2)
    };
    List<ScreenPoint> points = new List<ScreenPoint>();
    points.AddRange((IEnumerable<ScreenPoint>) screenPointArray1);
    points.AddRange((IEnumerable<ScreenPoint>) screenPointArray2);
    rc.DrawLine((IList<ScreenPoint>) screenPointArray1, this.GetSelectableColor(this.ActualColor2), this.StrokeThickness, this.ActualLineStyle.GetDashArray());
    rc.DrawLine((IList<ScreenPoint>) screenPointArray2, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, this.ActualLineStyle.GetDashArray());
    rc.DrawPolygon((IList<ScreenPoint>) points, this.GetSelectableFillColor(this.ActualFill), OxyColors.Undefined);
  }

  protected internal override void UpdateData()
  {
    base.UpdateData();
    if (this.ActualPoints == null)
      return;
    this.points2.Clear();
    if (this.ActualPoints.Count <= 1)
      return;
    this.points2.Add(new DataPoint(this.ActualPoints.Min<DataPoint>((Func<DataPoint, double>) (el => el.X)), this.Limit));
    this.points2.Add(new DataPoint(this.ActualPoints.Max<DataPoint>((Func<DataPoint, double>) (el => el.X)), this.Limit));
  }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    this.InternalUpdateMaxMin(this.points2);
  }
}
