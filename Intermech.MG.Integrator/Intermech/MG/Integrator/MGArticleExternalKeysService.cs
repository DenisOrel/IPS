// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGArticleExternalKeysService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data.SectionEntities;
using Intermech.Tools.DataExchange;
using Intermech.Tools.Integrators.Electrical;
using Intermech.Tools.Integrators.Mechanical;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGArticleExternalKeysService(
  MechanicalDriver driver,
  CaptureChangesDriverContext driverContext) : MechanicalDriverService(driver, driverContext), IArticleExternalKeysService
{
  public bool HasExternalKeySupport(SectionEntity articleItem, SectionEntity modelItem)
  {
    return articleItem.Sections.Get<ElectricalArticleCache>().ArticleType != 0;
  }

  public void CorrectExternalKeys(List<SectionEntity> articleItems, SectionEntity modelItem)
  {
  }

  public string GetExternalKey(SectionEntity articleItem, SectionEntity modelItem)
  {
    return ((IElectricalComponent) articleItem.Sections.Get<ElectricalArticleCache>().Article).UID;
  }
}
