// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format.TypeExtensions
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;

public static class TypeExtensions
{
  public static string ToXMLTag(this AttrType target) => EnumTypeHelper.GetCaption((Enum) target);

  public static string ToXMLTag(this ConditionType target)
  {
    return EnumTypeHelper.GetCaption((Enum) target);
  }

  public static string ToXMLTag(this IdConfigType target)
  {
    return EnumTypeHelper.GetCaption((Enum) target);
  }

  public static string ToXMLTag(this ParamSubType target)
  {
    return EnumTypeHelper.GetCaption((Enum) target);
  }

  public static string ToXMLTag(this NodeType target) => EnumTypeHelper.GetCaption((Enum) target);

  public static string ToXMLTag(this UniqueControlRule target)
  {
    return EnumTypeHelper.GetCaption((Enum) target);
  }

  public static string ToXMLTag(this IdConfigCalcResultType target)
  {
    return EnumTypeHelper.GetCaption((Enum) target);
  }

  public static string ToXMLTag(this ParamType target) => EnumTypeHelper.GetCaption((Enum) target);

  public static string ToXMLTag(this ParamConfigType target)
  {
    return EnumTypeHelper.GetCaption((Enum) target);
  }

  public static string ToXMLTag(this ValueDestType target)
  {
    return EnumTypeHelper.GetCaption((Enum) target);
  }

  public static IdConfigType ParseIdConfigType(this string xmlTag)
  {
    return TypeExtensions.ParseEnum<IdConfigType>(xmlTag);
  }

  public static ConditionType ParseConditionType(this string xmlTag)
  {
    return TypeExtensions.ParseEnum<ConditionType>(xmlTag);
  }

  public static ParamSubType ParseParamSubType(this string xmlTag)
  {
    return TypeExtensions.ParseEnum<ParamSubType>(xmlTag);
  }

  public static NodeType ParseNodeType(this string xmlTag)
  {
    return TypeExtensions.ParseEnum<NodeType>(xmlTag);
  }

  public static UniqueControlRule ParseUniqueControlRule(this string xmlTag)
  {
    return TypeExtensions.ParseEnum<UniqueControlRule>(xmlTag);
  }

  public static IdConfigCalcResultType ParseIdConfigCalcResultType(this string xmlTag)
  {
    return TypeExtensions.ParseEnum<IdConfigCalcResultType>(xmlTag);
  }

  public static ParamType ParseParamType(this string xmlTag)
  {
    return TypeExtensions.ParseEnum<ParamType>(xmlTag);
  }

  public static ParamConfigType ParseParamConfigType(this string xmlTag)
  {
    return TypeExtensions.ParseEnum<ParamConfigType>(xmlTag);
  }

  public static ValueDestType ParseValueDestType(this string xmlTag)
  {
    return TypeExtensions.ParseEnum<ValueDestType>(xmlTag);
  }

  private static T ParseEnum<T>(string sourceString) where T : struct, IConvertible
  {
    return EnumDescConverter.GetEnumValue(typeof (T), sourceString) is T enumValue ? enumValue : default (T);
  }
}
