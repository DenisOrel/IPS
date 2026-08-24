// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.TechcardToIpsConfig
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;

public class TechcardToIpsConfig
{
  public LoggerConfig LoggerConfig { get; set; }

  public OutputConfig OutputConfig { get; set; }

  public IdConfigs IdConfigs { get; set; }

  public ObjectConfigs ObjectConfigs { get; set; }

  public ValueConverterConfigs ValueConverterConfigs { get; set; }

  public ConstValueConfigs ConstValueConfigs { get; set; }
}
