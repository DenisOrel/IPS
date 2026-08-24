// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.BaseConfigContainer`1
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public abstract class BaseConfigContainer<T> : BaseConfigNode where T : BaseConfigNode, new()
{
  private IDictionary<string, T> _container = (IDictionary<string, T>) new Dictionary<string, T>();

  public override void LoadFromXML(XElement configNode)
  {
    base.LoadFromXML(configNode);
    this._container.Clear();
    if (!configNode.HasElements)
      return;
    foreach (XElement element in configNode.Elements())
    {
      BaseConfigNode configClassByNode = ConfigFormat.GetConfigClassByNode(element);
      if (configClassByNode != null && configClassByNode is T)
      {
        configClassByNode.LoadFromXML(element);
        this._container.Add(configClassByNode.GetUniqueID(), configClassByNode as T);
      }
    }
  }

  public IEnumerable<string> Names => (IEnumerable<string>) this._container.Keys;

  public T this[string name]
  {
    get => this._container.TryGetValue(name, out T _) ? this._container[name] : default (T);
  }

  public int Count => this._container.Count;

  public void ForEach(Action<T> action)
  {
    foreach (T obj in (IEnumerable<T>) this._container.Values)
      action(obj);
  }
}
