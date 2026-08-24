// Decompiled with JetBrains decompiler
// Type: OxyPlot.CanonicalSplineHelper
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot;

internal static class CanonicalSplineHelper
{
  internal static List<DataPoint> CreateSpline(
    List<DataPoint> points,
    double tension,
    IList<double> tensions,
    bool isClosed,
    double tolerance)
  {
    List<ScreenPoint> spline1 = CanonicalSplineHelper.CreateSpline((IList<ScreenPoint>) points.Select<DataPoint, ScreenPoint>((Func<DataPoint, ScreenPoint>) (p => new ScreenPoint(p.X, p.Y))).ToList<ScreenPoint>(), tension, tensions, isClosed, tolerance);
    List<DataPoint> spline2 = new List<DataPoint>(spline1.Count);
    foreach (ScreenPoint screenPoint in spline1)
      spline2.Add(new DataPoint(screenPoint.X, screenPoint.Y));
    return spline2;
  }

  internal static List<ScreenPoint> CreateSpline(
    IList<ScreenPoint> points,
    double tension,
    IList<double> tensions,
    bool isClosed,
    double tolerance)
  {
    List<ScreenPoint> points1 = new List<ScreenPoint>();
    if (points == null)
      return points1;
    int count = points.Count;
    if (count < 1)
      return points1;
    if (count < 2)
    {
      points1.AddRange((IEnumerable<ScreenPoint>) points);
      return points1;
    }
    if (count == 2)
    {
      if (!isClosed)
      {
        CanonicalSplineHelper.Segment((IList<ScreenPoint>) points1, points[0], points[0], points[1], points[1], tension, tension, tolerance);
      }
      else
      {
        CanonicalSplineHelper.Segment((IList<ScreenPoint>) points1, points[1], points[0], points[1], points[0], tension, tension, tolerance);
        CanonicalSplineHelper.Segment((IList<ScreenPoint>) points1, points[0], points[1], points[0], points[1], tension, tension, tolerance);
      }
    }
    else
    {
      bool flag = tensions != null && tensions.Count > 0;
      for (int index = 0; index < count; ++index)
      {
        double t1 = flag ? tensions[index % tensions.Count] : tension;
        double t2 = flag ? tensions[(index + 1) % tensions.Count] : tension;
        if (index == 0)
          CanonicalSplineHelper.Segment((IList<ScreenPoint>) points1, isClosed ? points[count - 1] : points[0], points[0], points[1], points[2], t1, t2, tolerance);
        else if (index == count - 2)
          CanonicalSplineHelper.Segment((IList<ScreenPoint>) points1, points[index - 1], points[index], points[index + 1], isClosed ? points[0] : points[index + 1], t1, t2, tolerance);
        else if (index == count - 1)
        {
          if (isClosed)
            CanonicalSplineHelper.Segment((IList<ScreenPoint>) points1, points[index - 1], points[index], points[0], points[1], t1, t2, tolerance);
        }
        else
          CanonicalSplineHelper.Segment((IList<ScreenPoint>) points1, points[index - 1], points[index], points[index + 1], points[index + 2], t1, t2, tolerance);
      }
    }
    return points1;
  }

  private static void Segment(
    IList<ScreenPoint> points,
    ScreenPoint pt0,
    ScreenPoint pt1,
    ScreenPoint pt2,
    ScreenPoint pt3,
    double t1,
    double t2,
    double tolerance)
  {
    double num1 = t1 * (pt2.X - pt0.X);
    double num2 = t1 * (pt2.Y - pt0.Y);
    double num3 = t2 * (pt3.X - pt1.X);
    double num4 = t2 * (pt3.Y - pt1.Y);
    double num5 = num1 + num3 + 2.0 * pt1.X - 2.0 * pt2.X;
    double num6 = num2 + num4 + 2.0 * pt1.Y - 2.0 * pt2.Y;
    double num7 = -2.0 * num1 - num3 - 3.0 * pt1.X + 3.0 * pt2.X;
    double num8 = -2.0 * num2 - num4 - 3.0 * pt1.Y + 3.0 * pt2.Y;
    double num9 = num1;
    double num10 = num2;
    double x = pt1.X;
    double y = pt1.Y;
    int num11 = (int) ((Math.Abs(pt1.X - pt2.X) + Math.Abs(pt1.Y - pt2.Y)) / tolerance);
    for (int index = 1; index < num11; ++index)
    {
      double num12 = (double) index / (double) (num11 - 1);
      ScreenPoint screenPoint = new ScreenPoint(num5 * num12 * num12 * num12 + num7 * num12 * num12 + num9 * num12 + x, num6 * num12 * num12 * num12 + num8 * num12 * num12 + num10 * num12 + y);
      points.Add(screenPoint);
    }
  }
}
