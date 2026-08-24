// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies.XmlObjectConvertStrategy
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyParams;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Provider.Ips.Serializer;
using System.Collections.Generic;
using XmlReaderAPI.Data;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies;

[DefaultConvertStrategyForType(typeof (IXmlObject))]
public class XmlObjectConvertStrategy : XmlDataEntityConvertStrategy
{
  public override XmlStrategyConvertResultType Convert()
  {
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_base_object_convertion"), (object) this.Target.Description));
    if (!(this.Target is IXmlObject))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_target_must_be"), (object) typeof (IXmlObject).Name));
      return XmlStrategyConvertResultType.WrongStrategyChoise;
    }
    if (!(this.TargetConfig is ObjectConfig))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_config_must_be"), (object) typeof (ObjectConfig).Name));
      return XmlStrategyConvertResultType.WrongStrategyChoise;
    }
    XmlStrategyConvertResultType convertResultType1 = base.Convert();
    if (convertResultType1 == XmlStrategyConvertResultType.FatalError)
      return convertResultType1;
    if (this.ConvertedData.ConvertedEntity != null && this.ConvertedData.ConvertedEntity is ImObject && convertResultType1 == XmlStrategyConvertResultType.Converted)
    {
      XmlStrategyConvertResultType serializer = this.OnAddToSerializer(this.ConvertedData.ConvertedEntity as ImObject);
      if (serializer == XmlStrategyConvertResultType.FatalError)
        return serializer;
    }
    if (((this.TargetConfig as ObjectConfig).ConvertationRules.Rules & ConvertationRules.Relation) == ConvertationRules.Relation)
    {
      this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_object_relations_convertation"), (object) this.Target.Description));
      XmlStrategyConvertResultType convertResultType2 = this.ConvertChildRelations(this.Target as IXmlObject);
      if (convertResultType2 == XmlStrategyConvertResultType.FatalError)
        return convertResultType2;
      this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_object_relations_convertation_complete"), (object) this.Target.Description));
    }
    else
      this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_object_relations_convertation_disabled"), (object) this.TargetConfig.Id));
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_base_object_convertion_complete"), (object) this.Target.Description));
    return XmlStrategyConvertResultType.Converted;
  }

  protected override XmlStrategyConvertResultType DoConvertEntity(
    IXmlDataEntity target,
    out IXmlEntity convertedTarget)
  {
    convertedTarget = (IXmlEntity) new ImObject();
    return XmlStrategyConvertResultType.Converted;
  }

  protected virtual XmlStrategyConvertResultType OnAddToSerializer(ImObject convertedObject)
  {
    if (string.IsNullOrEmpty(convertedObject.F_OBJECT_ID))
    {
      IdGenerator service = this.GlobalServices.GetService<IdGenerator>();
      convertedObject.F_OBJECT_ID = service.GetNewID().ToString();
      this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_object_id_generated"), (object) convertedObject.F_OBJECT_ID));
    }
    IpsDataSerializer service1 = this.GlobalServices.GetService<IpsDataSerializer>();
    if (service1.FindObjectById(convertedObject.F_OBJECT_ID) == null)
      service1.AddObject(convertedObject);
    return XmlStrategyConvertResultType.Converted;
  }

  protected virtual XmlStrategyConvertResultType DoConvertRelation(IXmlRelation target)
  {
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_convert_relation"), (object) target.Description));
    string configId = this.GlobalServices.GetService<ConfigIdCalculator>().FindConfigId((IXmlEntity) target);
    if (string.IsNullOrEmpty(configId))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_config_found"), (object) target.Description));
      this.Logger.Error(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_relation_not_converted"));
      return XmlStrategyConvertResultType.MinorError;
    }
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_search_complete"), (object) target.Description, (object) configId));
    RelationConfig relationConfig = (this.TargetConfig as ObjectConfig).RelationConfigs[configId];
    if (relationConfig == null)
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_config_found"), (object) target.Description));
      this.Logger.Error(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_relation_not_converted"));
      return XmlStrategyConvertResultType.MinorError;
    }
    StrategyExecutor service = this.GlobalServices.GetService<StrategyExecutor>();
    XmlStrategyConvertResultType convertResultType = XmlStrategyConvertResultType.FatalError;
    AddStrategyParams addStrategyParams = new AddStrategyParams();
    addStrategyParams.Add(AddStrategyParamType.GlobalServices, (object) this.GlobalServices);
    addStrategyParams.Add(AddStrategyParamType.ConvertTarget, (object) target);
    addStrategyParams.Add(AddStrategyParamType.ConvertTargetConfig, (object) relationConfig);
    AddStrategyParams strategyParams = addStrategyParams;
    foreach (string id in relationConfig.ConvertStrategies.Ids)
    {
      bool flag = false;
      convertResultType = service.ExecuteStrategy(relationConfig.ConvertStrategies[id], strategyParams);
      switch (convertResultType)
      {
        case XmlStrategyConvertResultType.FatalError:
          this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_critical_error_relation_convertion"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.MinorError:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_minor_error_relation_convertion"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.WrongStrategyChoise:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_wrong_strategy_selected_for_relation"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.Converted:
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_relation_converted"), (object) target.Description));
          flag = true;
          break;
      }
      if (flag)
        break;
    }
    if (convertResultType == XmlStrategyConvertResultType.WrongStrategyChoise || relationConfig.ConvertStrategies.Count == 0)
    {
      convertResultType = service.ExecuteDefaultStrategy(strategyParams);
      switch (convertResultType)
      {
        case XmlStrategyConvertResultType.FatalError:
          this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_critical_error_relation_convertion"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.MinorError:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_minor_error_relation_convertion"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.WrongStrategyChoise:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_wrong_strategy_selected_for_relation"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.Converted:
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_relation_converted"), (object) target.Description));
          break;
      }
    }
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_convert_relation_complete"), (object) target.Description));
    return convertResultType;
  }

  private XmlStrategyConvertResultType ConvertChildRelations(IXmlObject parent)
  {
    this.Logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_object_child_relations_convertation"));
    IReadOnlyCollection<IXmlRelation> objChildRelations = this.GlobalServices.GetService<IXmlDataProvider>().GetObjChildRelations(parent);
    if (objChildRelations != null)
    {
      foreach (IXmlRelation target in (IEnumerable<IXmlRelation>) objChildRelations)
      {
        XmlStrategyConvertResultType convertResultType = this.DoConvertRelation(target);
        if (convertResultType == XmlStrategyConvertResultType.FatalError)
          return convertResultType;
      }
    }
    this.Logger.Info(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_object_child_relations_convertation_complete"));
    return XmlStrategyConvertResultType.Converted;
  }
}
