// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.LocalValueConfig
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public class LocalValueConfig : ValueConfig
{
  private string _groupID = string.Empty;
  private int _orderInGroup;
  private string _delimiter = string.Empty;
  private ConfigFormat.GroupCondType _groupCond = ConfigFormat.GroupCondType.gctAND;

  public override void LoadFromXML(XElement configNode)
  {
    base.LoadFromXML(configNode);
    XAttribute xattribute1 = configNode.Attribute((XName) "group_id");
    this._groupID = xattribute1 != null ? xattribute1.Value : string.Empty;
    XAttribute xattribute2 = configNode.Attribute((XName) "group_cond");
    this._groupCond = xattribute2 != null ? ConfigFormat.ParseGroupCondType(xattribute2.Value) : ConfigFormat.GroupCondType.gctAND;
    XAttribute xattribute3 = configNode.Attribute((XName) "order_in_group");
    if (xattribute3 != null)
      int.TryParse(xattribute3.Value, out this._orderInGroup);
    XAttribute xattribute4 = configNode.Attribute((XName) "delim");
    this._delimiter = xattribute4 != null ? xattribute4.Value : string.Empty;
  }

  public override ConfigFormat.AttrValueType ValueType => ConfigFormat.AttrValueType.avtLocal;

  public string LocalAttrName => this.Value;

  public string GroupID
  {
    get => this._groupID;
    set => this._groupID = value;
  }

  public int OrderInGroup
  {
    get => this._orderInGroup;
    set => this._orderInGroup = value;
  }

  public string Delimiter
  {
    get => this._delimiter;
    set => this._delimiter = value;
  }

  public ConfigFormat.GroupCondType GroupCond
  {
    get => this._groupCond;
    set => this._groupCond = value;
  }
}
