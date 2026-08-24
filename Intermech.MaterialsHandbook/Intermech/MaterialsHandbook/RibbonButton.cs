// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonButton
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

[Designer(typeof (RibbonButtonDesigner))]
public class RibbonButton : RibbonItem, IContainsRibbonComponents
{
  private const int arrowWidth = 5;
  private RibbonButtonStyle _style;
  private Rectangle _imageBounds = Rectangle.Empty;
  private Image _smallImage;
  private Padding _dropDownMargin = new Padding(6);
  private Size _dropDownArrowSize = new Size(5, 3);
  private Point _lastMousePos = Point.Empty;
  private IContainer components;

  public event EventHandler DropDownShowing;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public Rectangle ButtonFaceBounds { get; private set; }

  internal RibbonDropDown DropDown { get; private set; }

  [Browsable(false)]
  public Size DropDownArrowSize
  {
    get => this._dropDownArrowSize;
    set
    {
      this._dropDownArrowSize = value;
      this.NotifyOwnerRegionsChanged();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public Rectangle DropDownBounds { get; private set; }

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public RibbonItemCollection DropDownItems { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool DropDownPressed { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool DropDownSelected { get; private set; }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool DropDownVisible { get; set; }

  [DefaultValue(null)]
  public virtual Image SmallImage
  {
    get => this._smallImage;
    set
    {
      this._smallImage = value;
      this.NotifyOwnerRegionsChanged();
    }
  }

  public RibbonButtonStyle Style
  {
    get => this._style;
    set
    {
      this._style = value;
      this.NotifyOwnerRegionsChanged();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Rectangle TextBounds { get; private set; }

  public RibbonButton()
  {
    this.Image = this.CreateImage(32 /*0x20*/);
    this.SmallImage = this.CreateImage(16 /*0x10*/);
    this.DropDownItems = new RibbonItemCollection();
    this.ButtonFaceBounds = Rectangle.Empty;
    this.DropDownBounds = Rectangle.Empty;
    this.TextBounds = Rectangle.Empty;
  }

  public static Size MeasureStringLargeSize(Graphics g, string text, Font font)
  {
    Size size1 = Size.Empty;
    if (!string.IsNullOrEmpty(text))
    {
      Size size2 = g.MeasureString(text, font).ToSize();
      string[] strArray = text.Split(' ');
      string empty = string.Empty;
      int width1 = size2.Width;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].Length > empty.Length)
          empty = strArray[index];
      }
      if (strArray.Length > 1)
      {
        int val1 = size2.Width / 2;
        SizeF sizeF = g.MeasureString(empty, font);
        int width2 = sizeF.ToSize().Width;
        int width3 = Math.Max(val1, width2) + 1;
        sizeF = g.MeasureString(text, font, width3);
        Size size3 = sizeF.ToSize();
        size1 = new Size(size3.Width, size3.Height);
      }
      else
        size1 = g.MeasureString(text, font).ToSize();
    }
    return size1;
  }

  private void OnDropDown_Closed(object sender, EventArgs e)
  {
    this.SetPressed(false);
    this.DropDownPressed = this.DropDownVisible = false;
    this.SetSelected(false);
    this.RedrawItem();
  }

  private void OnDropDown_MouseEnter(object sender, EventArgs e)
  {
    this.SetSelected(true);
    this.RedrawItem();
  }

  internal override void SetOwner(Ribbon owner)
  {
    base.SetOwner(owner);
    if (this.DropDownItems == null)
      return;
    this.DropDownItems.Owner = owner;
  }

  internal override void SetOwnerPanel(RibbonPanel ownerPanel)
  {
    base.SetOwnerPanel(ownerPanel);
    if (this.DropDownItems == null)
      return;
    this.DropDownItems.OwnerPanel = ownerPanel;
  }

  internal override void SetOwnerTab(RibbonTab ownerTab)
  {
    base.SetOwnerTab(ownerTab);
    if (this.DropDownItems == null)
      return;
    this.DropDownItems.OwnerTab = ownerTab;
  }

  public override void ClearSelection()
  {
    if (this.DropDown != null)
      return;
    this.SetSelected(false);
  }

  protected override bool ClosesDropDownAt(Point p) => this.Style != RibbonButtonStyle.DropDown;

  public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
  {
    RibbonElementSizeMode nearestSize = this.GetNearestSize(e.SizeMode);
    int horizontal = this.Owner.ItemMargin.Horizontal;
    int vertical = this.Owner.ItemMargin.Vertical;
    int num = this.OwnerPanel == null ? 0 : this.OwnerPanel.ContentBounds.Height - this.Owner.ItemPadding.Vertical;
    Size size1 = this.SmallImage != null ? this.SmallImage.Size : Size.Empty;
    Size size2 = this.Image != null ? this.Image.Size : Size.Empty;
    Size empty = Size.Empty;
    int width;
    int height;
    switch (nearestSize)
    {
      case RibbonElementSizeMode.Overflow:
      case RibbonElementSizeMode.Large:
        Size size3 = RibbonButton.MeasureStringLargeSize(e.Graphics, this.Text, this.Owner.Font);
        if (!string.IsNullOrEmpty(this.Text))
        {
          width = horizontal + Math.Max(size3.Width + 1, size2.Width);
          height = num;
          break;
        }
        width = horizontal + size2.Width;
        height = vertical + size2.Height;
        break;
      case RibbonElementSizeMode.Compact:
        width = horizontal + size1.Width;
        height = vertical + size1.Height;
        break;
      case RibbonElementSizeMode.Medium:
      case RibbonElementSizeMode.DropDown:
        Size size4 = TextRenderer.MeasureText(this.Text, this.Owner.Font);
        if (!string.IsNullOrEmpty(this.Text))
          horizontal += size4.Width + 1;
        width = horizontal + (size1.Width + this.Owner.ItemMargin.Horizontal);
        height = vertical + Math.Max(size4.Height, size1.Height);
        break;
      default:
        throw new ApplicationException("SizeMode not supported: " + e.SizeMode.ToString());
    }
    if (nearestSize == RibbonElementSizeMode.DropDown)
      height += 2;
    if (this.Style == RibbonButtonStyle.DropDown)
      width += 5 + this.Owner.ItemMargin.Right;
    this.LastMeasuredSize = new Size(width, height);
    return this.LastMeasuredSize;
  }

  public override void OnClick(EventArgs e)
  {
    if (this.Style != RibbonButtonStyle.Normal && !this.ButtonFaceBounds.Contains(this._lastMousePos))
      return;
    base.OnClick(e);
  }

  public override void OnMouseDown(MouseEventArgs e)
  {
    if (!this.Enabled)
      return;
    if ((this.DropDownSelected || this.Style == RibbonButtonStyle.DropDown) && this.DropDownItems.Count > 0)
    {
      this.DropDownPressed = true;
      this.ShowDropDown();
    }
    base.OnMouseDown(e);
  }

  public override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    this.DropDownSelected = false;
  }

  public override void OnMouseMove(MouseEventArgs e)
  {
    if (!this.Enabled)
      return;
    this._lastMousePos = new Point(e.X, e.Y);
    base.OnMouseMove(e);
  }

  public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
  {
    if (this.Owner == null)
      return;
    this.OnPaintBackground(e);
    this.OnPaintImage(e);
    this.OnPaintText(e);
  }

  public override void SetBounds(Rectangle bounds)
  {
    base.SetBounds(bounds);
    RibbonElementSizeMode nearestSize = this.GetNearestSize(this.SizeMode);
    this._imageBounds = this.OnGetImageBounds(nearestSize, bounds);
    this.TextBounds = this.OnGetTextBounds(nearestSize, bounds);
  }

  internal override void SetSelected(bool selected)
  {
    base.SetSelected(selected);
    this.SetPressed(false);
  }

  internal override void SetSizeMode(RibbonElementSizeMode sizeMode)
  {
    base.SetSizeMode(sizeMode == RibbonElementSizeMode.Overflow ? RibbonElementSizeMode.Large : sizeMode);
  }

  public override string ToString()
  {
    return string.Format("{1}: {0}", (object) this.Text, (object) this.GetType().Name);
  }

  private Image CreateImage(int size) => (Image) new Bitmap(size, size);

  private void OnDropDownShowing(EventArgs e)
  {
    EventHandler dropDownShowing = this.DropDownShowing;
    if (dropDownShowing == null)
      return;
    dropDownShowing((object) this, e);
  }

  private void OnPaintBackground(RibbonElementPaintEventArgs e)
  {
    this.Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(this.Owner, e.Graphics, e.Clip, (RibbonItem) this));
  }

  private void OnPaintImage(RibbonElementPaintEventArgs e)
  {
    RibbonElementSizeMode nearestSize = this.GetNearestSize(e.Mode);
    if ((nearestSize != RibbonElementSizeMode.Large || this.Image == null) && this.SmallImage == null)
      return;
    this.Owner.Renderer.OnRenderRibbonItemImage(new RibbonItemBoundsEventArgs(this.Owner, e.Graphics, e.Clip, (RibbonItem) this, this.OnGetImageBounds(nearestSize, this.Bounds)));
  }

  protected virtual void OnPaintText(RibbonElementPaintEventArgs e)
  {
    if (this.SizeMode == RibbonElementSizeMode.Compact)
      return;
    StringFormat format = new StringFormat()
    {
      LineAlignment = StringAlignment.Center,
      Alignment = StringAlignment.Near
    };
    if (this.SizeMode == RibbonElementSizeMode.Large)
    {
      format.Alignment = StringAlignment.Center;
      if (!string.IsNullOrEmpty(this.Text) && !this.Text.Contains(" "))
        format.LineAlignment = StringAlignment.Near;
    }
    this.Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(this.Owner, e.Graphics, e.Clip, (RibbonItem) this, this.TextBounds, this.Text, format));
  }

