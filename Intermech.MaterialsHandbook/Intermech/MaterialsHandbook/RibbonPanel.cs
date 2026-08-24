// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonPanel
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
[Designer(typeof (RibbonPanelDesigner))]
public class RibbonPanel : 
  Component,
  IContainsSelectableRibbonItems,
  IContainsRibbonComponents,
  IRibbonElement
{
  private Ribbon _owner;
  private RibbonTab _ownerTab;
  private Image _image;
  private RibbonElementSizeMode _sizeMode;
  private RibbonPanelFlowDirection _flowsTo;
  private string _text = string.Empty;
  private bool _enabled = true;
  internal Rectangle overflowBoundsBuffer = Rectangle.Empty;
  private IContainer components;

  public event EventHandler Click;

  public event EventHandler DoubleClick;

  public event MouseEventHandler MouseDown;

  public event MouseEventHandler MouseEnter;

  public event MouseEventHandler MouseLeave;

  public event MouseEventHandler MouseMove;

  public event MouseEventHandler MouseUp;

  public event PaintEventHandler Paint;

  public event EventHandler Resize;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool Collapsed => this.SizeMode == RibbonElementSizeMode.Overflow;

  [Browsable(false)]
  public Rectangle ContentBounds { get; set; }

  [DefaultValue(true)]
  public bool Enabled
  {
    get => this._enabled;
    set
    {
      this._enabled = value;
      foreach (RibbonItem ribbonItem in (List<RibbonItem>) this.Items)
        ribbonItem.Enabled = value;
    }
  }

  [DefaultValue(RibbonPanelFlowDirection.Bottom)]
  public RibbonPanelFlowDirection FlowsTo
  {
    get => this._flowsTo;
    set
    {
      this._flowsTo = value;
      if (this.Owner == null)
        return;
      this.Owner.OnRegionsChanged();
    }
  }

  [DefaultValue(null)]
  public Image Image
  {
    get => this._image;
    set
    {
      this._image = value;
      if (this.Owner == null)
        return;
      this.Owner.OnRegionsChanged();
    }
  }

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public RibbonItemCollection Items { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool OverflowMode => this.SizeMode == RibbonElementSizeMode.Overflow;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Ribbon Owner
  {
    get => this._owner;
    set
    {
      this._owner = value;
      this.Items.Owner = value;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RibbonTab OwnerTab
  {
    get => this._ownerTab;
    set
    {
      this._ownerTab = value;
      this.Items.OwnerTab = value;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  internal Control PopUp { get; set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  internal bool PopUpShowed { get; set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool Pressed { get; set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual bool Selected { get; set; }

  public RibbonElementSizeMode SizeMode
  {
    get => this._sizeMode;
    set
    {
      this._sizeMode = value;
      foreach (RibbonItem ribbonItem in (List<RibbonItem>) this.Items)
        ribbonItem.SetSizeMode(value);
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
      if (this.Owner == null)
        return;
      this.Owner.OnRegionsChanged();
    }
  }

  public RibbonPanel()
  {
    this._sizeMode = RibbonElementSizeMode.None;
    this.ContentBounds = Rectangle.Empty;
    this.Items = new RibbonItemCollection();
    this.Bounds = Rectangle.Empty;
    this.Items.OwnerPanel = this;
  }

  public RibbonPanel(string text)
    : this()
  {
    this._text = text;
  }

  private Size MeasureSizeFlowsToBottom(object sender, RibbonElementMeasureSizeEventArgs e)
  {
    Padding padding1 = this.Owner.PanelMargin;
    int left = padding1.Left;
    padding1 = this.Owner.ItemPadding;
    int horizontal1 = padding1.Horizontal;
    int x = left + horizontal1;
    int y = this.ContentBounds.Top + this.Owner.ItemPadding.Vertical;
    int val1_1 = 0;
    int val1_2 = 0;
    foreach (RibbonItem ribbonItem in (List<RibbonItem>) this.Items)
    {
      Size size = ribbonItem.MeasureSize((object) this, new RibbonElementMeasureSizeEventArgs(e.Graphics, e.SizeMode));
      int num1 = y + size.Height;
      Rectangle contentBounds = this.ContentBounds;
      int bottom1 = contentBounds.Bottom;
      Padding itemPadding;
      if (num1 > bottom1)
      {
        contentBounds = this.ContentBounds;
        int top = contentBounds.Top;
        itemPadding = this.Owner.ItemPadding;
        int vertical = itemPadding.Vertical;
        y = top + vertical;
        int num2 = val1_1;
        itemPadding = this.Owner.ItemPadding;
        int horizontal2 = itemPadding.Horizontal;
        x = num2 + horizontal2;
      }
      Rectangle rectangle = new Rectangle(x, y, size.Width, size.Height);
      int right = rectangle.Right;
      int bottom2 = rectangle.Bottom;
      int bottom3 = rectangle.Bottom;
      itemPadding = this.Owner.ItemPadding;
      int vertical1 = itemPadding.Vertical;
      y = bottom3 + vertical1 + 1;
      val1_1 = Math.Max(val1_1, right);
      val1_2 = Math.Max(val1_2, bottom2);
    }
    int num3 = val1_1;
    Padding padding2 = this.Owner.ItemPadding;
    int right1 = padding2.Right;
    int num4 = num3 + right1;
    padding2 = this.Owner.PanelMargin;
    int right2 = padding2.Right;
    return new Size(num4 + right2 + 1, 0);
  }

  private Size MeasureSizeFlowsToRight(object sender, RibbonElementMeasureSizeEventArgs e)
  {
    int horizontal = this.Owner.PanelMargin.Horizontal;
    int val1_1 = 0;
    int val1_2 = 0;
    int num = 0;
    foreach (RibbonItem ribbonItem in (List<RibbonItem>) this.Items)
    {
      Size size = ribbonItem.MeasureSize((object) this, e);
      horizontal += size.Width + this.Owner.ItemPadding.Horizontal + 1;
      val1_1 = Math.Max(val1_1, size.Width);
      val1_2 = Math.Max(val1_2, size.Height);
    }
    switch (e.SizeMode)
    {
      case RibbonElementSizeMode.Compact:
        num = horizontal / 3;
        break;
      case RibbonElementSizeMode.Medium:
        num = horizontal / 2;
        break;
      case RibbonElementSizeMode.Large:
        num = horizontal / 1;
        break;
    }
    int val2 = num + this.Owner.PanelMargin.Horizontal;
    return new Size(Math.Max(val1_1, val2) + this.Owner.PanelMargin.Horizontal, 0);
  }

  private void ShowOverflowPopup()
  {
    Rectangle bounds = this.Bounds;
    RibbonPanelPopup ribbonPanelPopup = new RibbonPanelPopup(this);
    Point screen = this.Owner.PointToScreen(new Point(bounds.Left, bounds.Bottom));
    this.PopUpShowed = true;
    Point screenLocation = screen;
    ribbonPanelPopup.Show(screenLocation);
  }

  private void UpdateRegionsFlowsToBottom(Graphics g, RibbonElementSizeMode mode)
  {
    int x = this.ContentBounds.Left + this.Owner.ItemPadding.Horizontal;
    int y = this.ContentBounds.Top + this.Owner.ItemPadding.Vertical;
    int val2 = x;
    List<RibbonItem> items = new List<RibbonItem>();
    foreach (RibbonItem ribbonItem in (List<RibbonItem>) this.Items)
    {
      Size lastMeasuredSize = ribbonItem.LastMeasuredSize;
      int num = y + lastMeasuredSize.Height;
      Rectangle rectangle = this.ContentBounds;
      int bottom = rectangle.Bottom;
      if (num > bottom)
      {
        rectangle = this.ContentBounds;
        y = rectangle.Top + this.Owner.ItemPadding.Vertical;
        x = val2 + this.Owner.ItemPadding.Horizontal;
        this.Items.CenterItemsVerticallyInto((IEnumerable<RibbonItem>) items, this.ContentBounds);
        items.Clear();
      }
      ribbonItem.SetBounds(new Rectangle(x, y, lastMeasuredSize.Width, lastMeasuredSize.Height));
      rectangle = ribbonItem.Bounds;
      val2 = Math.Max(rectangle.Right, val2);
      rectangle = ribbonItem.Bounds;
      y = rectangle.Bottom + this.Owner.ItemPadding.Vertical + 1;
      items.Add(ribbonItem);
    }
    this.Items.CenterItemsVerticallyInto((IEnumerable<RibbonItem>) items, this.Items.GetItemsBounds());
  }

  private void UpdateRegionsFlowsToRight(Graphics g, RibbonElementSizeMode mode)
  {
    int x = this.ContentBounds.Left;
    int y = this.ContentBounds.Top;
    int num1 = mode == RibbonElementSizeMode.Medium ? 7 : 0;
    int num2 = 0;
    RibbonItem[] array = this.Items.ToArray();
    Size lastMeasuredSize;
    for (int index1 = array.Length - 1; index1 >= 0; --index1)
    {
      for (int index2 = 1; index2 <= index1; ++index2)
      {
        lastMeasuredSize = array[index2 - 1].LastMeasuredSize;
        int width1 = lastMeasuredSize.Width;
        lastMeasuredSize = array[index2].LastMeasuredSize;
        int width2 = lastMeasuredSize.Width;
        if (width1 < width2)
        {
          RibbonItem ribbonItem = array[index2 - 1];
          array[index2 - 1] = array[index2];
          array[index2] = ribbonItem;
        }
      }
    }
    List<RibbonItem> ribbonItemList = new List<RibbonItem>((IEnumerable<RibbonItem>) array);
    while (ribbonItemList.Count > 0)
    {
      RibbonItem ribbonItem = ribbonItemList[0];
      ribbonItemList.Remove(ribbonItem);
      int num3 = x;
      lastMeasuredSize = ribbonItem.LastMeasuredSize;
      int width3 = lastMeasuredSize.Width;
      int num4 = num3 + width3;
      Rectangle rectangle = this.ContentBounds;
      int right = rectangle.Right;
      Padding itemPadding;
      if (num4 > right)
      {
        rectangle = this.ContentBounds;
        x = rectangle.Left;
        int num5 = num2;
        itemPadding = this.Owner.ItemPadding;
        int vertical = itemPadding.Vertical;
        y = num5 + vertical + 1 + num1;
      }
      ribbonItem.SetBounds(new Rectangle(new Point(x, y), ribbonItem.LastMeasuredSize));
      int num6 = x;
      rectangle = ribbonItem.Bounds;
      int width4 = rectangle.Width;
      itemPadding = this.Owner.ItemPadding;
      int horizontal1 = itemPadding.Horizontal;
      int num7 = width4 + horizontal1;
      x = num6 + num7;
      int val1_1 = num2;
      rectangle = ribbonItem.Bounds;
      int bottom1 = rectangle.Bottom;
      num2 = Math.Max(val1_1, bottom1);
      rectangle = this.ContentBounds;
      int num8 = rectangle.Right - x;
      for (int index = 0; index < ribbonItemList.Count; ++index)
      {
        lastMeasuredSize = ribbonItemList[index].LastMeasuredSize;
        if (lastMeasuredSize.Width < num8)
        {
          ribbonItemList[index].SetBounds(new Rectangle(new Point(x, y), ribbonItemList[index].LastMeasuredSize));
          int num9 = x;
          rectangle = ribbonItemList[index].Bounds;
          int width5 = rectangle.Width;
          itemPadding = this.Owner.ItemPadding;
          int horizontal2 = itemPadding.Horizontal;
          int num10 = width5 + horizontal2;
          x = num9 + num10;
          int val1_2 = num2;
          rectangle = ribbonItemList[index].Bounds;
          int bottom2 = rectangle.Bottom;
          num2 = Math.Max(val1_2, bottom2);
          rectangle = this.ContentBounds;
          num8 = rectangle.Right - x;
          ribbonItemList.RemoveAt(index);
          index = 0;
        }
      }
    }
  }

  public void ClearSelection()
  {
    this.Selected = false;
    if (this.Items == null)
      return;
    foreach (RibbonItem ribbonItem in (List<RibbonItem>) this.Items)
      ribbonItem.ClearSelection();
  }

  public Size SwitchToSize(Control ctrl, Graphics g, RibbonElementSizeMode size)
  {
    Size size1 = this.MeasureSize((object) this, new RibbonElementMeasureSizeEventArgs(g, size));
    this.SetBounds(new Rectangle(0, 0, size1.Width, size1.Height));
    this.UpdateItemsRegions(g, size);
    return size1;
  }

  public void UpdateItemsRegions(Graphics g, RibbonElementSizeMode mode)
  {
    switch (this.FlowsTo)
    {
      case RibbonPanelFlowDirection.Bottom:
        this.UpdateRegionsFlowsToBottom(g, mode);
        break;
      case RibbonPanelFlowDirection.Right:
        this.UpdateRegionsFlowsToRight(g, mode);
        break;
    }
    this.Items.CenterItemsInto(this.ContentBounds);
  }

  public virtual void OnClick(EventArgs e)
  {
    if (!this.Enabled)
      return;
    EventHandler click = this.Click;
    if (click != null)
      click((object) this, e);
    if (!this.Collapsed || this.PopUp != null)
      return;
    this.ShowOverflowPopup();
  }

  public virtual void OnDoubleClick(EventArgs e)
  {
    if (!this.Enabled)
      return;
    EventHandler doubleClick = this.DoubleClick;
    if (doubleClick == null)
      return;
    doubleClick((object) this, e);
  }

  public virtual void OnMouseDown(MouseEventArgs e)
  {
    if (!this.Enabled)
      return;
    MouseEventHandler mouseDown = this.MouseDown;
    if (mouseDown != null)
      mouseDown((object) this, e);
    this.Pressed = true;
  }

  public virtual void OnMouseEnter(MouseEventArgs e)
  {
    if (!this.Enabled)
      return;
    MouseEventHandler mouseEnter = this.MouseEnter;
    if (mouseEnter == null)
      return;
    mouseEnter((object) this, e);
  }

  public virtual void OnMouseLeave(MouseEventArgs e)
  {
    if (!this.Enabled)
      return;
    MouseEventHandler mouseLeave = this.MouseLeave;
    if (mouseLeave == null)
      return;
    mouseLeave((object) this, e);
  }

  public virtual void OnMouseMove(MouseEventArgs e)
  {
    if (!this.Enabled)
      return;
    MouseEventHandler mouseMove = this.MouseMove;
    if (mouseMove == null)
      return;
    mouseMove((object) this, e);
  }

  public virtual void OnMouseUp(MouseEventArgs e)
  {
    if (!this.Enabled)
      return;
    MouseEventHandler mouseUp = this.MouseUp;
    if (mouseUp != null)
      mouseUp((object) this, e);
    this.Pressed = false;
  }

  protected virtual void OnResize(EventArgs e)
  {
    EventHandler resize = this.Resize;
    if (resize == null)
      return;
    resize((object) this, e);
  }

  public IEnumerable<RibbonItem> GetItems() => (IEnumerable<RibbonItem>) this.Items;

  public IEnumerable<Component> GetAllChildComponents()
  {
    return (IEnumerable<Component>) this.Items.ToArray();
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Rectangle Bounds { get; private set; }

  public Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
  {
    Size empty = Size.Empty;
    int height = this.OwnerTab.TabContentBounds.Height - this.Owner.PanelPadding.Vertical;
    empty.Width = e.Graphics.MeasureString(this.Text, this.Owner.Font).ToSize().Width + this.Owner.PanelMargin.Horizontal + 1;
    Size size1;
    if (e.SizeMode == RibbonElementSizeMode.Overflow)
    {
      size1 = new Size(RibbonButton.MeasureStringLargeSize(e.Graphics, this.Text, this.Owner.Font).Width + this.Owner.PanelMargin.Horizontal, height);
    }
    else
    {
      Size size2;
      switch (this.FlowsTo)
      {
        case RibbonPanelFlowDirection.Bottom:
          size2 = this.MeasureSizeFlowsToBottom(sender, e);
          break;
        case RibbonPanelFlowDirection.Right:
          size2 = this.MeasureSizeFlowsToRight(sender, e);
          break;
        default:
          size2 = Size.Empty;
          break;
      }
      size1 = new Size(Math.Max(size2.Width, empty.Width), height);
    }
    return size1;
  }

  public virtual void OnPaint(object sender, RibbonElementPaintEventArgs e)
  {
    PaintEventHandler paint = this.Paint;
    if (paint != null)
      paint((object) this, new PaintEventArgs(e.Graphics, e.Clip));
    if (this.PopUpShowed && e.Control == this.Owner)
    {
      RibbonPanel panel = new RibbonPanel(this.Text)
      {
        Image = this.Image,
        SizeMode = RibbonElementSizeMode.Overflow
      };
      panel.SetBounds(this.overflowBoundsBuffer);
      panel.Pressed = true;
      panel.Owner = this.Owner;
      this.Owner.Renderer.OnRenderRibbonPanelBackground(new RibbonPanelRenderEventArgs(this.Owner, e.Graphics, e.Clip, panel, e.Control));
      this.Owner.Renderer.OnRenderRibbonPanelText(new RibbonPanelRenderEventArgs(this.Owner, e.Graphics, e.Clip, panel, e.Control));
    }
    else
    {
      this.Owner.Renderer.OnRenderRibbonPanelBackground(new RibbonPanelRenderEventArgs(this.Owner, e.Graphics, e.Clip, this, e.Control));
      this.Owner.Renderer.OnRenderRibbonPanelText(new RibbonPanelRenderEventArgs(this.Owner, e.Graphics, e.Clip, this, e.Control));
    }
    if (e.Mode == RibbonElementSizeMode.Overflow && (e.Control == null || e.Control != this.PopUp))
      return;
    foreach (RibbonItem ribbonItem in (List<RibbonItem>) this.Items)
      ribbonItem.OnPaint((object) this, new RibbonElementPaintEventArgs(ribbonItem.Bounds, e.Graphics, ribbonItem.SizeMode, (Control) null));
  }

  public void SetBounds(Rectangle bounds)
  {
    this.Bounds = bounds;
    this.OnResize(EventArgs.Empty);
    if (this.Owner == null)
      return;
    this.ContentBounds = Rectangle.FromLTRB(bounds.X + this.Owner.PanelMargin.Left, bounds.Y + this.Owner.PanelMargin.Top, bounds.Right - this.Owner.PanelMargin.Right, bounds.Bottom - this.Owner.PanelMargin.Bottom);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent() => this.components = (IContainer) new System.ComponentModel.Container();
}
