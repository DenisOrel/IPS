// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.ImbaseKeyObjectCache
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

[Serializable]
internal class ImbaseKeyObjectCache
{
  private readonly IDictionary<string, DictionaryValue> _imbaseKey2ObjValueCache = (IDictionary<string, DictionaryValue>) new ConcurrentDictionary<string, DictionaryValue>();

  public void Add(string imbaseKey, DictionaryValue objectValue)
  {
    this._imbaseKey2ObjValueCache[imbaseKey] = objectValue;
  }

  public DictionaryValue GetObjectInfo(string imbaseKey)
  {
    DictionaryValue dictionaryValue;
    return this._imbaseKey2ObjValueCache.TryGetValue(imbaseKey, out dictionaryValue) ? dictionaryValue : (DictionaryValue) null;
  }
}
