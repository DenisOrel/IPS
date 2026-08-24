// Decompiled with JetBrains decompiler
// Type: OxyPlot.SutherlandHodgmanClipping
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot;

public static class SutherlandHodgmanClipping
{
  public static List<ScreenPoint> ClipPolygon(OxyRect bounds, IList<ScreenPoint> v)
  {
    List<ScreenPoint> v1 = SutherlandHodgmanClipping.ClipOneAxis(bounds, SutherlandHodgmanClipping.RectangleEdge.Left, v);
    List<ScreenPoint> v2 = SutherlandHodgmanClipping.ClipOneAxis(bounds, SutherlandHodgmanClipping.RectangleEdge.Right, (IList<ScreenPoint>) v1);
    List<ScreenPoint> v3 = SutherlandHodgmanClipping.ClipOneAxis(bounds, SutherlandHodgmanClipping.RectangleEdge.Top, (IList<ScreenPoint>) v2);
    return SutherlandHodgmanClipping.ClipOneAxis(bounds, SutherlandHodgmanClipping.RectangleEdge.Bottom, (IList<ScreenPoint>) v3);
  }

  private static List<ScreenPoint> ClipOneAxis(
    OxyRect bounds,
    SutherlandHodgmanClipping.RectangleEdge edge,
    IList<ScreenPoint> v)
  {
    if (v.Count == 0)
      return new List<ScreenPoint>();
    List<ScreenPoint> screenPointList = new List<ScreenPoint>(v.Count);
    ScreenPoint screenPoint1 = v[v.Count - 1];
    for (int index = 0; index < v.Count; ++index)
    {
      ScreenPoint screenPoint2 = v[index];
      bool flag1 = SutherlandHodgmanClipping.IsInside(bounds, edge, screenPoint2);
      bool flag2 = SutherlandHodgmanClipping.IsInside(bounds, edge, screenPoint1);
      if (flag2 & flag1)
        screenPointList.Add(screenPoint2);
      else if (flag2)
        screenPointList.Add(SutherlandHodgmanClipping.LineIntercept(bounds, edge, screenPoint1, screenPoint2));
      else if (flag1)
      {
        screenPointList.Add(SutherlandHodgmanClipping.LineIntercept(bounds, edge, screenPoint1, screenPoint2));
        screenPointList.Add(screenPoint2);
      }
      screenPoint1 = screenPoint2;
    }
    return screenPointList;
  }

  private static bool IsInside(
    OxyRect bounds,
    SutherlandHodgmanClipping.RectangleEdge edge,
    ScreenPoint p)
  {
    switch (edge)
    {
      case SutherlandHodgmanClipping.RectangleEdge.Left:
        return p.X >= bounds.Left;
      case SutherlandHodgmanClipping.RectangleEdge.Right:
        return p.X < bounds.Right;
      case SutherlandHodgmanClipping.RectangleEdge.Top:
        return p.Y >= bounds.Top;
      case SutherlandHodgmanClipping.RectangleEdge.Bottom:
        return p.Y < bounds.Bottom;
      default:
        throw new ArgumentException(nameof (edge));
    }
  }

  private static ScreenPoint LineIntercept(
    OxyRect bounds,
    SutherlandHodgmanClipping.RectangleEdge edge,
    ScreenPoint a,
    ScreenPoint b)
  {
    if (a.x.Equals(b.x) && a.y.Equals(b.y))
      return a;
    switch (edge)
    {
      case SutherlandHodgmanClipping.RectangleEdge.Left:
        if (b.X.Equals(a.X))
          throw new ArgumentException("no intercept found");
        return new ScreenPoint(bounds.Left, a.Y + (b.Y - a.Y) * (bounds.Left - a.X) / (b.X - a.X));
      case SutherlandHodgmanClipping.RectangleEdge.Right:
        if (b.X.Equals(a.X))
          throw new ArgumentException("no intercept found");
        return new ScreenPoint(bounds.Right, a.Y + (b.Y - a.Y) * (bounds.Right - a.X) / (b.X - a.X));
      case SutherlandHodgmanClipping.RectangleEdge.Top:
        if (b.Y.Equals(a.Y))
          throw new ArgumentException("no intercept found");
        return new ScreenPoint(a.X + (b.X - a.X) * (bounds.Top - a.Y) / (b.Y - a.Y), bounds.Top);
      case SutherlandHodgmanClipping.RectangleEdge.Bottom:
        if (b.Y.Equals(a.Y))
          throw new ArgumentException("no intercept found");
        return new ScreenPoint(a.X + (b.X - a.X) * (bounds.Bottom - a.Y) / (b.Y - a.Y), bounds.Bottom);
      default:
        throw new ArgumentException("no intercept found");
    }
  }

  private enum RectangleEdge
  {
    Left,
    Right,
    Top,
    Bottom,
  }
}
