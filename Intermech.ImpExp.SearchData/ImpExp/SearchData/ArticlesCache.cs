// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ArticlesCache
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System.Configuration;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public sealed class ArticlesCache
{
  private readonly bool _ignoreImportedPL;

  public ArticlesCache(CacheCategory articlesCache)
  {
    this.Cache = articlesCache;
    string str = ConfigurationManager.AppSettings.Get("Search.IgnoreImportedPL");
    if (string.IsNullOrEmpty(str))
      return;
    bool.TryParse(str, out this._ignoreImportedPL);
  }

  public void Release() => this.Cache.Release();

  public CacheCategory Cache { get; }

  private bool IgnoreImportedPLMode(int sectID) => sectID == 99999990 && this._ignoreImportedPL;

  public void Add(int artID, long newArtID, ArticleTag atag, int sectID)
  {
    if (newArtID != 0L)
      this.Cache.AddValue((object) artID, newArtID, (ITagImportObject) atag);
    else
      BasePumpHelper.AppManager.AddWarningMessage($"Изделие (ART_ID={artID}) не создано!");
  }

  public long CheckID(int artID, int sectID)
  {
    long newKey = this.Cache.GetNewKey((object) artID);
    if (newKey == 0L || !this.IgnoreImportedPLMode(sectID))
      return newKey;
    this.Cache.ClearValue((object) artID);
    return 0;
  }

  public DictionaryValue GetValue(int artID) => this.Cache.GetValue((object) artID);
}
