// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies.XmlEntityConvertStrategy
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyParams;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies;

public abstract class XmlEntityConvertStrategy
{
  private AddStrategyParams _strategyParams;

  public AddStrategyParams StrategyParams
  {
    get => this._strategyParams;
    set => this._strategyParams = value;
  }

  public abstract XmlStrategyConvertResultType Convert();

  protected IServiceProvider GlobalServices
  {
    get
    {
      object obj;
      return !this.StrategyParams.TryGetValue(AddStrategyParamType.GlobalServices, out obj) || !(obj is IServiceProvider) ? (IServiceProvider) null : obj as IServiceProvider;
    }
  }

  protected IpsXmlLogger Logger
  {
    get
    {
      return this.GlobalServices != null ? this.GlobalServices.GetService<IpsXmlLogger>() : (IpsXmlLogger) null;
    }
  }

  protected IXmlEntity Target
  {
    get
    {
      object obj;
      return !this.StrategyParams.TryGetValue(AddStrategyParamType.ConvertTarget, out obj) || !(obj is IXmlEntity) ? (IXmlEntity) null : obj as IXmlEntity;
    }
  }

  protected BaseConfig TargetConfig
  {
    get
    {
      object obj;
      return !this.StrategyParams.TryGetValue(AddStrategyParamType.ConvertTargetConfig, out obj) || !(obj is BaseConfig) ? (BaseConfig) null : obj as BaseConfig;
    }
  }
}
