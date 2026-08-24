// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies.XmlRelationConvertStrategy
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using Intermech.Interfaces.XmlExchange;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyParams;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Utils;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Provider.Ips.Serializer;
using XmlReaderAPI.Data;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies;

[DefaultConvertStrategyForType(typeof (IXmlRelation))]
public class XmlRelationConvertStrategy : XmlDataEntityConvertStrategy
{
  private ImRelation convertedRelation;

  public override XmlStrategyConvertResultType Convert()
  {
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_base_relation_convertion"), (object) this.Target.Description));
    if (!(this.Target is IXmlRelation))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_target_must_be"), (object) typeof (IXmlRelation).Name));
      return XmlStrategyConvertResultType.WrongStrategyChoise;
    }
    if (!(this.TargetConfig is RelationConfig))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_config_must_be"), (object) typeof (RelationConfig).Name));
      return XmlStrategyConvertResultType.WrongStrategyChoise;
    }
    XmlStrategyConvertResultType convertResultType1 = this.DoConvertChild(this.Target as IXmlRelation, this.GlobalServices.GetService<IXmlDataProvider>().GetRelChildObj(this.Target as IXmlRelation));
    if (convertResultType1 == XmlStrategyConvertResultType.FatalError)
      return convertResultType1;
    XmlStrategyConvertResultType convertResultType2 = base.Convert();
    if (convertResultType2 == XmlStrategyConvertResultType.FatalError)
      return convertResultType2;
    if (this.convertedRelation != null)
    {
      if (string.IsNullOrEmpty(this.convertedRelation.GetAsString("F_PRJLINK_ID", string.Empty)))
      {
        int newId = this.GlobalServices.GetService<IdGenerator>().GetNewID();
        this.convertedRelation.SetAsInt32("F_PRJLINK_ID", newId);
        this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_relation_id_generated"), (object) newId));
      }
      this.GlobalServices.GetService<IpsDataSerializer>().AddRelation(this.convertedRelation);
    }
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_base_relation_convertion_complete"), (object) this.Target.Description));
    return convertResultType2;
  }

  protected virtual XmlStrategyConvertResultType DoConvertChild(
    IXmlRelation targetRelation,
    IXmlObject target)
  {
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_child_object_convertion"), (object) target.Description));
    string configId = this.GlobalServices.GetService<ConfigIdCalculator>().FindConfigId((IXmlEntity) target);
    if (string.IsNullOrEmpty(configId))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_config_found"), (object) target.Description));
      this.Logger.Error(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_relation_not_converted"));
      return XmlStrategyConvertResultType.MinorError;
    }
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_search_complete"), (object) target.Description, (object) configId));
    ObjectConfig objectConfig = this.GlobalServices.GetService<TechcardToIpsConfig>().ObjectConfigs[configId];
    if (objectConfig == null)
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_config_found"), (object) target.Description));
      this.Logger.Error(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_relation_not_converted"));
      return XmlStrategyConvertResultType.MinorError;
    }
    XmlStrategyConvertResultType convertResultType = XmlStrategyConvertResultType.FatalError;
    AddStrategyParams addStrategyParams = new AddStrategyParams();
    addStrategyParams.Add(AddStrategyParamType.GlobalServices, (object) this.GlobalServices);
    addStrategyParams.Add(AddStrategyParamType.ConvertTarget, (object) target);
    addStrategyParams.Add(AddStrategyParamType.ConvertTargetConfig, (object) objectConfig);
    addStrategyParams.Add(AddStrategyParamType.ConvertTargetRelation, (object) targetRelation);
    AddStrategyParams strategyParams = addStrategyParams;
    foreach (string id in objectConfig.ConvertStrategies.Ids)
    {
      bool flag = false;
      convertResultType = this.GlobalServices.GetService<StrategyExecutor>().ExecuteStrategy(objectConfig.ConvertStrategies[id], strategyParams);
      switch (convertResultType)
      {
        case XmlStrategyConvertResultType.FatalError:
          this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_critical_error_object_convertion"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.MinorError:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_minor_error_object_convertion"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.WrongStrategyChoise:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_wrong_strategy_selected_for_obj"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.Converted:
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_obj_converted"), (object) target.Description));
          flag = true;
          break;
      }
      if (flag)
        break;
    }
    if (convertResultType == XmlStrategyConvertResultType.WrongStrategyChoise || objectConfig.ConvertStrategies.Count == 0)
    {
      convertResultType = this.GlobalServices.GetService<StrategyExecutor>().ExecuteDefaultStrategy(strategyParams);
      switch (convertResultType)
      {
        case XmlStrategyConvertResultType.FatalError:
          this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_critical_error_object_convertion"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.MinorError:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_minor_error_object_convertion"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.WrongStrategyChoise:
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_wrong_strategy_selected_for_obj"), (object) target.Description));
          break;
        case XmlStrategyConvertResultType.Converted:
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_obj_converted"), (object) target.Description));
          break;
      }
    }
    this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_child_object_convertion_complete"), (object) target.Description));
    return convertResultType;
  }

  protected override XmlStrategyConvertResultType DoConvertEntity(
    IXmlDataEntity target,
    out IXmlEntity convertedTarget)
  {
    convertedTarget = (IXmlEntity) null;
    IXmlDataProvider service = this.GlobalServices.GetService<IXmlDataProvider>();
    IXmlObject relParentObj = service.GetRelParentObj(target as IXmlRelation);
    ConvertedData convertedData1 = ConvertUtils.FindConvertedData((IXmlEntity) relParentObj, this.GlobalServices);
    if (convertedData1 == null)
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_converted_parent_for_relation"), (object) relParentObj.Description, (object) target.Description));
      return XmlStrategyConvertResultType.MinorError;
    }
    IXmlObject relChildObj = service.GetRelChildObj(target as IXmlRelation);
    ConvertedData convertedData2 = ConvertUtils.FindConvertedData((IXmlEntity) relChildObj, this.GlobalServices);
    if (convertedData2 == null)
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_converted_child_for_relation"), (object) relChildObj.Description, (object) target.Description));
      return XmlStrategyConvertResultType.MinorError;
    }
    this.convertedRelation = new ImRelation();
    convertedTarget = (IXmlEntity) this.convertedRelation;
    this.convertedRelation.SetAsInt32(XmlExchangeConsts.XML.F_PROJ_OBJ, System.Convert.ToInt32((convertedData1.ConvertedEntity as ImObject).F_OBJECT_ID));
    this.convertedRelation.SetAsInt32(XmlExchangeConsts.XML.F_PART_OBJ, System.Convert.ToInt32((convertedData2.ConvertedEntity as ImObject).F_OBJECT_ID));
    return XmlStrategyConvertResultType.Converted;
  }
}
