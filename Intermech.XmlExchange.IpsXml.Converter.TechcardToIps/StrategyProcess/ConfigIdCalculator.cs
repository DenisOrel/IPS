// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess.ConfigIdCalculator
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using Intermech.XmlExchange.IpsXml.Provider.Techcard.Format;
using Intermech.XmlExchange.IpsXml.Provider.Techcard.Params;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess;

internal class ConfigIdCalculator
{
  private readonly List<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig> preparedIdConfigs = new List<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig>();
  private Dictionary<IdConfigType, List<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig>> _findIdConfigs = new Dictionary<IdConfigType, List<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig>>()
  {
    {
      IdConfigType.Object,
      new List<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig>()
    },
    {
      IdConfigType.Relation,
      new List<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig>()
    },
    {
      IdConfigType.Param,
      new List<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig>()
    }
  };
  private IServiceProvider _services;
  private IpsXmlLogger _logger;
  private const string ANY_VALUE = "_any_";

  public ConfigIdCalculator(IServiceProvider services)
  {
    this._services = services;
    this._logger = this._services.GetService<IpsXmlLogger>();
  }

  public void Prepare(IdConfigs idConfigs)
  {
    this._logger.Info(LocalizationHolder.rm.GetString("msg_prepare_id_configs"));
    idConfigs.ForEach((Action<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig>) (config =>
    {
      if (config.CalcResultType == IdConfigCalcResultType.ConfigID)
        return;
      this._findIdConfigs[config.Type].Add(config);
      if (!(config.Content is IdPartGroup))
        return;
      this.PrepareIdPartGroupConfig(config.Content as IdPartGroup);
    }));
    this._findIdConfigs.Values.ToList<List<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig>>().ForEach((Action<List<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig>>) (configs => configs.Sort((Comparison<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig>) ((left, right) => left.Order - right.Order))));
    this._logger.Info(LocalizationHolder.rm.GetString("msg_prepare_id_configs_complete"));
  }

  public string FindConfigId(IXmlEntity target)
  {
    this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_searching_config_id"), (object) target.Description));
    List<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig> findIdConfig;
    switch (target)
    {
      case IXmlRelation _:
        findIdConfig = this._findIdConfigs[IdConfigType.Relation];
        break;
      case IXmlObject _:
        findIdConfig = this._findIdConfigs[IdConfigType.Object];
        break;
      case IXmlParam _:
        findIdConfig = this._findIdConfigs[IdConfigType.Param];
        break;
      default:
        this._logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_obj_type_not_supported"), (object) target.GetType().Name));
        this._logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_obj_type_must_be_one_of"), (object) typeof (IXmlObject).Name, (object) typeof (IXmlRelation).Name, (object) typeof (IXmlParam).Name));
        return string.Empty;
    }
    foreach (Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig config in findIdConfig)
    {
      if (this.CheckIdConfigCondition(config, target))
      {
        this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_config_id_found"), (object) config.Id));
        return config.Id;
      }
    }
    this._logger.Warn(LocalizationHolder.rm.GetString("msg_config_id_not_found"));
    return string.Empty;
  }

  public string CalcId(IXmlEntity target, Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig config)
  {
    this._logger.Info(LocalizationHolder.rm.GetString("msg_calc_id"));
    string str = string.Empty;
    if (config.Content is IdPart)
      str = this.CalcIdPartConfig(config.Content as IdPart, target);
    else if (config.Content is IdPartGroup)
      str = this.CalcIdPartGroupConfig(config.Content as IdPartGroup, target);
    this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_calc_id_complete"), (object) str));
    return $"{config.Id}_{str}";
  }

  private void PrepareIdPartGroupConfig(IdPartGroup group)
  {
    group.Content.Sort((Comparison<BaseIdPart>) ((left, right) => left.Order - right.Order));
    group.Content.ForEach((Action<BaseIdPart>) (part =>
    {
      if (!(part is IdPartGroup))
        return;
      this.PrepareIdPartGroupConfig(part as IdPartGroup);
    }));
  }

  private bool CheckIdConfigCondition(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig config, IXmlEntity target)
  {
    this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_check_id_config_condition"), (object) config.Id));
    bool flag = !(config.Content is IdPart) ? config.Content is IdPartGroup && this.CheckIdPartGroupCondition(config.Content as IdPartGroup, target) : this.CheckIdPartCondition(config.Content as IdPart, target);
    if (flag)
      this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_id_config_condition_applied"), (object) config.Id));
    else
      this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_id_config_condition_not_applied"), (object) config.Id));
    return flag;
  }

