// Decompiled with JetBrains decompiler
// Type: OxyPlot.PortableDocument
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

public class PortableDocument
{
  private readonly List<PortableDocument.PortableDocumentObject> objects = new List<PortableDocument.PortableDocumentObject>();
  private readonly Dictionary<double, string> strokeAlphaCache = new Dictionary<double, string>();
  private readonly Dictionary<double, string> fillAlphaCache = new Dictionary<double, string>();
  private readonly Dictionary<PortableDocumentFont, string> fontCache = new Dictionary<PortableDocumentFont, string>();
  private readonly Dictionary<PortableDocumentImage, string> imageCache = new Dictionary<PortableDocumentImage, string>();
  private readonly PortableDocument.PortableDocumentObject catalog;
  private readonly PortableDocument.PortableDocumentObject pages;
  private readonly PortableDocument.PortableDocumentObject metadata;
  private readonly PortableDocument.PortableDocumentObject resources;
  private readonly Dictionary<string, object> fonts;
  private readonly Dictionary<string, object> xobjects;
  private readonly Dictionary<string, object> extgstate;
  private readonly IList<PortableDocument.PortableDocumentObject> pageReferences = (IList<PortableDocument.PortableDocumentObject>) new List<PortableDocument.PortableDocumentObject>();
  private PortableDocument.PortableDocumentObject currentPageContents;
  private PortableDocumentFont currentFont;
  private double currentFontSize;

  public PortableDocument()
  {
    this.metadata = this.AddObject();
    this.metadata["/CreationDate"] = (object) DateTime.Now;
    this.catalog = this.AddObject(PdfWriter.ObjectType.Catalog);
    this.pages = this.AddObject(PdfWriter.ObjectType.Pages);
    this.catalog["/Pages"] = (object) this.pages;
    this.fonts = new Dictionary<string, object>();
    this.xobjects = new Dictionary<string, object>();
    this.extgstate = new Dictionary<string, object>();
    this.resources = this.AddObject();
    this.resources["/ProcSet"] = (object) new string[5]
    {
      "/PDF",
      "/Text",
      "/ImageB",
      "/ImageC",
      "/ImageI"
    };
    this.resources["/Font"] = (object) this.fonts;
    this.resources["/XObject"] = (object) this.xobjects;
    this.resources["/ExtGState"] = (object) this.extgstate;
    this.currentFont = StandardFonts.Helvetica.GetFont(false, false);
    this.currentFontSize = 12.0;
  }

  public double PageWidth { get; private set; }

  public double PageHeight { get; private set; }

  public string Title
  {
    set => this.metadata["/Title"] = (object) PortableDocument.EscapeString(value);
  }

  public string Author
  {
    set => this.metadata["/Author"] = (object) PortableDocument.EscapeString(value);
  }

  public string Subject
  {
    set => this.metadata["/Subject"] = (object) PortableDocument.EscapeString(value);
  }

  public string Keywords
  {
    set => this.metadata["/Keywords"] = (object) PortableDocument.EscapeString(value);
  }

  public string Creator
  {
    set => this.metadata["/Creator"] = (object) PortableDocument.EscapeString(value);
  }

  public string Producer
  {
    set => this.metadata["/Producer"] = (object) PortableDocument.EscapeString(value);
  }

  public void SetLineWidth(double w) => this.AppendLine("{0:0.####} w", (object) w);

  public void SetLineCap(LineCap cap) => this.AppendLine("{0} J", (object) (int) cap);

  public void SetLineJoin(LineJoin lineJoin) => this.AppendLine("{0} j", (object) (int) lineJoin);

  public void SetMiterLimit(double ml) => this.AppendLine("{0:0.####} M", (object) ml);

  public void SetLineDashPattern(double[] dashArray, double dashPhase)
  {
    this.Append("[");
    for (int index = 0; index < dashArray.Length; ++index)
    {
      if (index > 0)
        this.Append(" ");
      this.Append("{0:0.####}", (object) dashArray[index]);
    }
    this.AppendLine("]{0:0.####} d", (object) dashPhase);
  }

