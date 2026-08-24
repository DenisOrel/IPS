// Decompiled with JetBrains decompiler
// Type: OxyPlot.ScreenPointHelper
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot;

public static class ScreenPointHelper
{
  public static ScreenPoint FindNearestPointOnPolyline(ScreenPoint point, IList<ScreenPoint> points)
  {
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    double num = double.MaxValue;
    ScreenPoint nearestPointOnPolyline = new ScreenPoint();
    for (int index = 0; index + 1 < points.Count; ++index)
    {
      ScreenPoint point1 = points[index];
      ScreenPoint point2 = points[index + 1];
      if (!ScreenPoint.IsUndefined(point1) && !ScreenPoint.IsUndefined(point2))
      {
        ScreenPoint pointOnLine = ScreenPointHelper.FindPointOnLine(point, point1, point2);
        if (!ScreenPoint.IsUndefined(pointOnLine))
        {
          double lengthSquared = (point - pointOnLine).LengthSquared;
          if (lengthSquared < num)
          {
            nearestPointOnPolyline = pointOnLine;
            num = lengthSquared;
          }
        }
      }
    }
    return nearestPointOnPolyline;
  }

  public static ScreenPoint FindPointOnLine(ScreenPoint p, ScreenPoint p1, ScreenPoint p2)
  {
    double num1 = p2.x - p1.x;
    double num2 = p2.y - p1.y;
    double d = ScreenPointHelper.FindPositionOnLine(p, p1, p2);
    if (double.IsNaN(d))
      d = 0.0;
    if (d < 0.0)
      d = 0.0;
    if (d > 1.0)
      d = 1.0;
    return new ScreenPoint(p1.x + d * num1, p1.y + d * num2);
  }

  public static double FindPositionOnLine(ScreenPoint p, ScreenPoint p1, ScreenPoint p2)
  {
    double num1 = p2.x - p1.x;
    double num2 = p2.y - p1.y;
    double num3 = (p.x - p1.x) * num1 + (p.y - p1.y) * num2;
    double num4 = num1 * num1 + num2 * num2;
    return num4 < 1E-06 ? double.NaN : num3 / num4;
  }

  public static bool IsPointInPolygon(ScreenPoint p, IList<ScreenPoint> pts)
  {
    if (pts == null)
      return false;
    int count = pts.Count;
    bool flag = false;
    int index1 = 0;
    int index2 = count - 1;
    for (; index1 < count; index2 = index1++)
    {
      ScreenPoint pt = pts[index1];
      int num1 = pt.Y > p.Y ? 1 : 0;
      pt = pts[index2];
      int num2 = pt.Y > p.Y ? 1 : 0;
      if (num1 != num2)
      {
        double x1 = p.X;
        pt = pts[index2];
        double x2 = pt.X;
        pt = pts[index1];
        double x3 = pt.X;
        double num3 = x2 - x3;
        double y1 = p.Y;
        pt = pts[index1];
        double y2 = pt.Y;
        double num4 = y1 - y2;
        pt = pts[index2];
        double y3 = pt.Y;
        pt = pts[index1];
        double y4 = pt.Y;
        double num5 = y3 - y4;
        double num6 = num4 / num5;
        double num7 = num3 * num6;
        pt = pts[index1];
        double x4 = pt.X;
        double num8 = num7 + x4;
        if (x1 < num8)
          flag = !flag;
      }
    }
    return flag;
  }

  public static IList<ScreenPoint> ResamplePoints(
    IList<ScreenPoint> allPoints,
    double minimumDistance)
  {
    double num = minimumDistance * minimumDistance;
    int count = allPoints.Count;
    List<ScreenPoint> screenPointList = new List<ScreenPoint>(count);
    if (count > 0)
    {
      screenPointList.Add(allPoints[0]);
      int index1 = 0;
      for (int index2 = 1; index2 < count; ++index2)
      {
        if (allPoints[index1].DistanceToSquared(allPoints[index2]) >= num || index2 == count - 1)
        {
          index1 = index2;
          screenPointList.Add(allPoints[index2]);
        }
      }
    }
    return (IList<ScreenPoint>) screenPointList;
  }

  public static ScreenPoint GetCentroid(IList<ScreenPoint> points)
  {
    double num1 = 0.0;
    double num2 = 0.0;
    double num3 = 0.0;
    for (int index1 = 0; index1 < points.Count; ++index1)
    {
      int index2 = (index1 + 1) % points.Count;
      double num4 = points[index1].x * points[index2].y - points[index2].x * points[index1].y;
      num1 += (points[index1].x + points[index2].x) * num4;
      num2 += (points[index1].y + points[index2].y) * num4;
      num3 += num4;
    }
    double num5 = num3 * 0.5;
    return new ScreenPoint(num1 / (6.0 * num5), num2 / (6.0 * num5));
  }
}
