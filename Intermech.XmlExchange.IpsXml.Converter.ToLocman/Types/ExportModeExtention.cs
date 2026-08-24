// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.ToLocman.Types.ExportModeExtention
// Assembly: Intermech.XmlExchange.IpsXml.Converter.ToLocman, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 76EBC069-92E6-4D74-866F-DCC1A2BB2547
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.ToLocman.dll

using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.ToLocman.Types;

public static class ExportModeExtention
{
  public static string ToConfigTag(this ExportMode target)
  {
    return EnumTypeHelper.GetCaption((Enum) target);
  }

  private static T ParseEnum<T>(string sourceString) where T : struct, IConvertible
  {
    return (T) EnumDescConverter.GetEnumValue(typeof (T), sourceString);
  }

  public static ExportMode ParseNodeType(this string xmlTag)
  {
    return ExportModeExtention.ParseEnum<ExportMode>(xmlTag);
  }
}
