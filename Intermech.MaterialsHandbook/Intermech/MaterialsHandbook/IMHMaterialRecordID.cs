// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHMaterialRecordID
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.DataFormats;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHMaterialRecordID : IDBObjectID
{
  public IMHMaterialRecordID(long parentID, long value, string designation = "")
  {
    this.Caption = string.Empty;
    this.ID = parentID;
    this.Owner = 0L;
    this.Value = value;
    this.Designation = designation;
  }

  public string Caption { get; }

  public long ID { get; }

  public long Owner { get; }

  public long Value { get; }

  public string Designation { get; }
}
