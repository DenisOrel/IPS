// Decompiled with JetBrains decompiler
// Type: OxyPlot.PdfExporter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.IO;

#nullable disable
namespace OxyPlot;

public class PdfExporter
{
  public double Width { get; set; }

  public double Height { get; set; }

  public OxyColor Background { get; set; }

  public static void Export(IPlotModel model, Stream stream, double width, double height)
  {
    new PdfExporter()
    {
      Width = width,
      Height = height,
      Background = model.Background
    }.Export(model, stream);
  }

  public void Export(IPlotModel model, Stream stream)
  {
    PdfRenderContext rc = new PdfRenderContext(this.Width, this.Height, this.Background);
    model.Update(true);
    model.Render((IRenderContext) rc, this.Width, this.Height);
    rc.Save(stream);
  }
}
