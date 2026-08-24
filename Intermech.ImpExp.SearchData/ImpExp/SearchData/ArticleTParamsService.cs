// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ArticleTParamsService
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal sealed class ArticleTParamsService(
  IDbConnection connection,
  SimpleLogger logger,
  CacheCategory importedObjects,
  CacheCategory themeParams) : ObjectTParamsService(connection, logger, importedObjects, themeParams, "art_id")
{
  protected override string sql4TableParams
  {
    get
    {
      return "select a.art_id, t.param_id from articles a, param4art t where a.art_id=t.art_id and a.art_id>0 order by a.doc_id, a.art_id";
    }
  }

  protected override string GetParamTableName(string[] tabs)
  {
    return string.IsNullOrEmpty(tabs[0]) ? (string) null : tabs[0];
  }
}
