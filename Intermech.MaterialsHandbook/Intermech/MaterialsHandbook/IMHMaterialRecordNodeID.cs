// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHMaterialRecordNodeID
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class IMHMaterialRecordNodeID : INodeID
{
  public IMHMaterialRecordID RecordId { get; set; }

  public IMHMaterialRecordNodeID(IMHMaterialRecordID recordId) => this.RecordId = recordId;

  public int CategoryID => 1;

  public int TypeID => -1;

  public object Cookie { get; set; }
}
