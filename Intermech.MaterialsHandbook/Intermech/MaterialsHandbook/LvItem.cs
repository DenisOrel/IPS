// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.LvItem
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class LvItem
{
  internal long A_TableID { get; set; }

  internal long M_TableID { get; set; }

  internal long RecID { get; set; }

  internal string Caption { get; set; }

  internal bool Selectable { get; set; }

  public LvItem(long aTableID, long mTableID, long recID, string caption, bool selectable = true)
    : this(mTableID, recID, caption)
  {
    this.A_TableID = aTableID;
    this.Selectable = selectable;
  }

  public LvItem(long mTableID, long recID, string caption)
  {
    this.A_TableID = 0L;
    this.Selectable = false;
    this.M_TableID = mTableID;
    this.RecID = recID;
    this.Caption = caption;
  }
}
