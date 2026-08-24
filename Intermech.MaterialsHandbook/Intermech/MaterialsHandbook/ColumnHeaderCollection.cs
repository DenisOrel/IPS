// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ColumnHeaderCollection
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class ColumnHeaderCollection : CollectionBase
{
  public ColumnHeader this[int index]
  {
    get
    {
      ColumnHeader columnHeader;
      try
      {
        columnHeader = this.List[index] as ColumnHeader;
      }
      catch
      {
        columnHeader = new ColumnHeader();
      }
      return columnHeader ?? new ColumnHeader();
    }
    set
    {
      this.List[index] = (object) value;
      ((ColumnHeader) this.List[index]).WidthResized += new EventHandler(this.OnWidthResized);
    }
  }

  public int Add(ColumnHeader colHeader)
  {
    lock (this.List.SyncRoot)
    {
      colHeader.WidthResized += new EventHandler(this.OnWidthResized);
      colHeader.Index = this.List.Add((object) colHeader);
      this.OnWidthResized((object) this, new EventArgs());
    }
    return colHeader.Index;
  }

  internal event EventHandler WidthResized;

  private void OnWidthResized(object sender, EventArgs e)
  {
    EventHandler widthResized = this.WidthResized;
    if (widthResized == null)
      return;
    widthResized(sender, e);
  }
}
