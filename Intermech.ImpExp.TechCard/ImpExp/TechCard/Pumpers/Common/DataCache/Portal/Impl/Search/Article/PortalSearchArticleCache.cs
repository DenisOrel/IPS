// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Article.PortalSearchArticleCache
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Common;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Article;

public class PortalSearchArticleCache : PortalImportedObjectCache<PortalSearchArticle>
{
  protected override string GetQueryToPortalImportData() => "select ART_ID, F_GUID from GUIDS_ART";

  protected override IDataBase GetDbConnection() => SearchConnectionsManager.GetConnection();

  protected override PortalSearchArticle CreateImportedData() => new PortalSearchArticle();

  protected override bool FillImportedData(PortalSearchArticle target, IDataReader dataReader)
  {
    target.ArtId = dataReader.GetInt32(0);
    Guid result;
    if (Guid.TryParse(dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1), out result))
      target.IpsObjVerGuid = result;
    return true;
  }

  public override string GetUniqueObjId(PortalSearchArticle target) => target.ArtId.ToString();

  public override string GetUniqueObjId(params object[] idParams)
  {
    return idParams.Length != 0 ? idParams[0].ToString() : string.Empty;
  }

  public override Guid ObjectType => new Guid("cad00268-306c-11d8-b4e9-00304f19f545");
}
