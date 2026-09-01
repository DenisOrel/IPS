// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.NameCache
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System.Collections.Concurrent;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class NameCache
{
  private static readonly ConcurrentDictionary<string, object> s_ht = new ConcurrentDictionary<string, object>();
  private string _name;

  internal object GetCachedValue(string name)
  {
    this._name = name;
    object obj;
    return !NameCache.s_ht.TryGetValue(name, out obj) ? (object) null : obj;
  }

  internal void SetCachedValue(object value) => NameCache.s_ht[this._name] = value;
}
