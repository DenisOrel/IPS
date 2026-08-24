// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.ScenarioProperty
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

[Serializable]
internal class ScenarioProperty
{
  public ScenarioProperty()
  {
    this.ObjectGuid = Guid.Empty;
    this.Catalog = new CatalogLinkData();
  }

  public Guid SlideGuid { get; set; }

  public int VidDet { get; set; }

  public int VidZag { get; set; }

  public bool IsReCountButton { get; set; }

  public int SlideId { get; set; }

  public CatalogLinkData Catalog { get; private set; }

  public Guid ObjectGuid { get; set; }
}
