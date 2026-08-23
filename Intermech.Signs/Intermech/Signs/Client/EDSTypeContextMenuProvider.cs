// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.EDSTypeContextMenuProvider
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Менюшка на тип объекта "Подписи"</summary>
public class EDSTypeContextMenuProvider : ICommandsProvider
{
  /// <summary>Получение информации о командах</summary>
  /// <param name="items">выбранные объекты</param>
  /// <param name="viewServices">сервисы</param>
  /// <returns>Информация о командах</returns>
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  /// <summary>Получение информации о командах</summary>
  /// <param name="items">выбранные объекты</param>
  /// <param name="viewServices">сервисы</param>
  /// <returns>Информация о командах</returns>
  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items.Count != 1)
      return CommandsInfo.Empty;
    CommandsInfo groupCommands = new CommandsInfo();
    groupCommands.Suppress("Create", 0);
    groupCommands.Suppress("Cut", 0);
    groupCommands.Suppress("Copy", 0);
    groupCommands.Suppress("ReplaceObjectInComposition", 0);
    return groupCommands;
  }
}
