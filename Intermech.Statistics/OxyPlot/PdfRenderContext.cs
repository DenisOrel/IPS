// Decompiled with JetBrains decompiler
// Type: OxyPlot.PdfRenderContext
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace OxyPlot;

public class PdfRenderContext : RenderContextBase
{
  private readonly PortableDocument doc;
  private readonly Dictionary<OxyImage, PortableDocumentImage> images = new Dictionary<OxyImage, PortableDocumentImage>();

  public PdfRenderContext(double width, double height, OxyColor background)
  {
    this.doc = new PortableDocument();
    this.doc.AddPage(width, height);
    this.RendersToScreen = false;
    if (!background.IsVisible())
      return;
    this.doc.SetFillColor(background);
    this.doc.FillRectangle(0.0, 0.0, width, height);
  }

  public void Save(Stream s) => this.doc.Save(s);

  public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
  {
    bool flag1 = stroke.IsVisible() && thickness > 0.0;
    bool flag2 = fill.IsVisible();
    if (!flag1 && !flag2)
      return;
    double y = this.doc.PageHeight - rect.Bottom;
    if (flag1)
    {
      this.SetLineWidth(thickness);
      this.doc.SetColor(stroke);
      if (flag2)
      {
        this.doc.SetFillColor(fill);
        this.doc.DrawEllipse(rect.Left, y, rect.Width, rect.Height, true);
      }
      else
        this.doc.DrawEllipse(rect.Left, y, rect.Width, rect.Height);
    }
    else
    {
      this.doc.SetFillColor(fill);
      this.doc.FillEllipse(rect.Left, y, rect.Width, rect.Height);
    }
  }

  public override void DrawLine(
    IList<ScreenPoint> points,
    OxyColor stroke,
    double thickness,
    double[] dashArray,
    LineJoin lineJoin,
    bool aliased)
  {
    this.doc.SetColor(stroke);
    this.SetLineWidth(thickness);
    if (dashArray != null)
      this.SetLineDashPattern(dashArray, 0.0);
    this.doc.SetLineJoin(PdfRenderContext.Convert(lineJoin));
    double pageHeight = this.doc.PageHeight;
    PortableDocument doc1 = this.doc;
    double x1 = points[0].X;
    double num1 = pageHeight;
    ScreenPoint point = points[0];
    double y1 = point.Y;
    double y1_1 = num1 - y1;
    doc1.MoveTo(x1, y1_1);
    for (int index = 1; index < points.Count; ++index)
    {
      PortableDocument doc2 = this.doc;
      point = points[index];
      double x2 = point.X;
      double num2 = pageHeight;
      point = points[index];
      double y2 = point.Y;
      double y1_2 = num2 - y2;
      doc2.LineTo(x2, y1_2);
    }
    this.doc.Stroke(false);
    if (dashArray == null)
      return;
    this.doc.ResetLineDashPattern();
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
    bool flag1 = stroke.IsVisible() && thickness > 0.0;
    bool flag2 = fill.IsVisible();
    if (!flag1 && !flag2)
      return;
    double pageHeight = this.doc.PageHeight;
    this.doc.MoveTo(points[0].X, pageHeight - points[0].Y);
    for (int index = 1; index < points.Count; ++index)
    {
      PortableDocument doc = this.doc;
      ScreenPoint point = points[index];
      double x = point.X;
      double num = pageHeight;
      point = points[index];
      double y = point.Y;
      double y1 = num - y;
      doc.LineTo(x, y1);
    }
    if (flag1)
    {
      this.doc.SetColor(stroke);
      this.SetLineWidth(thickness);
      if (dashArray != null)
        this.SetLineDashPattern(dashArray, 0.0);
      this.doc.SetLineJoin(PdfRenderContext.Convert(lineJoin));
      if (flag2)
      {
        this.doc.SetFillColor(fill);
        this.doc.FillAndStroke();
      }
      else
        this.doc.Stroke();
      if (dashArray == null)
        return;
      this.doc.ResetLineDashPattern();
    }
    else
    {
      this.doc.SetFillColor(fill);
      this.doc.Fill();
    }
  }

