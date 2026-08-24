// Decompiled with JetBrains decompiler
// Type: OxyPlot.WindowsForms.PngExporter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Drawing;
using System.IO;

#nullable disable
namespace OxyPlot.WindowsForms;

public class PngExporter
{
  public PngExporter()
  {
    this.Width = 700;
    this.Height = 400;
    this.Resolution = 96 /*0x60*/;
    this.Background = OxyColors.White;
  }

  public int Width { get; set; }

  public int Height { get; set; }

  public int Resolution { get; set; }

  public OxyColor Background { get; set; }

  public static void Export(
    IPlotModel model,
    string fileName,
    int width,
    int height,
    Brush background = null)
  {
    using (FileStream fileStream = File.Create(fileName))
      new PngExporter()
      {
        Width = width,
        Height = height,
        Background = background.ToOxyColor()
      }.Export(model, (Stream) fileStream);
  }

  public void Export(IPlotModel model, Stream stream)
  {
    using (Bitmap bitmap = new Bitmap(this.Width, this.Height))
    {
      using (Graphics graphics = Graphics.FromImage((Image) bitmap))
      {
        if (this.Background.IsVisible())
        {
          using (Brush brush = this.Background.ToBrush())
            graphics.FillRectangle(brush, 0, 0, this.Width, this.Height);
        }
        GraphicsRenderContext graphicsRenderContext = new GraphicsRenderContext(graphics);
        graphicsRenderContext.RendersToScreen = false;
        using (GraphicsRenderContext rc = graphicsRenderContext)
        {
          model.Update(true);
          model.Render((IRenderContext) rc, (double) this.Width, (double) this.Height);
        }
        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
      }
    }
  }
}
