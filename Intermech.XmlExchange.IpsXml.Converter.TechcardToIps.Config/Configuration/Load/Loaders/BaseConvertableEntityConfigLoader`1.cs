// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.BaseConvertableEntityConfigLoader`1
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders;

internal abstract class BaseConvertableEntityConfigLoader<ConfigClass> : 
  BaseConfigLoader<ConfigClass>
  where ConfigClass : BaseConvertableEntityConfig, new()
{
  public BaseConvertableEntityConfigLoader(ConfigLoadService loadersService, IpsXmlLogger logger)
    : base(loadersService, logger)
  {
  }

  protected override void OnLoadAddParams(ConfigClass target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_base_convertable_entity_config"));
    XElement source1 = source.Element((XName) NodeType.ConvertStrategyConfigs.ToXMLTag());
    if (source1 != null)
    {
      BaseConfig baseConfig = this.LoadersService.LoadConfig(source1);
      if (baseConfig is ConvertStrategyConfigs)
        target.ConvertStrategies = baseConfig as ConvertStrategyConfigs;
      else
        this.Logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_error_loading_strategies"), (object) target.Id));
    }
    else
      this.Logger.Info(LocalizationHolder.rm.GetString("msg_no_strategies_found"));
    if (target.ConvertStrategies == null)
      target.ConvertStrategies = new ConvertStrategyConfigs();
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_base_convertable_entity_config_complete"));
  }
}
