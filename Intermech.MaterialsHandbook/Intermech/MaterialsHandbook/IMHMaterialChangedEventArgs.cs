// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHMaterialChangedEventArgs
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHMaterialChangedEventArgs
{
  public long TebleRefID { get; }

  public long RecordID { get; }

  public string Designation { get; }

  public bool Selectable { get; }

  public IMHMaterialChangedEventArgs(
    long tableRefID,
    long recID,
    bool selectable,
    string designation = "")
  {
    this.TebleRefID = tableRefID;
    this.RecordID = recID;
    this.Selectable = selectable;
    this.Designation = designation;
  }
}
