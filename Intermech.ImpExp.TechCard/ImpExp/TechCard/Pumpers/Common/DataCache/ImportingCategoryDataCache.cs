// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.ImportingCategoryDataCache
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache;

internal class ImportingCategoryDataCache
{
  private readonly PluginClass _plugin;
  private readonly IDictionary<string, ImportingCategoryData> _categoryDataCache = (IDictionary<string, ImportingCategoryData>) new ConcurrentDictionary<string, ImportingCategoryData>();
  private static ImportingCategoryDataCache _instance;

  private string GetCategoriesHash(ImportingCategory[] categories)
  {
    if (categories == null)
      return string.Empty;
    List<string> list = ((IEnumerable<ImportingCategory>) categories).Select<ImportingCategory, string>((Func<ImportingCategory, string>) (category => category.ToString())).ToList<string>();
    list.Sort();
    for (int index = list.Count - 1; index > 0; --index)
    {
      if (list[index] == list[index - 1])
        list.RemoveAt(index);
    }
    return string.Join(",", list.ToArray());
  }

  private ImportingCategoryDataCache(PluginClass plugin) => this._plugin = plugin;

  public IImportingData GetCache(ImportingCategory[] categories)
  {
    try
    {
      string categoriesHash = this.GetCategoriesHash(categories);
      ImportingCategoryData importingCategoryData;
      if (this._categoryDataCache.TryGetValue(categoriesHash, out importingCategoryData))
        return importingCategoryData.Data;
      IImportingData cache = ServiceUtils.GetService<ICache>((object) ApplicationServices.Container, false)?.GetCache(categories);
      this._categoryDataCache.Add(categoriesHash, new ImportingCategoryData(categories, cache));
      return cache;
    }
    catch (Exception ex)
    {
      this._plugin.appManager.AddErrorMessage($"Ошибка получения интерфейса для работы с закэшированными данными: {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    return (IImportingData) null;
  }

  public void FreeCache(ImportingCategory[] categories)
  {
    ServiceUtils.GetService<ICache>((object) ApplicationServices.Container, false)?.ReleaseCache(categories);
    this._categoryDataCache.Remove(this.GetCategoriesHash(categories));
  }

  public void ClearCaches()
  {
    ICache service = ServiceUtils.GetService<ICache>((object) ApplicationServices.Container, false);
    if (service != null)
    {
      foreach (KeyValuePair<string, ImportingCategoryData> keyValuePair in (IEnumerable<KeyValuePair<string, ImportingCategoryData>>) this._categoryDataCache)
        service.ReleaseCache(keyValuePair.Value.Categories);
    }
    this._categoryDataCache.Clear();
  }

  public static ImportingCategoryDataCache Instance
  {
    get
    {
      return ImportingCategoryDataCache._instance ?? (ImportingCategoryDataCache._instance = new ImportingCategoryDataCache(TechcardConsts.Plugin));
    }
  }
}
