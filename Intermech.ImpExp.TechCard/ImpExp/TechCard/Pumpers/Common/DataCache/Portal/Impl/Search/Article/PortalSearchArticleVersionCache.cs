// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Article.PortalSearchArticleVersionCache
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Interfaces;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Article;

public class PortalSearchArticleVersionCache : 
  PortalImportedObjectVersionCache<PortalSearchArticleVersion>,
  IPortalSearchArticleVersionCache,
  IPortalImportedObjectCache<PortalSearchArticleVersion>
{
  protected override string GetQueryToPortalImportData()
  {
    return "select va.VART_ID, va.ART_ID, va.ART_VER_ID, p.F_GUID from GUIDS_VART p, V_ARTICLES va where va.VART_ID = p.VART_ID";
  }

  protected override IDataBase GetDbConnection() => SearchConnectionsManager.GetConnection();

  protected override PortalSearchArticleVersion CreateImportedData()
  {
    return new PortalSearchArticleVersion();
  }

  protected override bool FillImportedData(
    PortalSearchArticleVersion target,
    IDataReader dataReader)
  {
    target.ArtVerId = dataReader.GetInt32(0);
    target.ArtId = dataReader.GetInt32(1);
    target.ArtVer = dataReader.GetInt32(2);
    Guid result;
    if (Guid.TryParse(dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3), out result))
      target.IpsObjVerGuid = result;
    return true;
  }

  public override string GetUniqueObjId(PortalSearchArticleVersion target)
  {
    return $"{target.ArtId}|{target.ArtVer}";
  }

  public override string GetUniqueObjId(params object[] idParams)
  {
    return idParams.Length >= 2 ? $"{idParams[0]}|{idParams[1]}" : string.Empty;
  }

  public override Guid ObjectType => new Guid("cad00268-306c-11d8-b4e9-00304f19f545");
}
