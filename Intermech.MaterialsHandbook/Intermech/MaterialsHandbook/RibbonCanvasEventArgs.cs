// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonCanvasEventArgs
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class RibbonCanvasEventArgs : EventArgs
{
  public Rectangle Bounds { get; set; }

  public Control Canvas { get; set; }

  public Graphics Graphics { get; set; }

  public Ribbon Owner { get; set; }

  public object RelatedObject { get; set; }

  public RibbonCanvasEventArgs(
    Ribbon owner,
    Graphics g,
    Rectangle bounds,
    Control canvas,
    object relatedObject)
  {
    this.Owner = owner;
    this.Graphics = g;
    this.Bounds = bounds;
    this.Canvas = canvas;
    this.RelatedObject = relatedObject;
  }
}