  protected virtual void CreateDropDown()
  {
    this.DropDown = new RibbonDropDown((RibbonItem) this, (IEnumerable<RibbonItem>) this.DropDownItems, this.Owner);
  }

  public void CloseDropDown()
  {
    if (this.DropDown != null)
      RibbonPopupManager.Dismiss((RibbonPopup) this.DropDown, RibbonPopupManager.DismissReason.NewPopup);
    this.DropDownVisible = false;
  }

  public void ShowDropDown()
  {
    if (this.Style == RibbonButtonStyle.Normal || this.DropDownItems.Count == 0)
    {
      if (this.DropDown == null)
        return;
      RibbonPopupManager.DismissChildren((RibbonPopup) this.DropDown, RibbonPopupManager.DismissReason.NewPopup);
    }
    else
    {
      if (this.Style == RibbonButtonStyle.DropDown)
        this.SetPressed(true);
      else
        this.DropDownPressed = true;
      this.CreateDropDown();
      this.DropDown.MouseEnter += new EventHandler(this.OnDropDown_MouseEnter);
      this.DropDown.Closed += new EventHandler(this.OnDropDown_Closed);
      Control canvas = this.Canvas;
      Point downMenuLocation = this.OnGetDropDownMenuLocation();
      Size dropDownMenuSize = this.OnGetDropDownMenuSize();
      if (!dropDownMenuSize.IsEmpty)
        this.DropDown.MinimumSize = dropDownMenuSize;
      this.OnDropDownShowing(EventArgs.Empty);
      this.DropDownVisible = true;
      this.DropDown.SelectionService = this.GetService(typeof (ISelectionService)) as ISelectionService;
      this.DropDown.Show(downMenuLocation);
      this.DropDown.Focus();
    }
  }

