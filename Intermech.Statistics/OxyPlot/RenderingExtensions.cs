// Decompiled with JetBrains decompiler
// Type: OxyPlot.RenderingExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot;

public static class RenderingExtensions
{
  private static readonly double M1 = Math.Tan(Math.PI / 6.0);
  private static readonly double M2 = Math.Sqrt(1.0 + RenderingExtensions.M1 * RenderingExtensions.M1);
  private static readonly double M3 = Math.Tan(Math.PI / 4.0);

  public static void DrawClippedLine(
    this IRenderContext rc,
    OxyRect clippingRectangle,
    IList<ScreenPoint> points,
    double minDistSquared,
    OxyColor stroke,
    double strokeThickness,
    double[] dashArray,
    LineJoin lineJoin,
    bool aliased,
    List<ScreenPoint> outputBuffer = null,
    Action<IList<ScreenPoint>> pointsRendered = null)
  {
    int count = points.Count;
    if (count == 0)
      return;
    if (outputBuffer != null)
      outputBuffer.Clear();
    else
      outputBuffer = new List<ScreenPoint>(count);
    Action action = (Action) (() =>
    {
      RenderingExtensions.EnsureNonEmptyLineIsVisible((IList<ScreenPoint>) outputBuffer);
      rc.DrawLine((IList<ScreenPoint>) outputBuffer, stroke, strokeThickness, dashArray, lineJoin, aliased);
      if (pointsRendered == null)
        return;
      pointsRendered((IList<ScreenPoint>) outputBuffer);
    });
    CohenSutherlandClipping sutherlandClipping = new CohenSutherlandClipping(clippingRectangle);
    if (count == 1 && sutherlandClipping.IsInside(points[0]))
      outputBuffer.Add(points[0]);
    int index1 = 0;
    for (int index2 = 1; index2 < count; ++index2)
    {
      ScreenPoint point1 = points[index2 - 1];
      ScreenPoint point2 = points[index2];
      if (sutherlandClipping.ClipLine(ref point1, ref point2))
      {
        double num1 = point2.X - points[index1].X;
        double num2 = point2.Y - points[index1].Y;
        if (num1 * num1 + num2 * num2 > minDistSquared || outputBuffer.Count == 0 || index2 == count - 1)
        {
          if (point1.X != points[index1].X || point1.Y != points[index1].Y || outputBuffer.Count == 0)
            outputBuffer.Add(new ScreenPoint(point1.X, point1.Y));
          outputBuffer.Add(new ScreenPoint(point2.X, point2.Y));
          index1 = index2;
        }
        if (!sutherlandClipping.IsInside(points[index2]) && outputBuffer.Count != 0)
        {
          action();
          outputBuffer.Clear();
        }
      }
    }
    if (outputBuffer.Count <= 0)
      return;
    action();
  }

  public static void DrawClippedLineSegments(
    this IRenderContext rc,
    OxyRect clippingRectangle,
    IList<ScreenPoint> points,
    OxyColor stroke,
    double strokeThickness,
    double[] dashArray,
    LineJoin lineJoin,
    bool aliased)
  {
    if (rc.SetClip(clippingRectangle))
    {
      rc.DrawLineSegments(points, stroke, strokeThickness, dashArray, lineJoin, aliased);
      rc.ResetClip();
    }
    else
    {
      CohenSutherlandClipping sutherlandClipping = new CohenSutherlandClipping(clippingRectangle);
      List<ScreenPoint> points1 = new List<ScreenPoint>(points.Count);
      for (int index = 0; index + 1 < points.Count; index += 2)
      {
        ScreenPoint point1 = points[index];
        ScreenPoint point2 = points[index + 1];
        if (sutherlandClipping.ClipLine(ref point1, ref point2))
        {
          points1.Add(point1);
          points1.Add(point2);
        }
      }
      rc.DrawLineSegments((IList<ScreenPoint>) points1, stroke, strokeThickness, dashArray, lineJoin, aliased);
    }
  }

