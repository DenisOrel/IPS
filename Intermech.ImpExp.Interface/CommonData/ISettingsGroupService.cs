// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.ISettingsGroupService
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData;

/// <summary>Сервис групп настроек</summary>
public interface ISettingsGroupService
{
  /// <summary>Список групп</summary>
  List<ISettingsGroup> Groups { get; }

  /// <summary>Событие о том, что привязка изменилась</summary>
  event ItemBindChangedEventHandler ItemBindChangedEvent;

  /// <summary>Генерация события об изменении привязки</summary>
  /// <param name="group">Группа</param>
  /// <param name="item">Метаданное</param>
  void FireItemBindChanged(ISettingsGroup group, ISettingsItem item);
}
