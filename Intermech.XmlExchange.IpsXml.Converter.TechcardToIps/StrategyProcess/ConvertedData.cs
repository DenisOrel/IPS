// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess.ConvertedData
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyParams;
using Intermech.XmlExchange.IpsXml.Interfaces;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess;

public class ConvertedData
{
  public ConvertedData() => this.ConvertedEntityParams = new ParamsCache();

  public IXmlEntity ConvertedEntity { get; set; }

  public ParamsCache ConvertedEntityParams { get; private set; }
}
