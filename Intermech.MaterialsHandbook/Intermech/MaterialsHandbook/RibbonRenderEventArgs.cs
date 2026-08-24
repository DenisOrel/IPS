// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonRenderEventArgs
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Drawing;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class RibbonRenderEventArgs : EventArgs
{
  public Graphics Graphics { get; private set; }

  public Rectangle Rectangle { get; private set; }

  public Ribbon Ribbon { get; private set; }

  public RibbonRenderEventArgs(Ribbon owner, Graphics g, Rectangle rect)
  {
    this.Ribbon = owner;
    this.Graphics = g;
    this.Rectangle = rect;
  }
}