  internal virtual Rectangle OnGetDropDownBounds(RibbonElementSizeMode sMode, Rectangle bounds)
  {
    Rectangle dropDownBounds = Rectangle.Empty;
    switch (this.SizeMode)
    {
      case RibbonElementSizeMode.Overflow:
      case RibbonElementSizeMode.Large:
        dropDownBounds = Rectangle.FromLTRB(bounds.Left, bounds.Top + this.Image.Height + this.Owner.ItemMargin.Vertical, bounds.Right, bounds.Bottom);
        break;
      case RibbonElementSizeMode.Compact:
      case RibbonElementSizeMode.Medium:
      case RibbonElementSizeMode.DropDown:
        dropDownBounds = Rectangle.FromLTRB(bounds.Right - this._dropDownMargin.Horizontal - 2, bounds.Top, bounds.Right, bounds.Bottom);
        break;
    }
    return dropDownBounds;
  }

  internal virtual Point OnGetDropDownMenuLocation()
  {
    Point empty = Point.Empty;
    Point screen;
    if (this.Canvas is RibbonDropDown)
    {
      Control canvas = this.Canvas;
      Rectangle bounds = this.Bounds;
      int right = bounds.Right;
      bounds = this.Bounds;
      int top = bounds.Top;
      Point p = new Point(right, top);
      screen = canvas.PointToScreen(p);
    }
    else
    {
      Control canvas = this.Canvas;
      Rectangle bounds = this.Bounds;
      int left = bounds.Left;
      bounds = this.Bounds;
      int bottom = bounds.Bottom;
      Point p = new Point(left, bottom);
      screen = canvas.PointToScreen(p);
    }
    return screen;
  }

