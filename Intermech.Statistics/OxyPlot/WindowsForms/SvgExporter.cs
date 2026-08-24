// Decompiled with JetBrains decompiler
// Type: OxyPlot.WindowsForms.SvgExporter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Drawing;

#nullable disable
namespace OxyPlot.WindowsForms;

public class SvgExporter : OxyPlot.SvgExporter, IDisposable
{
  private Graphics g;
  private GraphicsRenderContext grc;

  public SvgExporter()
  {
    this.g = Graphics.FromHwnd(IntPtr.Zero);
    this.TextMeasurer = (IRenderContext) (this.grc = new GraphicsRenderContext(this.g));
  }

  public void Dispose()
  {
    this.g.Dispose();
    this.grc.Dispose();
  }
}