  private bool CheckIdPartCondition(IdPart idPartConfig, IXmlEntity target)
  {
    this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_check_id_part_condition"), (object) idPartConfig.Id));
    switch (idPartConfig.ParamSubType)
    {
      case ParamSubType.Unknown:
        this._logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_unsupported_condition_type"), (object) idPartConfig.ParamSubType.ToXMLTag()));
        return false;
      case ParamSubType.ObjectType:
        IXmlEntity xmlEntity;
        switch (idPartConfig.ParamType)
        {
          case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ChildObject:
          case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ParentObject:
            if (!(target is IXmlRelation))
            {
              this._logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_wrong_param_sub_type"), (object) idPartConfig.Id, (object) target.Description));
              return false;
            }
            IXmlDataProvider service1 = this._services.GetService<IXmlDataProvider>();
            xmlEntity = idPartConfig.ParamType != Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ChildObject ? (IXmlEntity) service1.GetRelParentObj(target as IXmlRelation) : (IXmlEntity) service1.GetRelChildObj(target as IXmlRelation);
            break;
          default:
            xmlEntity = target;
            break;
        }
        bool flag1 = xmlEntity is BaseTechXmlNode && ((xmlEntity as BaseTechXmlNode).NodeType.ToTechXMLTag() == idPartConfig.Value || idPartConfig.Value == "_any_");
        if (flag1)
          this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_equal_node_types"), (object) idPartConfig.Value));
        else
          this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_not_equal_node_types"), (object) idPartConfig.Value, xmlEntity is BaseTechXmlNode ? (object) (xmlEntity as BaseTechXmlNode).NodeType.ToTechXMLTag() : (object) xmlEntity.Description));
        return flag1;
      default:
        switch (target)
        {
          case IXmlDataEntity _:
            IXmlDataEntity xmlDataEntity;
            switch (idPartConfig.ParamType)
            {
              case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ChildObject:
              case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ParentObject:
                if (!(target is IXmlRelation))
                {
                  this._logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_wrong_param_sub_type"), (object) idPartConfig.Id, (object) target.Description));
                  return false;
                }
                IXmlDataProvider service2 = this._services.GetService<IXmlDataProvider>();
                xmlDataEntity = idPartConfig.ParamType != Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ChildObject ? (IXmlDataEntity) service2.GetRelParentObj(target as IXmlRelation) : (IXmlDataEntity) service2.GetRelChildObj(target as IXmlRelation);
                break;
              default:
                xmlDataEntity = target as IXmlDataEntity;
                break;
            }
            if (string.IsNullOrEmpty(idPartConfig.Name))
            {
              this._logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_empty_config_paramname"), (object) idPartConfig.Id));
              return false;
            }
            this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_find_param"), (object) idPartConfig.Name));
            foreach (IXmlParam xmlParam in (IEnumerable<IXmlParam>) xmlDataEntity.XmlParams)
            {
              if (xmlParam.Name == idPartConfig.Name)
              {
                bool flag2 = xmlParam.Value == idPartConfig.Value || idPartConfig.Value == "_any_";
                if (xmlParam is TechXmlParam)
                  flag2 &= (xmlParam as TechXmlParam).ParamType.ToTechXMLTag() == idPartConfig.ParamSubType.ToXMLTag();
                if (flag2)
                {
                  this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_equal_values"), (object) idPartConfig.Name, (object) idPartConfig.Value, (object) idPartConfig.ParamSubType.ToXMLTag()));
                  return flag2;
                }
              }
            }
            this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_param_not_found"), (object) idPartConfig.Name));
            break;
          case IXmlParam _:
            return (target as IXmlParam).Name == idPartConfig.Name;
        }
        this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_not_equal_values"), (object) idPartConfig.Name, (object) idPartConfig.Value, (object) idPartConfig.ParamSubType.ToXMLTag()));
        return false;
    }
  }

