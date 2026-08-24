// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ColumnHeader
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel;
using System.Data;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class ColumnHeader : ICloneable
{
  private int _width;

  [Category("Data")]
  public DataTable DataSource { get; set; }

  [Category("Data")]
  public string DisplayMember { get; set; }

  [Category("Data")]
  [Description("The index of this column in the collection.")]
  public int Index { get; set; }

  [Browsable(false)]
  internal SortMode SortedMode { get; private set; }

  [Category("Appearance")]
  [Description("The title of this column header.")]
  public string Text { get; set; }

  [Category("Behavior")]
  [Description("The width in pixels of this column header.")]
  [DefaultValue(100)]
  public int Width
  {
    get => this._width;
    set
    {
      this._width = value;
      this.OnWidthResized();
    }
  }

  [Category("Data")]
  public string ValueMember { get; set; }

  public ColumnHeader(string text = "", int width = 0)
  {
    this.Text = text;
    this._width = width;
    this.Index = 0;
    this.SortedMode = SortMode.None;
  }

  internal event EventHandler WidthResized;

  public object Clone()
  {
    return (object) new ColumnHeader(this.Text, this._width)
    {
      Index = this.Index
    };
  }

  public override string ToString() => this.Text;

  private void OnWidthResized()
  {
    EventHandler widthResized = this.WidthResized;
    if (widthResized == null)
      return;
    widthResized((object) this, new EventArgs());
  }

  public void ClearSort() => this.SortedMode = SortMode.None;

  public void SetSortMode()
  {
    if (this.SortedMode == SortMode.None)
      this.SortedMode = SortMode.Asc;
    else if (this.SortedMode == SortMode.Asc)
    {
      this.SortedMode = SortMode.Desc;
    }
    else
    {
      if (this.SortedMode != SortMode.Desc)
        return;
      this.SortedMode = SortMode.Asc;
    }
  }
}