  public void ResetLineDashPattern() => this.SetLineDashPattern(new double[0], 0.0);

  public void MoveTo(double x1, double y1)
  {
    this.AppendLine("{0:0.####} {1:0.####} m", (object) x1, (object) y1);
  }

  public void LineTo(double x1, double y1)
  {
    this.AppendLine("{0:0.####} {1:0.####} l", (object) x1, (object) y1);
  }

  public void AppendCubicBezier(
    double x1,
    double y1,
    double x2,
    double y2,
    double x3,
    double y3)
  {
    this.AppendLine("{0:0.####} {1:0.####} {2:0.####} {3:0.####} {4:0.####} {5:0.####} c", (object) x1, (object) y1, (object) x2, (object) y2, (object) x3, (object) y3);
  }

  public void SaveState() => this.AppendLine("q");

  public void RestoreState() => this.AppendLine("Q");

  public void Translate(double x, double y) => this.Transform(1.0, 0.0, 0.0, 1.0, x, y);

  public void Scale(double sx, double sy) => this.Transform(sx, 0.0, 0.0, sy, 0.0, 0.0);

  public void Transform(double a, double b, double c, double d, double e, double f)
  {
    this.AppendLine("{0:0.#####} {1:0.#####} {2:0.#####} {3:0.#####} {4:0.#####} {5:0.#####} cm", (object) a, (object) b, (object) c, (object) d, (object) e, (object) f);
  }

  public void SetHorizontalTextScaling(double scale)
  {
    this.AppendLine("{0:0.#####} Tz", (object) scale);
  }

  public void RotateAt(double x, double y, double angle)
  {
    this.Translate(x, y);
    this.Rotate(angle);
    this.Translate(-x, -y);
  }

  public void Rotate(double angle)
  {
    double num = angle / 180.0 * Math.PI;
    this.Transform(Math.Cos(num), Math.Sin(num), -Math.Sin(num), Math.Cos(num), 0.0, 0.0);
  }

  public void SetStrokeAlpha(double alpha)
  {
    this.AppendLine("{0:0.####} gs", (object) PortableDocument.GetCached<double, string>(alpha, this.strokeAlphaCache, (Func<string>) (() => this.AddExtGState("/CA", (object) alpha))));
  }

  public void SetFillAlpha(double alpha)
  {
    this.AppendLine("{0:0.####} gs", (object) PortableDocument.GetCached<double, string>(alpha, this.fillAlphaCache, (Func<string>) (() => this.AddExtGState("/ca", (object) alpha))));
  }

  public void Stroke(bool close = true) => this.AppendLine(close ? "s" : "S");

  public void Fill(bool evenOddRule = false) => this.AppendLine(evenOddRule ? "f>" : "f");

  public void FillAndStroke(bool close = true, bool evenOddRule = false)
  {
    if (evenOddRule)
      this.AppendLine(close ? "b>" : "B>");
    else
      this.AppendLine(close ? "b" : "B");
  }

  public void SetClippingPath(bool evenOddRule = false)
  {
    this.AppendLine(evenOddRule ? "W>" : "W");
  }

  public void EndPath() => this.AppendLine("n");

  public void CloseSubPath() => this.AppendLine("h");

  public void AppendRectangle(double x, double y, double w, double h)
  {
    this.AppendLine("{0:0.####} {1:0.####} {2:0.####} {3:0.####} re", (object) x, (object) y, (object) w, (object) h);
  }

  public void DrawLine(double x1, double y1, double x2, double y2)
  {
    this.AppendLine("{0:0.####} {1:0.####} m {2:0.####} {3:0.####} l S", (object) x1, (object) y1, (object) x2, (object) y2);
  }

  public void DrawRectangle(double x, double y, double w, double h, bool fill = false)
  {
    this.AppendLine("{0:0.####} {1:0.####} {2:0.####} {3:0.####} re {4}", (object) x, (object) y, (object) w, (object) h, fill ? (object) "B" : (object) "S");
  }

