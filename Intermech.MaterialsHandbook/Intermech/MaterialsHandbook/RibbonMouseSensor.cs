// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonMouseSensor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class RibbonMouseSensor : IDisposable
{
  private Ribbon _ribbon;
  private List<RibbonTab> _tabs = new List<RibbonTab>();
  private RibbonTab _hittedTab;
  private RibbonTab _tabLimit;
  private List<RibbonPanel> _panels = new List<RibbonPanel>();
  private RibbonPanel _selectedPanel;
  private RibbonPanel _hittedPanel;
  private RibbonItem _selectedItem;
  private RibbonItem _hittedItem;
  private RibbonItem _selectedSubItem;
  private RibbonItem _hittedSubItem;
  private bool _disposed;
  private bool _suspended;
  private bool _hittedTabScrollLeft;
  private bool _hittedTabScrollRight;

  public Control Control { get; }

  internal bool HittedTabScroll => this._hittedTabScrollLeft || this._hittedTabScrollRight;

  public IEnumerable<RibbonItem> ItemsSource { get; set; }

  public List<RibbonItem> Items { get; }

  public RibbonPanel PanelLimit { get; set; }

  private RibbonMouseSensor() => this.Items = new List<RibbonItem>();

  public RibbonMouseSensor(Control control, Ribbon ribbon)
    : this()
  {
    this.Control = control;
    this._ribbon = ribbon;
    this.AddHandlers();
  }

  public RibbonMouseSensor(Control control, Ribbon ribbon, RibbonTab tab)
    : this(control, ribbon)
  {
    this._tabs.Add(tab);
    this._panels.AddRange((IEnumerable<RibbonPanel>) tab.Panels);
    foreach (RibbonPanel panel in (List<RibbonPanel>) tab.Panels)
      this.Items.AddRange((IEnumerable<RibbonItem>) panel.Items);
  }

  public RibbonMouseSensor(Control control, Ribbon ribbon, IEnumerable<RibbonItem> itemsSource)
    : this(control, ribbon)
  {
    this.ItemsSource = itemsSource;
  }

  private void OnControl_MouseClick(object sender, MouseEventArgs e)
  {
    if (this._suspended || this._disposed)
      return;
    this._hittedPanel?.OnClick((EventArgs) e);
    this._hittedItem?.OnClick((EventArgs) e);
    this._hittedSubItem?.OnClick((EventArgs) e);
  }

  private void OnControl_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    if (this._suspended || this._disposed)
      return;
    this._hittedPanel?.OnDoubleClick((EventArgs) e);
    this._hittedItem?.OnDoubleClick((EventArgs) e);
    this._hittedSubItem?.OnDoubleClick((EventArgs) e);
  }

  private void OnControl_MouseDown(object sender, MouseEventArgs e)
  {
    if (this._suspended || this._disposed)
      return;
    this.HitTest(e.Location);
    if (this._hittedTab != null)
    {
      if (this._hittedTabScrollLeft)
      {
        this._hittedTab.ScrollLeftPressed = true;
        this.Control.Invalidate(this._hittedTab.ScrollLeftBounds);
      }
      if (this._hittedTabScrollRight)
      {
        this._hittedTab.ScrollRightPressed = true;
        this.Control.Invalidate(this._hittedTab.ScrollRightBounds);
      }
    }
    if (this._hittedPanel != null)
    {
      this._hittedPanel.Pressed = true;
      this._hittedPanel.OnMouseDown(e);
      this.Control.Invalidate(this._hittedPanel.Bounds);
    }
    if (this._hittedItem != null)
    {
      this._hittedItem.SetPressed(true);
      this._hittedItem.OnMouseDown(e);
      this.Control.Invalidate(this._hittedItem.Bounds);
    }
    if (this._hittedSubItem == null)
      return;
    this._hittedSubItem.SetPressed(true);
    this._hittedSubItem.OnMouseDown(e);
    if (this._hittedItem == null)
      return;
    this.Control.Invalidate(Rectangle.Intersect(this._hittedItem.Bounds, this._hittedSubItem.Bounds));
  }

  private void OnControl_MouseLeave(object sender, EventArgs e)
  {
    if (this._suspended || this._disposed)
      return;
    this._ribbon.ClearSelection();
    this._ribbon.Invalidate();
  }

  private void OnControl_MouseMove(object sender, MouseEventArgs e)
  {
    if (this._suspended || this._disposed)
      return;
    this.HitTest(e.Location);
    if (this._selectedPanel != null && this._selectedPanel != this._hittedPanel)
    {
      this._selectedPanel.Selected = false;
      this._selectedPanel.OnMouseLeave(e);
      this.Control.Invalidate(this._selectedPanel.Bounds);
    }
    if (this._selectedItem != null && this._selectedItem != this._hittedItem)
    {
      this._selectedItem.SetSelected(false);
      this._selectedItem.OnMouseLeave(e);
      this.Control.Invalidate(this._selectedItem.Bounds);
    }
    if (this._selectedSubItem != null && this._selectedSubItem != this._hittedSubItem)
    {
      this._selectedSubItem.SetSelected(false);
      this._selectedSubItem.OnMouseLeave(e);
      if (this._selectedItem != null)
        this.Control.Invalidate(Rectangle.Intersect(this._selectedItem.Bounds, this._selectedSubItem.Bounds));
    }
    if (this._hittedTab != null)
    {
      if (this._hittedTab.ScrollLeftVisible)
      {
        this._hittedTab.ScrollLeftSelected = this._hittedTabScrollLeft;
        this.Control.Invalidate(this._hittedTab.ScrollLeftBounds);
      }
      if (this._hittedTab.ScrollRightVisible)
      {
        this._hittedTab.ScrollRightSelected = this._hittedTabScrollRight;
        this.Control.Invalidate(this._hittedTab.ScrollRightBounds);
      }
    }
    if (this._hittedPanel != null)
    {
      if (this._hittedPanel == this._selectedPanel)
      {
        this._hittedPanel.OnMouseMove(e);
      }
      else
      {
        this._hittedPanel.Selected = true;
        this._hittedPanel.OnMouseEnter(e);
        this.Control.Invalidate(this._hittedPanel.Bounds);
      }
    }
    if (this._hittedItem != null)
    {
      if (this._hittedItem == this._selectedItem)
      {
        this._hittedItem.OnMouseMove(e);
      }
      else
      {
        this._hittedItem.SetSelected(true);
        this._hittedItem.OnMouseEnter(e);
        this.Control.Invalidate(this._hittedItem.Bounds);
      }
    }
    if (this._hittedSubItem == null)
      return;
    if (this._hittedSubItem == this._selectedSubItem)
    {
      this._hittedSubItem.OnMouseMove(e);
    }
    else
    {
      this._hittedSubItem.SetSelected(true);
      this._hittedSubItem.OnMouseEnter(e);
      if (this._hittedItem == null)
        return;
      this.Control.Invalidate(Rectangle.Intersect(this._hittedItem.Bounds, this._hittedSubItem.Bounds));
    }
  }

  private void OnControl_MouseUp(object sender, MouseEventArgs e)
  {
    if (this._suspended || this._disposed)
      return;
    if (this._hittedTab != null)
    {
      if (this._hittedTab.ScrollLeftVisible)
      {
        this._hittedTab.ScrollLeftPressed = false;
        this.Control.Invalidate(this._hittedTab.ScrollLeftBounds);
      }
      if (this._hittedTab.ScrollRightVisible)
      {
        this._hittedTab.ScrollRightPressed = false;
        this.Control.Invalidate(this._hittedTab.ScrollRightBounds);
      }
    }
    if (this._hittedPanel != null)
    {
      this._hittedPanel.Pressed = false;
      this._hittedPanel.OnMouseUp(e);
      this.Control.Invalidate(this._hittedPanel.Bounds);
    }
    if (this._hittedItem != null)
    {
      this._hittedItem.SetPressed(false);
      this._hittedItem.OnMouseUp(e);
      this.Control.Invalidate(this._hittedItem.Bounds);
    }
    if (this._hittedSubItem == null)
      return;
    this._hittedSubItem.SetPressed(false);
    this._hittedSubItem.OnMouseUp(e);
    if (this._hittedItem == null)
      return;
    this.Control.Invalidate(Rectangle.Intersect(this._hittedItem.Bounds, this._hittedSubItem.Bounds));
  }

  private void AddHandlers()
  {
    this.Control.MouseClick += new MouseEventHandler(this.OnControl_MouseClick);
    this.Control.MouseDoubleClick += new MouseEventHandler(this.OnControl_MouseDoubleClick);
    this.Control.MouseDown += new MouseEventHandler(this.OnControl_MouseDown);
    this.Control.MouseLeave += new EventHandler(this.OnControl_MouseLeave);
    this.Control.MouseMove += new MouseEventHandler(this.OnControl_MouseMove);
    this.Control.MouseUp += new MouseEventHandler(this.OnControl_MouseUp);
  }

  private void HitTest(Point p)
  {
    this._selectedPanel = this._hittedPanel;
    this._selectedItem = this._hittedItem;
    this._selectedSubItem = this._hittedSubItem;
    this._hittedTab = (RibbonTab) null;
    this._hittedTabScrollLeft = false;
    this._hittedTabScrollRight = false;
    this._hittedPanel = (RibbonPanel) null;
    this._hittedItem = (RibbonItem) null;
    this._hittedSubItem = (RibbonItem) null;
    if (this._tabLimit != null)
    {
      if (this._tabLimit.TabContentBounds.Contains(p))
        this._hittedTab = this._tabLimit;
    }
    else
    {
      foreach (RibbonTab tab in this._tabs)
      {
        if (tab.TabContentBounds.Contains(p))
        {
          this._hittedTab = tab;
          break;
        }
      }
    }
    if (this._hittedTab != null)
    {
      this._hittedTabScrollLeft = this._hittedTab.ScrollLeftVisible && this._hittedTab.ScrollLeftBounds.Contains(p);
      this._hittedTabScrollRight = this._hittedTab.ScrollRightVisible && this._hittedTab.ScrollRightBounds.Contains(p);
    }
    if (this.HittedTabScroll)
      return;
    if (this.PanelLimit != null)
    {
      if (this.PanelLimit.Bounds.Contains(p))
        this._hittedPanel = this.PanelLimit;
    }
    else
    {
      foreach (RibbonPanel panel in this._panels)
      {
        if (panel.Bounds.Contains(p))
        {
          this._hittedPanel = panel;
          break;
        }
      }
    }
    IEnumerable<RibbonItem> ribbonItems = (IEnumerable<RibbonItem>) this.Items;
    if (this.ItemsSource != null)
      ribbonItems = this.ItemsSource;
    foreach (RibbonItem ribbonItem in ribbonItems)
    {
      if ((ribbonItem.OwnerPanel == null || !ribbonItem.OwnerPanel.OverflowMode || this.Control is RibbonPanelPopup) && ribbonItem.Bounds.Contains(p))
      {
        this._hittedItem = ribbonItem;
        break;
      }
    }
    if (!(this._hittedItem is IContainsSelectableRibbonItems hittedItem))
      return;
    foreach (RibbonItem ribbonItem in hittedItem.GetItems())
    {
      Rectangle bounds = ribbonItem.Bounds;
      bounds.Intersect(this._hittedItem.Bounds);
      if (bounds.Contains(p))
        this._hittedSubItem = ribbonItem;
    }
  }

  private void RemoveHandlers()
  {
    foreach (RibbonItem ribbonItem in this.Items)
    {
      ribbonItem.SetSelected(false);
      ribbonItem.SetPressed(false);
    }
    this.Control.MouseClick -= new MouseEventHandler(this.OnControl_MouseClick);
    this.Control.MouseDoubleClick -= new MouseEventHandler(this.OnControl_MouseDoubleClick);
    this.Control.MouseDown -= new MouseEventHandler(this.OnControl_MouseDown);
    this.Control.MouseLeave -= new EventHandler(this.OnControl_MouseLeave);
    this.Control.MouseMove -= new MouseEventHandler(this.OnControl_MouseMove);
    this.Control.MouseUp -= new MouseEventHandler(this.OnControl_MouseUp);
  }

  public void Resume() => this._suspended = false;

  public void Suspend() => this._suspended = true;

  public void Dispose()
  {
    this._disposed = true;
    this.RemoveHandlers();
    this._tabLimit?.Dispose();
    this._hittedItem?.Dispose();
    this._hittedSubItem?.Dispose();
  }
}
