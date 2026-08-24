// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TypeExtensions
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

public static class TypeExtensions
{
  /// <summary>Преобразование в наименование алиаса БД</summary>
  /// <param name="target">Тип атрибута.</param>
  /// <returns>Наименование атрибута.</returns>
  public static string ToDbSettingsName(this ConnStrType target)
  {
    return EnumTypeHelper.GetCaption((Enum) target);
  }

  public static string ToDbAlias(this ConnStrType target)
  {
    if (target == ConnStrType.Search)
      return "SEARCH PLUGIN CONNECTION";
    return target == ConnStrType.Imbase ? "DATABASE CONNECTION" : EnumTypeHelper.GetCaption((Enum) target);
  }
}
