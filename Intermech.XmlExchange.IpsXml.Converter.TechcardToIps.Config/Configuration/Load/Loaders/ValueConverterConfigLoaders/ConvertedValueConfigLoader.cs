// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ValueConverterConfigLoaders.ConvertedValueConfigLoader
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ValueConverterConfigLoaders;

[ConfigLoader(NodeType.Convertation)]
internal class ConvertedValueConfigLoader(ConfigLoadService loadersService, IpsXmlLogger logger) : 
  BaseConfigLoader<ConvertedValueConfig>(loadersService, logger)
{
  protected override void OnLoadAddParams(ConvertedValueConfig target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_converted_value_config"));
    if (string.IsNullOrEmpty(target.Value))
      target.Value = source.Value;
    if (string.IsNullOrEmpty(target.Value))
    {
      this.Logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_error_empty_converted_value"), (object) source.ToString()));
    }
    else
    {
      target.Id = target.Value;
      target.Context = this.GetAttrValue(source, AttrType.Context);
      if (string.IsNullOrEmpty(target.Context))
        this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_error_empty_context_convert_value"), (object) source.ToString()));
      this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_converted_value_config_complete"));
    }
  }
}
