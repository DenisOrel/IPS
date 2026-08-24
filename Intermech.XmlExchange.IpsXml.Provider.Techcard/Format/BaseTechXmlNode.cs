// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.Format.BaseTechXmlNode
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using Intermech.XmlExchange.IpsXml.Interfaces;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard.Format;

public class BaseTechXmlNode : IXmlEntity
{
  public NodeType NodeType { get; set; }

  public virtual string Description => this.NodeType.ToTechXMLTag();
}
