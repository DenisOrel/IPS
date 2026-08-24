// Decompiled with JetBrains decompiler
// Type: OxyPlot.WindowsForms.PlotModelExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Drawing;

#nullable disable
namespace OxyPlot.WindowsForms;

public static class PlotModelExtensions
{
  public static string ToSvg(this PlotModel model, double width, double height, bool isDocument)
  {
    using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
    {
      GraphicsRenderContext graphicsRenderContext = new GraphicsRenderContext(graphics);
      graphicsRenderContext.RendersToScreen = false;
      using (GraphicsRenderContext textMeasurer = graphicsRenderContext)
        return OxyPlot.SvgExporter.ExportToString((IPlotModel) model, width, height, isDocument, (IRenderContext) textMeasurer);
    }
  }
}
