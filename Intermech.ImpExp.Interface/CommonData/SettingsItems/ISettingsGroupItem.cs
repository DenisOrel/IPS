// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.SettingsItems.ISettingsGroupItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.SettingsItems;

/// <summary>
/// Интерфейс для доступа к элементу группы настройки атрибутов
/// </summary>
public interface ISettingsGroupItem
{
  /// <summary>Заголовок элемента группы настройки атрибутов</summary>
  string Caption { get; }

  /// <summary>
  /// Список элементов настройки на атрибут, относящихся к данному элементу группы
  /// </summary>
  List<ISettingsItem> SettingsItems { get; }

  /// <summary>Сортировать SettingsItems</summary>
  void Sort();

  /// <summary>Дополнительные данные</summary>
  object Tag { get; set; }
}
