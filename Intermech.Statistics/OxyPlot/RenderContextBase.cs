// Decompiled with JetBrains decompiler
// Type: OxyPlot.RenderContextBase
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot;

public abstract class RenderContextBase : IRenderContext
{
  protected RenderContextBase() => this.RendersToScreen = true;

  public bool RendersToScreen { get; set; }

  public virtual void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
  {
    this.DrawPolygon((IList<ScreenPoint>) RenderContextBase.CreateEllipse(rect), fill, stroke, thickness, (double[]) null, LineJoin.Miter, false);
  }

  public virtual void DrawEllipses(
    IList<OxyRect> rectangles,
    OxyColor fill,
    OxyColor stroke,
    double thickness)
  {
    foreach (OxyRect rectangle in (IEnumerable<OxyRect>) rectangles)
      this.DrawEllipse(rectangle, fill, stroke, thickness);
  }

  public abstract void DrawLine(
    IList<ScreenPoint> points,
    OxyColor stroke,
    double thickness,
    double[] dashArray,
    LineJoin lineJoin,
    bool aliased);

  public virtual void DrawLineSegments(
    IList<ScreenPoint> points,
    OxyColor stroke,
    double thickness,
    double[] dashArray,
    LineJoin lineJoin,
    bool aliased)
  {
    for (int index = 0; index + 1 < points.Count; index += 2)
      this.DrawLine((IList<ScreenPoint>) new ScreenPoint[2]
      {
        points[index],
        points[index + 1]
      }, stroke, thickness, dashArray, lineJoin, aliased);
  }

  public abstract void DrawPolygon(
    IList<ScreenPoint> points,
    OxyColor fill,
    OxyColor stroke,
    double thickness,
    double[] dashArray,
    LineJoin lineJoin,
    bool aliased);

  public virtual void DrawPolygons(
    IList<IList<ScreenPoint>> polygons,
    OxyColor fill,
    OxyColor stroke,
    double thickness,
    double[] dashArray,
    LineJoin lineJoin,
    bool aliased)
  {
    foreach (IList<ScreenPoint> polygon in (IEnumerable<IList<ScreenPoint>>) polygons)
      this.DrawPolygon(polygon, fill, stroke, thickness, dashArray, lineJoin, aliased);
  }

  public virtual void DrawRectangle(
    OxyRect rect,
    OxyColor fill,
    OxyColor stroke,
    double thickness)
  {
    this.DrawPolygon((IList<ScreenPoint>) RenderContextBase.CreateRectangle(rect), fill, stroke, thickness, (double[]) null, LineJoin.Miter, true);
  }

  public virtual void DrawRectangles(
    IList<OxyRect> rectangles,
    OxyColor fill,
    OxyColor stroke,
    double thickness)
  {
    foreach (OxyRect rectangle in (IEnumerable<OxyRect>) rectangles)
      this.DrawRectangle(rectangle, fill, stroke, thickness);
  }

  public abstract void DrawText(
    ScreenPoint p,
    string text,
    OxyColor fill,
    string fontFamily,
    double fontSize,
    double fontWeight,
    double rotate,
    HorizontalAlignment halign,
    VerticalAlignment valign,
    OxySize? maxSize);

  public abstract OxySize MeasureText(
    string text,
    string fontFamily,
    double fontSize,
    double fontWeight);

  public virtual void SetToolTip(string text)
  {
  }

  public virtual void CleanUp()
  {
  }

  public virtual void DrawImage(
    OxyImage source,
    double srcX,
    double srcY,
    double srcWidth,
    double srcHeight,
    double destX,
    double destY,
    double destWidth,
    double destHeight,
    double opacity,
    bool interpolate)
  {
  }

  public virtual bool SetClip(OxyRect rect) => false;

  public virtual void ResetClip()
  {
  }

  protected static ScreenPoint[] CreateEllipse(OxyRect rect, int n = 40)
  {
    double x = rect.Center.X;
    double y = rect.Center.Y;
    double num1 = rect.Width / 2.0;
    double num2 = rect.Height / 2.0;
    ScreenPoint[] ellipse = new ScreenPoint[n];
    for (int index = 0; index < n; ++index)
    {
      double num3 = 2.0 * Math.PI * (double) index / (double) n;
      ellipse[index] = new ScreenPoint(x + Math.Cos(num3) * num1, y + Math.Sin(num3) * num2);
    }
    return ellipse;
  }

  protected static ScreenPoint[] CreateRectangle(OxyRect rect)
  {
    return new ScreenPoint[4]
    {
      new ScreenPoint(rect.Left, rect.Top),
      new ScreenPoint(rect.Left, rect.Bottom),
      new ScreenPoint(rect.Right, rect.Bottom),
      new ScreenPoint(rect.Right, rect.Top)
    };
  }
}
