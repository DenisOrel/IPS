// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.AttrConfig
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public class AttrConfig : BaseContextedConfigNode
{
  private ValueConfigs _valueConfigs = new ValueConfigs();
  private bool _export = true;
  private bool _forComparison;

  public override void LoadFromXML(XElement configNode)
  {
    base.LoadFromXML(configNode);
    XAttribute xattribute1 = configNode.Attribute((XName) "export");
    this._export = xattribute1 == null || !xattribute1.Value.Equals("false");
    XAttribute xattribute2 = configNode.Attribute((XName) "cmp");
    this._forComparison = xattribute2 != null && xattribute2.Value.Equals("true");
    XElement configNode1 = configNode.Element((XName) "value_configs");
    if (configNode1 == null)
      return;
    this._valueConfigs.LoadFromXML(configNode1);
  }

  public ValueConfigs ValueConfigs
  {
    get => this._valueConfigs;
    set => this._valueConfigs = value;
  }

  public bool Export
  {
    get => this._export;
    set => this._export = value;
  }

  public bool ForComparison
  {
    get => this._forComparison;
    set => this._forComparison = value;
  }
}
