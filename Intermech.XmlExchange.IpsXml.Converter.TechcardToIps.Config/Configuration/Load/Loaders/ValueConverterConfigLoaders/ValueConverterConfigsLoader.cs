// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ValueConverterConfigLoaders.ValueConverterConfigsLoader
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ValueConverterConfigLoaders;

[ConfigLoader(NodeType.ValueConverterConfigs)]
internal class ValueConverterConfigsLoader(ConfigLoadService loadersService, IpsXmlLogger logger) : 
  BaseConfigLoader<ValueConverterConfigs>(loadersService, logger)
{
  protected override void OnLoadAddParams(ValueConverterConfigs target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_value_converter_configs"));
    foreach (XElement element in source.Elements())
    {
      BaseConfig baseConfig = this.LoadersService.LoadConfig(element);
      if (baseConfig is Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig.ValueConverterConfig && !string.IsNullOrEmpty(baseConfig.Id))
      {
        if (target[baseConfig.Id] != null)
          this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_dublicate_config_id"), (object) baseConfig.Id));
        else
          target[baseConfig.Id] = baseConfig as Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig.ValueConverterConfig;
      }
    }
    if (target.Count == 0)
      this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_no_childs_for_parent_found"), (object) target.Id, (object) target.Name, (object) NodeType.ValueConverterConfig.ToXMLTag()));
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_value_converter_configs_complete"));
  }
}
