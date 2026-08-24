// Decompiled with JetBrains decompiler
// Type: OxyPlot.PortableDocumentFontFamily
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class PortableDocumentFontFamily
{
  public PortableDocumentFont RegularFont { get; set; }

  public PortableDocumentFont BoldFont { get; set; }

  public PortableDocumentFont ItalicFont { get; set; }

  public PortableDocumentFont BoldItalicFont { get; set; }

  public PortableDocumentFont GetFont(bool bold, bool italic)
  {
    if (bold & italic && this.BoldItalicFont != null)
      return this.BoldItalicFont;
    if (bold && this.BoldFont != null)
      return this.BoldFont;
    return italic && this.ItalicFont != null ? this.ItalicFont : this.RegularFont;
  }
}
