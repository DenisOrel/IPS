// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies.XmlDataEntityConvertStrategy
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyParams;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess;
using Intermech.XmlExchange.IpsXml.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies;

public abstract class XmlDataEntityConvertStrategy : XmlEntityConvertStrategy
{
  public ConvertedData ConvertedData { get; set; }

  public override XmlStrategyConvertResultType Convert()
  {
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_base_dataentity_convertion"), (object) this.Target.Description));
    if (!(this.Target is IXmlDataEntity))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_entity_type_must_be_inherited_from"), (object) typeof (IXmlDataEntity).Name));
      return XmlStrategyConvertResultType.WrongStrategyChoise;
    }
    if (this.TargetConfig is BaseConvertableEntityWithParamsConfig targetConfig)
    {
      ConvertationRules rules = targetConfig.ConvertationRules.Rules;
      UniqueControlRuleConfig uniqueControlRule = targetConfig.UniqueControlRule;
      XmlStrategyConvertResultType convertResultType = XmlStrategyConvertResultType.Converted;
      if ((rules & ConvertationRules.Object) == ConvertationRules.Object)
      {
        this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_entity_convertation"), (object) this.Target.Description));
        if (uniqueControlRule.Rule == UniqueControlRule.IdControl)
        {
          this.Logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_entity_unique_control"));
          this.ConvertedData = this.FindConvertedData(this.Target, uniqueControlRule, this.TargetConfig.Id);
          this.Logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_entity_unique_control_complete"));
        }
        if (this.ConvertedData != null)
        {
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_entity_already_converted"), (object) this.Target.Description));
          return XmlStrategyConvertResultType.Converted;
        }
        IXmlEntity convertedTarget;
        convertResultType = this.DoConvertEntity(this.Target as IXmlDataEntity, out convertedTarget);
        switch (convertResultType)
        {
          case XmlStrategyConvertResultType.FatalError:
            return convertResultType;
          case XmlStrategyConvertResultType.Converted:
            if (convertedTarget != null)
            {
              this.ConvertedData = this.CacheConvertedData(this.Target, convertedTarget, uniqueControlRule, this.TargetConfig.Id);
              break;
            }
            break;
        }
        if (this.ConvertedData == null)
          this.ConvertedData = new ConvertedData()
          {
            ConvertedEntity = convertedTarget
          };
        this.Logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_entity_convertation_complete"));
      }
      else
        this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_entity_convertation_disabled"), (object) this.TargetConfig.Id));
      if ((rules & ConvertationRules.ObjectParams) == ConvertationRules.ObjectParams || (rules & ConvertationRules.RelationParams) == ConvertationRules.RelationParams)
      {
        this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_entity_params_convertation"), (object) this.Target.Description));
        convertResultType = this.ConvertParams(this.Target as IXmlDataEntity, this.ConvertedData);
        if (convertResultType == XmlStrategyConvertResultType.FatalError)
          return convertResultType;
        this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_entity_params_convertation_complete"), (object) this.Target.Description));
      }
      else
        this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_entity_params_convertation_disabled"), (object) this.TargetConfig.Id));
      this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_base_dataentity_convertion_complete"), (object) this.Target.Description));
      return convertResultType;
    }
    this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_config_must_be_one_of"), (object) $"{typeof (ObjectConfig).Name},{typeof (RelationConfig).Name}"));
    return XmlStrategyConvertResultType.WrongStrategyChoise;
  }

  protected abstract XmlStrategyConvertResultType DoConvertEntity(
    IXmlDataEntity target,
    out IXmlEntity convertedTarget);

  protected XmlStrategyConvertResultType ConvertParams(
    IXmlDataEntity owner,
    ConvertedData convertedData)
  {
    List<ParamConfig> paramConfigList1 = new List<ParamConfig>();
    ParamConfigs paramConfigs1 = ((BaseConvertableEntityWithParamsConfig) this.TargetConfig).ParamConfigs;
    foreach (string id in paramConfigs1.ConstConfigs.Ids)
      paramConfigList1.Add(paramConfigs1.ConstConfigs[id]);
    if (paramConfigList1.Count > 0)
    {
      paramConfigList1.Sort((Comparison<ParamConfig>) ((left, right) => left.Order - right.Order));
      foreach (ParamConfig targetConfig in paramConfigList1)
      {
        XmlStrategyConvertResultType convertResultType = this.DoConvertParam(owner, convertedData, (IXmlParam) null, targetConfig);
        if (convertResultType == XmlStrategyConvertResultType.FatalError)
          return convertResultType;
      }
    }
    List<IXmlParam> xmlParamList = new List<IXmlParam>();
    foreach (IXmlParam xmlParam in (IEnumerable<IXmlParam>) owner.XmlParams)
    {
      if (ParamConfig.IsArrayParam(xmlParam.Name))
      {
        xmlParamList.Add(xmlParam);
      }
      else
      {
        ParamConfig paramConfig1;
        XmlStrategyConvertResultType paramConfig2 = this.FindParamConfig(xmlParam, out paramConfig1);
        if (paramConfig2 == XmlStrategyConvertResultType.FatalError)
          return paramConfig2;
        if (paramConfig1 != null)
        {
          XmlStrategyConvertResultType convertResultType = this.DoConvertParam(owner, convertedData, xmlParam, paramConfig1);
          if (convertResultType == XmlStrategyConvertResultType.FatalError)
            return convertResultType;
        }
      }
    }
    ParamConfigs paramConfigs2 = ((BaseConvertableEntityWithParamsConfig) this.TargetConfig).ParamConfigs;
    foreach (string id in paramConfigs2.SimpleConfigs.Ids)
    {
      XmlStrategyConvertResultType convertResultType = XmlStrategyConvertResultType.Converted;
      ParamConfig simpleConfig = paramConfigs2.SimpleConfigs[id];
      if (simpleConfig.ParamParentType != ParamType.Object)
        convertResultType = this.DoConvertParam(owner, convertedData, (IXmlParam) null, simpleConfig);
      if (convertResultType == XmlStrategyConvertResultType.FatalError)
        return convertResultType;
    }
    foreach (IXmlParam target in xmlParamList)
    {
      ParamConfig paramConfig;
      XmlStrategyConvertResultType arrayParamConfig = this.FindArrayParamConfig(target, out paramConfig);
      if (arrayParamConfig == XmlStrategyConvertResultType.FatalError)
        return arrayParamConfig;
      if (paramConfig != null)
      {
        XmlStrategyConvertResultType convertResultType = this.DoConvertParam(owner, convertedData, target, paramConfig);
        if (convertResultType == XmlStrategyConvertResultType.FatalError)
          return convertResultType;
      }
    }
    List<ParamConfig> paramConfigList2 = new List<ParamConfig>();
    ParamConfigs paramConfigs3 = ((BaseConvertableEntityWithParamsConfig) this.TargetConfig).ParamConfigs;
    foreach (string id in paramConfigs3.CalcConfigs.Ids)
      paramConfigList2.Add(paramConfigs3.CalcConfigs[id]);
    if (paramConfigList2.Count > 0)
    {
      paramConfigList2.Sort((Comparison<ParamConfig>) ((left, right) => left.Order - right.Order));
      foreach (ParamConfig targetConfig in paramConfigList2)
      {
        XmlStrategyConvertResultType convertResultType = this.DoConvertParam(owner, convertedData, (IXmlParam) null, targetConfig);
        if (convertResultType == XmlStrategyConvertResultType.FatalError)
          return convertResultType;
      }
    }
    return XmlStrategyConvertResultType.Converted;
  }

  protected virtual XmlStrategyConvertResultType DoConvertParam(
    IXmlDataEntity owner,
    ConvertedData convertedData,
    IXmlParam target,
    ParamConfig targetConfig)
  {
    string str = target != null ? target.Description : targetConfig.Id;
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_convert_param"), (object) str));
    StrategyExecutor service = this.GlobalServices.GetService<StrategyExecutor>();
    XmlStrategyConvertResultType convertResultType = XmlStrategyConvertResultType.FatalError;
    IXmlEntity xmlEntity = (IXmlEntity) null;
    ParamsCache paramsCache;
    if (convertedData == null)
    {
      paramsCache = new ParamsCache();
    }
    else
    {
      xmlEntity = convertedData.ConvertedEntity;
      paramsCache = convertedData.ConvertedEntityParams;
    }
    AddStrategyParams addStrategyParams = new AddStrategyParams();
    addStrategyParams.Add(AddStrategyParamType.GlobalServices, (object) this.GlobalServices);
    addStrategyParams.Add(AddStrategyParamType.ConvertTarget, (object) target);
    addStrategyParams.Add(AddStrategyParamType.ConvertTargetType, (object) typeof (IXmlParam));
    addStrategyParams.Add(AddStrategyParamType.ConvertTargetConfig, (object) targetConfig);
    addStrategyParams.Add(AddStrategyParamType.ParamOwner, (object) owner);
    addStrategyParams.Add(AddStrategyParamType.ConvertedParamOwner, (object) xmlEntity);
    addStrategyParams.Add(AddStrategyParamType.ConvertedOwnerParamsCache, (object) paramsCache);
    AddStrategyParams strategyParams = addStrategyParams;
    foreach (string id in targetConfig.ConvertStrategies.Ids)
    {
      bool flag = false;
      convertResultType = service.ExecuteStrategy(targetConfig.ConvertStrategies[id], strategyParams);
      switch (convertResultType)
      {
        case XmlStrategyConvertResultType.FatalError:
          this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_critical_error_param_convertion"), (object) str));
          break;
        case XmlStrategyConvertResultType.MinorError:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_minor_error_param_convertion"), (object) str));
          break;
        case XmlStrategyConvertResultType.WrongStrategyChoise:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_wrong_strategy_selected_for_param"), (object) str));
          break;
        case XmlStrategyConvertResultType.Converted:
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_param_converted"), (object) str));
          flag = true;
          break;
      }
      if (flag)
        break;
    }
    if (convertResultType == XmlStrategyConvertResultType.WrongStrategyChoise || targetConfig.ConvertStrategies.Count == 0)
    {
      convertResultType = service.ExecuteDefaultStrategy(strategyParams);
      switch (convertResultType)
      {
        case XmlStrategyConvertResultType.FatalError:
          this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_critical_error_param_convertion"), (object) str));
          break;
        case XmlStrategyConvertResultType.MinorError:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_minor_error_param_convertion"), (object) str));
          break;
        case XmlStrategyConvertResultType.WrongStrategyChoise:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_wrong_strategy_selected_for_param"), (object) str));
          break;
        case XmlStrategyConvertResultType.Converted:
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_param_converted"), (object) str));
          break;
      }
    }
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_convert_param_complete"), (object) str));
    return convertResultType;
  }

  private XmlStrategyConvertResultType FindParamConfig(IXmlParam param, out ParamConfig paramConfig)
  {
    paramConfig = (ParamConfig) null;
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_search_param_config_by_param_name"), (object) param.Id));
    if (this.TargetConfig is BaseConvertableEntityWithParamsConfig targetConfig1)
    {
      paramConfig = targetConfig1.ParamConfigs.SimpleConfigs[param.Id];
      if (paramConfig != null)
      {
        if (paramConfig.ParamParentType == ParamType.Object)
        {
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_search_complete"), (object) param.Description, (object) paramConfig.Id));
        }
        else
        {
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_param_parent_type_must_be_object"), (object) param.Description, (object) paramConfig.Id, (object) paramConfig.ParamParentType.ToXMLTag()));
          paramConfig = (ParamConfig) null;
        }
      }
      else
      {
        string configId = this.GlobalServices.GetService<ConfigIdCalculator>().FindConfigId((IXmlEntity) param);
        if (string.IsNullOrEmpty(configId))
        {
          this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_config_found"), (object) param.Description));
          this.Logger.Error(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_param_not_converted"));
          return XmlStrategyConvertResultType.MinorError;
        }
        this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_search_complete"), (object) param.Description, (object) configId));
        if (this.TargetConfig is BaseConvertableEntityWithParamsConfig targetConfig)
          paramConfig = targetConfig.ParamConfigs.SimpleConfigs[configId];
      }
      if (paramConfig != null)
        return XmlStrategyConvertResultType.Converted;
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_config_found"), (object) param.Description));
      this.Logger.Error(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_param_not_converted"));
      return XmlStrategyConvertResultType.MinorError;
    }
    this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_config_must_be_one_of"), (object) $"{typeof (ObjectConfig).Name},{typeof (RelationConfig).Name}"));
    return XmlStrategyConvertResultType.WrongStrategyChoise;
  }

  private XmlStrategyConvertResultType FindArrayParamConfig(
    IXmlParam param,
    out ParamConfig paramConfig)
  {
    paramConfig = (ParamConfig) null;
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_search_array_param_config"), (object) param.Id));
    string arrayParamName;
    if (!ParamConfig.IsArrayParam(param.Name, out arrayParamName, out int _))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_wrong_array_param_name_format"), (object) param.Id));
      return XmlStrategyConvertResultType.MinorError;
    }
    if (this.TargetConfig is BaseConvertableEntityWithParamsConfig targetConfig)
    {
      paramConfig = targetConfig.ParamConfigs.SimpleConfigs[arrayParamName];
      if (paramConfig != null)
      {
        this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_search_complete"), (object) param.Description, (object) paramConfig.Id));
        return XmlStrategyConvertResultType.Converted;
      }
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_config_found"), (object) param.Description));
      this.Logger.Error(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_param_not_converted"));
      return XmlStrategyConvertResultType.MinorError;
    }
    this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_config_must_be_one_of"), (object) $"{typeof (ObjectConfig).Name},{typeof (RelationConfig).Name}"));
    return XmlStrategyConvertResultType.WrongStrategyChoise;
  }

  private ConvertedData FindConvertedData(
    IXmlEntity xmlDataEntity,
    UniqueControlRuleConfig uniqueRuleConfig,
    string parentConfigId)
  {
    this.Logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_find_converted_data"));
    ConfigIdCalculator service1 = this.GlobalServices.GetService<ConfigIdCalculator>();
    TechcardToIpsConfig service2 = this.GlobalServices.GetService<TechcardToIpsConfig>();
    if (string.IsNullOrEmpty(uniqueRuleConfig.Id))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_empty_unique_rule_id"), (object) parentConfigId));
      return (ConvertedData) null;
    }
    Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig idConfig = service2.IdConfigs[uniqueRuleConfig.Id];
    if (idConfig == null)
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_id_config_not_found"), (object) parentConfigId));
      return (ConvertedData) null;
    }
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_id_config_found"), (object) idConfig.Id));
    string key = service1.CalcId(xmlDataEntity, idConfig);
    if (string.IsNullOrEmpty(key))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_empty_data_id"), (object) xmlDataEntity.Description, (object) uniqueRuleConfig.Id));
      return (ConvertedData) null;
    }
    ConvertedData convertedData;
    if (this.GlobalServices.GetService<ConvertedDataCache>().TryGetValue(key, out convertedData))
    {
      this.Logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_converted_data_found"));
      return convertedData;
    }
    this.Logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_converted_data_not_found"));
    return (ConvertedData) null;
  }

  private ConvertedData CacheConvertedData(
    IXmlEntity sourceData,
    IXmlEntity convertedEntity,
    UniqueControlRuleConfig uniqueRuleConfig,
    string parentConfigId)
  {
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_cache_converted_object"), (object) convertedEntity.Description));
    ConfigIdCalculator service1 = this.GlobalServices.GetService<ConfigIdCalculator>();
    TechcardToIpsConfig service2 = this.GlobalServices.GetService<TechcardToIpsConfig>();
    if (string.IsNullOrEmpty(uniqueRuleConfig.Id))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_empty_unique_rule_id"), (object) parentConfigId));
      return (ConvertedData) null;
    }
    Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig idConfig = service2.IdConfigs[uniqueRuleConfig.Id];
    if (idConfig == null)
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_id_config_not_found"), (object) parentConfigId));
      return (ConvertedData) null;
    }
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_id_config_found"), (object) idConfig.Id));
    string key = service1.CalcId(sourceData, idConfig);
    if (string.IsNullOrEmpty(key))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_empty_data_id"), (object) convertedEntity.Description, (object) uniqueRuleConfig.Id));
      return (ConvertedData) null;
    }
    ConvertedDataCache service3 = this.GlobalServices.GetService<ConvertedDataCache>();
    if (service3.ContainsKey(key))
    {
      this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_data_already_exists_in_cache"), (object) convertedEntity.Description));
      return service3[key];
    }
    ConvertedData convertedData = new ConvertedData()
    {
      ConvertedEntity = convertedEntity
    };
    service3.Add(key, convertedData);
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_data_cached"), (object) sourceData.Description, (object) key));
    return convertedData;
  }
}
