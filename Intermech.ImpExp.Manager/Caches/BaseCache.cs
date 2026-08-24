// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.Caches.BaseCache
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.Caches;

public class BaseCache
{
  protected Dictionary<object, DictionaryValue> _cache;
  public bool Init;

  public BaseCache() => this._cache = new Dictionary<object, DictionaryValue>();

  public virtual void Close()
  {
    if (this._cache == null)
      return;
    this._cache = new Dictionary<object, DictionaryValue>();
  }

  public virtual void AddValue(object oldKey, long newKey)
  {
    this.AddValue(oldKey, newKey, string.Empty, (ITagImportObject) null);
  }

  public virtual void AddValue(object oldKey, long newKey, string caption)
  {
    this.AddValue(oldKey, newKey, caption, (ITagImportObject) null);
  }

  public virtual void AddValue(object oldKey, long newKey, ITagImportObject tag)
  {
    this.AddValue(oldKey, newKey, string.Empty, tag);
  }

  public virtual void AddValue(object oldKey, long newKey, string caption, ITagImportObject tag)
  {
    this._cache.Add((object) Convert.ToInt64(oldKey), new DictionaryValue(newKey, caption, tag));
  }

  public virtual long GetNewKey(object oldKey)
  {
    DictionaryValue dictionaryValue;
    return this._cache.TryGetValue((object) Convert.ToInt64(oldKey), out dictionaryValue) ? dictionaryValue.NewObjectID : 0L;
  }

  public virtual string GetCaption(object oldKey)
  {
    DictionaryValue dictionaryValue;
    return this._cache.TryGetValue((object) Convert.ToInt64(oldKey), out dictionaryValue) ? dictionaryValue.Caption : string.Empty;
  }

  public virtual ITagImportObject GetTag(object oldKey)
  {
    DictionaryValue dictionaryValue;
    return this._cache.TryGetValue((object) Convert.ToInt64(oldKey), out dictionaryValue) ? dictionaryValue.Tag : (ITagImportObject) null;
  }

  public virtual void Remove(object oldKey) => this._cache.Remove((object) Convert.ToInt64(oldKey));

  public virtual DictionaryValue GetInfo(object oldKey)
  {
    DictionaryValue dictionaryValue;
    return this._cache.TryGetValue((object) Convert.ToInt64(oldKey), out dictionaryValue) ? dictionaryValue : (DictionaryValue) null;
  }

  public virtual Dictionary<object, DictionaryValue> Cache => this._cache;
}
