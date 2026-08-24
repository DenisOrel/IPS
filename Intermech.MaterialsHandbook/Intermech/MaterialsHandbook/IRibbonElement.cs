// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IRibbonElement
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System.Drawing;

#nullable disable
namespace Intermech.MaterialsHandbook;

public interface IRibbonElement
{
  Rectangle Bounds { get; }

  Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e);

  void OnPaint(object sender, RibbonElementPaintEventArgs e);

  void SetBounds(Rectangle bounds);
}
