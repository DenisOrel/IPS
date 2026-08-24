// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.Caches.CacheSession
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.Manager.Caches;

internal sealed class CacheSession : IImportingData
{
  private readonly Dictionary<int, BaseCache> _caches;
  private readonly FileManager _fileManager;
  private readonly CacheManager _manager;
  private readonly int _firstCategory;

  public CacheSession(
    Dictionary<int, BaseCache> caches,
    FileManager fileManager,
    CacheManager manager)
  {
    this._caches = caches;
    this._fileManager = fileManager;
    this._firstCategory = caches.Keys.First<int>();
    this._manager = manager;
    this._manager.NewCacheEvent += new NewCacheDelegate(this.Manager_NewCacheEvent);
  }

  private void CheckInit(object oldKey, int category)
  {
    if (this._caches[category].Init)
      return;
    switch (CacheHelper.GetRecordType(oldKey))
    {
      case RecordType.Int:
        this._caches[category] = (BaseCache) new IntKeyCache();
        this._manager.Caches[category] = this._caches[category];
        break;
      case RecordType.Int64:
        this._caches[category] = (BaseCache) new LongKeyCache();
        this._manager.Caches[category] = this._caches[category];
        break;
      case RecordType.Char:
        this._caches[category] = (BaseCache) new CharKeyCache();
        this._manager.Caches[category] = this._caches[category];
        break;
      case RecordType.String:
        this._caches[category] = (BaseCache) new StringKeyCache();
        this._manager.Caches[category] = this._caches[category];
        break;
    }
    this._caches[category].Init = true;
    this._manager.FireNewCache(category, this._caches[category]);
  }

  private void Manager_NewCacheEvent(int category, BaseCache newCache)
  {
    BaseCache baseCache;
    if (!this._caches.TryGetValue(category, out baseCache) || baseCache.Init)
      return;
    this._caches[category] = newCache;
  }

  public void AddValue(int category, object oldKey, long newKey)
  {
    this.AddNewValue(category, oldKey, newKey, string.Empty, (ITagImportObject) null);
  }

  public void AddValue(int category, object oldKey, long newKey, string caption)
  {
    this.AddNewValue(category, oldKey, newKey, caption, (ITagImportObject) null);
  }

  public void AddValue(
    int category,
    object oldKey,
    long newKey,
    string caption,
    ITagImportObject tag)
  {
    this.AddNewValue(category, oldKey, newKey, caption, tag);
  }

  public void AddValue(ImportingCategory category, object oldKey, long newKey)
  {
    this.AddValue((int) category, oldKey, newKey);
  }

  public void AddValue(ImportingCategory category, object oldKey, long newKey, string caption)
  {
    this.AddValue((int) category, oldKey, newKey, caption);
  }

  public void AddValue(
    ImportingCategory category,
    object oldKey,
    long newKey,
    ITagImportObject tag)
  {
    this.AddNewValue((int) category, oldKey, newKey, string.Empty, tag);
  }

  public void AddValue(
    ImportingCategory category,
    object oldKey,
    long newKey,
    string caption,
    ITagImportObject tag)
  {
    this.AddValue((int) category, oldKey, newKey, caption, tag);
  }

  public void AddValue(int category, object oldKey, long newKey, ITagImportObject tag)
  {
    this.AddNewValue(category, oldKey, newKey, string.Empty, tag);
  }

  private void AddNewValue(
    int category,
    object oldKey,
    long newKey,
    string caption,
    ITagImportObject tag)
  {
    this.CheckInit(oldKey, category);
    this._caches[category].AddValue(oldKey, newKey, caption, tag);
    this._fileManager.AddValue(category, oldKey, newKey, caption, tag);
  }

  public long GetNewKey(ImportingCategory category, object oldKey)
  {
    return this.GetNewKey((int) category, oldKey);
  }

  public long GetNewKey(int category, object oldKey)
  {
    this.CheckInit(oldKey, category);
    return this._caches[category].GetNewKey(oldKey);
  }

  public string GetCaption(ImportingCategory category, object oldKey)
  {
    return this.GetCaption((int) category, oldKey);
  }

  public string GetCaption(int category, object oldKey)
  {
    this.CheckInit(oldKey, category);
    return this._caches[category].GetCaption(oldKey);
  }

  public ITagImportObject GetTag(ImportingCategory category, object oldKey)
  {
    return this.GetTag((int) category, oldKey);
  }

  public ITagImportObject GetTag(int category, object oldKey)
  {
    this.CheckInit(oldKey, category);
    return this._caches[category].GetTag(oldKey);
  }

  public Dictionary<object, DictionaryValue> GetCategory(ImportingCategory category)
  {
    return this.GetCategory((int) category);
  }

  public Dictionary<object, DictionaryValue> GetCategory(int category)
  {
    return this._caches.ContainsKey(category) ? this._caches[category].Cache : (Dictionary<object, DictionaryValue>) null;
  }

  public bool IsCategoryPresent(ImportingCategory category)
  {
    return this.IsCategoryPresent((int) category);
  }

  public DictionaryValue GetValue(ImportingCategory category, object oldKey)
  {
    return this.GetValue((int) category, oldKey);
  }

  public DictionaryValue GetValue(int category, object oldKey)
  {
    this.CheckInit(oldKey, category);
    return this._caches[category].GetInfo(oldKey);
  }

  public void AddValue(object oldKey, long newKey)
  {
    this.AddValue(this._firstCategory, oldKey, newKey);
  }

  public void AddValue(object oldKey, long newKey, string caption)
  {
    this.AddValue(this._firstCategory, oldKey, newKey, caption);
  }

  public void AddValue(object oldKey, long newKey, ITagImportObject tag)
  {
    this.AddValue(this._firstCategory, oldKey, newKey, tag);
  }

  public void AddValue(object oldKey, long newKey, string caption, ITagImportObject tag)
  {
    this.AddValue(this._firstCategory, oldKey, newKey, caption, tag);
  }

  public long GetNewKey(object oldKey) => this.GetNewKey(this._firstCategory, oldKey);

  public string GetCaption(object oldKey) => this.GetCaption(this._firstCategory, oldKey);

  public ITagImportObject GetTag(object oldKey) => this.GetTag(this._firstCategory, oldKey);

  public DictionaryValue GetValue(object oldKey) => this.GetValue(this._firstCategory, oldKey);

  public Dictionary<object, DictionaryValue> GetCategory() => this.GetCategory(this._firstCategory);

  public bool SetNewKey(ImportingCategory category, object oldKey, long newKey)
  {
    return this.SetNewKey((int) category, oldKey, newKey);
  }

  public bool SetNewKey(object oldKey, long newKey)
  {
    return this.SetNewKey(this._firstCategory, oldKey, newKey);
  }

  public bool SetNewKey(int category, object oldKey, long newKey)
  {
    if (this._fileManager.SetNewKey(category, oldKey, newKey))
    {
      DictionaryValue info = this._caches[category].GetInfo(oldKey);
      if (info != null)
      {
        info.NewObjectID = newKey;
        return true;
      }
    }
    return false;
  }

  public bool ClearValue(int category, object oldKey)
  {
    if (!this._fileManager.ClearValue(category, oldKey))
      return false;
    this._caches[category].Remove(oldKey);
    return true;
  }

  public bool IsCategoryPresent(int category) => this._caches.ContainsKey(category);
}
