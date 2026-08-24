// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization.ParamConfigSerializer
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
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization;

[ConfigSerializer(NodeType.ParamConfig)]
internal class ParamConfigSerializer(
  ConfigSerializationService serializationService,
  IpsXmlLogger logger) : BaseConvertableEntityConfigSerializer<ParamConfig>(serializationService, logger)
{
  protected override void OnSaveAddParams(ParamConfig targetConfig, XElement targetNode)
  {
    this.SetAttrValue(targetNode, AttrType.Type, targetConfig.ConfigType.ToXMLTag());
    this.SetAttrValue(targetNode, AttrType.ParentType, targetConfig.ParamParentType.ToXMLTag());
    this.SetAttrValue(targetNode, AttrType.ParentParamId, targetConfig.ParentParamId);
    this.SetAttrValue(targetNode, AttrType.SubType, targetConfig.ParamSubType.ToXMLTag());
    this.SetAttrValue(targetNode, AttrType.Export, targetConfig.Export ? "1" : "0");
    this.SerializationService.Serialize((BaseConfig) targetConfig.ValueConfigs, targetNode);
  }
}
