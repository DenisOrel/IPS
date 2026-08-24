// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.CustomListBox
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics;

public sealed class CustomListBox : ListBox
{
  private readonly Dictionary<object, Icon> _itemsIconCache = new Dictionary<object, Icon>();
  public List<Icon> MyImageList;

  public CustomListBox() => this.DrawMode = DrawMode.OwnerDrawFixed;

  protected override void OnDrawItem(DrawItemEventArgs e)
  {
    e.DrawBackground();
    e.DrawFocusRectangle();
    Rectangle bounds = e.Bounds;
    if (this.Items.Count <= 0 || e.Index == -1)
      return;
    object key = this.Items[e.Index];
    Icon itemIcon;
    if (!this._itemsIconCache.TryGetValue(key, out itemIcon))
    {
      itemIcon = this.GetItemIcon(key);
      this._itemsIconCache.Add(key, itemIcon);
    }
    string itemCaption = this.GetItemCaption(this.Items[e.Index]);
    if (itemIcon.Height == 16 /*0x10*/)
    {
      e.Graphics.DrawIcon(itemIcon, new Rectangle(bounds.Left + 2, bounds.Top + 2, itemIcon.Width, 16 /*0x10*/));
    }
    else
    {
      float num = 16f / (float) itemIcon.Height;
      e.Graphics.DrawIcon(itemIcon, new Rectangle(bounds.Left + 2, bounds.Top + 2, Convert.ToInt32((float) itemIcon.Width * num), 16 /*0x10*/));
    }
    e.Graphics.DrawString(itemCaption, e.Font, (Brush) new SolidBrush(e.ForeColor), (float) (bounds.Left + 36), (float) (bounds.Top + 2));
    this.MeasureForHorizontalScroll(e, itemCaption);
  }

  private Icon GetItemIcon(object item)
  {
    Icon itemIcon = new Icon(SystemIcons.Error, 16 /*0x10*/, 16 /*0x10*/);
    Icon original = (Icon) null;
    switch (item)
    {
      case ListItem listItem:
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(Math.Abs(listItem.ObjID), false);
          if (objectActualCopy != null)
          {
            if (Statics.IconSrv != null)
            {
              original = Statics.IconSrv.GetIcon(4, objectActualCopy.ObjectType);
              break;
            }
            break;
          }
          if (Statics.IconSrv != null)
          {
            IMSAttributeType attributeType = MetaDataHelper.GetAttributeType((int) listItem.ID);
            if (attributeType != null)
            {
              original = Statics.IconSrv.GetIcon(3, -1, (object) attributeType.FieldType);
              break;
            }
            break;
          }
          break;
        }
      case FilterObject filterObject:
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(Math.Abs(filterObject.RootObject.ObjID), false);
          if (objectActualCopy != null)
          {
            if (Statics.IconSrv != null)
            {
              original = Statics.IconSrv.GetIcon(4, objectActualCopy.ObjectType);
              break;
            }
            break;
          }
          break;
        }
      case ObjectTypesListItem objectTypesListItem:
        if (Statics.IconSrv != null)
        {
          original = objectTypesListItem.ObjectTypeID != -1 ? Statics.IconSrv.GetIcon(4, objectTypesListItem.ObjectTypeID) : Statics.IconSrv.GetIcon(4, 0);
          break;
        }
        break;
      default:
        original = Statics.IconSrv.GetIcon(4, -1);
        break;
    }
    if (original != null)
    {
      float num = 16f / (float) original.Height;
      itemIcon = new Icon(original, Convert.ToInt32((float) original.Width * num), 16 /*0x10*/);
    }
    return itemIcon;
  }

  private string GetItemCaption(object item)
  {
    string itemCaption = string.Empty;
    switch (item)
    {
      case ListItem listItem:
        itemCaption = listItem.Caption;
        break;
      case FilterObject filterObject:
        itemCaption = filterObject.ToString();
        break;
      case ObjectTypesListItem objectTypesListItem:
        itemCaption = objectTypesListItem.ObjectTypeName;
        break;
    }
    return itemCaption;
  }

  private void MeasureForHorizontalScroll(DrawItemEventArgs e, string caption)
  {
    SizeF sizeF = e.Graphics.MeasureString(caption, e.Font);
    if (this.HorizontalExtent >= (int) sizeF.Width + 36)
      return;
    this.HorizontalExtent = (int) sizeF.Width + 36;
  }

  protected override void Dispose(bool disposing)
  {
    foreach (KeyValuePair<object, Icon> keyValuePair in this._itemsIconCache)
      keyValuePair.Value.Dispose();
    base.Dispose(disposing);
  }
}