  public static void DrawImage(
    this IRenderContext rc,
    OxyImage image,
    double x,
    double y,
    double w,
    double h,
    double opacity,
    bool interpolate)
  {
    rc.DrawImage(image, 0.0, 0.0, (double) image.Width, (double) image.Height, x, y, w, h, opacity, interpolate);
  }

  public static void DrawClippedImage(
    this IRenderContext rc,
    OxyRect clippingRectangle,
    OxyImage source,
    double x,
    double y,
    double w,
    double h,
    double opacity,
    bool interpolate)
  {
    if (x > clippingRectangle.Right || x + w < clippingRectangle.Left || y > clippingRectangle.Bottom || y + h < clippingRectangle.Top)
      return;
    if (rc.SetClip(clippingRectangle))
    {
      rc.DrawImage(source, x, y, w, h, opacity, interpolate);
      rc.ResetClip();
    }
    else
    {
      double num1 = (clippingRectangle.Left - x) / w;
      double num2 = (clippingRectangle.Right - x) / w;
      double num3 = (clippingRectangle.Top - y) / h;
      double num4 = (clippingRectangle.Bottom - y) / h;
      double a1 = num1 < 0.0 ? 0.0 : num1 * (double) source.Width;
      double a2 = num3 < 0.0 ? 0.0 : num3 * (double) source.Height;
      double srcX = (double) (int) Math.Ceiling(a1);
      double srcY = (double) (int) Math.Ceiling(a2);
      double num5 = num2 > 1.0 ? (double) source.Width - srcX : num2 * (double) source.Width - srcX;
      double num6 = num4 > 1.0 ? (double) source.Height - srcY : num4 * (double) source.Height - srcY;
      double srcWidth = (double) (int) num5;
      double srcHeight = (double) (int) num6;
      if ((int) srcWidth <= 0 || (int) srcHeight <= 0)
        return;
      double destX = num1 < 0.0 ? x : x + srcX / (double) source.Width * w;
      double destY = num3 < 0.0 ? y : y + srcY / (double) source.Height * h;
      double destWidth = w * srcWidth / (double) source.Width;
      double destHeight = h * srcHeight / (double) source.Height;
      rc.DrawImage(source, srcX, srcY, srcWidth, srcHeight, destX, destY, destWidth, destHeight, opacity, interpolate);
    }
  }

  public static void DrawClippedPolygon(
    this IRenderContext rc,
    OxyRect clippingRectangle,
    IList<ScreenPoint> points,
    double minDistSquared,
    OxyColor fill,
    OxyColor stroke,
    double strokeThickness = 1.0,
    LineStyle lineStyle = LineStyle.Solid,
    LineJoin lineJoin = LineJoin.Miter,
    bool aliased = false)
  {
    if (rc.SetClip(clippingRectangle))
    {
      rc.DrawPolygon(points, fill, stroke, strokeThickness, lineStyle.GetDashArray(), lineJoin, aliased);
      rc.ResetClip();
    }
    else
    {
      List<ScreenPoint> points1 = SutherlandHodgmanClipping.ClipPolygon(clippingRectangle, points);
      rc.DrawPolygon((IList<ScreenPoint>) points1, fill, stroke, strokeThickness, lineStyle.GetDashArray(), lineJoin, aliased);
    }
  }

  public static void DrawClippedRectangle(
    this IRenderContext rc,
    OxyRect clippingRectangle,
    OxyRect rect,
    OxyColor fill,
    OxyColor stroke,
    double thickness)
  {
    if (rc.SetClip(clippingRectangle))
    {
      rc.DrawRectangle(rect, fill, stroke, thickness);
      rc.ResetClip();
    }
    else
    {
      OxyRect? nullable = RenderingExtensions.ClipRect(rect, clippingRectangle);
      if (!nullable.HasValue)
        return;
      rc.DrawRectangle(nullable.Value, fill, stroke, thickness);
    }
  }

