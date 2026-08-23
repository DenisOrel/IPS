// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.SignVersions
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Localization;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Версии подписи</summary>
[TypeConverter(typeof (EnumDescConverter))]
[CustomDescription("Attribute.Search.Interfaces_13")]
[Category("Misc")]
public enum SignVersions
{
  /// <summary>Непереносимая</summary>
  [CustomDescription("Attribute.Search.Interfaces_14")] Unbearable,
  /// <summary>Переносимая</summary>
  [CustomDescription("Attribute.Search.Interfaces_15")] Portable,
}
