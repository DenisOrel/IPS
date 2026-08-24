// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.Format.ParamTypeExtention
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard.Format;

public static class ParamTypeExtention
{
  private static Dictionary<ParamType, string> _EnumToString = new Dictionary<ParamType, string>();
  private static Dictionary<string, ParamType> _StringToEnum = new Dictionary<string, ParamType>();

  static ParamTypeExtention()
  {
    TypesExtensions.FillEnumValues<ParamType>(ParamTypeExtention._EnumToString, ParamTypeExtention._StringToEnum);
  }

  public static string ToTechXMLTag(this ParamType source)
  {
    string str;
    return ParamTypeExtention._EnumToString.TryGetValue(source, out str) ? str : string.Empty;
  }

  public static ParamType StringToEnum(string source)
  {
    ParamType paramType;
    return ParamTypeExtention._StringToEnum.TryGetValue(source, out paramType) ? paramType : ParamType.Unknown;
  }
}
