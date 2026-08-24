// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess.ValueConverter
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ValueConverterConfig;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess;

public class ValueConverter
{
  private readonly IServiceProvider _services;
  private readonly Dictionary<string, Dictionary<string, string>> _convertations = new Dictionary<string, Dictionary<string, string>>();
  private readonly Dictionary<string, Dictionary<string, string>> _defaults = new Dictionary<string, Dictionary<string, string>>();

  public ValueConverter(IServiceProvider services)
  {
    this._services = services;
    this.Initialize();
  }

  public string Convert(string originValue, string converterId, string context = "")
  {
    if (string.IsNullOrEmpty(converterId))
      return originValue;
    string key = originValue;
    if (!context.Equals(string.Empty))
      key = $"{key}_{context}";
    Dictionary<string, string> dictionary1;
    string str;
    if (this._convertations.TryGetValue(converterId, out dictionary1) && dictionary1.TryGetValue(key, out str))
      return str;
    Dictionary<string, string> dictionary2;
    if (this._defaults.TryGetValue(converterId, out dictionary2) && dictionary2.Count > 0)
    {
      if (context.Equals(string.Empty))
        return dictionary2.Keys.First<string>();
      if (dictionary2.TryGetValue(context, out str))
        return str;
    }
    return originValue;
  }

  private void Initialize()
  {
    TechcardToIpsConfig service = this._services.GetService<TechcardToIpsConfig>();
    this._convertations.Clear();
    this._defaults.Clear();
    foreach (string id1 in service.ValueConverterConfigs.Ids)
    {
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      this._convertations.Add(id1, dictionary1);
      Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
      this._defaults.Add(id1, dictionary2);
      foreach (OriginValueConfig originValueConfig in service.ValueConverterConfigs[id1].OriginValueConfigs)
      {
        if (!originValueConfig.IsDefault)
        {
          foreach (string id2 in originValueConfig.Ids)
          {
            ConvertedValueConfig convertedValueConfig = originValueConfig[id2];
            if (!string.IsNullOrEmpty(convertedValueConfig.Context))
              dictionary1.Add($"{originValueConfig.Value}_{convertedValueConfig.Context}", convertedValueConfig.Value);
            else
              dictionary1.Add(originValueConfig.Value, convertedValueConfig.Value);
          }
        }
        else
        {
          foreach (string id3 in originValueConfig.Ids)
          {
            ConvertedValueConfig convertedValueConfig = originValueConfig[id3];
            if (!string.IsNullOrEmpty(convertedValueConfig.Context))
              dictionary2.Add(convertedValueConfig.Context, convertedValueConfig.Value);
            else
              dictionary2.Add(convertedValueConfig.Value, string.Empty);
          }
        }
      }
    }
  }
}
