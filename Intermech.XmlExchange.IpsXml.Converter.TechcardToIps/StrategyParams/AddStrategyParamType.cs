// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyParams.AddStrategyParamType
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyParams;

public enum AddStrategyParamType
{
  [Description("unknown")] Unknown,
  [Description("global_services")] GlobalServices,
  [Description("convert_target")] ConvertTarget,
  [Description("convert_target_type")] ConvertTargetType,
  [Description("convert_target_config")] ConvertTargetConfig,
  [Description("convert_target_relation")] ConvertTargetRelation,
  [Description("param_owner")] ParamOwner,
  [Description("converted_param_owner")] ConvertedParamOwner,
  [Description("converted_owners_params_cache")] ConvertedOwnerParamsCache,
}
