// Decompiled with JetBrains decompiler
// Type: OxyPlot.PortableDocumentFont
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class PortableDocumentFont
{
  public PortableDocumentFont()
  {
    this.FirstChar = 0;
    this.Encoding = FontEncoding.WinAnsiEncoding;
  }

  public FontSubType SubType { get; set; }

  public string BaseFont { get; set; }

  public FontEncoding Encoding { get; set; }

  public int FirstChar { get; set; }

  public int[] Widths { get; set; }

  public int Ascent { get; set; }

  public int CapHeight { get; set; }

  public int Descent { get; set; }

  public int Flags { get; set; }

  public int[] FontBoundingBox { get; set; }

  public int ItalicAngle { get; set; }

  public int StemV { get; set; }

  public int XHeight { get; set; }

  public string FontName { get; set; }

  public void Measure(string text, double fontSize, out double width, out double height)
  {
    int num = 0;
    for (int index = 0; index < text.Length; ++index)
    {
      if ((int) text[index] < this.FirstChar + this.Widths.Length)
        num += this.Widths[(int) text[index] - this.FirstChar];
    }
    width = (double) num * fontSize / 1000.0;
    height = (double) (this.Ascent - this.Descent) * fontSize / 1000.0;
  }
}
