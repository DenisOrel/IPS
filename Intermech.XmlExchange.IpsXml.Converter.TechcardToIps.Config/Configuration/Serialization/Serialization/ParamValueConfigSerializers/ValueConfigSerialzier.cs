// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization.ParamValueConfigSerializers.ValueConfigSerialzier
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ParamValueConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.SerializationService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization.ParamValueConfigSerializers;

[ConfigSerializer(NodeType.ValueConfig)]
internal class ValueConfigSerialzier(
  ConfigSerializationService serializationService,
  IpsXmlLogger logger) : BaseConfigSerializer<ValueConfig>(serializationService, logger)
{
  protected override void OnSaveAddParams(ValueConfig targetConfig, XElement targetNode)
  {
    this.SetAttrValue(targetNode, AttrType.AttrId, targetConfig.AttrId);
    this.SetAttrValue(targetNode, AttrType.LinkedValueId, targetConfig.LinkedValueId);
    this.SetAttrValue(targetNode, AttrType.Destination, targetConfig.Destination.ToXMLTag());
    this.SetAttrValue(targetNode, AttrType.FieldName, targetConfig.DestFieldName);
    this.SetAttrValue(targetNode, AttrType.Export, targetConfig.Export ? "1" : "0");
    this.SetAttrValue(targetNode, AttrType.SurrSymbol, targetConfig.SurrSymbol);
    this.SetAttrValue(targetNode, AttrType.GroupId, targetConfig.GroupId);
    this.SetAttrValue(targetNode, AttrType.Delimiter, targetConfig.Delimiter);
    this.SetAttrValue(targetNode, AttrType.Condition, targetConfig.GroupCond.ToXMLTag());
    if (string.IsNullOrEmpty(targetConfig.ConverterReference.Id))
      return;
    this.SerializationService.Serialize((BaseConfig) targetConfig.ConverterReference, targetNode);
  }
}
