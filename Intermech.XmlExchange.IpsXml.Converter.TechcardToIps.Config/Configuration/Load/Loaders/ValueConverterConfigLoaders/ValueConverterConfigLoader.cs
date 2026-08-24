// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ValueConverterConfigLoaders.ValueConverterConfigLoader
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ValueConverterConfigLoaders;

[ConfigLoader(NodeType.ValueConverterConfig)]
internal class ValueConverterConfigLoader(ConfigLoadService loadersService, IpsXmlLogger logger) : 
  BaseConfigLoader<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig.ValueConverterConfig>(loadersService, logger)
{
  protected override void OnLoadAddParams(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig.ValueConverterConfig target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_value_converter"));
    List<OriginValueConfig> originValueConfigList = new List<OriginValueConfig>();
    foreach (XElement element in source.Elements())
    {
      BaseConfig baseConfig = this.LoadersService.LoadConfig(element);
      if (baseConfig is OriginValueConfig)
        originValueConfigList.Add(baseConfig as OriginValueConfig);
    }
    if (originValueConfigList.Count == 0)
      this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_no_childs_for_parent_found"), (object) target.Id, (object) target.Name, (object) NodeType.Origin.ToXMLTag()));
    target.OriginValueConfigs = originValueConfigList;
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_value_converter_complete"));
  }
}
