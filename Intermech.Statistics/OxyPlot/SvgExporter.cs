// Decompiled with JetBrains decompiler
// Type: OxyPlot.SvgExporter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.IO;

#nullable disable
namespace OxyPlot;

public class SvgExporter
{
  public SvgExporter()
  {
    this.Width = 600.0;
    this.Height = 400.0;
    this.IsDocument = true;
  }

  public double Width { get; set; }

  public double Height { get; set; }

  public bool IsDocument { get; set; }

  public IRenderContext TextMeasurer { get; set; }

  public static void Export(
    IPlotModel model,
    Stream stream,
    double width,
    double height,
    bool isDocument,
    IRenderContext textMeasurer = null)
  {
    if (textMeasurer == null)
      textMeasurer = (IRenderContext) new PdfRenderContext(width, height, model.Background);
    using (SvgRenderContext rc = new SvgRenderContext(stream, width, height, true, textMeasurer, model.Background))
    {
      model.Update(true);
      model.Render((IRenderContext) rc, width, height);
      rc.Complete();
      rc.Flush();
    }
  }

  public static string ExportToString(
    IPlotModel model,
    double width,
    double height,
    bool isDocument,
    IRenderContext textMeasurer = null)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      SvgExporter.Export(model, (Stream) memoryStream, width, height, isDocument, textMeasurer);
      memoryStream.Flush();
      memoryStream.Position = 0L;
      return new StreamReader((Stream) memoryStream).ReadToEnd();
    }
  }

  public void Export(IPlotModel model, Stream stream)
  {
    SvgExporter.Export(model, stream, this.Width, this.Height, this.IsDocument, this.TextMeasurer);
  }

  public string ExportToString(IPlotModel model)
  {
    return SvgExporter.ExportToString(model, this.Width, this.Height, this.IsDocument, this.TextMeasurer);
  }
}
