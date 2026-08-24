// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ItemFactories.ProductionListItem
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.SearchData.ItemFactories;

internal sealed class ProductionListItem
{
  public int ID { get; set; }

  public int ZakazID { get; set; }

  public int ZakazVer { get; set; }

  public int ZRecID { get; set; }

  public int ZParentRecID { get; set; }

  public int PartArticleID { get; set; }

  public int PartArticleVer { get; set; }

  public double CountPC { get; set; }

  public string MUShortName { get; set; }

  public int Razdel { get; set; }

  public string Positio { get; set; }

  public string Note { get; set; }

  public string LinkType { get; set; }

  public string Format { get; set; }

  public string Material { get; set; }

  public int ZVer2 { get; set; }

  public int ZVer3 { get; set; }

  public int ChgCode { get; set; }

  public int ZFrom { get; set; }

  public int ZTill { get; set; }

  public int OPCode { get; set; }

  public int OPVars { get; set; }

  public long PartObjectID { get; set; }

  public long PartID { get; set; }

  public int PartObjectTypeID { get; set; } = -1;

  public string PartCaption { get; set; } = string.Empty;

  public Dictionary<string, object> AdditionalItems { get; set; }
}
