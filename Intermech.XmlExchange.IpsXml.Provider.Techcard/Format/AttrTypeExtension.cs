// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.Format.AttrTypeExtension
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard.Format;

public static class AttrTypeExtension
{
  private static Dictionary<AttrType, string> _EnumToString = new Dictionary<AttrType, string>();
  private static Dictionary<string, AttrType> _StringToEnum = new Dictionary<string, AttrType>();

  static AttrTypeExtension()
  {
    TypesExtensions.FillEnumValues<AttrType>(AttrTypeExtension._EnumToString, AttrTypeExtension._StringToEnum);
  }

  public static string ToTechXMLTag(this AttrType source)
  {
    string str;
    return AttrTypeExtension._EnumToString.TryGetValue(source, out str) ? str : string.Empty;
  }

  public static AttrType StringToEnum(string source)
  {
    AttrType attrType;
    return AttrTypeExtension._StringToEnum.TryGetValue(source, out attrType) ? attrType : AttrType.Unknown;
  }
}
