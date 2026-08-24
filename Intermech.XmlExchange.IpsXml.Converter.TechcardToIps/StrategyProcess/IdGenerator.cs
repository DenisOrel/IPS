// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess.IdGenerator
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess;

public sealed class IdGenerator
{
  private int _counter;

  public IdGenerator(IServiceProvider services)
  {
  }

  public int GetNewID() => this._counter++;
}