  public void SetClippingRectangle(double x, double y, double w, double h, bool evenOddRule = false)
  {
  }

  public void FillRectangle(double x, double y, double w, double h)
  {
    this.AppendLine("{0:0.####} {1:0.####} {2:0.####} {3:0.####} re f", (object) x, (object) y, (object) w, (object) h);
  }

  public void DrawCircle(double x, double y, double r, bool fill = false)
  {
    this.DrawEllipse(x - r, y - r, r * 2.0, r * 2.0, fill);
  }

  public void FillCircle(double x, double y, double r)
  {
    this.FillEllipse(x - r, y - r, r * 2.0, r * 2.0);
  }

  public void DrawEllipse(double x, double y, double w, double h, bool fill = false)
  {
    this.AppendEllipse(x, y, w, h);
    if (!fill)
      this.Stroke();
    else
      this.FillAndStroke();
  }

  public void FillEllipse(double x, double y, double w, double h)
  {
    this.AppendEllipse(x, y, w, h);
    this.Fill();
  }

  public void AppendEllipse(double x, double y, double w, double h)
  {
    double num1 = w * 0.5 * 0.5522848;
    double num2 = h * 0.5 * 0.5522848;
    double num3 = x + w;
    double num4 = y + h;
    double x3 = x + w * 0.5;
    double num5 = y + h * 0.5;
    this.MoveTo(x, num5);
    this.AppendCubicBezier(x, num5 - num2, x3 - num1, y, x3, y);
    this.AppendCubicBezier(x3 + num1, y, num3, num5 - num2, num3, num5);
    this.AppendCubicBezier(num3, num5 + num2, x3 + num1, num4, x3, num4);
    this.AppendCubicBezier(x3 - num1, num4, x, num5 + num2, x, num5);
  }

  public void SetFont(string fontName, double fontSize, bool bold = false, bool italic = false)
  {
    this.currentFont = PortableDocument.GetFont(fontName, bold, italic);
    this.currentFontSize = fontSize;
  }

  public void DrawText(double x, double y, string text)
  {
    if (string.IsNullOrEmpty(text))
      return;
    string cached = PortableDocument.GetCached<PortableDocumentFont, string>(this.currentFont, this.fontCache, (Func<string>) (() => this.AddFont(this.currentFont)));
    this.AppendLine("BT");
    this.AppendLine("{0} {1:0.####} Tf", (object) cached, (object) this.currentFontSize);
    text = PortableDocument.EncodeString(text, this.currentFont.Encoding);
    text = PortableDocument.EscapeString(text);
    y -= (double) this.currentFont.Descent * this.currentFontSize / 1000.0;
    this.AppendLine("{0:0.####} {1:0.####} Td", (object) x, (object) y);
    this.AppendLine("{0} Tj", (object) text);
    this.AppendLine("ET");
  }

  public void MeasureText(string text, out double width, out double height)
  {
    if (string.IsNullOrEmpty(text))
      width = height = 0.0;
    else
      this.currentFont.Measure(text, this.currentFontSize, out width, out height);
  }

