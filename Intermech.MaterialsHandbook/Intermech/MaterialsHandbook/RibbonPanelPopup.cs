// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonPanelPopup
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

[ToolboxItem(false)]
public class RibbonPanelPopup : RibbonPopup
{
  private RibbonMouseSensor _sensor;
  private bool _ignoreNext;

  public RibbonPanel Panel { get; private set; }

  internal RibbonPanelPopup(RibbonPanel panel)
  {
    this.DoubleBuffered = true;
    this._sensor = new RibbonMouseSensor((Control) this, panel.Owner, (IEnumerable<RibbonItem>) panel.Items)
    {
      PanelLimit = panel
    };
    this.Panel = panel;
    this.Panel.PopUp = (Control) this;
    panel.Owner.SuspendSensor();
    using (Graphics graphics = this.CreateGraphics())
    {
      panel.overflowBoundsBuffer = panel.Bounds;
      Size size = panel.SwitchToSize((Control) this, graphics, this.GetSizeMode(panel));
      size.Width += 100;
      size.Height += 100;
      this.Size = size;
    }
    foreach (RibbonItem ribbonItem in (List<RibbonItem>) panel.Items)
      ribbonItem.Canvas = (Control) this;
  }

  protected override void OnClosed(EventArgs e)
  {
    foreach (RibbonItem ribbonItem in (List<RibbonItem>) this.Panel.Items)
      ribbonItem.Canvas = (Control) null;
    this.Panel.Pressed = false;
    this.Panel.Selected = false;
    this.Panel.Owner.UpdateRegions((Graphics) null);
    this.Panel.Owner.Refresh();
    this.Panel.PopUp = (Control) null;
    this.Panel.Owner.ResumeSensor();
    this.Panel.PopUpShowed = false;
    this.Panel.Owner.RedrawArea(this.Panel.Bounds);
  }

  protected override void OnMouseUp(MouseEventArgs e)
  {
    base.OnMouseUp(e);
    if (!this._ignoreNext)
      return;
    this._ignoreNext = false;
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);
    this.Panel.Owner.Renderer.OnRenderPanelPopupBackground(new RibbonCanvasEventArgs(this.Panel.Owner, e.Graphics, new Rectangle(Point.Empty, this.ClientSize), (Control) this, (object) this.Panel));
    foreach (RibbonItem ribbonItem in (List<RibbonItem>) this.Panel.Items)
      ribbonItem.OnPaint((object) this, new RibbonElementPaintEventArgs(e.ClipRectangle, e.Graphics, RibbonElementSizeMode.Large, (Control) null));
    this.Panel.Owner.Renderer.OnRenderRibbonPanelText(new RibbonPanelRenderEventArgs(this.Panel.Owner, e.Graphics, e.ClipRectangle, this.Panel, (Control) this));
  }

  public RibbonElementSizeMode GetSizeMode(RibbonPanel pnl)
  {
    return pnl.FlowsTo != RibbonPanelFlowDirection.Right ? RibbonElementSizeMode.Large : RibbonElementSizeMode.Medium;
  }

  public void IgnoreNextClickDeactivation() => this._ignoreNext = true;
}
