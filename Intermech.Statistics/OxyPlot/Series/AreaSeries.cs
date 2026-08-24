// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.AreaSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class AreaSeries : LineSeries
{
  private readonly List<DataPoint> points2 = new List<DataPoint>();
  private readonly List<DataPoint> itemsSourcePoints2 = new List<DataPoint>();
  private List<DataPoint> actualPoints2;

  public AreaSeries()
  {
    this.Reverse2 = true;
    this.Color2 = OxyColors.Automatic;
    this.Fill = OxyColors.Automatic;
  }

  public double ConstantY2 { get; set; }

  public string DataFieldX2 { get; set; }

  public string DataFieldY2 { get; set; }

  public OxyColor Color2 { get; set; }

  public OxyColor ActualColor2 => this.Color2.GetActualColor(this.ActualColor);

  public OxyColor Fill { get; set; }

  public OxyColor ActualFill
  {
    get => this.Fill.GetActualColor(OxyColor.FromAColor((byte) 100, this.ActualColor));
  }

  public List<DataPoint> Points2 => this.points2;

  public bool Reverse2 { get; set; }

  protected List<DataPoint> ActualPoints2
  {
    get => this.ItemsSource == null ? this.actualPoints2 : this.itemsSourcePoints2;
  }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    TrackerHitResult trackerHitResult1;
    TrackerHitResult trackerHitResult2;
    if (interpolate && this.CanTrackerInterpolatePoints)
    {
      trackerHitResult1 = this.GetNearestInterpolatedPointInternal(this.ActualPoints, point);
      trackerHitResult2 = this.GetNearestInterpolatedPointInternal(this.ActualPoints2, point);
    }
    else
    {
      trackerHitResult1 = this.GetNearestPointInternal((IEnumerable<DataPoint>) this.ActualPoints, point);
      trackerHitResult2 = this.GetNearestPointInternal((IEnumerable<DataPoint>) this.ActualPoints2, point);
    }
    TrackerHitResult nearestPoint = trackerHitResult1 == null || trackerHitResult2 == null ? trackerHitResult1 ?? trackerHitResult2 : (trackerHitResult1.Position.DistanceTo(point) < trackerHitResult2.Position.DistanceTo(point) ? trackerHitResult1 : trackerHitResult2);
    if (nearestPoint != null)
      nearestPoint.Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, nearestPoint.Item, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(nearestPoint.DataPoint.X), (object) (this.YAxis.Title ?? "Y"), this.YAxis.GetValue(nearestPoint.DataPoint.Y));
    return nearestPoint;
  }

  public override void Render(IRenderContext rc)
  {
    List<DataPoint> actualPoints = this.ActualPoints;
    List<DataPoint> actualPoints2 = this.ActualPoints2;
    if (actualPoints.Count == 0)
      return;
    this.VerifyAxes();
    double minDistSquared = this.MinimumSegmentLength * this.MinimumSegmentLength;
    OxyRect clippingRect = this.GetClippingRect();
    rc.SetClip(clippingRect);
    IEnumerable<IEnumerable<DataPoint>> source1 = this.Split<DataPoint>((IEnumerable<DataPoint>) actualPoints, (Func<DataPoint, bool>) (p => double.IsNaN(p.Y)));
    IEnumerable<IEnumerable<DataPoint>> source2 = this.Split<DataPoint>((IEnumerable<DataPoint>) actualPoints2, (Func<DataPoint, bool>) (p => double.IsNaN(p.Y)));
    DataPoint dataPoint;
    for (int index1 = 0; index1 < source1.Count<IEnumerable<DataPoint>>(); ++index1)
    {
      List<DataPoint> list = source1.ElementAt<IEnumerable<DataPoint>>(index1).ToList<DataPoint>();
      int count = list.Count;
      IList<ScreenPoint> screenPointList1 = (IList<ScreenPoint>) new ScreenPoint[count];
      for (int index2 = 0; index2 < count; ++index2)
      {
        IList<ScreenPoint> screenPointList2 = screenPointList1;
        int index3 = index2;
        Axis xaxis = this.XAxis;
        dataPoint = list[index2];
        double x = dataPoint.X;
        dataPoint = list[index2];
        double y = dataPoint.Y;
        Axis yaxis = this.YAxis;
        ScreenPoint screenPoint = xaxis.Transform(x, y, yaxis);
        screenPointList2[index3] = screenPoint;
      }
      if (this.Smooth)
        screenPointList1 = (IList<ScreenPoint>) CanonicalSplineHelper.CreateSpline(ScreenPointHelper.ResamplePoints(screenPointList1, this.MinimumSegmentLength), 0.5, (IList<double>) null, false, 0.25);
      double[] actualDashArray = this.ActualDashArray;
      rc.DrawClippedLine(clippingRect, screenPointList1, minDistSquared, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, actualDashArray, this.LineJoin, false);
    }
    for (int index4 = 0; index4 < source2.Count<IEnumerable<DataPoint>>(); ++index4)
    {
      List<DataPoint> list = source2.ElementAt<IEnumerable<DataPoint>>(index4).ToList<DataPoint>();
      int count = list.Count;
      IList<ScreenPoint> screenPointList3 = (IList<ScreenPoint>) new ScreenPoint[count];
      for (int index5 = 0; index5 < count; ++index5)
      {
        int num = this.Reverse2 ? count - 1 - index5 : index5;
        IList<ScreenPoint> screenPointList4 = screenPointList3;
        int index6 = num;
        Axis xaxis = this.XAxis;
        dataPoint = list[index5];
        double x = dataPoint.X;
        dataPoint = list[index5];
        double y = dataPoint.Y;
        Axis yaxis = this.YAxis;
        ScreenPoint screenPoint = xaxis.Transform(x, y, yaxis);
        screenPointList4[index6] = screenPoint;
      }
      if (this.Smooth)
        screenPointList3 = (IList<ScreenPoint>) CanonicalSplineHelper.CreateSpline(ScreenPointHelper.ResamplePoints(screenPointList3, this.MinimumSegmentLength), 0.5, (IList<double>) null, false, 0.25);
      double[] actualDashArray = this.ActualDashArray;
      rc.DrawClippedLine(clippingRect, screenPointList3, minDistSquared, this.GetSelectableColor(this.ActualColor2), this.StrokeThickness, actualDashArray, this.LineJoin, false);
    }
    if (source1.Count<IEnumerable<DataPoint>>() != source2.Count<IEnumerable<DataPoint>>())
    {
      rc.ResetClip();
    }
    else
    {
      for (int index7 = 0; index7 < source1.Count<IEnumerable<DataPoint>>(); ++index7)
      {
        List<DataPoint> list1 = source1.ElementAt<IEnumerable<DataPoint>>(index7).ToList<DataPoint>();
        List<DataPoint> list2 = source2.ElementAt<IEnumerable<DataPoint>>(index7).ToList<DataPoint>();
        int count1 = list1.Count;
        IList<ScreenPoint> screenPointList5 = (IList<ScreenPoint>) new ScreenPoint[count1];
        for (int index8 = 0; index8 < count1; ++index8)
        {
          IList<ScreenPoint> screenPointList6 = screenPointList5;
          int index9 = index8;
          Axis xaxis = this.XAxis;
          dataPoint = list1[index8];
          double x = dataPoint.X;
          dataPoint = list1[index8];
          double y = dataPoint.Y;
          Axis yaxis = this.YAxis;
          ScreenPoint screenPoint = xaxis.Transform(x, y, yaxis);
          screenPointList6[index9] = screenPoint;
        }
        int count2 = list2.Count;
        IList<ScreenPoint> screenPointList7 = (IList<ScreenPoint>) new ScreenPoint[count2];
        for (int index10 = 0; index10 < count2; ++index10)
        {
          int num = this.Reverse2 ? count2 - 1 - index10 : index10;
          IList<ScreenPoint> screenPointList8 = screenPointList7;
          int index11 = num;
          Axis xaxis = this.XAxis;
          dataPoint = list2[index10];
          double x = dataPoint.X;
          dataPoint = list2[index10];
          double y = dataPoint.Y;
          Axis yaxis = this.YAxis;
          ScreenPoint screenPoint = xaxis.Transform(x, y, yaxis);
          screenPointList8[index11] = screenPoint;
        }
        if (this.Smooth)
        {
          IList<ScreenPoint> points1 = ScreenPointHelper.ResamplePoints(screenPointList5, this.MinimumSegmentLength);
          IList<ScreenPoint> points2 = ScreenPointHelper.ResamplePoints(screenPointList7, this.MinimumSegmentLength);
          screenPointList5 = (IList<ScreenPoint>) CanonicalSplineHelper.CreateSpline(points1, 0.5, (IList<double>) null, false, 0.25);
          screenPointList7 = (IList<ScreenPoint>) CanonicalSplineHelper.CreateSpline(points2, 0.5, (IList<double>) null, false, 0.25);
        }
        double[] actualDashArray = this.ActualDashArray;
        List<ScreenPoint> points = new List<ScreenPoint>();
        points.AddRange((IEnumerable<ScreenPoint>) screenPointList7);
        points.AddRange((IEnumerable<ScreenPoint>) screenPointList5);
        rc.DrawClippedPolygon(clippingRect, (IList<ScreenPoint>) points, minDistSquared, this.GetSelectableFillColor(this.ActualFill), OxyColors.Undefined);
        double[] markerSize = new double[1]
        {
          this.MarkerSize
        };
        rc.DrawMarkers(clippingRect, screenPointList5, this.MarkerType, (IList<ScreenPoint>) null, (IList<double>) markerSize, this.MarkerFill, this.MarkerStroke, this.MarkerStrokeThickness, 1);
        rc.DrawMarkers(clippingRect, screenPointList7, this.MarkerType, (IList<ScreenPoint>) null, (IList<double>) markerSize, this.MarkerFill, this.MarkerStroke, this.MarkerStrokeThickness, 1);
      }
      rc.ResetClip();
    }
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
    rc.DrawLine((IList<ScreenPoint>) screenPointArray1, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, this.ActualLineStyle.GetDashArray());
    rc.DrawLine((IList<ScreenPoint>) screenPointArray2, this.GetSelectableColor(this.ActualColor2), this.StrokeThickness, this.ActualLineStyle.GetDashArray());
    rc.DrawPolygon((IList<ScreenPoint>) points, this.GetSelectableFillColor(this.ActualFill), OxyColors.Undefined);
  }

  protected internal override void UpdateData()
  {
    base.UpdateData();
    if (this.ItemsSource == null)
    {
      if (this.points2.Count > 0)
        this.actualPoints2 = this.points2;
      else
        this.actualPoints2 = this.GetConstantPoints2().ToList<DataPoint>();
    }
    else
    {
      this.itemsSourcePoints2.Clear();
      if (this.DataFieldX2 != null && this.DataFieldY2 != null)
        this.itemsSourcePoints2.AddRange(this.ItemsSource, this.DataFieldX2, this.DataFieldY2);
      else
        this.itemsSourcePoints2.AddRange(this.GetConstantPoints2());
    }
  }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    this.InternalUpdateMaxMin(this.ActualPoints2);
  }

  private IEnumerable<DataPoint> GetConstantPoints2()
  {
    List<DataPoint> actualPoints = this.ActualPoints;
    if (!double.IsNaN(this.ConstantY2) && actualPoints.Count > 0)
    {
      double x = actualPoints[0].X;
      double x1 = actualPoints[actualPoints.Count - 1].X;
      yield return new DataPoint(x, this.ConstantY2);
      yield return new DataPoint(x1, this.ConstantY2);
    }
  }

  private IEnumerable<IEnumerable<T>> Split<T>(IEnumerable<T> source, Func<T, bool> splitCondition)
  {
    for (source = source.SkipWhile<T>(splitCondition); source.Any<T>(); source = source.SkipWhile<T>((Func<T, bool>) (x => !splitCondition(x))).SkipWhile<T>(splitCondition))
      yield return source.TakeWhile<T>((Func<T, bool>) (x => !splitCondition(x)));
  }
}