  private string CalcIdPartConfig(IdPart idPartConfig, IXmlEntity target)
  {
    this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_calc_id_part_condition"), (object) idPartConfig.Id));
    string str1 = string.Empty;
    switch (idPartConfig.ParamSubType)
    {
      case ParamSubType.Unknown:
        this._logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_unsupported_condition_type"), (object) idPartConfig.ParamSubType.ToXMLTag()));
        return string.Empty;
      case ParamSubType.ObjectType:
        IXmlEntity xmlEntity;
        switch (idPartConfig.ParamType)
        {
          case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ChildObject:
          case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ParentObject:
            if (!(target is IXmlRelation))
            {
              this._logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_wrong_param_sub_type"), (object) idPartConfig.Id, (object) target.Description));
              return string.Empty;
            }
            IXmlDataProvider service1 = this._services.GetService<IXmlDataProvider>();
            xmlEntity = idPartConfig.ParamType != Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ChildObject ? (IXmlEntity) service1.GetRelParentObj(target as IXmlRelation) : (IXmlEntity) service1.GetRelChildObj(target as IXmlRelation);
            break;
          default:
            xmlEntity = target;
            break;
        }
        string str2 = xmlEntity is BaseTechXmlNode ? (xmlEntity as BaseTechXmlNode).NodeType.ToTechXMLTag() : string.Empty;
        this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_calc_id_part_condition_complete"), (object) str2));
        return str2;
      default:
        switch (target)
        {
          case IXmlDataEntity _:
            IXmlDataEntity xmlDataEntity;
            switch (idPartConfig.ParamType)
            {
              case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ChildObject:
              case Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ParentObject:
                if (!(target is IXmlRelation))
                {
                  this._logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_wrong_param_sub_type"), (object) idPartConfig.Id, (object) target.Description));
                  return string.Empty;
                }
                IXmlDataProvider service2 = this._services.GetService<IXmlDataProvider>();
                xmlDataEntity = idPartConfig.ParamType != Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.ParamType.ChildObject ? (IXmlDataEntity) service2.GetRelParentObj(target as IXmlRelation) : (IXmlDataEntity) service2.GetRelChildObj(target as IXmlRelation);
                break;
              default:
                xmlDataEntity = target as IXmlDataEntity;
                break;
            }
            using (IEnumerator<IXmlParam> enumerator = xmlDataEntity.XmlParams.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IXmlParam current = enumerator.Current;
                if (current.Name == idPartConfig.Name && !string.IsNullOrEmpty(current.Value) && (!(current is TechXmlParam) || (current as TechXmlParam).ParamType.ToTechXMLTag() == idPartConfig.ParamSubType.ToXMLTag()))
                  str1 = !string.IsNullOrEmpty(str1) ? $"{str1}_{current.Value}" : current.Value;
              }
              break;
            }
          case IXmlParam _:
            str1 += (target as IXmlParam).Value;
            break;
        }
        this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_calc_id_part_condition_complete"), (object) str1));
        return str1;
    }
  }

  private bool CheckIdPartGroupCondition(IdPartGroup idPartGroupConfig, IXmlEntity target)
  {
    this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_check_id_part_group_condition"), (object) idPartGroupConfig.Id));
    bool flag1 = false;
    for (int index = 0; index < idPartGroupConfig.Content.Count; ++index)
    {
      bool flag2 = false;
      BaseIdPart baseIdPart = idPartGroupConfig.Content[index];
      if (baseIdPart is IdPart)
        flag2 = this.CheckIdPartCondition(baseIdPart as IdPart, target);
      else if (baseIdPart is IdPartGroup)
        flag2 = this.CheckIdPartGroupCondition(baseIdPart as IdPartGroup, target);
      if (index == 0)
      {
        flag1 = flag2;
      }
      else
      {
        switch (idPartGroupConfig.Condition)
        {
          case ConditionType.And:
            flag1 &= flag2;
            break;
          case ConditionType.Or:
            if (flag2)
              return flag2;
            break;
        }
        if (!flag1 && idPartGroupConfig.Condition == ConditionType.And)
          break;
      }
    }
    if (flag1)
      this._logger.Info(LocalizationHolder.rm.GetString("msg_id_part_group_condition_executed"));
    else
      this._logger.Info(LocalizationHolder.rm.GetString("msg_id_part_group_condition_not_executed"));
    return flag1;
  }

  private string CalcIdPartGroupConfig(IdPartGroup idPartGroupConfig, IXmlEntity target)
  {
    this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_calc_id_part_group_condition"), (object) idPartGroupConfig.Id));
    string str1 = string.Empty;
    for (int index = 0; index < idPartGroupConfig.Content.Count; ++index)
    {
      string str2 = string.Empty;
      BaseIdPart baseIdPart = idPartGroupConfig.Content[index];
      if (baseIdPart is IdPart)
        str2 = this.CalcIdPartConfig(baseIdPart as IdPart, target);
      else if (baseIdPart is IdPartGroup)
        str2 = this.CalcIdPartGroupConfig(baseIdPart as IdPartGroup, target);
      if (!string.IsNullOrEmpty(str2))
      {
        if (index == 0)
        {
          str1 = str2;
        }
        else
        {
          str1 = !string.IsNullOrEmpty(str1) ? $"{str1}_{str2}" : str2;
          if (idPartGroupConfig.Condition == ConditionType.Or)
            return str1;
        }
      }
    }
    if (!string.IsNullOrEmpty(str1))
      this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_calc_id_part_group_condition_complete"), (object) str1));
    else
      this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_calc_id_part_group_condition_complete"), (object) "Empty"));
    return str1;
  }
}
