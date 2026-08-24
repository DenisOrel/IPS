// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonWrappedDropDown
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

[DesignTimeVisible(false)]
internal class RibbonWrappedDropDown : ToolStripDropDown
{
  public RibbonWrappedDropDown()
  {
    this.DoubleBuffered = false;
    this.SetStyle(ControlStyles.Opaque, true);
    this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
    this.SetStyle(ControlStyles.Selectable, false);
    this.SetStyle(ControlStyles.ResizeRedraw, false);
    this.AutoSize = false;
  }
}
