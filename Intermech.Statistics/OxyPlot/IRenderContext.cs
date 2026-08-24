// Decompiled with JetBrains decompiler
// Type: OxyPlot.IRenderContext
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;

#nullable disable
namespace OxyPlot;

public interface IRenderContext
{
  bool RendersToScreen { get; }

  void DrawEllipse(OxyRect extents, OxyColor fill, OxyColor stroke, double thickness = 1.0);

  void DrawEllipses(IList<OxyRect> extents, OxyColor fill, OxyColor stroke, double thickness = 1.0);

  void DrawLine(
    IList<ScreenPoint> points,
    OxyColor stroke,
    double thickness = 1.0,
    double[] dashArray = null,
    LineJoin lineJoin = LineJoin.Miter,
    bool aliased = false);

  void DrawLineSegments(
    IList<ScreenPoint> points,
    OxyColor stroke,
    double thickness = 1.0,
    double[] dashArray = null,
    LineJoin lineJoin = LineJoin.Miter,
    bool aliased = false);

  void DrawPolygon(
    IList<ScreenPoint> points,
    OxyColor fill,
    OxyColor stroke,
    double thickness = 1.0,
    double[] dashArray = null,
    LineJoin lineJoin = LineJoin.Miter,
    bool aliased = false);

  void DrawPolygons(
    IList<IList<ScreenPoint>> polygons,
    OxyColor fill,
    OxyColor stroke,
    double thickness = 1.0,
    double[] dashArray = null,
    LineJoin lineJoin = LineJoin.Miter,
    bool aliased = false);

  void DrawRectangle(OxyRect rectangle, OxyColor fill, OxyColor stroke, double thickness = 1.0);

  void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness = 1.0);

  void DrawText(
    ScreenPoint p,
    string text,
    OxyColor fill,
    string fontFamily = null,
    double fontSize = 10.0,
    double fontWeight = 400.0,
    double rotation = 0.0,
    HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
    VerticalAlignment verticalAlignment = VerticalAlignment.Top,
    OxySize? maxSize = null);

  OxySize MeasureText(string text, string fontFamily = null, double fontSize = 10.0, double fontWeight = 500.0);

  void SetToolTip(string text);

  void CleanUp();

  void DrawImage(
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
    bool interpolate);

  bool SetClip(OxyRect clippingRectangle);

  void ResetClip();
}
