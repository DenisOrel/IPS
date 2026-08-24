// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.ManufactOrdersCommandsProvider
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.DataFormats;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>Провайдер команды "Создать | Производственный заказ"</summary>
internal sealed class ManufactOrdersCommandsProvider : ICommandsProvider
{
  /// <summary>
  /// Метод вызывается для получения допустимых и подавляемых команд контекстного меню для
  /// выделенных элементов навигации одной категории и типа.
  /// Например, если в «Навигаторе» выделены элементы навигации нескольких разных категорий и типов,
  /// то данная команда будет вызываться для каждой из подгрупп этих элементов, сгруппированных
  /// по их категориям и типам. Наиболее применяемый метод даного интерфейса.
  /// Позволяет перекрывать команды контекстного меню для элементов навигации определённых категорий,
  /// типов, задавая более высокий приоритет описаниям этих команд.
  /// ВНИМАНИЕ! Основное требование к данному методу – нельзя выполнять обращения к базе даных  для того,
  /// чтобы проверить, можно ли отображать команду меню или нет!
  /// </summary>
  /// <param name="items">Коллекция выбранных пользователем элементов навигации.</param>
  /// <param name="viewServices">Контейнер сервисов, которыми могут пользоваться команды.</param>
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  /// <summary>
  /// Метод вызывается для получения допустимых и подавляемых команд контекстного меню для всей группы выделенных
  /// элементов навигации. Особенности данного метода:
  /// 1. Если команда зарегистрирована на все категории, то метод вызывается один раз и получает в качестве параметра
  /// items все выделенные в «Навигаторе» элементы навигации;
  /// 2. Если команда зарегистрирована на конкретную категорию, то метод будет вызван один раз для всех выделенных
  /// элементов навигации только в том случае, если все они принадлежат одной категории; для всех выделенных
  /// элементов навигации только в том случае, если все они принадлежат указанной категории;
  /// 3. Если команда зарегистрирована на конкретные категорию и тип, то метод будет вызван один раз для всех
  /// выделенных элементов навигации только в том случае, если все они принадлежат указанной категории и типу.
  /// </summary>
  /// <param name="items">Коллекция выбранных пользователем элементов навигации.</param>
  /// <param name="viewServices">Контейнер сервисов, которыми могут пользоваться команды.</param>
  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (((viewServices.GetService(typeof (IViewState)) is IViewState service ? (long) service.ViewState : 0L) & 2L) == 2L)
      return CommandsInfo.Empty;
    ManufactOrdersAPI.objectIDs = new List<IDBTypedObjectID>(items.Count);
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData)
        ManufactOrdersAPI.objectIDs.Add(itemData);
    }
    if (ManufactOrdersAPI.objectIDs.Count <= 0)
      return CommandsInfo.Empty;
    CommandsInfo groupCommands = new CommandsInfo();
    groupCommands.Add("MRP.CreateManufactOrder", new CommandInfo(0, new ClickEventHandler(ManufactOrdersAPI.CreateManufactOrder)));
    return groupCommands;
  }
}
