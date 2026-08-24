// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonElementMeasureSizeEventArgs
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Drawing;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class RibbonElementMeasureSizeEventArgs : EventArgs
{
  public Graphics Graphics { get; private set; }

  public RibbonElementSizeMode SizeMode { get; private set; }

  internal RibbonElementMeasureSizeEventArgs(Graphics graphics, RibbonElementSizeMode sizeMode)
  {
    this.Graphics = graphics;
    this.SizeMode = sizeMode;
  }
}
