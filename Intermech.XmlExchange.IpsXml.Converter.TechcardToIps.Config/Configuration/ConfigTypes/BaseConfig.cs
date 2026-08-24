// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.BaseConfig
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;

public abstract class BaseConfig
{
  public string Id { get; set; }

  public string Name { get; set; }

  public string Description { get; set; }

  public string Value { get; set; }

  public int Order { get; set; }
}
