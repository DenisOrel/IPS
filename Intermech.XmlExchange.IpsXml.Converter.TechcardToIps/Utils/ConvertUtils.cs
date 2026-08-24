// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Utils.ConvertUtils
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Utils;

public class ConvertUtils
{
  public static ConvertedData FindConvertedData(IXmlEntity target, IServiceProvider globalServices)
  {
    IpsXmlLogger service1 = globalServices.GetService<IpsXmlLogger>();
    service1.Info(LocalizationHolder.rm.GetString("msg_find_converted_data"));
    ConfigIdCalculator service2 = globalServices.GetService<ConfigIdCalculator>();
    string configId = service2.FindConfigId(target);
    if (string.IsNullOrEmpty(configId))
    {
      service1.Error(string.Format(LocalizationHolder.rm.GetString("msg_no_config_found"), (object) target.Description));
      return (ConvertedData) null;
    }
    TechcardToIpsConfig service3 = globalServices.GetService<TechcardToIpsConfig>();
    ObjectConfig objectConfig = service3.ObjectConfigs[configId];
    if (objectConfig == null)
    {
      service1.Error(string.Format(LocalizationHolder.rm.GetString("msg_no_config_with_id_found"), (object) configId));
      return (ConvertedData) null;
    }
    if (string.IsNullOrEmpty(objectConfig.UniqueControlRule.Id))
    {
      service1.Error(string.Format(LocalizationHolder.rm.GetString("msg_empty_unique_rule_id"), (object) objectConfig.Id));
      return (ConvertedData) null;
    }
    Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig idConfig = service3.IdConfigs[objectConfig.UniqueControlRule.Id];
    if (idConfig == null)
    {
      service1.Error(string.Format(LocalizationHolder.rm.GetString("msg_id_config_not_found"), (object) objectConfig.Id));
      return (ConvertedData) null;
    }
    service1.Info(string.Format(LocalizationHolder.rm.GetString("msg_id_config_found"), (object) idConfig.Id));
    string key = service2.CalcId(target, idConfig);
    if (string.IsNullOrEmpty(key))
    {
      service1.Error(string.Format(LocalizationHolder.rm.GetString("msg_empty_data_id"), (object) target.Description, (object) objectConfig.Id));
      return (ConvertedData) null;
    }
    ConvertedData convertedData;
    if (globalServices.GetService<ConvertedDataCache>().TryGetValue(key, out convertedData))
    {
      service1.Info(LocalizationHolder.rm.GetString("msg_converted_data_found"));
      return convertedData;
    }
    service1.Info(LocalizationHolder.rm.GetString("msg_converted_data_not_found"));
    return (ConvertedData) null;
  }
}
