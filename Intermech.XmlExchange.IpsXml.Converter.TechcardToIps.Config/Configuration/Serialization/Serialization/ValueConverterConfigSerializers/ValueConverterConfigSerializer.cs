// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization.ValueConverterConfigSerializers.ValueConverterConfigSerializer
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.SerializationService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization.ValueConverterConfigSerializers;

[ConfigSerializer(NodeType.ValueConverterConfig)]
internal class ValueConverterConfigSerializer(
  ConfigSerializationService serializationService,
  IpsXmlLogger logger) : BaseConfigSerializer<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig.ValueConverterConfig>(serializationService, logger)
{
  protected override void OnSaveAddParams(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig.ValueConverterConfig targetConfig, XElement targetNode)
  {
    foreach (BaseConfig originValueConfig in targetConfig.OriginValueConfigs)
      this.SerializationService.Serialize(originValueConfig, targetNode);
  }
}
