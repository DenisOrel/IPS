// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.BaseConvertableEntityWithParamsConfig
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;

public class BaseConvertableEntityWithParamsConfig : BaseConvertableEntityConfig
{
  public ConvertationRulesConfig ConvertationRules { get; set; }

  public UniqueControlRuleConfig UniqueControlRule { get; set; }

  public ParamConfigs ParamConfigs { get; set; }
}
