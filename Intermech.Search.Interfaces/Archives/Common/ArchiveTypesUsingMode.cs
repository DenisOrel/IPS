// Decompiled with JetBrains decompiler
// Type: Intermech.Archives.Common.ArchiveTypesUsingMode
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Localization;
using System.ComponentModel;

#nullable disable
namespace Intermech.Archives.Common;

/// <summary>
/// Режим использования списка типов файлов
/// 0 - Архив может содержать документы любых типов,
/// 1 - Архив может содержать документы только перечисленных типов,
/// 2 - Архив не может содержать документы перечисленных типов,
/// </summary>
[TypeConverter(typeof (EnumDescConverter))]
[CustomDescription("Attribute.Search.Interfaces_25")]
[Category("Misc")]
public enum ArchiveTypesUsingMode
{
  /// <summary>Архив может содержать документы любых типов</summary>
  [CustomDescription("Attribute.Search.Interfaces_22")] AnyType,
  /// <summary>
  /// Архив может содержать документы только перечисленных типов
  /// </summary>
  [CustomDescription("Attribute.Search.Interfaces_23")] PermittedTypes,
  /// <summary>
  /// Архив не может содержать документы перечисленных типов
  /// </summary>
  [CustomDescription("Attribute.Search.Interfaces_24")] ForbiddenTypes,
}
