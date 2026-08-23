// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.EDSContextMenuProvider
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Цифровые подписи в контекстном меню</summary>
public class EDSContextMenuProvider : ICommandsProvider
{
  /// <summary>Получение команд</summary>
  /// <param name="items">выбранные объекты</param>
  /// <param name="viewServices">сервисы</param>
  /// <returns>Информация о командах</returns>
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Suppress("EditDocument", 0);
    mergedCommands.Suppress("ViewDocument", 0);
    mergedCommands.Suppress("PrintDocument", 0);
    mergedCommands.Suppress("CheckOut", 0);
    mergedCommands.Suppress("CheckIn", 0);
    mergedCommands.Suppress("SaveChanges", 0);
    mergedCommands.Suppress("CancelChanges", 0);
    mergedCommands.Suppress("Permissions", 0);
    mergedCommands.Suppress("Exclude", 0);
    mergedCommands.Suppress("Cut", 0);
    mergedCommands.Suppress("Copy", 0);
    mergedCommands.Suppress("ReplaceObjectInComposition", 0);
    mergedCommands.Suppress("Delete", 0);
    return mergedCommands;
  }

  /// <summary>Получение информации о командах</summary>
  /// <param name="items">Выбранные объекты</param>
  /// <param name="viewServices">севисы</param>
  /// <returns>Информация о командах</returns>
  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo groupCommands = new CommandsInfo();
    groupCommands.Suppress("Create", 0);
    groupCommands.Suppress("CreateNew", 0);
    groupCommands.Suppress("CreateInclude", 0);
    groupCommands.Suppress("CreateProto", 0);
    groupCommands.Suppress("CreateVersion", 0);
    groupCommands.Suppress("Add", 0);
    return groupCommands;
  }
}
