// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.StairStepSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class StairStepSeries : LineSeries
{
  public StairStepSeries()
  {
    this.VerticalStrokeThickness = double.NaN;
    this.VerticalLineStyle = this.LineStyle;
  }

  public double VerticalStrokeThickness { get; set; }

  public LineStyle VerticalLineStyle { get; set; }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (this.XAxis == null || this.YAxis == null)
      return (TrackerHitResult) null;
    double num1 = 256.0;
    TrackerHitResult nearestPointInternal = this.GetNearestPointInternal((IEnumerable<DataPoint>) this.ActualPoints, point);
    if (!interpolate && nearestPointInternal != null && nearestPointInternal.Position.DistanceToSquared(point) < num1)
    {
      nearestPointInternal.Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, nearestPointInternal.Item, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(nearestPointInternal.DataPoint.X), (object) (this.YAxis.Title ?? "Y"), this.YAxis.GetValue(nearestPointInternal.DataPoint.Y));
      return nearestPointInternal;
    }
    TrackerHitResult nearestPoint = (TrackerHitResult) null;
    int count = this.ActualPoints.Count;
    for (int index = 0; index < count; ++index)
    {
      DataPoint actualPoint1 = this.ActualPoints[index];
      DataPoint actualPoint2 = this.ActualPoints[index + 1 < count ? index + 1 : index];
      ScreenPoint screenPoint1 = this.Transform(actualPoint1.X, actualPoint1.Y);
      ScreenPoint screenPoint2 = this.Transform(actualPoint2.X, actualPoint1.Y);
      double num2 = screenPoint2.x - screenPoint1.x;
      double num3 = screenPoint2.y - screenPoint1.y;
      double num4 = (point.x - screenPoint1.x) * num2 + (point.y - screenPoint1.y) * num3;
      double num5 = num2 * num2 + num3 * num3;
      if (num2 * num2 + num3 * num3 < 4.0)
      {
        num4 = 0.0;
        num5 = 1.0;
      }
      if (Math.Abs(num5) >= double.Epsilon)
      {
        double num6 = num4 / num5;
        if (num6 >= 0.0 && num6 <= 1.0)
        {
          double x1 = screenPoint1.x + num6 * num2;
          double y1 = screenPoint1.y + num6 * num3;
          double num7 = point.x - x1;
          double num8 = point.y - y1;
          double num9 = num7 * num7 + num8 * num8;
          if (num9 < num1)
          {
            double x2 = actualPoint1.X + num6 * (actualPoint2.X - actualPoint1.X);
            double y2 = actualPoint1.Y;
            object obj = this.GetItem(index);
            nearestPoint = new TrackerHitResult()
            {
              Series = (OxyPlot.Series.Series) this,
              DataPoint = new DataPoint(x2, y2),
              Position = new ScreenPoint(x1, y1),
              Item = obj,
              Index = (double) index,
              Text = StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, obj, (object) this.Title, (object) (this.XAxis.Title ?? "X"), this.XAxis.GetValue(x2), (object) (this.YAxis.Title ?? "Y"), this.YAxis.GetValue(y2))
            };
            num1 = num9;
          }
        }
      }
    }
    return nearestPoint;
  }

  public override void Render(IRenderContext rc)
  {
    if (this.ActualPoints == null || this.ActualPoints.Count == 0)
      return;
    this.VerifyAxes();
    OxyRect clippingRect = this.GetClippingRect();
    double[] dashArray = this.ActualDashArray;
    double[] verticalLineDashArray = this.VerticalLineStyle.GetDashArray();
    LineStyle lineStyle = this.ActualLineStyle;
    double verticalStrokeThickness = double.IsNaN(this.VerticalStrokeThickness) ? this.StrokeThickness : this.VerticalStrokeThickness;
    OxyColor actualColor = this.GetSelectableColor(this.ActualColor);
    Action<IList<ScreenPoint>, IList<ScreenPoint>> action = (Action<IList<ScreenPoint>, IList<ScreenPoint>>) ((lpts, mpts) =>
    {
      if (this.StrokeThickness > 0.0 && lineStyle != LineStyle.None)
      {
        if (!verticalStrokeThickness.Equals(this.StrokeThickness) || this.VerticalLineStyle != lineStyle)
        {
          List<ScreenPoint> points1 = new List<ScreenPoint>();
          List<ScreenPoint> points2 = new List<ScreenPoint>();
          for (int index = 0; index + 2 < lpts.Count; index += 2)
          {
            points1.Add(lpts[index]);
            points1.Add(lpts[index + 1]);
            points2.Add(lpts[index + 1]);
            points2.Add(lpts[index + 2]);
          }
          rc.DrawClippedLineSegments(clippingRect, (IList<ScreenPoint>) points1, actualColor, this.StrokeThickness, dashArray, this.LineJoin, false);
          rc.DrawClippedLineSegments(clippingRect, (IList<ScreenPoint>) points2, actualColor, verticalStrokeThickness, verticalLineDashArray, this.LineJoin, false);
        }
        else
          rc.DrawClippedLine(clippingRect, lpts, 0.0, actualColor, this.StrokeThickness, dashArray, this.LineJoin, false);
      }
      if (this.MarkerType == MarkerType.None)
        return;
      rc.DrawMarkers(clippingRect, mpts, this.MarkerType, (IList<ScreenPoint>) this.MarkerOutline, (IList<double>) new double[1]
      {
        this.MarkerSize
      }, this.MarkerFill, this.MarkerStroke, this.MarkerStrokeThickness);
    });
    List<ScreenPoint> screenPointList1 = new List<ScreenPoint>();
    List<ScreenPoint> screenPointList2 = new List<ScreenPoint>();
    double num = double.NaN;
    foreach (DataPoint actualPoint in this.ActualPoints)
    {
      if (!this.IsValidPoint(actualPoint))
      {
        action((IList<ScreenPoint>) screenPointList1, (IList<ScreenPoint>) screenPointList2);
        screenPointList1.Clear();
        screenPointList2.Clear();
        num = double.NaN;
      }
      else
      {
        ScreenPoint screenPoint = this.Transform(actualPoint);
        if (!double.IsNaN(num))
          screenPointList1.Add(new ScreenPoint(screenPoint.X, num));
        screenPointList1.Add(screenPoint);
        screenPointList2.Add(screenPoint);
        num = screenPoint.Y;
      }
    }
    action((IList<ScreenPoint>) screenPointList1, (IList<ScreenPoint>) screenPointList2);
    if (this.LabelFormatString == null)
      return;
    this.RenderPointLabels(rc, clippingRect);
  }
}
