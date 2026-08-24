// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.ValueConverter
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public class ValueConverter : BaseConfigNode
{
  private IDictionary<string, string> _convertations = (IDictionary<string, string>) new Dictionary<string, string>();
  private IDictionary<string, string> _defaults = (IDictionary<string, string>) new Dictionary<string, string>();

  public string Convert(string originValue, string context = "")
  {
    string key = originValue;
    if (!context.Equals(string.Empty))
      key = $"{key}_{context}";
    string str;
    if (this._convertations.TryGetValue(key, out str))
      return str;
    if (this._defaults.Count > 0)
    {
      if (!context.Equals(string.Empty))
      {
        if (this._defaults.TryGetValue(context, out str))
          return str;
      }
      else if (this._defaults.Count > 0)
        return this._defaults.Keys.First<string>();
    }
    return originValue;
  }

  public override void LoadFromXML(XElement configNode)
  {
    base.LoadFromXML(configNode);
    this._convertations.Clear();
    this._defaults.Clear();
    foreach (XElement element in configNode.Elements())
    {
      XAttribute xattribute1 = element.Attribute((XName) "origin");
      string key1 = xattribute1 != null ? xattribute1.Value : string.Empty;
      switch (element.Name.ToString())
      {
        case "value":
          if (element.HasElements)
          {
            using (IEnumerator<XElement> enumerator = element.Elements().GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                XElement current = enumerator.Current;
                string valueAttrValue = ConfigFormat.GetValueAttrValue(current);
                if (!valueAttrValue.Equals(string.Empty))
                {
                  XAttribute xattribute2 = current.Attribute((XName) "context");
                  string str = xattribute2 != null ? xattribute2.Value : string.Empty;
                  string key2 = $"{key1}_{str}";
                  if (!this._convertations.ContainsKey(key2))
                    this._convertations.Add(key2, valueAttrValue);
                }
              }
              continue;
            }
          }
          string valueAttrValue1 = ConfigFormat.GetValueAttrValue(element);
          if (!valueAttrValue1.Equals(string.Empty) && !this._convertations.ContainsKey(key1))
          {
            this._convertations.Add(key1, valueAttrValue1);
            continue;
          }
          continue;
        case "default":
          string valueAttrValue2 = ConfigFormat.GetValueAttrValue(element);
          XAttribute xattribute3 = element.Attribute((XName) "context");
          string key3 = xattribute3 == null || xattribute3.Value.Equals(string.Empty) ? valueAttrValue2 : string.Empty;
          if (!key3.Equals(string.Empty))
          {
            this._defaults.Add(key3, valueAttrValue2);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }
}
