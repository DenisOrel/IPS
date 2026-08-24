// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonDropDown
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

[ToolboxItem(false)]
public class RibbonDropDown : RibbonPopup
{
  private bool _ignoreNext;
  private readonly RibbonMouseSensor _sensor;

  public bool DrawIconsBar { get; set; }

  public IEnumerable<RibbonItem> Items { get; private set; }

  public RibbonElementSizeMode MeasuringSize { get; set; }

  public Ribbon OwnerRibbon { get; private set; }

  public RibbonItem ParentItem { get; private set; }

  internal ISelectionService SelectionService { get; set; }

  private RibbonDropDown()
  {
    this.DoubleBuffered = true;
    this.MeasuringSize = RibbonElementSizeMode.None;
    this.DrawIconsBar = true;
  }

  internal RibbonDropDown(RibbonItem parentItem, IEnumerable<RibbonItem> items, Ribbon ownerRibbon)
    : this(parentItem, items, ownerRibbon, RibbonElementSizeMode.DropDown)
  {
  }

  internal RibbonDropDown(
    RibbonItem parentItem,
    IEnumerable<RibbonItem> items,
    Ribbon ownerRibbon,
    RibbonElementSizeMode measuringSize)
    : this()
  {
    this.Items = items;
    this.OwnerRibbon = ownerRibbon;
    this.ParentItem = parentItem;
    this.MeasuringSize = measuringSize;
    this._sensor = new RibbonMouseSensor((Control) this, this.OwnerRibbon, items);
    if (this.Items != null)
    {
      foreach (RibbonItem ribbonItem in this.Items)
      {
        ribbonItem.SetSizeMode(RibbonElementSizeMode.DropDown);
        ribbonItem.Canvas = (Control) this;
      }
    }
    this.UpdateSize();
  }

  protected override void OnMouseLeave(EventArgs e)
  {
    base.OnMouseLeave(e);
    foreach (RibbonItem ribbonItem in this.Items)
      ribbonItem.SetSelected(false);
  }

  protected override void OnMouseUp(MouseEventArgs e)
  {
    base.OnMouseUp(e);
    if (this._ignoreNext)
    {
      this._ignoreNext = false;
    }
    else
    {
      if (RibbonDesigner.Current == null)
        return;
      this.Close();
    }
  }

  protected override void OnOpening(CancelEventArgs e)
  {
    base.OnOpening(e);
    this.UpdateItemsBounds();
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);
    this.OwnerRibbon.Renderer.OnRenderDropDownBackground(new RibbonCanvasEventArgs(this.OwnerRibbon, e.Graphics, new Rectangle(Point.Empty, this.ClientSize), (Control) this, (object) this.ParentItem));
    foreach (RibbonItem ribbonItem in this.Items)
      ribbonItem.OnPaint((object) this, new RibbonElementPaintEventArgs(ribbonItem.Bounds, e.Graphics, RibbonElementSizeMode.DropDown, (Control) null));
  }

  protected override void OnShowed(EventArgs e)
  {
    base.OnShowed(e);
    foreach (RibbonItem ribbonItem in this.Items)
      ribbonItem.SetSelected(false);
  }

  private void UpdateItemsBounds()
  {
    int top = this.OwnerRibbon.DropDownMargin.Top;
    int left = this.OwnerRibbon.DropDownMargin.Left;
    int width = this.ClientSize.Width - this.OwnerRibbon.DropDownMargin.Horizontal;
    int num = 0;
    foreach (RibbonItem ribbonItem in this.Items)
      num += ribbonItem.LastMeasuredSize.Height;
    foreach (RibbonItem ribbonItem in this.Items)
    {
      ribbonItem.SetBounds(new Rectangle(left, top, width, ribbonItem.LastMeasuredSize.Height));
      top += ribbonItem.Bounds.Height;
    }
  }

  private void UpdateSize()
  {
    int vertical = this.OwnerRibbon.DropDownMargin.Vertical;
    int val1 = 0;
    using (Graphics graphics = this.CreateGraphics())
    {
      foreach (RibbonItem ribbonItem in this.Items)
      {
        Size size = ribbonItem.MeasureSize((object) this, new RibbonElementMeasureSizeEventArgs(graphics, this.MeasuringSize));
        vertical += size.Height;
        val1 = Math.Max(val1, size.Width);
      }
    }
    this.Size = new Size(val1 + this.OwnerRibbon.DropDownMargin.Horizontal, vertical);
    if (this.WrappedDropDown == null)
      return;
    this.WrappedDropDown.Size = this.Size;
  }

  public void IgnoreNextClickDeactivation() => this._ignoreNext = true;
}
