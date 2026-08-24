// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.BaseConfigContainer`1
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;

public abstract class BaseConfigContainer<T> : BaseConfig where T : BaseConfig, new()
{
  private IDictionary<string, T> _container = (IDictionary<string, T>) new Dictionary<string, T>();

  public IEnumerable<string> Ids => (IEnumerable<string>) this._container.Keys;

  public T this[string id]
  {
    get
    {
      T obj;
      return this._container.TryGetValue(id, out obj) ? obj : default (T);
    }
    set => this._container[id] = value;
  }

  public bool Remove(string id) => this._container.Remove(id);

  public int Count => this._container.Count;

  public bool Contains(string id) => this._container.ContainsKey(id);

  public void ForEach(Action<T> action)
  {
    foreach (T obj in (IEnumerable<T>) this._container.Values)
      action(obj);
  }
}
