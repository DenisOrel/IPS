// Decompiled with JetBrains decompiler
// Type: OxyPlot.SvgWriter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace OxyPlot;

public class SvgWriter : XmlWriterBase
{
  private bool endIsWritten;
  private string clipPath;
  private int clipPathNumber = 1;

  public SvgWriter(Stream stream, double width, double height, bool isDocument = true)
    : base(stream)
  {
    this.IsDocument = isDocument;
    this.NumberFormat = "0.####";
    this.WriteHeader(width, height);
  }

  public bool IsDocument { get; set; }

  public string NumberFormat { get; set; }

  public override void Close()
  {
    if (!this.endIsWritten)
      this.Complete();
    base.Close();
  }

  public void Complete()
  {
    this.WriteEndElement();
    if (this.IsDocument)
      this.WriteEndDocument();
    this.endIsWritten = true;
  }

  public string CreateStyle(
    OxyColor fill,
    OxyColor stroke,
    double thickness,
    double[] dashArray = null,
    LineJoin lineJoin = LineJoin.Miter)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (fill.IsInvisible())
    {
      stringBuilder.AppendFormat("fill:none;");
    }
    else
    {
      stringBuilder.AppendFormat("fill:{0};", (object) this.ColorToString(fill));
      if (fill.A != byte.MaxValue)
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "fill-opacity:{0};", (object) ((double) fill.A / (double) byte.MaxValue));
    }
    if (stroke.IsInvisible())
    {
      stringBuilder.AppendFormat("stroke:none;");
    }
    else
    {
      string format = $"stroke:{{0}};stroke-width:{{1:{this.NumberFormat}}}";
      stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, format, (object) this.ColorToString(stroke), (object) thickness);
      switch (lineJoin)
      {
        case LineJoin.Round:
          stringBuilder.AppendFormat(";stroke-linejoin:round");
          break;
        case LineJoin.Bevel:
          stringBuilder.AppendFormat(";stroke-linejoin:bevel");
          break;
      }
      if (stroke.A != byte.MaxValue)
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, ";stroke-opacity:{0}", (object) ((double) stroke.A / (double) byte.MaxValue));
      if (dashArray != null && dashArray.Length != 0)
      {
        stringBuilder.Append(";stroke-dasharray:");
        for (int index = 0; index < dashArray.Length; ++index)
          stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}{1}", index > 0 ? (object) "," : (object) string.Empty, (object) dashArray[index]);
      }
    }
    return stringBuilder.ToString();
  }

  public void WriteEllipse(double x, double y, double width, double height, string style)
  {
    this.WriteStartElement("ellipse");
    this.WriteAttributeString("cx", x + width / 2.0);
    this.WriteAttributeString("cy", y + height / 2.0);
    this.WriteAttributeString("rx", width / 2.0);
    this.WriteAttributeString("ry", height / 2.0);
    this.WriteAttributeString(nameof (style), style);
    this.WriteClipPathAttribute();
    this.WriteEndElement();
  }

  public void BeginClip(double x, double y, double width, double height)
  {
    this.clipPath = "clipPath" + (object) this.clipPathNumber++;
    this.WriteStartElement("g");
    this.WriteAttributeString("clip-rule", "nonzero");
    this.WriteStartElement("clipPath");
    this.WriteAttributeString("id", this.clipPath);
    this.WriteStartElement("rect");
    this.WriteAttributeString(nameof (x), x);
    this.WriteAttributeString(nameof (y), y);
    this.WriteAttributeString(nameof (width), width);
    this.WriteAttributeString(nameof (height), height);
    this.WriteEndElement();
    this.WriteEndElement();
  }

  public void EndClip()
  {
    this.WriteEndElement();
    this.clipPath = (string) null;
  }

  public void WriteImage(
    double srcX,
    double srcY,
    double srcWidth,
    double srcHeight,
    double destX,
    double destY,
    double destWidth,
    double destHeight,
    OxyImage image)
  {
    double x = destX - srcX / srcWidth * destWidth;
    double width = (double) image.Width / srcWidth * destWidth;
    double y = destY - srcY / srcHeight * destHeight;
    double height = (double) image.Height / srcHeight * destHeight;
    this.BeginClip(destX, destY, destWidth, destHeight);
    this.WriteImage(x, y, width, height, image);
    this.EndClip();
  }

  public void WriteImage(double x, double y, double width, double height, OxyImage image)
  {
    this.WriteStartElement(nameof (image));
    this.WriteAttributeString(nameof (x), x);
    this.WriteAttributeString(nameof (y), y);
    this.WriteAttributeString(nameof (width), width);
    this.WriteAttributeString(nameof (height), height);
    this.WriteAttributeString("preserveAspectRatio", "none");
    byte[] data = image.GetData();
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("data:");
    stringBuilder.Append("image/png");
    stringBuilder.Append(";base64,");
    stringBuilder.Append(Convert.ToBase64String(data));
    this.WriteAttributeString("xlink", "href", (string) null, stringBuilder.ToString());
    this.WriteClipPathAttribute();
    this.WriteEndElement();
  }

  public void WriteLine(ScreenPoint p1, ScreenPoint p2, string style)
  {
    this.WriteStartElement("line");
    this.WriteAttributeString("x1", p1.X);
    this.WriteAttributeString("y1", p1.Y);
    this.WriteAttributeString("x2", p2.X);
    this.WriteAttributeString("y2", p2.Y);
    this.WriteAttributeString(nameof (style), style);
    this.WriteClipPathAttribute();
    this.WriteEndElement();
  }

  public void WritePolygon(IEnumerable<ScreenPoint> points, string style)
  {
    this.WriteStartElement("polygon");
    this.WriteAttributeString(nameof (points), this.PointsToString(points));
    this.WriteAttributeString(nameof (style), style);
    this.WriteClipPathAttribute();
    this.WriteEndElement();
  }

  public void WritePolyline(IEnumerable<ScreenPoint> pts, string style)
  {
    this.WriteStartElement("polyline");
    this.WriteAttributeString("points", this.PointsToString(pts));
    this.WriteAttributeString(nameof (style), style);
    this.WriteClipPathAttribute();
    this.WriteEndElement();
  }

  public void WriteRectangle(double x, double y, double width, double height, string style)
  {
    this.WriteStartElement("rect");
    this.WriteAttributeString(nameof (x), x);
    this.WriteAttributeString(nameof (y), y);
    this.WriteAttributeString(nameof (width), width);
    this.WriteAttributeString(nameof (height), height);
    this.WriteAttributeString(nameof (style), style);
    this.WriteClipPathAttribute();
    this.WriteEndElement();
  }

  public void WriteText(
    ScreenPoint position,
    string text,
    OxyColor fill,
    string fontFamily = null,
    double fontSize = 10.0,
    double fontWeight = 400.0,
    double rotate = 0.0,
    HorizontalAlignment halign = HorizontalAlignment.Left,
    VerticalAlignment valign = VerticalAlignment.Top)
  {
    this.WriteStartElement(nameof (text));
    string str1 = "hanging";
    if (valign == VerticalAlignment.Middle)
      str1 = "middle";
    if (valign == VerticalAlignment.Bottom)
      str1 = "baseline";
    this.WriteAttributeString("dominant-baseline", str1);
    string str2 = "start";
    if (halign == HorizontalAlignment.Center)
      str2 = "middle";
    if (halign == HorizontalAlignment.Right)
      str2 = "end";
    this.WriteAttributeString("text-anchor", str2);
    string str3 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, $"translate({{0:{this.NumberFormat}}},{{1:{this.NumberFormat}}})", (object) position.X, (object) position.Y);
    if (Math.Abs(rotate) > 0.0)
      str3 += string.Format((IFormatProvider) CultureInfo.InvariantCulture, " rotate({0})", (object) rotate);
    this.WriteAttributeString("transform", str3);
    if (fontFamily != null)
      this.WriteAttributeString("font-family", fontFamily);
    if (fontSize > 0.0)
      this.WriteAttributeString("font-size", fontSize);
    if (fontWeight > 0.0)
      this.WriteAttributeString("font-weight", fontWeight);
    this.WriteAttributeString(nameof (fill), this.ColorToString(fill));
    this.WriteClipPathAttribute();
    this.WriteString(text);
    this.WriteEndElement();
  }

  protected string ColorToString(OxyColor color)
  {
    if (color.Equals(OxyColors.Black))
      return "black";
    return string.Format($"rgb({{0:{this.NumberFormat}}},{{1:{this.NumberFormat}}},{{2:{this.NumberFormat}}})", (object) color.R, (object) color.G, (object) color.B);
  }

  protected void WriteAttributeString(string name, double value)
  {
    this.WriteAttributeString(name, value.ToString(this.NumberFormat, (IFormatProvider) CultureInfo.InvariantCulture));
  }

  private void WriteClipPathAttribute()
  {
    if (this.clipPath == null)
      return;
    this.WriteAttributeString("clip-path", $"url(#{this.clipPath})");
  }

  private string GetAutoValue(double value, string auto)
  {
    return double.IsNaN(value) ? auto : value.ToString(this.NumberFormat, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  private string PointsToString(IEnumerable<ScreenPoint> points)
  {
    StringBuilder stringBuilder = new StringBuilder();
    string format = $"{{0:{this.NumberFormat}}},{{1:{this.NumberFormat}}} ";
    foreach (ScreenPoint point in points)
      stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, format, (object) point.X, (object) point.Y);
    return stringBuilder.ToString().Trim();
  }

  private void WriteHeader(double width, double height)
  {
    if (this.IsDocument)
    {
      this.WriteStartDocument(false);
      this.WriteDocType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", (string) null);
    }
    this.WriteStartElement("svg", "http://www.w3.org/2000/svg");
    this.WriteAttributeString(nameof (width), this.GetAutoValue(width, "100%"));
    this.WriteAttributeString(nameof (height), this.GetAutoValue(height, "100%"));
    this.WriteAttributeString("version", "1.1");
    this.WriteAttributeString("xmlns", "xlink", (string) null, "http://www.w3.org/1999/xlink");
  }
}
