// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.BaseConvertableEntityWithParamsConfigLoader`1
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders;

internal abstract class BaseConvertableEntityWithParamsConfigLoader<ConfigClass> : 
  BaseConvertableEntityConfigLoader<ConfigClass>
  where ConfigClass : BaseConvertableEntityWithParamsConfig, new()
{
  public BaseConvertableEntityWithParamsConfigLoader(
    ConfigLoadService loadersService,
    IpsXmlLogger logger)
    : base(loadersService, logger)
  {
  }

  protected override void OnLoadAddParams(ConfigClass target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_base_convertable_entity_with_params_config"));
    base.OnLoadAddParams(target, source);
    this.LoadParamsConfigs(target, source);
    this.LoadUniqueRulesConfig(target, source);
    this.LoadConvertationRulesConfig(target, source);
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_base_convertable_entity_with_params_config_complete"));
  }

  private void LoadParamsConfigs(ConfigClass target, XElement source)
  {
    XElement xelement = source.Element((XName) NodeType.ParamConfigs.ToXMLTag());
    if (xelement != null && xelement.HasElements)
    {
      target.ParamConfigs = new ParamConfigs();
      foreach (XElement element in xelement.Elements())
      {
        BaseConfig baseConfig = this.LoadersService.LoadConfig(element);
        if (baseConfig is ParamConfig)
        {
          ParamConfig paramConfig = baseConfig as ParamConfig;
          if (string.IsNullOrEmpty(paramConfig.Id))
            this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_warn_no_id_found_in_config"), (object) element.ToString(), (object) source.Attribute((XName) AttrType.Id.ToXMLTag())));
          else if (((IEnumerable<PropertyInfo>) target.ParamConfigs.GetType().GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (prop => ((IEnumerable<object>) prop.GetCustomAttributes(typeof (ParamConfigsContainerAttribute), true)).Any<object>((Func<object, bool>) (item => ((IEnumerable<ParamConfigType>) (item as ParamConfigsContainerAttribute).ParamConfigTypes).Contains<ParamConfigType>(paramConfig.ConfigType))))).Select<PropertyInfo, object>((Func<PropertyInfo, object>) (prop => prop.GetValue((object) target.ParamConfigs))).FirstOrDefault<object>() is BaseConfigContainer<ParamConfig> baseConfigContainer)
          {
            if (baseConfigContainer[paramConfig.Id] != null)
              this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_dublicate_config_id"), (object) paramConfig.Id));
            else
              baseConfigContainer[paramConfig.Id] = paramConfig;
          }
        }
      }
    }
    if (target.ParamConfigs != null)
      return;
    target.ParamConfigs = new ParamConfigs();
    this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_empty_param_configs"), (object) target.Id));
  }

  private void LoadConvertationRulesConfig(ConfigClass target, XElement source)
  {
    XElement source1 = source.Element((XName) NodeType.ConvertationRulesConfig.ToXMLTag());
    if (source1 != null)
    {
      BaseConfig baseConfig = this.LoadersService.LoadConfig(source1);
      if (baseConfig is ConvertationRulesConfig)
        target.ConvertationRules = baseConfig as ConvertationRulesConfig;
    }
    if (target.ConvertationRules != null)
      return;
    target.ConvertationRules = new ConvertationRulesConfig();
    this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_empty_convert_rules_config"), (object) target.Id));
  }

  private void LoadUniqueRulesConfig(ConfigClass target, XElement source)
  {
    XElement source1 = source.Element((XName) NodeType.UniqueRuleConfig.ToXMLTag());
    if (source1 != null)
    {
      BaseConfig baseConfig = this.LoadersService.LoadConfig(source1);
      if (baseConfig is UniqueControlRuleConfig)
        target.UniqueControlRule = baseConfig as UniqueControlRuleConfig;
    }
    if (target.UniqueControlRule != null)
      return;
    target.UniqueControlRule = new UniqueControlRuleConfig();
    this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_empty_unique_rules_config"), (object) target.Id));
  }
}
