// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.BaseContextedConfigContainer`1
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public abstract class BaseContextedConfigContainer<T> : BaseConfigContainer<T> where T : BaseContextedConfigNode, new()
{
  private bool _contexted;
  private HashSet<string> _contexts = new HashSet<string>();

  public override void LoadFromXML(XElement configNode)
  {
    base.LoadFromXML(configNode);
    XAttribute xattribute = configNode.Attribute((XName) "contexted");
    this._contexted = xattribute != null && xattribute.Value.Equals("true");
    this._contexts.Clear();
    this.ForEach((Action<T>) (cfg =>
    {
      if (this._contexts.Contains(cfg.Context))
        return;
      this._contexts.Add(cfg.Context);
    }));
  }

  public bool Contexted => this._contexted;

  public HashSet<string> contexts => this._contexts;

  public void ForEach(Action<T> action, string context, Predicate<T> onCheckContext = null)
  {
    if (this._contexted)
    {
      foreach (string name in this.Names)
      {
        T obj = this[name];
        if (onCheckContext != null)
        {
          if (onCheckContext(obj))
            action(obj);
        }
        else if (string.IsNullOrEmpty(context) || obj.Context.Equals(context))
          action(obj);
      }
    }
    else
      this.ForEach(action);
  }
}
