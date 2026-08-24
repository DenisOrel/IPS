// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.CacheCategory
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces.Client;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp;

public class CacheCategory
{
  private ImportingCategory _category;
  private IImportingData _data;

  public CacheCategory(ImportingCategory cat)
  {
    this._category = cat;
    this._data = (ServicesManager.GetService(typeof (ICache)) as ICache).GetCache(this._category);
  }

  public ImportingCategory Category => this._category;

  public Dictionary<object, DictionaryValue> Items => this._data.GetCategory(this._category);

  public DictionaryValue GetValue(object oldKey) => this._data.GetValue(this._category, oldKey);

  public long GetNewKey(object oldKey) => this._data.GetNewKey(this._category, oldKey);

  public void SetNewKey(object oldKey, long newKey)
  {
    this._data.SetNewKey(this._category, oldKey, newKey);
  }

  public void AddValue(object oldKey, long newKey)
  {
    this._data.AddValue(this._category, oldKey, newKey);
  }

  public void AddValue(object oldKey, long newKey, ITagImportObject tag)
  {
    this._data.AddValue(this._category, oldKey, newKey, tag);
  }

  public void AddValue(object oldKey, long newKey, string caption, ITagImportObject tag)
  {
    this._data.AddValue(this._category, oldKey, newKey, caption, tag);
  }

  public void AddValue(object oldKey, long newKey, string caption)
  {
    this._data.AddValue(this._category, oldKey, newKey, caption);
  }

  public void ClearValue(object oldKey) => this._data.ClearValue((int) this._category, oldKey);

  public void Clear()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    service.DeleteCache(this._category);
    this._data = service.GetCache(this._category);
  }

  public void Release()
  {
    (ServicesManager.GetService(typeof (ICache)) as ICache).ReleaseCache(this._category);
    PumpCache.Category.Remove(this._category);
  }
}
