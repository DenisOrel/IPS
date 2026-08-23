// Decompiled with JetBrains decompiler
// Type: Intermech.Archives.Common.ArchiveStructureChangeAction
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Localization;
using System.ComponentModel;

#nullable disable
namespace Intermech.Archives.Common;

/// <summary>Действие проводимое с архивом</summary>
[TypeConverter(typeof (EnumDescConverter))]
[CustomDescription("Attribute.Search.Interfaces_9")]
public enum ArchiveStructureChangeAction
{
  /// <summary>Ничего не делалось</summary>
  [CustomDescription("Attribute.Search.Interfaces_1")] NothingToDo,
  /// <summary>Добавить к архиву</summary>
  [CustomDescription("Attribute.Search.Interfaces_2")] AddToArchive,
  /// <summary>Удалить из архива</summary>
  [CustomDescription("Attribute.Search.Interfaces_3")] DeleteFromArchive,
  /// <summary>Добавить вновь созданный</summary>
  [CustomDescription("AddNewToArchive")] AddNewToArchive,
  /// <summary>Удалить из архива и у документов</summary>
  [CustomDescription("DeleteFromArchiveAndDocs")] DeleteFromArchiveAndDocs,
}
