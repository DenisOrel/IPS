// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.TechcardToIpsConfigSerializer
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.SerializationService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization;

public sealed class TechcardToIpsConfigSerializer
{
  private IpsXmlLogger _logger;
  private ConfigSerializationService _serilizationService;
  private const string TECHCARD_TOIPS_CONFIG = "TechardToIps";

  public TechcardToIpsConfigSerializer(IServiceProvider services)
  {
    this._logger = services.GetService<IpsXmlLogger>();
    this._serilizationService = new ConfigSerializationService(services);
  }

  public void SerializeConfig(TechcardToIpsConfig config, string configFileName)
  {
    XDocument xdocument = new XDocument();
    XElement xelement = new XElement((XName) NodeType.Config.ToXMLTag(), (object) new XAttribute((XName) AttrType.Name.ToXMLTag(), (object) "TechardToIps"));
    xdocument.Add((object) xelement);
    this._serilizationService.Serialize((BaseConfig) config.LoggerConfig, xelement);
    this._serilizationService.Serialize((BaseConfig) config.OutputConfig, xelement);
    this._serilizationService.Serialize((BaseConfig) config.ConstValueConfigs, xelement);
    this._serilizationService.Serialize((BaseConfig) config.ObjectConfigs, xelement);
    this._serilizationService.Serialize((BaseConfig) config.IdConfigs, xelement);
    this._serilizationService.Serialize((BaseConfig) config.ValueConverterConfigs, xelement);
    xdocument.Save(configFileName);
  }
}
