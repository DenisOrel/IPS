// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.IpsProductionObj
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[Serializable]
internal class IpsProductionObj
{
  protected long _objID;
  protected ProductInfo _prodInfo = new ProductInfo();

  public IpsProductionObj(long objID, ProductInfo prodInfo)
  {
    this._objID = objID;
    this._prodInfo.Copy(prodInfo);
  }

  public long ObjID
  {
    get => this._objID;
    set => this._objID = value;
  }

  public ProductInfo ProdInfo => this._prodInfo;
}
