// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ParamValueConfig.ValueConfig
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ParamValueConfig;

[ConfigNodeType(NodeType.ValueConfig)]
public class ValueConfig : BaseConfig
{
  public ValueConfig()
  {
    this.Destination = ValueDestType.ImAttribute;
    this.DestFieldName = "F_VALUE";
    this.Export = true;
  }

  public ValueDestType Destination { get; set; }

  public string DestFieldName { get; set; }

  public string AttrId { get; set; }

  public string LinkedValueId { get; set; }

  public bool Export { get; set; }

  public string SurrSymbol { get; set; }

  public string GroupId { get; set; }

  public ConditionType GroupCond { get; set; }

  public string Delimiter { get; set; }

  public ValueConverterReference ConverterReference { get; set; }
}
