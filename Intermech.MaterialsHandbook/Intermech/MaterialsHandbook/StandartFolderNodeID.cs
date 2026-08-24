// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.StandartFolderNodeID
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class StandartFolderNodeID : NodeID
{
  public string ImbaseKey { get; }

  public string Standart { get; }

  public long ObjectId { get; }

  public StandartFolderNodeID(CreateObjectNodeParams e)
    : base(e)
  {
    this.ImbaseKey = this.Standart = string.Empty;
  }

  public StandartFolderNodeID(
    CreateObjectNodeParams e,
    string imbaseKey,
    string standart,
    long objectId)
    : base(e)
  {
    this.ImbaseKey = imbaseKey;
    this.Standart = standart;
    this.ObjectId = objectId;
  }

  public override int CategoryID => Consts.IMHStandartFolderCategoryID;

  public override bool Equals(object obj)
  {
    bool flag = false;
    if (obj is StandartFolderNodeID standartFolderNodeId)
      flag = this.Caption == standartFolderNodeId.Caption && this.ImbaseKey == standartFolderNodeId.ImbaseKey && this.Standart == standartFolderNodeId.Standart && this.ObjectId == standartFolderNodeId.ObjectId;
    return flag;
  }

  public override int GetHashCode() => this.Caption.GetHashCode();
}
