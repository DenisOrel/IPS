// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.ProductInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[Serializable]
internal class ProductInfo
{
  public int ProductionID;
  public string Name;
  public int Version;
  public string Litera;
  public int Flags;
  public string Loc_Litera;
  public int NumInTP;

  public void Clear()
  {
    this.ProductionID = 0;
    this.Loc_Litera = "";
    this.Name = "";
    this.Version = 0;
    this.Litera = "";
    this.Flags = 0;
    this.NumInTP = 0;
  }

  public void Copy(ProductInfo source)
  {
    if (source == null)
    {
      this.Clear();
    }
    else
    {
      this.ProductionID = source.ProductionID;
      this.Loc_Litera = source.Loc_Litera;
      this.Name = source.Name;
      this.Version = source.Version;
      this.Litera = source.Litera;
      this.Flags = source.Flags;
      this.NumInTP = source.NumInTP;
    }
  }
}
