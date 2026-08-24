// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.TechCardToIpsConfigLoader
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Utils;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load;

public sealed class TechCardToIpsConfigLoader
{
  private IpsXmlLogger _logger;
  private ConfigLoadService _loadersService;

  public TechCardToIpsConfigLoader(IServiceProvider services)
  {
    this._logger = services.GetService<IpsXmlLogger>();
    this._loadersService = new ConfigLoadService(services);
  }

  public TechcardToIpsConfig LoadConfig(string configFileName)
  {
    TechcardToIpsConfig config = new TechcardToIpsConfig();
    this._logger.Info(LocalizationHolder.rm.GetString("msg_parse_config_file"));
    XElement xelement = XDocument.Load(configFileName).Elements().FirstOrDefault<XElement>((Func<XElement, bool>) (targetNode => targetNode.Name == (XName) NodeType.Config.ToXMLTag() && targetNode.Attribute((XName) AttrType.Name.ToXMLTag()).Value == "TechardToIps"));
    if (xelement == null)
    {
      this._logger.Error(LocalizationHolder.rm.GetString("msg_wrong_config_file_format"));
      return (TechcardToIpsConfig) null;
    }
    this._logger.Info(LocalizationHolder.rm.GetString("msg_parse_config_file_complete"));
    foreach (XElement element in xelement.Elements())
    {
      BaseConfig baseConfig = this._loadersService.LoadConfig(element);
      switch (baseConfig)
      {
        case LoggerConfig _:
          config.LoggerConfig = baseConfig as LoggerConfig;
          continue;
        case OutputConfig _:
          config.OutputConfig = baseConfig as OutputConfig;
          continue;
        case IdConfigs _:
          config.IdConfigs = baseConfig as IdConfigs;
          continue;
        case ObjectConfigs _:
          config.ObjectConfigs = baseConfig as ObjectConfigs;
          continue;
        case ValueConverterConfigs _:
          config.ValueConverterConfigs = baseConfig as ValueConverterConfigs;
          continue;
        case ConstValueConfigs _:
          config.ConstValueConfigs = baseConfig as ConstValueConfigs;
          continue;
        default:
          continue;
      }
    }
    if (config.LoggerConfig == null)
    {
      this._logger.Warn(LocalizationHolder.rm.GetString("msg_warn_log_config_default"));
      config.LoggerConfig = new LoggerConfig();
    }
    if (config.OutputConfig == null)
    {
      this._logger.Warn(LocalizationHolder.rm.GetString("msg_warn_output_config_not_found"));
      config.OutputConfig = new OutputConfig();
    }
    if (config.IdConfigs == null)
    {
      this._logger.Warn(LocalizationHolder.rm.GetString("msg_warn_id_configs_not_found"));
      config.IdConfigs = new IdConfigs();
    }
    if (config.ObjectConfigs == null)
    {
      this._logger.Warn(LocalizationHolder.rm.GetString("msg_warn_object_configs_not_found"));
      config.ObjectConfigs = new ObjectConfigs();
    }
    if (config.ValueConverterConfigs == null)
    {
      this._logger.Warn(LocalizationHolder.rm.GetString("msg_warn_value_converters_configs_not_found"));
      config.ValueConverterConfigs = new ValueConverterConfigs();
    }
    if (config.ConstValueConfigs == null)
    {
      this._logger.Warn(LocalizationHolder.rm.GetString("msg_warn_const_value_configs_not_found"));
      config.ConstValueConfigs = new ConstValueConfigs();
    }
    this.ReplaceConstsWithValues(config);
    return config;
  }

  private void ReplaceConstsWithValues(TechcardToIpsConfig config)
  {
    ((IEnumerable<PropertyInfo>) config.GetType().GetProperties()).ToList<PropertyInfo>().ForEach((Action<PropertyInfo>) (p =>
    {
      if (!(p.GetValue((object) config) is BaseConfig target2))
        return;
      this.InternalReplaceInBaseConfig(target2, config.ConstValueConfigs);
    }));
  }

  private void InternalReplaceInBaseConfig(BaseConfig target, ConstValueConfigs constValueConfigs)
  {
    if (target is ConstValueConfig)
      return;
    foreach (PropertyInfo property in target.GetType().GetProperties())
    {
      if (((IEnumerable<ParameterInfo>) property.GetIndexParameters()).Count<ParameterInfo>() == 0)
      {
        object obj = property.GetValue((object) target);
        if (obj != null)
        {
          Type type = obj.GetType();
          if (type == typeof (string))
          {
            if (property.CanWrite)
            {
              string id = obj as string;
              if (constValueConfigs.Contains(id))
              {
                ConstValueConfig constValueConfig = constValueConfigs[id];
                if (constValueConfig != null)
                  property.SetValue((object) target, (object) constValueConfig.Value);
              }
            }
          }
          else if (obj is BaseConfig target1)
            this.InternalReplaceInBaseConfig(target1, constValueConfigs);
          else if (type == typeof (ParamConfigs))
          {
            this.InternalReplaceInBaseConfig((BaseConfig) (obj as ParamConfigs).ConstConfigs, constValueConfigs);
            this.InternalReplaceInBaseConfig((BaseConfig) (obj as ParamConfigs).SimpleConfigs, constValueConfigs);
            this.InternalReplaceInBaseConfig((BaseConfig) (obj as ParamConfigs).CalcConfigs, constValueConfigs);
          }
        }
      }
    }
    if (!ConfigTypesUtils.IsComplexConfig(target))
      return;
    IReadOnlyList<BaseConfig> fromComplexConfig = ConfigTypesUtils.GetChildsFromComplexConfig(target);
    if (fromComplexConfig == null)
      return;
    foreach (BaseConfig target2 in (IEnumerable<BaseConfig>) fromComplexConfig)
      this.InternalReplaceInBaseConfig(target2, constValueConfigs);
  }
}