  public void DrawImage(PortableDocumentImage image)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    this.AppendLine("{0} Do", (object) PortableDocument.GetCached<PortableDocumentImage, string>(image, this.imageCache, (Func<string>) (() => this.AddImage(image))));
  }

  public void SetColor(double r, double g, double b)
  {
    this.AppendLine("{0:0.####} {1:0.####} {2:0.####} RG", (object) r, (object) g, (object) b);
  }

  public void SetColor(double c, double m, double y, double k)
  {
    this.AppendLine("{0:0.####} {1:0.####} {2:0.####} {3:0.####} K", (object) c, (object) m, (object) y, (object) k);
  }

  public void SetFillColor(double r, double g, double b)
  {
    this.AppendLine("{0:0.####} {1:0.####} {2:0.####} rg", (object) r, (object) g, (object) b);
  }

  public void AddPage(PageSize pageSize, PageOrientation pageOrientation = PageOrientation.Portrait)
  {
    double num1 = double.NaN;
    double num2 = double.NaN;
    switch (pageSize)
    {
      case PageSize.A4:
        num1 = 595.0;
        num2 = 842.0;
        break;
      case PageSize.A3:
        num1 = 842.0;
        num2 = 1190.0;
        break;
      case PageSize.Letter:
        num1 = 612.0;
        num2 = 792.0;
        break;
    }
    if (pageOrientation == PageOrientation.Portrait)
      this.AddPage(num1, num2);
    else
      this.AddPage(num2, num1);
  }

  public void AddPage(double width = 595.0, double height = 842.0)
  {
    this.PageWidth = width;
    this.PageHeight = height;
    this.currentPageContents = this.AddObject();
    PortableDocument.PortableDocumentObject portableDocumentObject = this.AddObject(PdfWriter.ObjectType.Page);
    portableDocumentObject["/Parent"] = (object) this.pages;
    portableDocumentObject["/MediaBox"] = (object) new double[4]
    {
      0.0,
      0.0,
      width,
      height
    };
    portableDocumentObject["/Contents"] = (object) this.currentPageContents;
    portableDocumentObject["/Resources"] = (object) this.resources;
    this.pageReferences.Add(portableDocumentObject);
  }

  public void Save(Stream s)
  {
    using (PdfWriter w = new PdfWriter(s))
    {
      this.pages["/Count"] = (object) this.pageReferences.Count;
      this.pages["/Kids"] = (object) this.pageReferences;
      w.WriteLine("%PDF-1.3");
      Dictionary<PortableDocument.PortableDocumentObject, long> dictionary1 = new Dictionary<PortableDocument.PortableDocumentObject, long>();
      foreach (PortableDocument.PortableDocumentObject key in this.objects)
      {
        dictionary1.Add(key, w.Position);
        key.Write(w);
      }
      long position = w.Position;
      w.WriteLine("xref");
      w.WriteLine("0 {0}", (object) (this.objects.Count + 1));
      w.WriteLine("0000000000 65535 f ");
      foreach (PortableDocument.PortableDocumentObject key in this.objects)
        w.WriteLine("{0:0000000000} 00000 n ", (object) dictionary1[key]);
      w.WriteLine("trailer");
      Dictionary<string, object> dictionary2 = new Dictionary<string, object>()
      {
        {
          "/Size",
          (object) (this.objects.Count + 1)
        },
        {
          "/Root",
          (object) this.catalog
        },
        {
          "/Info",
          (object) this.metadata
        }
      };
      w.Write(dictionary2);
      w.WriteLine();
      w.WriteLine("startxref");
      w.WriteLine("{0}", (object) position);
      w.Write("%%EOF");
    }
  }

  private static string EncodeString(string text, FontEncoding encoding) => text;

  private static string EscapeString(string text)
  {
    text = text.Replace("\\", "\\\\");
    text = text.Replace("(", "\\(");
    text = text.Replace(")", "\\)");
    return $"({text})";
  }

  private static string Ascii85Encode(byte[] ba)
  {
    byte[] encodedBlock = new byte[5];
    StringBuilder sb = new StringBuilder(ba.Length * 5 / 4);
    Action<int, uint> action = (Action<int, uint>) ((length, t) =>
    {
      for (int index = encodedBlock.Length - 1; index >= 0; --index)
      {
        encodedBlock[index] = (byte) (t % 85U + 33U);
        t /= 85U;
      }
      for (int index = 0; index < length; ++index)
        sb.Append((char) encodedBlock[index]);
    });
    uint num1 = 0;
    int num2 = 0;
    foreach (byte num3 in ba)
    {
      if (num2 >= 3)
      {
        uint num4 = num1 | (uint) num3;
        if (num4 == 0U)
          sb.Append('z');
        else
          action(encodedBlock.Length, num4);
        num1 = 0U;
        num2 = 0;
      }
      else
      {
        num1 |= (uint) num3 << 24 - num2 * 8;
        ++num2;
      }
    }
    if (num2 > 0)
      action(num2 + 1, num1);
    sb.Append("~>");
    return sb.ToString();
  }

  private static PortableDocumentFont GetFont(string fontName, bool bold, bool italic)
  {
    if (fontName != null)
      fontName = fontName.ToLower();
    if (fontName == "arial" || fontName == "helvetica")
      return StandardFonts.Helvetica.GetFont(bold, italic);
    if (fontName == "times" || fontName == "times new roman")
      return StandardFonts.Times.GetFont(bold, italic);
    return fontName == "courier" || fontName == "courier new" ? StandardFonts.Courier.GetFont(bold, italic) : StandardFonts.Helvetica.GetFont(bold, italic);
  }

  private static T2 GetCached<T1, T2>(T1 key, Dictionary<T1, T2> cache, Func<T2> create)
  {
    T2 cached1;
    if (cache.TryGetValue(key, out cached1))
      return cached1;
    T2 cached2 = create();
    cache[key] = cached2;
    return cached2;
  }

  private PortableDocument.PortableDocumentObject AddObject()
  {
    PortableDocument.PortableDocumentObject portableDocumentObject = new PortableDocument.PortableDocumentObject(this.objects.Count + 1);
    this.objects.Add(portableDocumentObject);
    return portableDocumentObject;
  }

  private PortableDocument.PortableDocumentObject AddObject(PdfWriter.ObjectType type)
  {
    PortableDocument.PortableDocumentObject portableDocumentObject = this.AddObject();
    portableDocumentObject["/Type"] = (object) type;
    return portableDocumentObject;
  }

  private string AddExtGState(string key, object value)
  {
    PortableDocument.PortableDocumentObject portableDocumentObject = this.AddObject(PdfWriter.ObjectType.ExtGState);
    portableDocumentObject[key] = value;
    string key1 = "/GS" + (object) this.extgstate.Count;
    this.extgstate.Add(key1, (object) portableDocumentObject);
    return key1;
  }

  private string AddImage(PortableDocumentImage image)
  {
    int num = this.xobjects.Count + 1;
    PortableDocument.PortableDocumentObject portableDocumentObject1 = this.AddObject(PdfWriter.ObjectType.XObject);
    portableDocumentObject1["/Subtype"] = (object) "/Image";
    portableDocumentObject1["/Width"] = (object) image.Width;
    portableDocumentObject1["/Height"] = (object) image.Height;
    portableDocumentObject1["/ColorSpace"] = (object) ("/" + (object) image.ColorSpace);
    portableDocumentObject1["/Interpolate"] = (object) image.Interpolate;
    portableDocumentObject1["/BitsPerComponent"] = (object) image.BitsPerComponent;
    string format1 = PortableDocument.Ascii85Encode(image.Bits);
    portableDocumentObject1["/Length"] = (object) format1.Length;
    portableDocumentObject1["/Filter"] = (object) "/ASCII85Decode";
    portableDocumentObject1.Append(format1);
    string key = "/Image" + (object) num;
    this.xobjects.Add(key, (object) portableDocumentObject1);
    if (image.MaskBits != null)
    {
      PortableDocument.PortableDocumentObject portableDocumentObject2 = this.AddObject(PdfWriter.ObjectType.XObject);
      portableDocumentObject2["/Subtype"] = (object) "/Image";
      portableDocumentObject2["/Width"] = (object) image.Width;
      portableDocumentObject2["/Height"] = (object) image.Height;
      portableDocumentObject2["/ColorSpace"] = (object) "/DeviceGray";
      portableDocumentObject2["/Interpolate"] = (object) image.Interpolate;
      portableDocumentObject2["/BitsPerComponent"] = (object) image.BitsPerComponent;
      string format2 = PortableDocument.Ascii85Encode(image.MaskBits);
      portableDocumentObject2["/Length"] = (object) format2.Length;
      portableDocumentObject2["/Filter"] = (object) "/ASCII85Decode";
      portableDocumentObject2.Append(format2);
      portableDocumentObject1["/SMask"] = (object) portableDocumentObject2;
    }
    return key;
  }

  private string AddFont(PortableDocumentFont font)
  {
    PortableDocument.PortableDocumentObject portableDocumentObject1 = (PortableDocument.PortableDocumentObject) null;
    if (font.SubType != FontSubType.Type1)
    {
      portableDocumentObject1 = this.AddObject(PdfWriter.ObjectType.FontDescriptor);
      portableDocumentObject1["/Ascent"] = (object) font.Ascent;
      portableDocumentObject1["/CapHeight"] = (object) font.CapHeight;
      portableDocumentObject1["/Descent"] = (object) font.Descent;
      portableDocumentObject1["/Flags"] = (object) font.Flags;
      portableDocumentObject1["/FontBBox"] = (object) font.FontBoundingBox;
      portableDocumentObject1["/ItalicAngle"] = (object) font.ItalicAngle;
      portableDocumentObject1["/StemV"] = (object) font.StemV;
      portableDocumentObject1["/XHeight"] = (object) font.XHeight;
      portableDocumentObject1["/FontName"] = (object) ("/" + font.FontName);
    }
    PortableDocument.PortableDocumentObject portableDocumentObject2 = this.AddObject(PdfWriter.ObjectType.Font);
    portableDocumentObject2["/Subtype"] = (object) ("/" + (object) font.SubType);
    portableDocumentObject2["/Encoding"] = (object) ("/" + (object) font.Encoding);
    portableDocumentObject2["/BaseFont"] = (object) ("/" + font.BaseFont);
    if (portableDocumentObject1 != null)
      portableDocumentObject2["/FontDescriptor"] = (object) portableDocumentObject1;
    if (font.SubType != FontSubType.Type1)
    {
      portableDocumentObject2["/FirstChar"] = (object) font.FirstChar;
      portableDocumentObject2["/LastChar"] = (object) (font.FirstChar + font.Widths.Length - 1);
      portableDocumentObject2["/Widths"] = (object) font.Widths;
    }
    string key = "/F" + (object) (this.fonts.Count + 1);
    this.fonts.Add(key, (object) portableDocumentObject2);
    return key;
  }

  private void AppendLine(string format, params object[] args)
  {
    if (this.currentPageContents == null)
      throw new InvalidOperationException("Cannot add content before a page has been added.");
    this.currentPageContents.AppendLine(format, args);
  }

  private void Append(string format, params object[] args)
  {
    if (this.currentPageContents == null)
      throw new InvalidOperationException("Cannot add content before a page has been added.");
    this.currentPageContents.Append(format, args);
  }

  internal class PortableDocumentObject : PdfWriter.IPortableDocumentObject
  {
    private readonly Dictionary<string, object> dictionary;
    private readonly int objectNumber;
    private readonly StringBuilder contents;

    public PortableDocumentObject(int objectNumber)
    {
      this.objectNumber = objectNumber;
      this.contents = new StringBuilder();
      this.dictionary = new Dictionary<string, object>();
    }

    public int ObjectNumber => this.objectNumber;

    public object this[string key]
    {
      set => this.dictionary[key] = value;
    }

    public void Append(string format, params object[] args)
    {
      this.contents.Append(string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, args));
    }

    public void AppendLine(string format, params object[] args)
    {
      this.contents.AppendLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, args));
    }

    public void Write(PdfWriter w)
    {
      w.WriteLine("{0} 0 obj", (object) this.ObjectNumber);
      byte[] bytes = (byte[]) null;
      if (this.contents != null && this.contents.Length > 0)
      {
        string str = this.contents.ToString().Trim();
        bytes = new byte[str.Length];
        for (int index = 0; index < str.Length; ++index)
          bytes[index] = (byte) str[index];
        this.dictionary["/Length"] = (object) bytes.Length;
      }
      w.Write(this.dictionary);
      w.WriteLine();
      if (bytes != null)
      {
        w.WriteLine("stream");
        w.Write(bytes);
        w.WriteLine();
        w.WriteLine("endstream");
      }
      w.WriteLine("endobj");
    }
  }
}