  internal virtual Size OnGetDropDownMenuSize() => Size.Empty;

  internal virtual Rectangle OnGetImageBounds(RibbonElementSizeMode sMode, Rectangle bounds)
  {
    Rectangle imageBounds = Rectangle.Empty;
    if (sMode == RibbonElementSizeMode.Large)
    {
      if (this.Image != null)
      {
        ref Rectangle local = ref imageBounds;
        Rectangle bounds1 = this.Bounds;
        int left = bounds1.Left;
        bounds1 = this.Bounds;
        int num = (bounds1.Width - this.Image.Width) / 2;
        int x = left + num;
        bounds1 = this.Bounds;
        int y = bounds1.Top + this.Owner.ItemMargin.Top + 4;
        int width = this.Image.Width;
        int height = this.Image.Height;
        local = new Rectangle(x, y, width, height);
      }
      else
        imageBounds = new Rectangle(this.ContentBounds.Location, new Size(32 /*0x20*/, 32 /*0x20*/));
    }
    else if (this.SmallImage != null)
    {
      ref Rectangle local = ref imageBounds;
      Rectangle bounds2 = this.Bounds;
      int x = bounds2.Left + this.Owner.ItemMargin.Left;
      bounds2 = this.Bounds;
      int top = bounds2.Top;
      bounds2 = this.Bounds;
      int num = (bounds2.Height - this.SmallImage.Height) / 2;
      int y = top + num;
      int width = this.SmallImage.Width;
      int height = this.SmallImage.Height;
      local = new Rectangle(x, y, width, height);
    }
    else
      imageBounds = new Rectangle(this.ContentBounds.Location, new Size(0, 0));
    return imageBounds;
  }

  internal virtual Rectangle OnGetTextBounds(RibbonElementSizeMode sMode, Rectangle bounds)
  {
    Rectangle empty = Rectangle.Empty;
    int width = this._imageBounds.Width;
    int height = this._imageBounds.Height;
    Rectangle textBounds;
    if (sMode == RibbonElementSizeMode.Large)
    {
      int left1 = this.Bounds.Left;
      Padding itemMargin = this.Owner.ItemMargin;
      int left2 = itemMargin.Left;
      int left3 = left1 + left2;
      Rectangle bounds1 = this.Bounds;
      int top1 = bounds1.Top;
      itemMargin = this.Owner.ItemMargin;
      int top2 = itemMargin.Top;
      int top3 = top1 + top2 + 24;
      bounds1 = this.Bounds;
      int right1 = bounds1.Right;
      itemMargin = this.Owner.ItemMargin;
      int right2 = itemMargin.Right;
      int right3 = right1 - right2;
      bounds1 = this.Bounds;
      int bottom1 = bounds1.Bottom;
      itemMargin = this.Owner.ItemMargin;
      int bottom2 = itemMargin.Bottom;
      int bottom3 = bottom1 - bottom2;
      textBounds = Rectangle.FromLTRB(left3, top3, right3, bottom3);
    }
    else
    {
      int horizontal1 = this.Style != RibbonButtonStyle.Normal ? this._dropDownMargin.Horizontal : 0;
      int num1 = this.Bounds.Left + width;
      Padding itemMargin = this.Owner.ItemMargin;
      int horizontal2 = itemMargin.Horizontal;
      int num2 = num1 + horizontal2;
      itemMargin = this.Owner.ItemMargin;
      int left4 = itemMargin.Left;
      int left5 = num2 + left4;
      int top4 = this.Bounds.Top;
      itemMargin = this.Owner.ItemMargin;
      int top5 = itemMargin.Top;
      int top6 = top4 + top5;
      int right = this.Bounds.Right - horizontal1;
      int bottom4 = this.Bounds.Bottom;
      itemMargin = this.Owner.ItemMargin;
      int bottom5 = itemMargin.Bottom;
      int bottom6 = bottom4 - bottom5;
      textBounds = Rectangle.FromLTRB(left5, top6, right, bottom6);
    }
    return textBounds;
  }

  public IEnumerable<Component> GetAllChildComponents()
  {
    return (IEnumerable<Component>) this.DropDownItems.ToArray();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent() => this.components = (IContainer) new System.ComponentModel.Container();
}
