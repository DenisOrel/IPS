// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonTab
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

[DesignTimeVisible(false)]
[Designer(typeof (RibbonTabDesigner))]
public class RibbonTab : Component, IContainsRibbonComponents, IRibbonElement
{
  private Ribbon _owner;
  private Rectangle _tabBounds;
  private Rectangle _tabContentBounds;
  private string _text = string.Empty;
  private bool _selected;
  private bool _pressed;
  private bool _active;
  private bool _scrollLeftSelected;
  private bool _scrollLeftPressed;
  private bool _scrollRightSelected;
  private bool _scrollRightPressed;
  private int _offset;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual bool Active
  {
    get => this._active;
    set
    {
      int num = this._active != value ? 1 : 0;
      this._active = value;
      if (num == 0)
        return;
      this.OnActiveChanged(EventArgs.Empty);
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Ribbon Owner
  {
    get => this._owner;
    set
    {
      this._owner = value;
      this.Panels.Owner = value;
      this.OnOwnerChanged(EventArgs.Empty);
    }
  }

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public RibbonPanelCollection Panels { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual bool Pressed
  {
    get => this._pressed;
    set
    {
      this._pressed = value;
      this.OnPressedChanged(EventArgs.Empty);
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public Rectangle ScrollLeftBounds { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public bool ScrollLeftPressed
  {
    get => this._scrollLeftPressed;
    set
    {
      this._scrollLeftPressed = value;
      if (value)
        this.ScrollLeft();
      this.OnScrollLeftPressedChanged(EventArgs.Empty);
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public bool ScrollLeftSelected
  {
    get => this._scrollLeftSelected;
    set
    {
      this._scrollLeftSelected = value;
      this.OnScrollLeftSelectedChanged(EventArgs.Empty);
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public bool ScrollLeftVisible { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public Rectangle ScrollRightBounds { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public bool ScrollRightPressed
  {
    get => this._scrollRightPressed;
    set
    {
      this._scrollRightPressed = value;
      if (value)
        this.ScrollRight();
      this.OnScrollRightPressedChanged(EventArgs.Empty);
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public bool ScrollRightSelected
  {
    get => this._scrollRightSelected;
    set
    {
      this._scrollRightSelected = value;
      this.OnScrollRightSelectedChanged(EventArgs.Empty);
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public bool ScrollRightVisible { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual bool Selected
  {
    get => this._selected;
    set
    {
      this._selected = value;
      if (value)
        this.OnMouseEnter(new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
      else
        this.OnMouseLeave(new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Rectangle TabBounds
  {
    get => this._tabBounds;
    set
    {
      this._tabBounds = value;
      this.OnTabBoundsChanged(EventArgs.Empty);
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Rectangle TabContentBounds
  {
    get => this._tabContentBounds;
    set
    {
      this._tabContentBounds = value;
      this.OnTabContentBoundsChanged(EventArgs.Empty);
    }
  }

  public object Tag { get; set; }

  [Localizable(true)]
  public string Text
  {
    get => this._text;
    set
    {
      this._text = value;
      this.OnTextChanged(EventArgs.Empty);
      this.Owner?.OnRegionsChanged();
    }
  }

  public RibbonTab()
  {
    this.Panels = new RibbonPanelCollection(this);
    this.ScrollLeftBounds = Rectangle.Empty;
    this.ScrollRightBounds = Rectangle.Empty;
  }

  public event EventHandler ActiveChanged;

  public event MouseEventHandler MouseEnter;

  public event MouseEventHandler MouseLeave;

  public event MouseEventHandler MouseMove;

  public event EventHandler OwnerChanged;

  public event EventHandler PressedChanged;

  public event EventHandler ScrollRightBoundsChanged;

  public event EventHandler ScrollRightPressedChanged;

  public event EventHandler ScrollRightSelectedChanged;

  public event EventHandler ScrollRightVisibleChanged;

  public event EventHandler ScrollLeftBoundsChanged;

  public event EventHandler ScrollLeftPressedChanged;

  public event EventHandler ScrollLeftSelectedChanged;

  public event EventHandler ScrollLeftVisibleChanged;

  public event EventHandler TabContentBoundsChanged;

  public event EventHandler TabBoundsChanged;

  public event EventHandler TextChanged;

  internal void UpdatePanelsRegions()
  {
    if (this.Panels.Count <= 0)
      return;
    bool flag = this.Site != null && this.Site.DesignMode;
    if (!flag)
      this._offset = 0;
    int left1 = this.TabContentBounds.Left;
    Padding panelPadding = this.Owner.PanelPadding;
    int left2 = panelPadding.Left;
    int x = left1 + left2 + this._offset;
    int top1 = this.TabContentBounds.Top;
    panelPadding = this.Owner.PanelPadding;
    int top2 = panelPadding.Top;
    int y = top1 + top2;
    using (Graphics graphics = this.Owner.CreateGraphics())
    {
      foreach (RibbonPanel panel in (List<RibbonPanel>) this.Panels)
      {
        RibbonElementSizeMode sizeMode = panel.FlowsTo == RibbonPanelFlowDirection.Right ? RibbonElementSizeMode.Medium : RibbonElementSizeMode.Large;
        int height1 = this.TabContentBounds.Height;
        panelPadding = this.Owner.PanelPadding;
        int vertical = panelPadding.Vertical;
        int height2 = height1 - vertical;
        panel.SetBounds(new Rectangle(0, 0, 1, height2));
        Size size = panel.MeasureSize((object) this, new RibbonElementMeasureSizeEventArgs(graphics, sizeMode));
        Rectangle bounds = new Rectangle(x, y, size.Width, size.Height);
        panel.SetBounds(bounds);
        panel.SizeMode = sizeMode;
        x = bounds.Right + 1 + this.Owner.PanelSpacing;
      }
      if (!flag)
      {
label_21:
        int num1 = x;
        Rectangle rectangle = this.TabContentBounds;
        int right = rectangle.Right;
        if (num1 > right && !this.AllPanelsOverflow())
        {
          RibbonPanel largerPanel = this.GetLargerPanel();
          if (largerPanel.SizeMode == RibbonElementSizeMode.Large)
            largerPanel.SizeMode = RibbonElementSizeMode.Medium;
          else if (largerPanel.SizeMode == RibbonElementSizeMode.Medium)
            largerPanel.SizeMode = RibbonElementSizeMode.Compact;
          else if (largerPanel.SizeMode == RibbonElementSizeMode.Compact)
            largerPanel.SizeMode = RibbonElementSizeMode.Overflow;
          Size size1 = largerPanel.MeasureSize((object) this, new RibbonElementMeasureSizeEventArgs(graphics, largerPanel.SizeMode));
          RibbonPanel ribbonPanel = largerPanel;
          rectangle = largerPanel.Bounds;
          Rectangle bounds = new Rectangle(rectangle.Location, new Size(size1.Width + this.Owner.PanelMargin.Horizontal, size1.Height));
          ribbonPanel.SetBounds(bounds);
          rectangle = this.TabContentBounds;
          x = rectangle.Left + this.Owner.PanelPadding.Left;
          using (List<RibbonPanel>.Enumerator enumerator = this.Panels.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              RibbonPanel current = enumerator.Current;
              rectangle = current.Bounds;
              Size size2 = rectangle.Size;
              current.SetBounds(new Rectangle(new Point(x, y), size2));
              int num2 = x;
              rectangle = current.Bounds;
              int num3 = rectangle.Width + 1 + this.Owner.PanelSpacing;
              x = num2 + num3;
            }
            goto label_21;
          }
        }
      }
      foreach (RibbonPanel panel in (List<RibbonPanel>) this.Panels)
        panel.UpdateItemsRegions(graphics, panel.SizeMode);
    }
    this.UpdateScrollBounds();
  }

  private bool AllPanelsOverflow()
  {
    bool flag = true;
    foreach (RibbonPanel panel in (List<RibbonPanel>) this.Panels)
    {
      if (panel.SizeMode != RibbonElementSizeMode.Overflow)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private RibbonPanel GetLargerPanel()
  {
    return this.GetLargerPanel(RibbonElementSizeMode.Large) ?? this.GetLargerPanel(RibbonElementSizeMode.Medium) ?? this.GetLargerPanel(RibbonElementSizeMode.Compact) ?? this.GetLargerPanel(RibbonElementSizeMode.Overflow);
  }

  private RibbonPanel GetLargerPanel(RibbonElementSizeMode size)
  {
    RibbonPanel largerPanel = (RibbonPanel) null;
    foreach (RibbonPanel panel in (List<RibbonPanel>) this.Panels)
    {
      if (panel.SizeMode == size)
      {
        if (largerPanel == null)
          largerPanel = panel;
        if (panel.Bounds.Width > largerPanel.Bounds.Width)
          largerPanel = panel;
      }
    }
    return largerPanel;
  }

  private void OnActiveChanged(EventArgs e)
  {
    EventHandler activeChanged = this.ActiveChanged;
    if (activeChanged == null)
      return;
    activeChanged((object) this, e);
  }

  private void OnMouseEnter(MouseEventArgs e)
  {
    MouseEventHandler mouseEnter = this.MouseEnter;
    if (mouseEnter == null)
      return;
    mouseEnter((object) this, e);
  }

  private void OnMouseLeave(MouseEventArgs e)
  {
    MouseEventHandler mouseLeave = this.MouseLeave;
    if (mouseLeave == null)
      return;
    mouseLeave((object) this, e);
  }

  private void OnMouseMove(MouseEventArgs e)
  {
    MouseEventHandler mouseMove = this.MouseMove;
    if (mouseMove == null)
      return;
    mouseMove((object) this, e);
  }

  private void OnOwnerChanged(EventArgs e)
  {
    EventHandler ownerChanged = this.OwnerChanged;
    if (ownerChanged == null)
      return;
    ownerChanged((object) this, e);
  }

  private void OnPressedChanged(EventArgs e)
  {
    EventHandler pressedChanged = this.PressedChanged;
    if (pressedChanged == null)
      return;
    pressedChanged((object) this, e);
  }

  private void OnScrollLeftBoundsChanged(EventArgs e)
  {
    EventHandler leftBoundsChanged = this.ScrollLeftBoundsChanged;
    if (leftBoundsChanged == null)
      return;
    leftBoundsChanged((object) this, e);
  }

  private void OnScrollLeftPressedChanged(EventArgs e)
  {
    EventHandler leftPressedChanged = this.ScrollLeftPressedChanged;
    if (leftPressedChanged == null)
      return;
    leftPressedChanged((object) this, e);
  }

  private void OnScrollLeftSelectedChanged(EventArgs e)
  {
    EventHandler leftSelectedChanged = this.ScrollLeftSelectedChanged;
    if (leftSelectedChanged == null)
      return;
    leftSelectedChanged((object) this, e);
  }

  private void OnScrollLeftVisibleChanged(EventArgs e)
  {
    EventHandler leftVisibleChanged = this.ScrollLeftVisibleChanged;
    if (leftVisibleChanged == null)
      return;
    leftVisibleChanged((object) this, e);
  }

  private void OnScrollRightBoundsChanged(EventArgs e)
  {
    EventHandler rightBoundsChanged = this.ScrollRightBoundsChanged;
    if (rightBoundsChanged == null)
      return;
    rightBoundsChanged((object) this, e);
  }

  private void OnScrollRightPressedChanged(EventArgs e)
  {
    EventHandler rightPressedChanged = this.ScrollRightPressedChanged;
    if (rightPressedChanged == null)
      return;
    rightPressedChanged((object) this, e);
  }

  private void OnScrollRightSelectedChanged(EventArgs e)
  {
    EventHandler rightSelectedChanged = this.ScrollRightSelectedChanged;
    if (rightSelectedChanged == null)
      return;
    rightSelectedChanged((object) this, e);
  }

  private void OnScrollRightVisibleChanged(EventArgs e)
  {
    EventHandler rightVisibleChanged = this.ScrollRightVisibleChanged;
    if (rightVisibleChanged == null)
      return;
    rightVisibleChanged((object) this, e);
  }

  private void OnTabBoundsChanged(EventArgs e)
  {
    EventHandler tabBoundsChanged = this.TabBoundsChanged;
    if (tabBoundsChanged == null)
      return;
    tabBoundsChanged((object) this, e);
  }

  private void OnTabContentBoundsChanged(EventArgs e)
  {
    EventHandler contentBoundsChanged = this.TabContentBoundsChanged;
    if (contentBoundsChanged == null)
      return;
    contentBoundsChanged((object) this, e);
  }

  private void OnTextChanged(EventArgs e)
  {
    EventHandler textChanged = this.TextChanged;
    if (textChanged == null)
      return;
    textChanged((object) this, e);
  }

  private void ScrollOffset(int amount)
  {
    this._offset += amount;
    foreach (RibbonPanel panel in (List<RibbonPanel>) this.Panels)
    {
      RibbonPanel ribbonPanel = panel;
      Rectangle bounds1 = panel.Bounds;
      int x = bounds1.Left + amount;
      bounds1 = panel.Bounds;
      int top = bounds1.Top;
      bounds1 = panel.Bounds;
      int width = bounds1.Width;
      bounds1 = panel.Bounds;
      int height = bounds1.Height;
      Rectangle bounds2 = new Rectangle(x, top, width, height);
      ribbonPanel.SetBounds(bounds2);
    }
    if (this.Site != null && this.Site.DesignMode)
      this.UpdatePanelsRegions();
    this.UpdateScrollBounds();
    this.Owner.Invalidate();
  }

  private void UpdateScrollBounds()
  {
    int num = 13;
    bool scrollRightVisible = this.ScrollRightVisible;
    Rectangle scrollRightBounds = this.ScrollRightBounds;
    Rectangle scrollLeftBounds = this.ScrollLeftBounds;
    if (this.Panels.Count <= 0)
      return;
    Rectangle rectangle = this.Panels[this.Panels.Count - 1].Bounds;
    int right1 = rectangle.Right;
    rectangle = this.TabContentBounds;
    int right2 = rectangle.Right;
    this.ScrollRightVisible = right1 > right2;
    if (this.ScrollRightVisible != scrollRightVisible)
      this.OnScrollRightVisibleChanged(EventArgs.Empty);
    this.ScrollLeftVisible = this._offset < 0;
    if (this.ScrollRightVisible != scrollRightVisible)
      this.OnScrollLeftVisibleChanged(EventArgs.Empty);
    if (!this.ScrollLeftVisible && !this.ScrollRightVisible)
      return;
    rectangle = this.Owner.ClientRectangle;
    int left = rectangle.Right - num;
    rectangle = this.TabContentBounds;
    int top1 = rectangle.Top;
    rectangle = this.Owner.ClientRectangle;
    int right3 = rectangle.Right;
    rectangle = this.TabContentBounds;
    int bottom1 = rectangle.Bottom;
    this.ScrollRightBounds = Rectangle.FromLTRB(left, top1, right3, bottom1);
    rectangle = this.TabContentBounds;
    int top2 = rectangle.Top;
    int right4 = num;
    rectangle = this.TabContentBounds;
    int bottom2 = rectangle.Bottom;
    this.ScrollLeftBounds = Rectangle.FromLTRB(0, top2, right4, bottom2);
    if (this.ScrollRightBounds != scrollRightBounds)
      this.OnScrollRightBoundsChanged(EventArgs.Empty);
    if (!(this.ScrollLeftBounds != scrollLeftBounds))
      return;
    this.OnScrollLeftBoundsChanged(EventArgs.Empty);
  }

  public void ClearSelection()
  {
    this._selected = false;
    if (this.Panels == null)
      return;
    foreach (RibbonPanel panel in (List<RibbonPanel>) this.Panels)
      panel.ClearSelection();
  }

  public void ScrollLeft() => this.ScrollOffset(50);

  public void ScrollRight() => this.ScrollOffset(-50);

  public IEnumerable<Component> GetAllChildComponents()
  {
    return (IEnumerable<Component>) this.Panels.ToArray();
  }

  [Browsable(false)]
  public Rectangle Bounds { get; private set; }

  public Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
  {
    return TextRenderer.MeasureText(this.Text, this.Owner.Font);
  }

  public void OnPaint(object sender, RibbonElementPaintEventArgs e)
  {
    if (this.Owner == null)
      return;
    this.Owner.Renderer.OnRenderRibbonTab(new RibbonTabRenderEventArgs(this.Owner, e.Graphics, e.Clip, this));
    this.Owner.Renderer.OnRenderRibbonTabText(new RibbonTabRenderEventArgs(this.Owner, e.Graphics, e.Clip, this));
    if (this.Active)
    {
      foreach (RibbonPanel panel in (List<RibbonPanel>) this.Panels)
        panel.OnPaint((object) this, new RibbonElementPaintEventArgs(e.Clip, e.Graphics, panel.SizeMode, e.Control));
    }
    this.Owner.Renderer.OnRenderTabScrollButtons(new RibbonTabRenderEventArgs(this.Owner, e.Graphics, e.Clip, this));
  }

  public void SetBounds(Rectangle bounds) => this.TabBounds = bounds;
}
