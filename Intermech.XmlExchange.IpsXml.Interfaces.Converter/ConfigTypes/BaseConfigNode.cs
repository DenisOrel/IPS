// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.BaseConfigNode
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public abstract class BaseConfigNode
{
  private string _name = string.Empty;
  private string _description = string.Empty;
  private string _value = string.Empty;
  private int _order;

  public string Name => this._name;

  public string Description => this._description;

  public string Value => this._value;

  public int Order => this._order;

  public virtual string GetUniqueID() => this.Name;

  public virtual void LoadFromXML(XElement configNode)
  {
    XAttribute xattribute1 = configNode.Attribute((XName) "name");
    this._name = xattribute1 != null ? xattribute1.Value : string.Empty;
    XAttribute xattribute2 = configNode.Attribute((XName) "descr");
    this._description = xattribute2 != null ? xattribute2.Value : string.Empty;
    XAttribute xattribute3 = configNode.Attribute((XName) "order");
    if (xattribute3 != null)
      int.TryParse(xattribute3.Value, out this._order);
    this._value = ConfigFormat.GetValueAttrValue(configNode);
    this._description = xattribute3 != null ? xattribute3.Value : string.Empty;
  }
}
