// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.Format.TypesExtensions
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard.Format;

public static class TypesExtensions
{
  public static AttrType ParseAttrType(this string xmlTag)
  {
    return AttrTypeExtension.StringToEnum(xmlTag);
  }

  public static NodeType ParseNodeType(this string xmlTag)
  {
    return NodeTypeExtention.StringToEnum(xmlTag);
  }

  public static ParamType ParseParmType(this string xmlTag)
  {
    return ParamTypeExtention.StringToEnum(xmlTag);
  }

  private static T ParseEnum<T>(string sourceString) where T : struct, IConvertible
  {
    return EnumDescConverter.GetEnumValue(typeof (T), sourceString) is T enumValue ? enumValue : default (T);
  }

  public static void FillEnumValues<T>(
    Dictionary<T, string> enumToString,
    Dictionary<string, T> stringToEnum)
    where T : struct, IConvertible
  {
    enumToString.Clear();
    stringToEnum.Clear();
    foreach (object enumValue in typeof (T).GetEnumValues())
    {
      T key = (T) enumValue;
      string enumDescription = EnumDescConverter.GetEnumDescription(enumValue.GetType(), enumValue.ToString());
      if (!enumToString.ContainsKey(key) && !string.IsNullOrEmpty(enumDescription))
        enumToString.Add(key, enumDescription);
    }
    foreach (T key in enumToString.Keys)
      stringToEnum.Add(enumToString[key], key);
  }
}
