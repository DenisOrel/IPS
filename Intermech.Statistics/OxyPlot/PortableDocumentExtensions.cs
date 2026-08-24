// Decompiled with JetBrains decompiler
// Type: OxyPlot.PortableDocumentExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public static class PortableDocumentExtensions
{
  public static void SetColor(this PortableDocument doc, OxyColor c)
  {
    doc.SetColor((double) c.R / (double) byte.MaxValue, (double) c.G / (double) byte.MaxValue, (double) c.B / (double) byte.MaxValue);
    doc.SetStrokeAlpha((double) c.A / (double) byte.MaxValue);
  }

  public static void SetFillColor(this PortableDocument doc, OxyColor c)
  {
    doc.SetFillColor((double) c.R / (double) byte.MaxValue, (double) c.G / (double) byte.MaxValue, (double) c.B / (double) byte.MaxValue);
    doc.SetFillAlpha((double) c.A / (double) byte.MaxValue);
  }
}