  public static void DrawClippedRectangleAsPolygon(
    this IRenderContext rc,
    OxyRect clippingRectangle,
    OxyRect rect,
    OxyColor fill,
    OxyColor stroke,
    double thickness)
  {
    if (rc.SetClip(clippingRectangle))
    {
      rc.DrawRectangleAsPolygon(rect, fill, stroke, thickness);
      rc.ResetClip();
    }
    else
    {
      OxyRect? nullable = RenderingExtensions.ClipRect(rect, clippingRectangle);
      if (!nullable.HasValue)
        return;
      rc.DrawRectangleAsPolygon(nullable.Value, fill, stroke, thickness);
    }
  }

  public static void DrawClippedEllipse(
    this IRenderContext rc,
    OxyRect clippingRectangle,
    OxyRect rect,
    OxyColor fill,
    OxyColor stroke,
    double thickness,
    int n = 100)
  {
    if (rc.SetClip(clippingRectangle))
    {
      rc.DrawEllipse(rect, fill, stroke, thickness);
      rc.ResetClip();
    }
    else
    {
      ScreenPoint[] points = new ScreenPoint[n];
      double num1 = (rect.Left + rect.Right) / 2.0;
      double num2 = (rect.Top + rect.Bottom) / 2.0;
      double num3 = (rect.Right - rect.Left) / 2.0;
      double num4 = (rect.Bottom - rect.Top) / 2.0;
      for (int index = 0; index < n; ++index)
      {
        double num5 = 2.0 * Math.PI * (double) index / (double) (n - 1);
        points[index] = new ScreenPoint(num1 + num3 * Math.Cos(num5), num2 + num4 * Math.Sin(num5));
      }
      rc.DrawClippedPolygon(clippingRectangle, (IList<ScreenPoint>) points, 4.0, fill, stroke, thickness);
    }
  }

  public static void DrawClippedText(
    this IRenderContext rc,
    OxyRect clippingRectangle,
    ScreenPoint p,
    string text,
    OxyColor fill,
    string fontFamily = null,
    double fontSize = 10.0,
    double fontWeight = 500.0,
    double rotate = 0.0,
    HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
    VerticalAlignment verticalAlignment = VerticalAlignment.Top,
    OxySize? maxSize = null)
  {
    if (rc.SetClip(clippingRectangle))
    {
      rc.DrawText(p, text, fill, fontFamily, fontSize, fontWeight, rotate, horizontalAlignment, verticalAlignment, maxSize);
      rc.ResetClip();
    }
    else
    {
      if (!clippingRectangle.Contains(p.X, p.Y))
        return;
      rc.DrawText(p, text, fill, fontFamily, fontSize, fontWeight, rotate, horizontalAlignment, verticalAlignment, maxSize);
    }
  }

  public static void DrawClippedMathText(
    this IRenderContext rc,
    OxyRect clippingRectangle,
    ScreenPoint p,
    string text,
    OxyColor fill,
    string fontFamily = null,
    double fontSize = 10.0,
    double fontWeight = 500.0,
    double rotate = 0.0,
    HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
    VerticalAlignment verticalAlignment = VerticalAlignment.Top,
    OxySize? maxSize = null)
  {
    if (rc.SetClip(clippingRectangle))
    {
      rc.DrawMathText(p, text, fill, fontFamily, fontSize, fontWeight, rotate, horizontalAlignment, verticalAlignment, maxSize);
      rc.ResetClip();
    }
    else
    {
      if (!clippingRectangle.Contains(p.X, p.Y))
        return;
      rc.DrawMathText(p, text, fill, fontFamily, fontSize, fontWeight, rotate, horizontalAlignment, verticalAlignment, maxSize);
    }
  }

