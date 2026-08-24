// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonItemCollection
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

#nullable disable
namespace Intermech.MaterialsHandbook;

[Editor("Intermech.MaterialsHandbook.RibbonItemCollectionEditor", typeof (UITypeEditor))]
public class RibbonItemCollection : List<RibbonItem>
{
  private Ribbon _owner;
  private RibbonTab _ownerTab;
  private RibbonPanel _ownerPanel;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Ribbon Owner
  {
    get => this._owner;
    set
    {
      this._owner = value;
      foreach (RibbonItem ribbonItem in (List<RibbonItem>) this)
        ribbonItem.SetOwner(value);
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public RibbonPanel OwnerPanel
  {
    get => this._ownerPanel;
    set
    {
      this._ownerPanel = value;
      foreach (RibbonItem ribbonItem in (List<RibbonItem>) this)
        ribbonItem.SetOwnerPanel(value);
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
      foreach (RibbonItem ribbonItem in (List<RibbonItem>) this)
        ribbonItem.SetOwnerTab(value);
    }
  }

  internal RibbonItemCollection()
  {
  }

  public new void Add(RibbonItem item)
  {
    item.SetOwner(this.Owner);
    item.SetOwnerPanel(this.OwnerPanel);
    item.SetOwnerTab(this.OwnerTab);
    base.Add(item);
  }

  public new void AddRange(IEnumerable<RibbonItem> items)
  {
    foreach (RibbonItem ribbonItem in items)
    {
      ribbonItem.SetOwner(this.Owner);
      ribbonItem.SetOwnerPanel(this.OwnerPanel);
      ribbonItem.SetOwnerTab(this.OwnerTab);
    }
    base.AddRange(items);
  }

  public new void Insert(int index, RibbonItem item)
  {
    item.SetOwner(this.Owner);
    item.SetOwnerPanel(this.OwnerPanel);
    item.SetOwnerTab(this.OwnerTab);
    base.Insert(index, item);
  }

  internal void CenterItemsHorizontallyInto(Rectangle rectangle)
  {
    this.CenterItemsHorizontallyInto((IEnumerable<RibbonItem>) this, rectangle);
  }

  internal void CenterItemsHorizontallyInto(IEnumerable<RibbonItem> items, Rectangle rectangle)
  {
    int x = rectangle.Left + (rectangle.Width - this.GetItemsWidth(items)) / 2;
    int itemsTop = this.GetItemsTop(items);
    this.MoveTo(items, new Point(x, itemsTop));
  }

  internal void CenterItemsInto(Rectangle rectangle)
  {
    this.CenterItemsInto((IEnumerable<RibbonItem>) this, rectangle);
  }

  internal void CenterItemsInto(IEnumerable<RibbonItem> items, Rectangle rectangle)
  {
    int x = rectangle.Left + (rectangle.Width - this.GetItemsWidth((IEnumerable<RibbonItem>) this)) / 2;
    int y = rectangle.Top + (rectangle.Height - this.GetItemsHeight((IEnumerable<RibbonItem>) this)) / 2;
    this.MoveTo(items, new Point(x, y));
  }

  internal void CenterItemsVerticallyInto(Rectangle rectangle)
  {
    this.CenterItemsVerticallyInto((IEnumerable<RibbonItem>) this, rectangle);
  }

  internal void CenterItemsVerticallyInto(IEnumerable<RibbonItem> items, Rectangle rectangle)
  {
    int itemsLeft = this.GetItemsLeft(items);
    int y = rectangle.Top + (rectangle.Height - this.GetItemsHeight(items)) / 2;
    this.MoveTo(items, new Point(itemsLeft, y));
  }

  internal int GetItemsBottom(IEnumerable<RibbonItem> items)
  {
    int itemsBottom = 0;
    if (this.Count > 0)
    {
      int num = int.MinValue;
      foreach (RibbonItem ribbonItem in items)
      {
        if (ribbonItem.Bounds.Bottom > num)
          num = ribbonItem.Bounds.Bottom;
      }
      itemsBottom = num;
    }
    return itemsBottom;
  }

  internal Rectangle GetItemsBounds() => this.GetItemsBounds((IEnumerable<RibbonItem>) this);

  internal Rectangle GetItemsBounds(IEnumerable<RibbonItem> items)
  {
    return Rectangle.FromLTRB(this.GetItemsLeft(items), this.GetItemsTop(items), this.GetItemsRight(items), this.GetItemsBottom(items));
  }

  internal int GetItemsHeight(IEnumerable<RibbonItem> items)
  {
    return this.GetItemsBottom(items) - this.GetItemsTop(items);
  }

  internal int GetItemsLeft(IEnumerable<RibbonItem> items)
  {
    int itemsLeft = 0;
    if (this.Count > 0)
    {
      int num = int.MaxValue;
      foreach (RibbonItem ribbonItem in items)
      {
        if (ribbonItem.Bounds.X < num)
          num = ribbonItem.Bounds.X;
      }
      itemsLeft = num;
    }
    return itemsLeft;
  }

  internal int GetItemsRight(IEnumerable<RibbonItem> items)
  {
    int itemsRight = 0;
    if (this.Count > 0)
    {
      int num = int.MinValue;
      foreach (RibbonItem ribbonItem in items)
      {
        if (ribbonItem.Bounds.Right > num)
          num = ribbonItem.Bounds.Right;
      }
      itemsRight = num;
    }
    return itemsRight;
  }

  internal int GetItemsTop(IEnumerable<RibbonItem> items)
  {
    int itemsTop = 0;
    if (this.Count > 0)
    {
      int num = int.MaxValue;
      foreach (RibbonItem ribbonItem in items)
      {
        if (ribbonItem.Bounds.Y < num)
          num = ribbonItem.Bounds.Y;
      }
      itemsTop = num;
    }
    return itemsTop;
  }

  internal int GetItemsWidth(IEnumerable<RibbonItem> items)
  {
    return this.GetItemsRight(items) - this.GetItemsLeft(items);
  }

  internal void MoveTo(Point p) => this.MoveTo((IEnumerable<RibbonItem>) this, p);

  internal void MoveTo(IEnumerable<RibbonItem> items, Point p)
  {
    Rectangle itemsBounds = this.GetItemsBounds(items);
    foreach (RibbonItem ribbonItem1 in items)
    {
      Rectangle bounds1 = ribbonItem1.Bounds;
      int num1 = bounds1.X - itemsBounds.Left;
      bounds1 = ribbonItem1.Bounds;
      int num2 = bounds1.Y - itemsBounds.Top;
      RibbonItem ribbonItem2 = ribbonItem1;
      Point location = new Point(p.X + num1, p.Y + num2);
      bounds1 = ribbonItem1.Bounds;
      Size size = bounds1.Size;
      Rectangle bounds2 = new Rectangle(location, size);
      ribbonItem2.SetBounds(bounds2);
    }
  }
}
