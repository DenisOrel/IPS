// Decompiled with JetBrains decompiler
// Type: OxyPlot.WindowsForms.GraphicsRenderContext
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;

#nullable disable
namespace OxyPlot.WindowsForms;

public class GraphicsRenderContext : RenderContextBase, IDisposable
{
  private const float FontsizeFactor = 0.8f;
  private readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();
  private readonly Dictionary<OxyImage, Image> imageCache = new Dictionary<OxyImage, Image>();
  private readonly Dictionary<OxyColor, Brush> brushes = new Dictionary<OxyColor, Brush>();
  private readonly StringFormat stringFormat;
  private Graphics g;

  public GraphicsRenderContext(Graphics graphics = null)
  {
    this.g = graphics;
    if (this.g != null)
      this.g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
    this.stringFormat = StringFormat.GenericTypographic;
  }

  public void SetGraphicsTarget(Graphics graphics)
  {
    this.g = graphics;
    this.g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
  }

  public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
  {
    bool flag = stroke.IsVisible() && thickness > 0.0;
    if (fill.IsVisible())
    {
      if (!flag)
        this.g.SmoothingMode = SmoothingMode.HighQuality;
      this.g.FillEllipse(this.GetCachedBrush(fill), (float) rect.Left, (float) rect.Top, (float) rect.Width, (float) rect.Height);
    }
    if (!flag)
      return;
    using (Pen pen = this.CreatePen(stroke, thickness))
    {
      this.g.SmoothingMode = SmoothingMode.HighQuality;
      this.g.DrawEllipse(pen, (float) rect.Left, (float) rect.Top, (float) rect.Width, (float) rect.Height);
    }
  }

  public override void DrawLine(
    IList<ScreenPoint> points,
    OxyColor stroke,
    double thickness,
    double[] dashArray,
    OxyPlot.LineJoin lineJoin,
    bool aliased)
  {
    if (stroke.IsInvisible() || thickness <= 0.0 || points.Count < 2)
      return;
    this.g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;
    using (Pen pen = this.CreatePen(stroke, thickness, dashArray, lineJoin))
      this.g.DrawLines(pen, this.ToPoints(points));
  }

  public override void DrawPolygon(
    IList<ScreenPoint> points,
    OxyColor fill,
    OxyColor stroke,
    double thickness,
    double[] dashArray,
    OxyPlot.LineJoin lineJoin,
    bool aliased)
  {
    if (points.Count < 2)
      return;
    this.g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;
    PointF[] points1 = this.ToPoints(points);
    if (fill.IsVisible())
      this.g.FillPolygon(fill.ToBrush(), points1);
    if (stroke.IsInvisible() || thickness <= 0.0)
      return;
    using (Pen pen = this.CreatePen(stroke, thickness))
    {
      if (dashArray != null)
        pen.DashPattern = this.ToFloatArray(dashArray);
      switch (lineJoin)
      {
        case OxyPlot.LineJoin.Round:
          pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
          break;
        case OxyPlot.LineJoin.Bevel:
          pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
          break;
      }
      this.g.DrawPolygon(pen, points1);
    }
  }

  public override void DrawRectangle(
    OxyRect rect,
    OxyColor fill,
    OxyColor stroke,
    double thickness)
  {
    if (fill.IsVisible())
      this.g.FillRectangle(fill.ToBrush(), (float) rect.Left, (float) rect.Top, (float) rect.Width, (float) rect.Height);
    if (stroke.IsInvisible() || thickness <= 0.0)
      return;
    using (Pen pen = this.CreatePen(stroke, thickness))
      this.g.DrawRectangle(pen, (float) rect.Left, (float) rect.Top, (float) rect.Width, (float) rect.Height);
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
    if (text == null)
      return;
    FontStyle fontStyle = fontWeight < 700.0 ? FontStyle.Regular : FontStyle.Bold;
    using (Font font = GraphicsRenderContext.CreateFont(fontFamily, fontSize, fontStyle))
    {
      this.stringFormat.Alignment = StringAlignment.Near;
      this.stringFormat.LineAlignment = StringAlignment.Near;
      SizeF sizeF = this.g.MeasureString(text, font, int.MaxValue, this.stringFormat);
      if (maxSize.HasValue)
      {
        double width1 = (double) sizeF.Width;
        OxySize oxySize = maxSize.Value;
        double width2 = oxySize.Width;
        if (width1 > width2)
        {
          ref SizeF local = ref sizeF;
          oxySize = maxSize.Value;
          double width3 = oxySize.Width;
          local.Width = (float) width3;
        }
        double height1 = (double) sizeF.Height;
        oxySize = maxSize.Value;
        double height2 = oxySize.Height;
        if (height1 > height2)
        {
          ref SizeF local = ref sizeF;
          oxySize = maxSize.Value;
          double height3 = oxySize.Height;
          local.Height = (float) height3;
        }
      }
      float dx = 0.0f;
      if (halign == HorizontalAlignment.Center)
        dx = (float) (-(double) sizeF.Width / 2.0);
      if (halign == HorizontalAlignment.Right)
        dx = -sizeF.Width;
      float dy = 0.0f;
      this.stringFormat.LineAlignment = StringAlignment.Near;
      if (valign == VerticalAlignment.Middle)
        dy = (float) (-(double) sizeF.Height / 2.0);
      if (valign == VerticalAlignment.Bottom)
        dy = -sizeF.Height;
      GraphicsState gstate = this.g.Save();
      this.g.TranslateTransform((float) p.X, (float) p.Y);
      if (Math.Abs(rotate) > double.Epsilon)
        this.g.RotateTransform((float) rotate);
      this.g.TranslateTransform(dx, dy);
      RectangleF layoutRectangle = new RectangleF(0.0f, 0.0f, sizeF.Width + 0.1f, sizeF.Height + 0.1f);
      this.g.DrawString(text, font, fill.ToBrush(), layoutRectangle, this.stringFormat);
      this.g.Restore(gstate);
    }
  }

