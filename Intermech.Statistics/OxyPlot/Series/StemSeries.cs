// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.StemSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class StemSeries : LineSeries
{
  public StemSeries() => this.Base = 0.0;

  public double Base { get; set; }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (this.XAxis == null || this.YAxis == null)
      return (TrackerHitResult) null;
    if (interpolate)
      return (TrackerHitResult) null;
    TrackerHitResult nearestPoint = (TrackerHitResult) null;
    double num = double.MaxValue;
    List<DataPoint> actualPoints = this.ActualPoints;
    for (int index = 0; index < actualPoints.Count; ++index)
    {
      DataPoint p1 = actualPoints[index];
      DataPoint p2 = new DataPoint(p1.X, this.Base);
      ScreenPoint p1_1 = this.Transform(p1);
      ScreenPoint p2_1 = this.Transform(p2);
      double positionOnLine = ScreenPointHelper.FindPositionOnLine(point, p1_1, p2_1);
      if (!double.IsNaN(positionOnLine) && positionOnLine >= 0.0 && positionOnLine <= 1.0)
      {
        ScreenPoint screenPoint = p1_1 + (p2_1 - p1_1) * positionOnLine;
        double lengthSquared = (point - screenPoint).LengthSquared;
        if (lengthSquared < num)
        {
          object obj = this.GetItem(index);
          nearestPoint = new TrackerHitResult()
          {
            Series = (OxyPlot.Series.Series) this,
            DataPoint = new DataPoint(p1.X, p1.Y),
            Position = new ScreenPoint(p1_1.x, p1_1.y),
            Item = this.GetItem(index),
            Index = (double) index,
            Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, obj, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(p1.X), (object) (this.YAxis.Title ?? "Y"), this.YAxis.GetValue(p1.Y))
          };
          num = lengthSquared;
        }
      }
    }
    return nearestPoint;
  }

  public override void Render(IRenderContext rc)
  {
    if (this.ActualPoints.Count == 0)
      return;
    this.VerifyAxes();
    double minDistSquared = this.MinimumSegmentLength * this.MinimumSegmentLength;
    OxyRect clippingRect = this.GetClippingRect();
    double[] actualDashArray = this.ActualDashArray;
    OxyColor selectableColor = this.GetSelectableColor(this.ActualColor);
    ScreenPoint[] points = new ScreenPoint[2];
    List<ScreenPoint> markerPoints = this.MarkerType != MarkerType.None ? new List<ScreenPoint>(this.ActualPoints.Count) : (List<ScreenPoint>) null;
    foreach (DataPoint actualPoint in this.ActualPoints)
    {
      if (this.IsValidPoint(actualPoint))
      {
        points[0] = this.Transform(actualPoint.X, this.Base);
        points[1] = this.Transform(actualPoint.X, actualPoint.Y);
        if (this.StrokeThickness > 0.0 && this.ActualLineStyle != LineStyle.None)
          rc.DrawClippedLine(clippingRect, (IList<ScreenPoint>) points, minDistSquared, selectableColor, this.StrokeThickness, actualDashArray, this.LineJoin, false);
        markerPoints?.Add(points[1]);
      }
    }
    if (this.MarkerType == MarkerType.None)
      return;
    rc.DrawMarkers(clippingRect, (IList<ScreenPoint>) markerPoints, this.MarkerType, (IList<ScreenPoint>) this.MarkerOutline, (IList<double>) new double[1]
    {
      this.MarkerSize
    }, this.MarkerFill, this.MarkerStroke, this.MarkerStrokeThickness);
  }
}
