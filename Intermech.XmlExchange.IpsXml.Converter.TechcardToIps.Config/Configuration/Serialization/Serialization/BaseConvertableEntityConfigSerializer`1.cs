// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization.BaseConvertableEntityConfigSerializer`1
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.SerializationService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization;

internal abstract class BaseConvertableEntityConfigSerializer<ConfigClass> : 
  BaseConfigSerializer<ConfigClass>
  where ConfigClass : BaseConvertableEntityConfig, new()
{
  public BaseConvertableEntityConfigSerializer(
    ConfigSerializationService serializationService,
    IpsXmlLogger logger)
    : base(serializationService, logger)
  {
  }

  protected override void OnSaveAddParams(ConfigClass targetConfig, XElement targetNode)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_save_base_convertable_entity_config"));
    this.SerializationService.Serialize((BaseConfig) targetConfig.ConvertStrategies, targetNode);
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_save_base_convertable_entity_config_complete"));
  }
}