  public static void DrawMultilineText(
    this IRenderContext rc,
    ScreenPoint point,
    string text,
    OxyColor color,
    string fontFamily = null,
    double fontSize = 10.0,
    double fontWeight = 400.0,
    double dy = 12.0)
  {
    string[] strArray = text.Split(new string[1]{ "\r\n" }, StringSplitOptions.None);
    for (int index = 0; index < strArray.Length; ++index)
    {
      IRenderContext renderContext = rc;
      ScreenPoint p = new ScreenPoint(point.X, point.Y + (double) index * dy);
      string text1 = strArray[index];
      OxyColor fill = color;
      double num = fontWeight;
      double fontSize1 = fontSize;
      double fontWeight1 = num;
      OxySize? maxSize = new OxySize?();
      renderContext.DrawText(p, text1, fill, fontSize: fontSize1, fontWeight: fontWeight1, maxSize: maxSize);
    }
  }

  public static void DrawLine(
    this IRenderContext rc,
    double x0,
    double y0,
    double x1,
    double y1,
    OxyPen pen,
    bool aliased = true)
  {
    if (pen == null)
      return;
    rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
    {
      new ScreenPoint(x0, y0),
      new ScreenPoint(x1, y1)
    }, pen.Color, pen.Thickness, pen.ActualDashArray, pen.LineJoin, (aliased ? 1 : 0) != 0);
  }

  public static void DrawLineSegments(
    this IRenderContext rc,
    IList<ScreenPoint> points,
    OxyPen pen,
    bool aliased = true)
  {
    if (pen == null)
      return;
    rc.DrawLineSegments(points, pen.Color, pen.Thickness, pen.ActualDashArray, pen.LineJoin, aliased);
  }

  public static void DrawMarker(
    this IRenderContext rc,
    OxyRect clippingRectangle,
    ScreenPoint p,
    MarkerType type,
    IList<ScreenPoint> outline,
    double size,
    OxyColor fill,
    OxyColor stroke,
    double strokeThickness)
  {
    rc.DrawMarkers(clippingRectangle, (IList<ScreenPoint>) new ScreenPoint[1]
    {
      p
    }, type, outline, (IList<double>) new double[1]{ size }, fill, stroke, strokeThickness);
  }

  public static void DrawMarkers(
    this IRenderContext rc,
    IList<ScreenPoint> markerPoints,
    OxyRect clippingRectangle,
    MarkerType markerType,
    IList<ScreenPoint> markerOutline,
    double markerSize,
    OxyColor markerFill,
    OxyColor markerStroke,
    double markerStrokeThickness,
    int resolution = 0,
    ScreenPoint binOffset = default (ScreenPoint))
  {
    rc.DrawMarkers(clippingRectangle, markerPoints, markerType, markerOutline, (IList<double>) new double[1]
    {
      markerSize
    }, markerFill, markerStroke, markerStrokeThickness, resolution, binOffset);
  }

  public static void DrawMarkers(
    this IRenderContext rc,
    OxyRect clippingRectangle,
    IList<ScreenPoint> markerPoints,
    MarkerType markerType,
    IList<ScreenPoint> markerOutline,
    IList<double> markerSize,
    OxyColor markerFill,
    OxyColor markerStroke,
    double markerStrokeThickness,
    int resolution = 0,
    ScreenPoint binOffset = default (ScreenPoint))
  {
    if (markerType == MarkerType.None)
      return;
    int count = markerPoints.Count;
    List<OxyRect> oxyRectList1 = new List<OxyRect>(count);
    List<OxyRect> oxyRectList2 = new List<OxyRect>(count);
    List<IList<ScreenPoint>> polygons = new List<IList<ScreenPoint>>(count);
    List<ScreenPoint> screenPointList = new List<ScreenPoint>(count);
    Dictionary<uint, bool> dictionary = new Dictionary<uint, bool>();
    int num = 0;
    double left = clippingRectangle.Left;
    double right = clippingRectangle.Right;
    double top = clippingRectangle.Top;
    double bottom = clippingRectangle.Bottom;
    foreach (ScreenPoint markerPoint in (IEnumerable<ScreenPoint>) markerPoints)
    {
      if (resolution > 1)
      {
        uint key = (uint) (((int) ((markerPoint.X - binOffset.X) / (double) resolution) << 16 /*0x10*/) + (int) ((markerPoint.Y - binOffset.Y) / (double) resolution));
        if (dictionary.ContainsKey(key))
        {
          ++num;
          continue;
        }
        dictionary.Add(key, true);
      }
      if ((markerPoint.x < left || markerPoint.x > right || markerPoint.y < top ? 1 : (markerPoint.y > bottom ? 1 : 0)) == 0)
      {
        int index = num < markerSize.Count ? num : 0;
        RenderingExtensions.AddMarkerGeometry(markerPoint, markerType, (IEnumerable<ScreenPoint>) markerOutline, markerSize[index], (IList<OxyRect>) oxyRectList1, (IList<OxyRect>) oxyRectList2, (IList<IList<ScreenPoint>>) polygons, (IList<ScreenPoint>) screenPointList);
      }
      ++num;
    }
    if (oxyRectList1.Count > 0)
      rc.DrawEllipses((IList<OxyRect>) oxyRectList1, markerFill, markerStroke, markerStrokeThickness);
    if (oxyRectList2.Count > 0)
      rc.DrawRectangles((IList<OxyRect>) oxyRectList2, markerFill, markerStroke, markerStrokeThickness);
    if (polygons.Count > 0)
      rc.DrawPolygons((IList<IList<ScreenPoint>>) polygons, markerFill, markerStroke, markerStrokeThickness);
    if (screenPointList.Count <= 0)
      return;
    rc.DrawLineSegments((IList<ScreenPoint>) screenPointList, markerStroke, markerStrokeThickness);
  }

  public static void DrawRectangleAsPolygon(
    this IRenderContext rc,
    OxyRect rect,
    OxyColor fill,
    OxyColor stroke,
    double thickness)
  {
    ScreenPoint screenPoint1 = new ScreenPoint(rect.Left, rect.Top);
    ScreenPoint screenPoint2 = new ScreenPoint(rect.Right, rect.Top);
    ScreenPoint screenPoint3 = new ScreenPoint(rect.Right, rect.Bottom);
    ScreenPoint screenPoint4 = new ScreenPoint(rect.Left, rect.Bottom);
    rc.DrawPolygon((IList<ScreenPoint>) new ScreenPoint[4]
    {
      screenPoint1,
      screenPoint2,
      screenPoint3,
      screenPoint4
    }, fill, stroke, thickness, aliased: true);
  }

  public static void DrawCircle(
    this IRenderContext rc,
    double x,
    double y,
    double r,
    OxyColor fill,
    OxyColor stroke,
    double thickness = 1.0)
  {
    rc.DrawEllipse(new OxyRect(x - r, y - r, r * 2.0, r * 2.0), fill, stroke, thickness);
  }

  public static void DrawCircle(
    this IRenderContext rc,
    ScreenPoint center,
    double r,
    OxyColor fill,
    OxyColor stroke,
    double thickness = 1.0)
  {
    rc.DrawCircle(center.X, center.Y, r, fill, stroke, thickness);
  }

  public static void FillCircle(
    this IRenderContext rc,
    ScreenPoint center,
    double r,
    OxyColor fill)
  {
    rc.DrawCircle(center.X, center.Y, r, fill, OxyColors.Undefined, 0.0);
  }

  public static void FillRectangle(this IRenderContext rc, OxyRect rectangle, OxyColor fill)
  {
    rc.DrawRectangle(rectangle, fill, OxyColors.Undefined, 0.0);
  }

  public static void DrawRectangleAsPolygon(
    this IRenderContext rc,
    OxyRect rect,
    OxyColor fill,
    OxyColor stroke,
    OxyThickness thickness)
  {
    if (thickness.Left.Equals(thickness.Right) && thickness.Left.Equals(thickness.Top) && thickness.Left.Equals(thickness.Bottom))
    {
      rc.DrawRectangleAsPolygon(rect, fill, stroke, thickness.Left);
    }
    else
    {
      ScreenPoint screenPoint1 = new ScreenPoint(rect.Left, rect.Top);
      ScreenPoint screenPoint2 = new ScreenPoint(rect.Right, rect.Top);
      ScreenPoint screenPoint3 = new ScreenPoint(rect.Right, rect.Bottom);
      ScreenPoint screenPoint4 = new ScreenPoint(rect.Left, rect.Bottom);
      rc.DrawPolygon((IList<ScreenPoint>) new ScreenPoint[4]
      {
        screenPoint1,
        screenPoint2,
        screenPoint3,
        screenPoint4
      }, fill, OxyColors.Undefined, 0.0, aliased: true);
      rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
      {
        screenPoint1,
        screenPoint2
      }, stroke, thickness.Top, aliased: true);
      rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
      {
        screenPoint2,
        screenPoint3
      }, stroke, thickness.Right, aliased: true);
      rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
      {
        screenPoint3,
        screenPoint4
      }, stroke, thickness.Bottom, aliased: true);
      rc.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
      {
        screenPoint4,
        screenPoint1
      }, stroke, thickness.Left, aliased: true);
    }
  }

  public static OxySize MeasureText(
    this IRenderContext rc,
    string text,
    string fontFamily,
    double fontSize,
    double fontWeight,
    double angle)
  {
    return RenderingExtensions.MeasureRotatedRectangleBound(rc.MeasureText(text, fontFamily, fontSize, fontWeight), angle);
  }

  private static void AddMarkerGeometry(
    ScreenPoint p,
    MarkerType type,
    IEnumerable<ScreenPoint> outline,
    double size,
    IList<OxyRect> ellipses,
    IList<OxyRect> rects,
    IList<IList<ScreenPoint>> polygons,
    IList<ScreenPoint> lines)
  {
    switch (type)
    {
      case MarkerType.Circle:
        ellipses.Add(new OxyRect(p.x - size, p.y - size, size * 2.0, size * 2.0));
        break;
      case MarkerType.Square:
        rects.Add(new OxyRect(p.x - size, p.y - size, size * 2.0, size * 2.0));
        break;
      case MarkerType.Diamond:
        polygons.Add((IList<ScreenPoint>) new ScreenPoint[4]
        {
          new ScreenPoint(p.x, p.y - RenderingExtensions.M2 * size),
          new ScreenPoint(p.x + RenderingExtensions.M2 * size, p.y),
          new ScreenPoint(p.x, p.y + RenderingExtensions.M2 * size),
          new ScreenPoint(p.x - RenderingExtensions.M2 * size, p.y)
        });
        break;
      case MarkerType.Triangle:
        polygons.Add((IList<ScreenPoint>) new ScreenPoint[3]
        {
          new ScreenPoint(p.x - size, p.y + RenderingExtensions.M1 * size),
          new ScreenPoint(p.x + size, p.y + RenderingExtensions.M1 * size),
          new ScreenPoint(p.x, p.y - RenderingExtensions.M2 * size)
        });
        break;
      case MarkerType.Plus:
      case MarkerType.Star:
        lines.Add(new ScreenPoint(p.x - size, p.y));
        lines.Add(new ScreenPoint(p.x + size, p.y));
        lines.Add(new ScreenPoint(p.x, p.y - size));
        lines.Add(new ScreenPoint(p.x, p.y + size));
        break;
      case MarkerType.Custom:
        if (outline == null)
          throw new ArgumentNullException(nameof (outline), "The outline should be set when MarkerType is 'Custom'.");
        List<ScreenPoint> list = outline.Select<ScreenPoint, ScreenPoint>((Func<ScreenPoint, ScreenPoint>) (o => new ScreenPoint(p.X + o.x * size, p.Y + o.y * size))).ToList<ScreenPoint>();
        polygons.Add((IList<ScreenPoint>) list);
        return;
    }
    if (type != MarkerType.Cross && type != MarkerType.Star)
      return;
    lines.Add(new ScreenPoint(p.x - size * RenderingExtensions.M3, p.y - size * RenderingExtensions.M3));
    lines.Add(new ScreenPoint(p.x + size * RenderingExtensions.M3, p.y + size * RenderingExtensions.M3));
    lines.Add(new ScreenPoint(p.x - size * RenderingExtensions.M3, p.y + size * RenderingExtensions.M3));
    lines.Add(new ScreenPoint(p.x + size * RenderingExtensions.M3, p.y - size * RenderingExtensions.M3));
  }

  private static OxyRect? ClipRect(OxyRect rect, OxyRect clippingRectangle)
  {
    if (rect.Right < clippingRectangle.Left)
      return new OxyRect?();
    if (rect.Left > clippingRectangle.Right)
      return new OxyRect?();
    if (rect.Top > clippingRectangle.Bottom)
      return new OxyRect?();
    if (rect.Bottom < clippingRectangle.Top)
      return new OxyRect?();
    double width = rect.Width;
    double left = rect.Left;
    double top = rect.Top;
    double height = rect.Height;
    if (left + width > clippingRectangle.Right)
      width = clippingRectangle.Right - left;
    if (left < clippingRectangle.Left)
    {
      width = rect.Right - clippingRectangle.Left;
      left = clippingRectangle.Left;
    }
    if (top < clippingRectangle.Top)
    {
      height = rect.Bottom - clippingRectangle.Top;
      top = clippingRectangle.Top;
    }
    if (top + height > clippingRectangle.Bottom)
      height = clippingRectangle.Bottom - top;
    return rect.Width <= 0.0 || rect.Height <= 0.0 ? new OxyRect?() : new OxyRect?(new OxyRect(left, top, width, height));
  }

  private static void EnsureNonEmptyLineIsVisible(IList<ScreenPoint> pts)
  {
    ScreenPoint pt;
    if (pts.Count == 2 && pts[0].DistanceTo(pts[1]) < 1.0)
    {
      IList<ScreenPoint> screenPointList1 = pts;
      double x1 = pts[0].X + 1.0;
      pt = pts[0];
      double y1 = pt.Y;
      ScreenPoint screenPoint1 = new ScreenPoint(x1, y1);
      screenPointList1[1] = screenPoint1;
      IList<ScreenPoint> screenPointList2 = pts;
      pt = pts[0];
      double x2 = pt.X - 1.0;
      pt = pts[0];
      double y2 = pt.Y;
      ScreenPoint screenPoint2 = new ScreenPoint(x2, y2);
      screenPointList2[0] = screenPoint2;
    }
    if (pts.Count != 1)
      return;
    IList<ScreenPoint> screenPointList3 = pts;
    pt = pts[0];
    double x3 = pt.X + 1.0;
    pt = pts[0];
    double y3 = pt.Y;
    ScreenPoint screenPoint3 = new ScreenPoint(x3, y3);
    screenPointList3.Add(screenPoint3);
    IList<ScreenPoint> screenPointList4 = pts;
    pt = pts[0];
    double x4 = pt.X - 1.0;
    pt = pts[0];
    double y4 = pt.Y;
    ScreenPoint screenPoint4 = new ScreenPoint(x4, y4);
    screenPointList4[0] = screenPoint4;
  }

  private static OxySize MeasureRotatedRectangleBound(OxySize bounds, double angle)
  {
    OxyRect bounds1 = bounds.GetBounds(angle, HorizontalAlignment.Center, VerticalAlignment.Middle);
    return new OxySize(bounds1.Width, bounds1.Height);
  }
}