  public override void DrawRectangle(
    OxyRect rect,
    OxyColor fill,
    OxyColor stroke,
    double thickness)
  {
    bool flag1 = stroke.IsVisible() && thickness > 0.0;
    bool flag2 = fill.IsVisible();
    if (!flag1 && !flag2)
      return;
    double y = this.doc.PageHeight - rect.Bottom;
    if (flag1)
    {
      this.SetLineWidth(thickness);
      this.doc.SetColor(stroke);
      if (flag2)
      {
        this.doc.SetFillColor(fill);
        this.doc.DrawRectangle(rect.Left, y, rect.Width, rect.Height, true);
      }
      else
        this.doc.DrawRectangle(rect.Left, y, rect.Width, rect.Height);
    }
    else
    {
      this.doc.SetFillColor(fill);
      this.doc.FillRectangle(rect.Left, y, rect.Width, rect.Height);
    }
  }

  public override void DrawText(
    ScreenPoint p,
    string text,
    OxyColor fill,
    string fontFamily,
    double fontSize,
    double fontWeight,
    double rotate,
    HorizontalAlignment halign,
    VerticalAlignment valign,
    OxySize? maxSize)
  {
    this.doc.SaveState();
    this.doc.SetFont(fontFamily, fontSize / 96.0 * 72.0, fontWeight > 500.0);
    this.doc.SetFillColor(fill);
    double width1;
    double height1;
    this.doc.MeasureText(text, out width1, out height1);
    if (maxSize.HasValue)
    {
      double num1 = width1;
      OxySize oxySize = maxSize.Value;
      double width2 = oxySize.Width;
      if (num1 > width2)
      {
        oxySize = maxSize.Value;
        width1 = Math.Max(oxySize.Width, 0.0);
      }
      double num2 = height1;
      oxySize = maxSize.Value;
      double height2 = oxySize.Height;
      if (num2 > height2)
      {
        oxySize = maxSize.Value;
        height1 = Math.Max(oxySize.Height, 0.0);
      }
    }
    double x = 0.0;
    if (halign == HorizontalAlignment.Center)
      x = -width1 / 2.0;
    if (halign == HorizontalAlignment.Right)
      x = -width1;
    double y1 = 0.0;
    if (valign == VerticalAlignment.Middle)
      y1 = -height1 / 2.0;
    if (valign == VerticalAlignment.Top)
      y1 = -height1;
    double y2 = this.doc.PageHeight - p.Y;
    this.doc.Translate(p.X, y2);
    if (Math.Abs(rotate) > 1E-06)
      this.doc.Rotate(-rotate);
    this.doc.Translate(x, y1);
    this.doc.SetClippingRectangle(0.0, 0.0, width1, height1);
    this.doc.DrawText(0.0, 0.0, text);
    this.doc.RestoreState();
  }

  public override OxySize MeasureText(
    string text,
    string fontFamily,
    double fontSize,
    double fontWeight)
  {
    this.doc.SetFont(fontFamily, fontSize / 96.0 * 72.0, fontWeight > 500.0);
    double width;
    double height;
    this.doc.MeasureText(text, out width, out height);
    return new OxySize(width, height);
  }

  public override bool SetClip(OxyRect rect)
  {
    this.doc.SaveState();
    this.doc.SetClippingRectangle(rect.Left, rect.Bottom, rect.Width, rect.Height);
    return true;
  }

  public override void ResetClip() => this.doc.RestoreState();

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
    PortableDocumentImage image;
    if (!this.images.TryGetValue(source, out image))
    {
      image = PortableDocumentImageUtilities.Convert(source, interpolate);
      if (image == null)
        return;
      this.images[source] = image;
    }
    this.doc.SaveState();
    double x = destX - srcX / srcWidth * destWidth;
    double sx = (double) image.Width / srcWidth * destWidth;
    double num = destY - srcY / srcHeight * destHeight;
    double sy = (double) image.Height / srcHeight * destHeight;
    this.doc.SetClippingRectangle(destX, this.doc.PageHeight - (destY - destHeight), destWidth, destHeight);
    this.doc.Translate(x, this.doc.PageHeight - (num + sy));
    this.doc.Scale(sx, sy);
    this.doc.DrawImage(image);
    this.doc.RestoreState();
  }

  private static LineJoin Convert(LineJoin lineJoin)
  {
    if (lineJoin == LineJoin.Miter)
      return LineJoin.Miter;
    return lineJoin == LineJoin.Bevel ? LineJoin.Bevel : LineJoin.Round;
  }

  private void SetLineWidth(double thickness) => this.doc.SetLineWidth(thickness / 96.0 * 72.0);

  private void SetLineDashPattern(double[] dashArray, double dashPhase)
  {
    this.doc.SetLineDashPattern(((IEnumerable<double>) dashArray).Select<double, double>((Func<double, double>) (d => d / 96.0 * 72.0)).ToArray<double>(), dashPhase / 96.0 * 72.0);
  }
}
