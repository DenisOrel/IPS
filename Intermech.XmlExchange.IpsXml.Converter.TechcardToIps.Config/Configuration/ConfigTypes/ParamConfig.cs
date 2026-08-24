// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ParamConfig
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ParamValueConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;

[ConfigNodeType(NodeType.ParamConfig)]
public class ParamConfig : BaseConvertableEntityConfig
{
  private const string ARRAY_PARAM = "_#A_";

  public ParamConfig() => this.Export = true;

  public ParamConfigType ConfigType { get; set; }

  public ParamType ParamParentType { get; set; }

  public string ParentParamId { get; set; }

  public ParamSubType ParamSubType { get; set; }

  public bool Export { get; set; }

  public ValueConfigs ValueConfigs { get; set; }

  public static bool IsArrayParam(
    string sourceParamName,
    out string arrayParamName,
    out int paramIndex)
  {
    string[] separator = new string[1]{ "_#A_" };
    string[] strArray = sourceParamName.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    arrayParamName = string.Empty;
    paramIndex = -1;
    if (strArray.Length != 2)
      return false;
    arrayParamName = strArray[0];
    return int.TryParse(strArray[1], out paramIndex);
  }

  public static bool IsArrayParam(string sourceParamName)
  {
    return ParamConfig.IsArrayParam(sourceParamName, out string _, out int _);
  }
}
