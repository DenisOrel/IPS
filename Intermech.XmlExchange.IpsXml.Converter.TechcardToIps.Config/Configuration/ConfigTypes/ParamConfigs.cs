// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ParamConfigs
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;

[ConfigNodeType(NodeType.ParamConfigs)]
public class ParamConfigs
{
  private BaseConfigContainer<ParamConfig> _constConfigs = (BaseConfigContainer<ParamConfig>) new ParamConfigs.InternalParamsConfigContainer();
  private BaseConfigContainer<ParamConfig> _simpleConfigs = (BaseConfigContainer<ParamConfig>) new ParamConfigs.InternalParamsConfigContainer();
  private BaseConfigContainer<ParamConfig> _calcConfigs = (BaseConfigContainer<ParamConfig>) new ParamConfigs.InternalParamsConfigContainer();

  [ParamConfigsContainer(new ParamConfigType[] {ParamConfigType.Const})]
  public BaseConfigContainer<ParamConfig> ConstConfigs => this._constConfigs;

  [ParamConfigsContainer(new ParamConfigType[] {ParamConfigType.Simple, ParamConfigType.File})]
  public BaseConfigContainer<ParamConfig> SimpleConfigs => this._simpleConfigs;

  [ParamConfigsContainer(new ParamConfigType[] {ParamConfigType.Calculated})]
  public BaseConfigContainer<ParamConfig> CalcConfigs => this._calcConfigs;

  private class InternalParamsConfigContainer : BaseConfigContainer<ParamConfig>
  {
  }
}