  public override OxySize MeasureText(
    string text,
    string fontFamily,
    double fontSize,
    double fontWeight)
  {
    if (text == null)
      return OxySize.Empty;
    FontStyle fontStyle = fontWeight < 700.0 ? FontStyle.Regular : FontStyle.Bold;
    using (Font font = GraphicsRenderContext.CreateFont(fontFamily, fontSize, fontStyle))
    {
      this.stringFormat.Alignment = StringAlignment.Near;
      this.stringFormat.LineAlignment = StringAlignment.Near;
      SizeF sizeF = this.g.MeasureString(text, font, int.MaxValue, this.stringFormat);
      return new OxySize((double) sizeF.Width, (double) sizeF.Height);
    }
  }

  public override void CleanUp()
  {
    foreach (OxyImage oxyImage in this.imageCache.Keys.Where<OxyImage>((Func<OxyImage, bool>) (i => !this.imagesInUse.Contains(i))).ToList<OxyImage>())
    {
      this.GetImage(oxyImage).Dispose();
      this.imageCache.Remove(oxyImage);
    }
    this.imagesInUse.Clear();
  }

  public override void DrawImage(
    OxyImage source,
    double srcX,
    double srcY,
    double srcWidth,
    double srcHeight,
    double x,
    double y,
    double w,
    double h,
    double opacity,
    bool interpolate)
  {
    Image image = this.GetImage(source);
    if (image == null)
      return;
    ImageAttributes imageAttrs = (ImageAttributes) null;
    if (opacity < 1.0)
    {
      ColorMatrix newColorMatrix = new ColorMatrix()
      {
        Matrix00 = 1f,
        Matrix11 = 1f,
        Matrix22 = 1f,
        Matrix33 = 1f,
        Matrix44 = (float) opacity
      };
      imageAttrs = new ImageAttributes();
      imageAttrs.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
    }
    this.g.InterpolationMode = interpolate ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
    int x1 = (int) Math.Floor(x);
    int y1 = (int) Math.Floor(y);
    int width = (int) Math.Ceiling(x + w) - x1;
    int height = (int) Math.Ceiling(y + h) - y1;
    Rectangle destRect = new Rectangle(x1, y1, width, height);
    this.g.DrawImage(image, destRect, (float) srcX - 0.5f, (float) srcY - 0.5f, (float) srcWidth, (float) srcHeight, GraphicsUnit.Pixel, imageAttrs);
  }

  public override bool SetClip(OxyRect rect)
  {
    this.g.SetClip(rect.ToRect(false));
    return true;
  }

  public override void ResetClip() => this.g.ResetClip();

  public void Dispose()
  {
    foreach (KeyValuePair<OxyImage, Image> keyValuePair in this.imageCache)
      keyValuePair.Value.Dispose();
    this.stringFormat.Dispose();
    foreach (Brush brush in this.brushes.Values)
      brush.Dispose();
  }

  private static Font CreateFont(string fontFamily, double fontSize, FontStyle fontStyle)
  {
    return new Font(fontFamily, (float) fontSize * 0.8f, fontStyle);
  }

  private Image GetImage(OxyImage source)
  {
    if (source == null)
      return (Image) null;
    if (!this.imagesInUse.Contains(source))
      this.imagesInUse.Add(source);
    Image image1;
    if (this.imageCache.TryGetValue(source, out image1))
      return image1;
    Image image2;
    using (MemoryStream memoryStream = new MemoryStream(source.GetData()))
      image2 = Image.FromStream((Stream) memoryStream);
    this.imageCache.Add(source, image2);
    return image2;
  }

  private Brush GetCachedBrush(OxyColor fill)
  {
    Brush brush;
    return this.brushes.TryGetValue(fill, out brush) ? brush : (this.brushes[fill] = fill.ToBrush());
  }

  private Pen CreatePen(OxyColor stroke, double thickness, double[] dashArray = null, OxyPlot.LineJoin lineJoin = OxyPlot.LineJoin.Miter)
  {
    Pen pen = new Pen(stroke.ToColor(), (float) thickness);
    if (dashArray != null)
      pen.DashPattern = this.ToFloatArray(dashArray);
    switch (lineJoin)
    {
      case OxyPlot.LineJoin.Round:
        pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
        break;
      case OxyPlot.LineJoin.Bevel:
        pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
        break;
    }
    return pen;
  }

  private float[] ToFloatArray(double[] a)
  {
    if (a == null)
      return (float[]) null;
    float[] floatArray = new float[a.Length];
    for (int index = 0; index < a.Length; ++index)
      floatArray[index] = (float) a[index];
    return floatArray;
  }

  private PointF[] ToPoints(IList<ScreenPoint> points)
  {
    if (points == null)
      return (PointF[]) null;
    PointF[] points1 = new PointF[points.Count<ScreenPoint>()];
    int num = 0;
    foreach (ScreenPoint point in (IEnumerable<ScreenPoint>) points)
      points1[num++] = new PointF((float) point.X, (float) point.Y);
    return points1;
  }
}
