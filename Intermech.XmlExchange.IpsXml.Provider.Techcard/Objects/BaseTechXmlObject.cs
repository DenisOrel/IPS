// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.Objects.BaseTechXmlObject
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Provider.Techcard.Format;
using Intermech.XmlExchange.IpsXml.Provider.Techcard.Params;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard.Objects;

public class BaseTechXmlObject : BaseTechXmlNode, IXmlDataEntity, IXmlEntity
{
  private IXmlParams _xmlParams = (IXmlParams) new TechXmlParams();

  public IXmlParams XmlParams => this._xmlParams;

  public string Id { get; set; }

  public void SetParams(IXmlParams xmlParams) => this._xmlParams = xmlParams;
}
