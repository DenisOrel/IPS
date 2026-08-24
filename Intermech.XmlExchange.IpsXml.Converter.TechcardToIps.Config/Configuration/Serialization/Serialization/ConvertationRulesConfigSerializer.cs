// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization.ConvertationRulesConfigSerializer
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

[ConfigSerializer(NodeType.ConvertationRulesConfig)]
internal class ConvertationRulesConfigSerializer(
  ConfigSerializationService serializationService,
  IpsXmlLogger logger) : BaseConfigSerializer<ConvertationRulesConfig>(serializationService, logger)
{
  protected override void OnSaveAddParams(ConvertationRulesConfig targetConfig, XElement targetNode)
  {
    this.SetAttrValue(targetNode, AttrType.Object, (targetConfig.Rules & ConvertationRules.Object) == ConvertationRules.Object ? "1" : "0");
    this.SetAttrValue(targetNode, AttrType.ObjectParams, (targetConfig.Rules & ConvertationRules.ObjectParams) == ConvertationRules.ObjectParams ? "1" : "0");
    this.SetAttrValue(targetNode, AttrType.Relation, (targetConfig.Rules & ConvertationRules.Relation) == ConvertationRules.Relation ? "1" : "0");
    this.SetAttrValue(targetNode, AttrType.RelationParams, (targetConfig.Rules & ConvertationRules.RelationParams) == ConvertationRules.RelationParams ? "1" : "0");
  }
}
