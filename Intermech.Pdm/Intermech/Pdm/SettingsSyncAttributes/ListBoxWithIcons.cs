// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SettingsSyncAttributes.ListBoxWithIcons
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.SettingsSyncAttributes;

internal class ListBoxWithIcons : ListBox
{
  public List<Icon> _myImageList;

  public ListBoxWithIcons() => this.DrawMode = DrawMode.OwnerDrawFixed;

  protected override void OnDrawItem(DrawItemEventArgs e)
  {
    e.DrawBackground();
    e.DrawFocusRectangle();
    Rectangle bounds = e.Bounds;
    try
    {
      ListItems listItems = (ListItems) this.Items[e.Index];
      if (listItems.ImageIndex != -1)
      {
        Icon icon = this._myImageList[e.Index];
        if (icon.Height > 16 /*0x10*/)
          icon = new Icon(icon, 16 /*0x10*/, 16 /*0x10*/);
        e.Graphics.DrawIcon(icon, bounds.Left + 2, bounds.Top + 2);
        using (SolidBrush solidBrush = new SolidBrush(e.ForeColor))
          e.Graphics.DrawString(listItems.AttrName, e.Font, (Brush) solidBrush, (float) (bounds.Left + 36), (float) (bounds.Top + 2));
      }
      else
      {
        using (SolidBrush solidBrush = new SolidBrush(e.ForeColor))
          e.Graphics.DrawString(listItems.AttrName, e.Font, (Brush) solidBrush, (float) bounds.Left, (float) bounds.Top);
      }
    }
    catch
    {
      using (SolidBrush solidBrush = new SolidBrush(e.ForeColor))
        e.Graphics.DrawString(e.Index != -1 ? this.Items[e.Index].ToString() : this.Text, e.Font, (Brush) solidBrush, (float) bounds.Left, (float) bounds.Top);
    }
    base.OnDrawItem(e);
  }
}
