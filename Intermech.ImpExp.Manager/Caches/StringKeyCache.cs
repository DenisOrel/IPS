// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.Caches.StringKeyCache
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.Caches;

internal class StringKeyCache : BaseCache
{
  private Dictionary<string, long> _extraCache;

  public StringKeyCache() => this._extraCache = new Dictionary<string, long>(1);

  public override Dictionary<object, DictionaryValue> Cache
  {
    get
    {
      Dictionary<object, DictionaryValue> cache = new Dictionary<object, DictionaryValue>(this._extraCache.Count);
      foreach (KeyValuePair<string, long> keyValuePair in this._extraCache)
        cache.Add((object) keyValuePair.Key, this._cache[(object) keyValuePair.Value]);
      return cache;
    }
  }

  public override void AddValue(object oldKey, long newKey, string caption, ITagImportObject tag)
  {
    long int64 = Convert.ToInt64(this._extraCache.Count);
    this._extraCache.Add((string) oldKey, int64);
    base.AddValue((object) int64, newKey, caption, tag);
  }

  public override DictionaryValue GetInfo(object oldKey)
  {
    long oldKey1 = -1;
    return this._extraCache.TryGetValue(oldKey.ToString(), out oldKey1) ? base.GetInfo((object) oldKey1) : (DictionaryValue) null;
  }

  public override string GetCaption(object oldKey)
  {
    long oldKey1 = -1;
    return this._extraCache.TryGetValue(oldKey.ToString(), out oldKey1) ? base.GetCaption((object) oldKey1) : string.Empty;
  }

  public override long GetNewKey(object oldKey)
  {
    long oldKey1 = -1;
    return this._extraCache.TryGetValue(oldKey.ToString(), out oldKey1) ? base.GetNewKey((object) oldKey1) : 0L;
  }

  public override ITagImportObject GetTag(object oldKey)
  {
    long oldKey1 = -1;
    return this._extraCache.TryGetValue(oldKey.ToString(), out oldKey1) ? base.GetTag((object) oldKey1) : (ITagImportObject) null;
  }
}
