// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.NodeType
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;

public enum NodeType
{
  [Description("unknown")] Unknown,
  [Description("config")] Config,
  [Description("logger")] Logger,
  [Description("output")] OutPut,
  [Description("id_configs")] IdConfigs,
  [Description("id_config")] IdConfig,
  [Description("id_part")] IdPart,
  [Description("id_part_group")] IdPartGroup,
  [Description("objects")] ObjectConfigs,
  [Description("object")] ObjectConfig,
  [Description("convert_strategies")] ConvertStrategyConfigs,
  [Description("convert_strategy")] ConvertStrategyConfig,
  [Description("convertation_rules")] ConvertationRulesConfig,
  [Description("unique_rule")] UniqueRuleConfig,
  [Description("relation")] RelationConfig,
  [Description("relations")] RelationConfigs,
  [Description("param")] ParamConfig,
  [Description("params")] ParamConfigs,
  [Description("value_converter")] ValueConverterConfig,
  [Description("value_converters")] ValueConverterConfigs,
  [Description("origin")] Origin,
  [Description("convertation")] Convertation,
  [Description("value")] ValueConfig,
  [Description("values")] ValueConfigs,
  [Description("value_converter_ref")] ValueConverterReference,
  [Description("const_value")] ConstValueConfig,
  [Description("const_values")] ConstValueConfigs,
}
