// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.DocumentTParamsService
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal sealed class DocumentTParamsService(
  IDbConnection connection,
  SimpleLogger logger,
  CacheCategory importedObjects,
  CacheCategory themeParams) : ObjectTParamsService(connection, logger, importedObjects, themeParams, "doc_id")
{
  protected override string sql4TableParams
  {
    get => "select t.doc_id, t.param_id from param4doc t where t.doc_id > 0 order by t.doc_id";
  }

  protected override string GetParamTableName(string[] tabs)
  {
    if (tabs.Length <= 1)
      return (string) null;
    return string.IsNullOrEmpty(tabs[1]) ? (string) null : tabs[1];
  }
}
