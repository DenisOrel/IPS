// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Utils.ConfigTypesUtils
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Utils;

public class ConfigTypesUtils
{
  public static bool IsComplexConfig(BaseConfig candidate)
  {
    return ConfigTypesUtils.IsSubclassOf(typeof (BaseConfigContainer<>), candidate.GetType());
  }

  public static IReadOnlyList<BaseConfig> GetChildsFromComplexConfig(BaseConfig complexConfig)
  {
    List<BaseConfig> baseConfigList = new List<BaseConfig>();
    PropertyInfo property1 = complexConfig.GetType().GetProperty("Ids");
    IEnumerable<string> strings = property1 != (PropertyInfo) null ? property1.GetValue((object) complexConfig) as IEnumerable<string> : (IEnumerable<string>) null;
    if (strings == null)
      return (IReadOnlyList<BaseConfig>) null;
    PropertyInfo property2 = complexConfig.GetType().GetProperty("Item");
    foreach (string str in strings)
    {
      if (property2.GetValue((object) complexConfig, new object[1]
      {
        (object) str
      }) is BaseConfig baseConfig)
        baseConfigList.Add(baseConfig);
    }
    return baseConfigList.Count > 0 ? (IReadOnlyList<BaseConfig>) baseConfigList : (IReadOnlyList<BaseConfig>) null;
  }

  private static bool IsSubclassOf(Type originType, Type candidateType)
  {
    for (; candidateType != (Type) null && candidateType != typeof (object); candidateType = candidateType.BaseType)
    {
      Type type = candidateType.IsGenericType ? candidateType.GetGenericTypeDefinition() : candidateType;
      if (originType == type)
        return true;
    }
    return false;
  }
}
