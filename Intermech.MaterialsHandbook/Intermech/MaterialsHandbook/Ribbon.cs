// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Ribbon
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

[Designer(typeof (RibbonDesigner))]
public class Ribbon : Control
{
  private RibbonMouseSensor _sensor;
  private RibbonTab _activeTab;
  private RibbonTab _lastSelectedTab;
  private float _tabSum;
  private Padding _tabMargin = new Padding(1, 2, 1, 2);
  private Size _lastSizeMeasured = Size.Empty;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RibbonTab ActiveTab
  {
    get => this._activeTab;
    set
    {
      foreach (RibbonTab tab in (List<RibbonTab>) this.Tabs)
        tab.Active = tab == value;
      this._activeTab = value;
      value.UpdatePanelsRegions();
      this.Invalidate();
      this.RenewSensor();
    }
  }

  [Browsable(false)]
  public RibbonWindowMode ActualBorderMode { get; set; }

  [Browsable(false)]
  [DefaultValue(DockStyle.Top)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public override DockStyle Dock
  {
    get => base.Dock;
    set => base.Dock = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Padding DropDownMargin { get; set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Padding ItemMargin { get; set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Padding ItemPadding { get; set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public override Size MaximumSize
  {
    get => new Size(0, 80 /*0x50*/);
    set
    {
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public override Size MinimumSize
  {
    get => new Size(0, 80 /*0x50*/);
    set
    {
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RibbonTab NextTab
  {
    get
    {
      RibbonTab nextTab;
      if (this.ActiveTab == null || this.Tabs.Count == 0)
      {
        nextTab = this.Tabs.Count == 0 ? (RibbonTab) null : this.Tabs[0];
      }
      else
      {
        int num = this.Tabs.IndexOf(this.ActiveTab);
        nextTab = num == this.Tabs.Count - 1 ? this.ActiveTab : this.Tabs[num + 1];
      }
      return nextTab;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Padding PanelMargin { get; set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Padding PanelPadding { get; set; }

  [DefaultValue(3)]
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int PanelSpacing { get; set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RibbonRenderer Renderer { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RibbonTab PreviousTab
  {
    get
    {
      RibbonTab previousTab;
      if (this.ActiveTab == null || this.Tabs.Count == 0)
      {
        previousTab = this.Tabs.Count == 0 ? (RibbonTab) null : this.Tabs[0];
      }
      else
      {
        int num = this.Tabs.IndexOf(this.ActiveTab);
        previousTab = num == 0 ? this.ActiveTab : this.Tabs[num - 1];
      }
      return previousTab;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Padding TabContentMargin { get; set; }

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public RibbonTabCollection Tabs { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Padding TabsMargin { get; set; }

  [DefaultValue(6)]
  public int TabSpacing { get; set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Padding TabsPadding { get; set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Padding TabTextMargin { get; set; }

  public Ribbon()
  {
    this.SetStyle(ControlStyles.ResizeRedraw, true);
    this.SetStyle(ControlStyles.Selectable, false);
    this.SetStyle(ControlStyles.UserPaint, true);
    this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    this.Dock = DockStyle.Top;
    this.Font = SystemFonts.CaptionFont;
    this.Renderer = new RibbonRenderer();
    this.Tabs = new RibbonTabCollection(this);
    this.ActualBorderMode = RibbonWindowMode.NonClientAreaGlass;
    this.DropDownMargin = new Padding(2);
    this.ItemMargin = new Padding(4, 2, 4, 2);
    this.ItemPadding = new Padding(1, 0, 1, 0);
    this.PanelMargin = new Padding(3, 2, 3, 15);
    this.PanelPadding = new Padding(3);
    this.PanelSpacing = 3;
    this.TabContentMargin = new Padding(1, 0, 1, 2);
    this.TabsMargin = new Padding(12, 2, 20, 0);
    this.TabSpacing = 6;
    this.TabsPadding = new Padding(8, 5, 8, 3);
    this.TabTextMargin = new Padding(4, 2, 4, 2);
  }

  internal void OnRegionsChanged()
  {
    if (this.Tabs.Count == 1)
      this.ActiveTab = this.Tabs[0];
    this._lastSizeMeasured = Size.Empty;
    this.Refresh();
  }

  internal void ResumeSensor() => this._sensor.Resume();

  internal void SuspendSensor()
  {
    if (this._sensor == null)
      return;
    this._sensor.Suspend();
  }

  internal bool TabHitTest(int x, int y)
  {
    bool flag = false;
    foreach (RibbonTab tab in (List<RibbonTab>) this.Tabs)
    {
      if (tab.TabBounds.Contains(x, y))
      {
        this.ActiveTab = tab;
        flag = true;
        break;
      }
    }
    return flag;
  }

  internal void UpdateRegions(Graphics g)
  {
    bool flag = false;
    if (this.IsDisposed)
      return;
    if (g == null)
    {
      g = this.CreateGraphics();
      flag = true;
    }
    int num1 = this.TabsMargin.Left;
    int val2_1 = 0;
    int val2_2 = 0;
    int num2 = 0;
    int num3 = 0;
    Padding padding;
    if (this.Tabs.Count > 1)
    {
      num2 = this.TabsPadding.Left + this.TabsPadding.Right;
      padding = this.TabsPadding;
      int top = padding.Top;
      padding = this.TabsPadding;
      int bottom = padding.Bottom;
      num3 = top + bottom;
    }
    foreach (RibbonTab tab in (List<RibbonTab>) this.Tabs)
    {
      Size size = Size.Empty;
      if (num2 > 0)
        size = tab.MeasureSize((object) this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));
      Rectangle rectangle1;
      ref Rectangle local = ref rectangle1;
      int x = num1;
      padding = this.TabsMargin;
      int top1 = padding.Top;
      int width = num2 + size.Width;
      int height = num3 + size.Height;
      local = new Rectangle(x, top1, width, height);
      tab.TabBounds = rectangle1;
      num1 = rectangle1.Right + this.TabSpacing;
      val2_1 = Math.Max(rectangle1.Width, val2_1);
      val2_2 = Math.Max(rectangle1.Bottom, val2_2);
      RibbonTab ribbonTab = tab;
      padding = this.TabContentMargin;
      int left = padding.Left;
      int num4 = val2_2;
      padding = this.TabContentMargin;
      int top2 = padding.Top;
      int top3 = num4 + top2;
      int num5 = this.ClientSize.Width - 1;
      padding = this.TabContentMargin;
      int right1 = padding.Right;
      int right2 = num5 - right1;
      int num6 = this.ClientSize.Height - 1;
      padding = this.TabContentMargin;
      int bottom1 = padding.Bottom;
      int bottom2 = num6 - bottom1;
      Rectangle rectangle2 = Rectangle.FromLTRB(left, top3, right2, bottom2);
      ribbonTab.TabContentBounds = rectangle2;
      if (tab.Active)
        tab.UpdatePanelsRegions();
    }
label_22:
    int num7 = num1;
    Rectangle rectangle3 = this.ClientRectangle;
    int right = rectangle3.Right;
    if (num7 > right && val2_1 > 0)
    {
      num1 = this.TabsMargin.Left;
      --val2_1;
      using (List<RibbonTab>.Enumerator enumerator = this.Tabs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          RibbonTab current = enumerator.Current;
          rectangle3 = current.TabBounds;
          Padding tabsMargin;
          if (rectangle3.Width >= val2_1)
          {
            RibbonTab ribbonTab = current;
            int x = num1;
            tabsMargin = this.TabsMargin;
            int top = tabsMargin.Top;
            int width = val2_1;
            rectangle3 = current.TabBounds;
            int height = rectangle3.Height;
            Rectangle rectangle4 = new Rectangle(x, top, width, height);
            ribbonTab.TabBounds = rectangle4;
          }
          else
          {
            RibbonTab ribbonTab = current;
            int x = num1;
            tabsMargin = this.TabsMargin;
            int top = tabsMargin.Top;
            Point location = new Point(x, top);
            rectangle3 = current.TabBounds;
            Size size = rectangle3.Size;
            Rectangle rectangle5 = new Rectangle(location, size);
            ribbonTab.TabBounds = rectangle5;
          }
          rectangle3 = current.TabBounds;
          num1 = rectangle3.Right + this.TabSpacing;
        }
        goto label_22;
      }
    }
    if (flag)
      g.Dispose();
    this._lastSizeMeasured = this.Size;
    this.RenewSensor();
  }

  protected override void OnMouseDown(MouseEventArgs e)
  {
    base.OnMouseDown(e);
    this.TabHitTest(e.X, e.Y);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    if (this.ActiveTab == null)
      return;
    bool flag = false;
    if (!this.ActiveTab.TabContentBounds.Contains(e.X, e.Y))
    {
      foreach (RibbonTab tab in (List<RibbonTab>) this.Tabs)
      {
        if (tab.TabBounds.Contains(e.X, e.Y))
        {
          this.SetSelectedTab(tab);
          flag = true;
        }
      }
    }
    if (flag)
      return;
    this.SetSelectedTab((RibbonTab) null);
  }

  protected override void OnMouseWheel(MouseEventArgs e)
  {
    base.OnMouseWheel(e);
    if (this.Tabs.Count == 0 || this.ActiveTab == null)
      return;
    int num = this.Tabs.IndexOf(this.ActiveTab);
    if (e.Delta < 0)
      this._tabSum += 0.4f;
    else
      this._tabSum -= 0.4f;
    int int16 = (int) Convert.ToInt16(Math.Round((double) this._tabSum));
    if (int16 == 0)
      return;
    int index = num + int16;
    if (index < 0)
      index = 0;
    else if (index >= this.Tabs.Count - 1)
      index = this.Tabs.Count - 1;
    this.ActiveTab = this.Tabs[index];
    this._tabSum = 0.0f;
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    if (this.Size != this._lastSizeMeasured)
      this.UpdateRegions(e.Graphics);
    this.Renderer.OnRenderRibbonBackground(new RibbonRenderEventArgs(this, e.Graphics, e.ClipRectangle));
    foreach (RibbonTab tab in (List<RibbonTab>) this.Tabs)
      tab.OnPaint((object) this, new RibbonElementPaintEventArgs(tab.TabBounds, e.Graphics, RibbonElementSizeMode.None, (Control) this));
  }

  protected override void OnSizeChanged(EventArgs e)
  {
    this.UpdateRegions((Graphics) null);
    base.OnSizeChanged(e);
  }

  private void RedrawTab(RibbonTab tab)
  {
    using (Graphics graphics = this.CreateGraphics())
    {
      Rectangle rect = Rectangle.FromLTRB(tab.TabBounds.Left, tab.TabBounds.Top, tab.TabBounds.Right, tab.TabBounds.Bottom);
      graphics.SetClip(rect);
      graphics.SmoothingMode = SmoothingMode.AntiAlias;
      graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
      tab.OnPaint((object) this, new RibbonElementPaintEventArgs(tab.TabBounds, graphics, RibbonElementSizeMode.None, (Control) null));
    }
  }

  private void RenewSensor()
  {
    if (this.ActiveTab == null)
      return;
    if (this._sensor != null)
      this._sensor.Dispose();
    this._sensor = new RibbonMouseSensor((Control) this, this, this.ActiveTab);
  }

  private void SetSelectedTab(RibbonTab tab)
  {
    if (tab == this._lastSelectedTab)
      return;
    if (this._lastSelectedTab != null)
    {
      this._lastSelectedTab.Selected = false;
      this.RedrawTab(this._lastSelectedTab);
    }
    if (tab != null)
    {
      tab.Selected = true;
      this.RedrawTab(tab);
    }
    this._lastSelectedTab = tab;
  }

  public void ClearSelection()
  {
    if (this.Tabs == null)
      return;
    foreach (RibbonTab tab in (List<RibbonTab>) this.Tabs)
      tab.ClearSelection();
  }

  public void RedrawArea(Rectangle area) => this._sensor.Control.Invalidate(area);
}
