// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.Params.TechXmlParam
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Provider.Techcard.Format;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard.Params;

public class TechXmlParam : BaseTechXmlNode, IXmlParam, IXmlEntity
{
  private string _name;
  private string _value;
  private ParamType _paramType;

  public TechXmlParam(string name, string value, ParamType paramType)
  {
    this._name = name;
    this._value = value;
    this._paramType = paramType;
    this.NodeType = NodeType.FormAttribute;
  }

  public string Name => this._name;

  public string Id => this.Name;

  public string Value => this._value;

  public override string Description => $"{this.Name}={this.Value}";

  public ParamType ParamType => this._paramType;
}
