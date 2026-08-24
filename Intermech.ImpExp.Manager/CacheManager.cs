// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CacheManager
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Manager.Caches;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager;

internal sealed class CacheManager : ICache
{
  public Dictionary<int, BaseCache> Caches;
  private Dictionary<int, int> _counts;
  private FileManager _fileManager;

  public event NewCacheDelegate NewCacheEvent;

  public CacheManager()
  {
    this._fileManager = new FileManager();
    this.Caches = new Dictionary<int, BaseCache>();
    this._counts = new Dictionary<int, int>();
  }

  internal void FireNewCache(int category, BaseCache newCache)
  {
    NewCacheDelegate newCacheEvent = this.NewCacheEvent;
    if (newCacheEvent == null)
      return;
    newCacheEvent(category, newCache);
  }

  public void Close() => this._fileManager.Close();

  public IImportingData GetCache(params int[] categories)
  {
    Dictionary<int, BaseCache> caches = new Dictionary<int, BaseCache>(categories.Length);
    foreach (int category in categories)
    {
      BaseCache baseCache;
      if (!this.Caches.TryGetValue(category, out baseCache))
      {
        baseCache = this._fileManager.ReadCache(category);
        this.Caches.Add(category, baseCache);
      }
      caches.Add(category, baseCache);
      this.AddCount(category);
    }
    return (IImportingData) new CacheSession(caches, this._fileManager, this);
  }

  private void AddCount(int category)
  {
    if (this._counts.ContainsKey(category))
      this._counts[category]++;
    else
      this._counts.Add(category, 1);
  }

  public void ReleaseCache(params int[] categories)
  {
    foreach (int category in categories)
    {
      int num;
      if (this._counts.TryGetValue(category, out num) && this.Caches.ContainsKey(category))
      {
        if (num > 1)
        {
          this._counts[category]--;
        }
        else
        {
          this._fileManager.CloseCategory(category);
          this._counts.Remove(category);
          this.Caches[category].Close();
          this.Caches.Remove(category);
        }
      }
    }
  }

  public void DeleteCache(params int[] categories)
  {
    this.ReleaseCache(categories);
    this._fileManager.DeleteCategory(categories);
  }

  public bool Exist(ImportingCategory category) => this.Caches.ContainsKey((int) category);

  public void ReleaseCache(params ImportingCategory[] categories)
  {
    this.ReleaseCache(Array.ConvertAll<ImportingCategory, int>(categories, (Converter<ImportingCategory, int>) (x => (int) x)));
  }

  public void DeleteCache(params ImportingCategory[] categories)
  {
    this.DeleteCache(Array.ConvertAll<ImportingCategory, int>(categories, (Converter<ImportingCategory, int>) (x => (int) x)));
  }

  public IImportingData GetCache(params ImportingCategory[] categories)
  {
    return this.GetCache(Array.ConvertAll<ImportingCategory, int>(categories, (Converter<ImportingCategory, int>) (x => (int) x)));
  }
}
