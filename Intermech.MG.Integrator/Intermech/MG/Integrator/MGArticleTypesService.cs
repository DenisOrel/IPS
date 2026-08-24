// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGArticleTypesService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data.SectionEntities;
using Intermech.Interfaces;
using Intermech.Tools.DataExchange;
using Intermech.Tools.Integrators.Electrical;
using Intermech.Tools.Integrators.Mechanical;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGArticleTypesService(
  MechanicalDriver driver,
  CaptureChangesDriverContext driverContext) : ArticleTypesService(driver, driverContext)
{
  protected override string DoGetArticleTypeAttributeName(SectionEntity articleItem)
  {
    return articleItem.Sections.Get<ElectricalArticleCache>().ArticleType != ArticleTypes.Component ? "Article type" : base.DoGetArticleTypeAttributeName(articleItem);
  }

  protected override List<LocalId<int>> DoGetPossibleArticleTypes(SectionEntity articleItem)
  {
    if (articleItem.Sections.Get<ElectricalArticleCache>().ArticleType == ArticleTypes.Component)
    {
      IMSObjectType objectType = MetaDataHelper.GetObjectType(new Guid("cad0038d-306c-11d8-b4e9-00304f19f545"));
      return new List<LocalId<int>>(1)
      {
        new LocalId<int>(objectType.ObjectTypeID, objectType.ObjectTypeName)
      };
    }
    SectionEntity articleInitialDocument = this.Driver.MechanicalOperations.Articles.TryGetArticleInitialDocument(articleItem);
    return articleInitialDocument != null ? this.Driver.MechanicalOperations.Articles.GetPossibleArticleTypes(ObjectSection.GetObjectType(articleInitialDocument)) : this.Driver.MechanicalOperations.Articles.GetPossibleArticleTypes(articleItem);
  }
}
