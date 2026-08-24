// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonElementPaintEventArgs
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class RibbonElementPaintEventArgs : EventArgs
{
  public Rectangle Clip { get; private set; }

  public Control Control { get; private set; }

  public Graphics Graphics { get; private set; }

  public RibbonElementSizeMode Mode { get; private set; }

  internal RibbonElementPaintEventArgs(
    Rectangle clip,
    Graphics graphics,
    RibbonElementSizeMode mode,
    Control control)
  {
    this.Clip = clip;
    this.Graphics = graphics;
    this.Mode = mode;
    this.Control = control;
  }
}
