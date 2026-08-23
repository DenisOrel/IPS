// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.ArchiveControl
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Localization;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>
/// enum для хранения информации о контроле подписей
/// нигде уже не используется
/// </summary>
[CustomDescription("Attribute.Search.Interfaces_4")]
[TypeConverter(typeof (EnumDescConverter))]
public enum ArchiveControl
{
  /// <summary>Не контролировать</summary>
  [CustomDescription("Attribute.Search.Interfaces_5")] NoCheck,
  /// <summary>Контролировать по архиву</summary>
  [CustomDescription("Attribute.Search.Interfaces_6")] CheckForArchive,
  /// <summary>Контролировать по типу объекта</summary>
  [CustomDescription("Attribute.Search.Interfaces_7")] CheckForObjectType,
  /// <summary>Контролировать все подписи</summary>
  [CustomDescription("Attribute.Search.Interfaces_8")] CheckAll,
}
