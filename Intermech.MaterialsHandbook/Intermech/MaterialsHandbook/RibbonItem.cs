// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonItem
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

[DesignTimeVisible(false)]
public class RibbonItem : Component, IRibbonElement
{
  private Control _canvas;
  private RibbonElementSizeMode _maxSize;
  private RibbonElementSizeMode _minSize;
  private string _text = string.Empty;
  private Image _image;
  private bool _enabled = true;
  private IContainer components;

  public event EventHandler CanvasChanged;

  public event EventHandler Click;

  public event EventHandler DoubleClick;

  public event MouseEventHandler MouseDown;

  public event MouseEventHandler MouseEnter;

  public event MouseEventHandler MouseLeave;

  public event MouseEventHandler MouseMove;

  public event MouseEventHandler MouseUp;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Control Canvas
  {
    get => this._canvas == null || this._canvas.IsDisposed ? (Control) this.Owner : this._canvas;
    set
    {
      this._canvas = value;
      this.OnCanvasChanged(EventArgs.Empty);
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual Rectangle ContentBounds
  {
    get
    {
      Rectangle bounds = this.Bounds;
      int left = bounds.Left + this.Owner.ItemMargin.Left;
      bounds = this.Bounds;
      int top = bounds.Top + this.Owner.ItemMargin.Top;
      bounds = this.Bounds;
      int right = bounds.Right - this.Owner.ItemMargin.Right;
      bounds = this.Bounds;
      int bottom = bounds.Bottom - this.Owner.ItemMargin.Bottom;
      return Rectangle.FromLTRB(left, top, right, bottom);
    }
  }

  [DefaultValue(true)]
  public virtual bool Enabled
  {
    get => this._enabled;
    set
    {
      this._enabled = value;
      this.NotifyOwnerRegionsChanged();
    }
  }

  public virtual Image Image
  {
    get => this._image;
    set
    {
      this._image = value;
      this.NotifyOwnerRegionsChanged();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Size LastMeasuredSize { get; set; }

  [DefaultValue(RibbonElementSizeMode.None)]
  public RibbonElementSizeMode MaxSizeMode
  {
    get => this._maxSize;
    set
    {
      this._maxSize = value;
      this.NotifyOwnerRegionsChanged();
    }
  }

  [DefaultValue(RibbonElementSizeMode.None)]
  public RibbonElementSizeMode MinSizeMode
  {
    get => this._minSize;
    set
    {
      this._minSize = value;
      this.NotifyOwnerRegionsChanged();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Ribbon Owner { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RibbonPanel OwnerPanel { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual bool Pressed { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual bool Selected { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RibbonElementSizeMode SizeMode { get; private set; }

  public object Tag { get; set; }

  [DefaultValue("")]
  [Localizable(true)]
  public virtual string Text
  {
    get => this._text;
    set
    {
      this._text = value;
      this.NotifyOwnerRegionsChanged();
    }
  }

  [DefaultValue("")]
  [Localizable(true)]
  public virtual string ToolTip { get; set; }

  [DefaultValue(true)]
  public bool Visible { get; set; }

  public RibbonItem()
  {
    this.LastMeasuredSize = Size.Empty;
    this.SizeMode = RibbonElementSizeMode.None;
    this.Bounds = Rectangle.Empty;
    this.Visible = true;
  }

  protected RibbonElementSizeMode GetNearestSize(RibbonElementSizeMode sizeMode)
  {
    int num = (int) sizeMode;
    int maxSizeMode = (int) this.MaxSizeMode;
    int minSizeMode = (int) this.MinSizeMode;
    int nearestSize = (int) sizeMode;
    if (maxSizeMode > 0 && num > maxSizeMode)
      nearestSize = maxSizeMode;
    if (minSizeMode > 0 && num < minSizeMode)
      nearestSize = minSizeMode;
    return (RibbonElementSizeMode) nearestSize;
  }

  public virtual void ClearSelection() => this.Selected = false;

  protected virtual bool ClosesDropDownAt(Point p) => true;

  public virtual void OnCanvasChanged(EventArgs e)
  {
    EventHandler canvasChanged = this.CanvasChanged;
    if (canvasChanged == null)
      return;
    canvasChanged((object) this, e);
  }

  public virtual void OnClick(EventArgs e)
  {
    if (!this.Enabled)
      return;
    EventHandler click = this.Click;
    if (click == null)
      return;
    click((object) this, e);
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
    if (this.Canvas is RibbonPopup)
    {
      if (this.ClosesDropDownAt(e.Location))
        RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.ItemClicked);
      this.OnClick(EventArgs.Empty);
    }
    this.SetPressed(true);
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
    if (!this.Pressed)
      return;
    this.SetPressed(false);
    this.RedrawItem();
  }

  public virtual void RedrawItem() => this.Canvas?.Invalidate(Rectangle.Inflate(this.Bounds, 1, 1));

  internal virtual void SetOwner(Ribbon owner) => this.Owner = owner;

  internal virtual void SetOwnerItem(RibbonItem item)
  {
  }

  internal virtual void SetOwnerPanel(RibbonPanel ownerPanel) => this.OwnerPanel = ownerPanel;

  internal virtual void SetOwnerTab(RibbonTab ownerTab)
  {
  }

  internal virtual void SetPressed(bool pressed) => this.Pressed = pressed;

  internal virtual void SetSelected(bool selected)
  {
    if (!this.Enabled)
      return;
    this.Selected = selected;
  }

  internal virtual void SetSizeMode(RibbonElementSizeMode sizeMode)
  {
    this.SizeMode = this.GetNearestSize(sizeMode);
  }

  protected void NotifyOwnerRegionsChanged()
  {
    if (this.Owner == null)
      return;
    if (this.Owner == this.Canvas)
      this.Owner.OnRegionsChanged();
    else
      this.Canvas?.Invalidate(this.Bounds);
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Rectangle Bounds { get; private set; }

  public virtual Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e) => Size.Empty;

  public virtual void OnPaint(object sender, RibbonElementPaintEventArgs e)
  {
  }

  public virtual void SetBounds(Rectangle bounds) => this.Bounds = bounds;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent() => this.components = (IContainer) new System.ComponentModel.Container();
}
