// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonPanelRenderEventArgs
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public sealed class RibbonPanelRenderEventArgs : RibbonRenderEventArgs
{
  public Control Canvas { get; set; }

  public RibbonPanel Panel { get; set; }

  public RibbonPanelRenderEventArgs(
    Ribbon owner,
    Graphics g,
    Rectangle clip,
    RibbonPanel panel,
    Control canvas)
    : base(owner, g, clip)
  {
    this.Panel = panel;
    this.Canvas = canvas;
  }
}
