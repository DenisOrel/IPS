// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.SettingsItems.ISettingsGroup
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.SettingsItems;

/// <summary>Интерфейс для доступа к группе настройки атрибутов</summary>
public interface ISettingsGroup
{
  /// <summary>Заголовок группы настройки атрибутов</summary>
  string Caption { get; }

  bool Visible { get; set; }

  event ObjectCreatedEventHandler ObjectCreated;

  /// <summary>
  /// Список элементов групп настройки атрибутов, относящихся к данной группе
  /// </summary>
  List<ISettingsGroupItem> GroupItems { get; }

  void DoObjectCreated();

  /// <summary>Сортировка внутри группы</summary>
  void Sort();

  /// <summary>Тип группы настроек</summary>
  SettingsGroupType GroupType { get; }
}
