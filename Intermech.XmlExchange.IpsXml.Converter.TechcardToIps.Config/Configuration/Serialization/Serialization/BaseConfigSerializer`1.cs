// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Serialization.BaseConfigSerializer`1
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

internal abstract class BaseConfigSerializer<ConfigClass> where ConfigClass : BaseConfig, new()
{
  public BaseConfigSerializer(ConfigSerializationService serializationService, IpsXmlLogger logger)
  {
    this.Logger = logger;
    this.SerializationService = serializationService;
  }

  [SerializeMethod]
  public ConfigClass Serialize(ConfigClass config, XElement parentConfig)
  {
    XElement xelement = new XElement((XName) config.ToNodeType().ToXMLTag());
    parentConfig.Add((object) xelement);
    this.SaveBaseParams(config, xelement);
    this.OnSaveAddParams(config, xelement);
    return config;
  }

  protected IpsXmlLogger Logger { get; }

  protected ConfigSerializationService SerializationService { get; }

  protected abstract void OnSaveAddParams(ConfigClass targetConfig, XElement targetNode);

  protected void SetAttrValue(XElement targetNode, AttrType attrType, string value)
  {
    if (string.IsNullOrEmpty(value))
      return;
    XAttribute content = new XAttribute((XName) attrType.ToXMLTag(), (object) value);
    targetNode.Add((object) content);
  }

  private void SaveBaseParams(ConfigClass config, XElement configNode)
  {
    this.Logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_save_base_config_params"), (object) config.Id));
    this.SetAttrValue(configNode, AttrType.Id, config.Id);
    this.SetAttrValue(configNode, AttrType.Name, config.Name);
    this.SetAttrValue(configNode, AttrType.Description, config.Description);
    this.SetAttrValue(configNode, AttrType.Value, config.Value);
    if (config.Order > 0)
      this.SetAttrValue(configNode, AttrType.Order, config.Order.ToString());
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_save_base_config_params_complete"));
  }
}
