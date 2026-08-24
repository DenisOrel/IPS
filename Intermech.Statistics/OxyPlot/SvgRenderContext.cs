// Decompiled with JetBrains decompiler
// Type: OxyPlot.SvgRenderContext
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace OxyPlot;

public class SvgRenderContext : RenderContextBase, IDisposable
{
  private readonly SvgWriter w;
  private bool disposed;

  public SvgRenderContext(
    Stream s,
    double width,
    double height,
    bool isDocument,
    IRenderContext textMeasurer,
    OxyColor background)
  {
    if (textMeasurer == null)
      throw new ArgumentNullException(nameof (textMeasurer), "A text measuring render context must be provided.");
    this.w = new SvgWriter(s, width, height, isDocument);
    this.TextMeasurer = textMeasurer;
    if (!background.IsVisible())
      return;
    this.w.WriteRectangle(0.0, 0.0, width, height, this.w.CreateStyle(background, OxyColors.Undefined, 0.0));
  }

  public IRenderContext TextMeasurer { get; set; }

  public void Close() => this.w.Close();

  public void Complete() => this.w.Complete();

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
  {
    this.w.WriteEllipse(rect.Left, rect.Top, rect.Width, rect.Height, this.w.CreateStyle(fill, stroke, thickness));
  }

  public override void DrawLine(
    IList<ScreenPoint> points,
    OxyColor stroke,
    double thickness,
    double[] dashArray,
    LineJoin lineJoin,
    bool aliased)
  {
    this.w.WritePolyline((IEnumerable<ScreenPoint>) points, this.w.CreateStyle(OxyColors.Undefined, stroke, thickness, dashArray, lineJoin));
  }

  public override void DrawPolygon(
    IList<ScreenPoint> points,
    OxyColor fill,
    OxyColor stroke,
    double thickness,
    double[] dashArray,
    LineJoin lineJoin,
    bool aliased)
  {
    this.w.WritePolygon((IEnumerable<ScreenPoint>) points, this.w.CreateStyle(fill, stroke, thickness, dashArray, lineJoin));
  }

  public override void DrawRectangle(
    OxyRect rect,
    OxyColor fill,
    OxyColor stroke,
    double thickness)
  {
    this.w.WriteRectangle(rect.Left, rect.Top, rect.Width, rect.Height, this.w.CreateStyle(fill, stroke, thickness));
  }

  public override void DrawText(
    ScreenPoint p,
    string text,
    OxyColor c,
    string fontFamily,
    double fontSize,
    double fontWeight,
    double rotate,
    HorizontalAlignment halign,
    VerticalAlignment valign,
    OxySize? maxSize)
  {
    if (string.IsNullOrEmpty(text))
      return;
    string[] strArray = Regex.Split(text, "\r\n");
    if (valign == VerticalAlignment.Bottom)
    {
      for (int index = strArray.Length - 1; index >= 0; --index)
      {
        string text1 = strArray[index];
        OxySize oxySize = this.MeasureText(text1, fontFamily, fontSize, fontWeight);
        this.w.WriteText(p, text1, c, fontFamily, fontSize, fontWeight, rotate, halign, valign);
        p += new ScreenVector(Math.Sin(rotate / 180.0 * Math.PI) * oxySize.Height, Math.Cos(rotate / 180.0 * Math.PI) * oxySize.Height);
      }
    }
    else
    {
      foreach (string text2 in strArray)
      {
        OxySize oxySize = this.MeasureText(text2, fontFamily, fontSize, fontWeight);
        this.w.WriteText(p, text2, c, fontFamily, fontSize, fontWeight, rotate, halign, valign);
        p += new ScreenVector(-Math.Sin(rotate / 180.0 * Math.PI) * oxySize.Height, Math.Cos(rotate / 180.0 * Math.PI) * oxySize.Height);
      }
    }
  }

  public void Flush() => this.w.Flush();

  public override OxySize MeasureText(
    string text,
    string fontFamily,
    double fontSize,
    double fontWeight)
  {
    return string.IsNullOrEmpty(text) ? OxySize.Empty : this.TextMeasurer.MeasureText(text, fontFamily, fontSize, fontWeight);
  }

  public override void DrawImage(
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
    this.w.WriteImage(srcX, srcY, srcWidth, srcHeight, destX, destY, destWidth, destHeight, source);
  }

  private void Dispose(bool disposing)
  {
    if (!this.disposed && disposing)
      this.w.Dispose();
    this.disposed = true;
  }
}
