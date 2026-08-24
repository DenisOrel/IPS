// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Controls.MaterialProperties.SelectablePanel
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook.Controls.MaterialProperties;

internal sealed class SelectablePanel : Panel
{
  public SelectablePanel()
  {
    this.SetStyle(ControlStyles.Selectable, true);
    this.SetStyle(ControlStyles.ResizeRedraw, true);
    this.TabStop = true;
    this.DoubleBuffered = true;
  }

  protected override void OnMouseDown(MouseEventArgs e)
  {
    this.Focus();
    base.OnMouseDown(e);
  }

  protected override bool IsInputKey(Keys keyData)
  {
    return keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right || base.IsInputKey(keyData);
  }

  protected override void OnEnter(EventArgs e)
  {
    this.Invalidate();
    base.OnEnter(e);
  }

  protected override void OnLeave(EventArgs e)
  {
    this.Invalidate();
    base.OnLeave(e);
  }

  protected override Point ScrollToControl(Control activeControl) => this.AutoScrollPosition;
}
