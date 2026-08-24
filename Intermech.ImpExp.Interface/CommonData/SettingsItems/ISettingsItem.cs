// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.SettingsItems.ISettingsItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using System;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.SettingsItems;

/// <summary>Интерфейс для доступа к элементу настройки на атрибут</summary>
public interface ISettingsItem
{
  /// <summary>Длинное имя</summary>
  string LongName { get; set; }

  /// <summary>
  /// Глобальный идентификатор атрибута, в который предполагается производить перекачку данных
  /// </summary>
  Guid AttrGuid { get; set; }

  /// <summary>
  /// Системный идентификатор атрибута, в который предполагается производить закачку
  /// </summary>
  int AttrSystemId { get; }

  /// <summary>Проблемы по перекачке</summary>
  ItemError Error { get; set; }
}
