// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization.BaseConvertableEntityWithParamsConfigSerializer`1
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.SerializationService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization;

internal abstract class BaseConvertableEntityWithParamsConfigSerializer<ConfigClass> : 
  BaseConvertableEntityConfigSerializer<ConfigClass>
  where ConfigClass : BaseConvertableEntityWithParamsConfig, new()
{
  public BaseConvertableEntityWithParamsConfigSerializer(
    ConfigSerializationService serializationService,
    IpsXmlLogger logger)
    : base(serializationService, logger)
  {
  }

  protected override void OnSaveAddParams(ConfigClass targetConfig, XElement targetNode)
  {
    base.OnSaveAddParams(targetConfig, targetNode);
    this.SerializationService.Serialize((BaseConfig) targetConfig.UniqueControlRule, targetNode);
    this.SerializationService.Serialize((BaseConfig) targetConfig.ConvertationRules, targetNode);
    this.SaveParamConfigs(targetConfig.ParamConfigs, targetNode);
  }

  private void SaveParamConfigs(ParamConfigs paramConfigs, XElement targetNode)
  {
    XElement xelement = new XElement((XName) NodeType.ParamConfigs.ToXMLTag());
    targetNode.Add((object) xelement);
    this.SaveGrouppedParamConfigs(paramConfigs.ConstConfigs, xelement);
    this.SaveGrouppedParamConfigs(paramConfigs.SimpleConfigs, xelement);
    this.SaveGrouppedParamConfigs(paramConfigs.CalcConfigs, xelement);
  }

  private void SaveGrouppedParamConfigs(
    BaseConfigContainer<ParamConfig> grouppedParamConfigs,
    XElement paramConfigsNode)
  {
    foreach (string id in grouppedParamConfigs.Ids)
      this.SerializationService.Serialize((BaseConfig) grouppedParamConfigs[id], paramConfigsNode);
  }
}
