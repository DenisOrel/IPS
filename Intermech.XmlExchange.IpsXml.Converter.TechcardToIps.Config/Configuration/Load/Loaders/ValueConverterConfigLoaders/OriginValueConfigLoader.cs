// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ValueConverterConfigLoaders.OriginValueConfigLoader
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

[ConfigLoader(NodeType.Origin)]
internal class OriginValueConfigLoader(ConfigLoadService loadersService, IpsXmlLogger logger) : 
  BaseConfigLoader<OriginValueConfig>(loadersService, logger)
{
  protected override void OnLoadAddParams(OriginValueConfig target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_origin_value_config"));
    target.Id = target.Value;
    target.IsDefault = this.GetAttrValue(source, AttrType.IsDefault) == "1";
    if (source.HasElements)
    {
      foreach (XElement element in source.Elements())
      {
        BaseConfig baseConfig = this.LoadersService.LoadConfig(element);
        if (baseConfig is ConvertedValueConfig && !string.IsNullOrEmpty(baseConfig.Value))
        {
          if (target[baseConfig.Value] != null)
            this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_dublicate_config_id"), (object) baseConfig.Value));
          else
            target[baseConfig.Value] = baseConfig as ConvertedValueConfig;
        }
      }
    }
    else
    {
      string attrValue = source.Value;
      if (string.IsNullOrEmpty(attrValue))
        attrValue = this.GetAttrValue(source, AttrType.Value);
      if (string.IsNullOrEmpty(attrValue))
      {
        this.Logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_error_empty_converted_value"), (object) source.ToString()));
        return;
      }
      ConvertedValueConfig convertedValueConfig = new ConvertedValueConfig();
      convertedValueConfig.Id = convertedValueConfig.Value = attrValue;
      target[convertedValueConfig.Id] = convertedValueConfig;
    }
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_origin_value_config_complete"));
  }
}
