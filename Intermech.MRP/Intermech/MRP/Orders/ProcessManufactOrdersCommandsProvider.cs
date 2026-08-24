// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.ProcessManufactOrdersCommandsProvider
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Compositions;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Провайдер команд "Планирование производства (MRP) | *"
/// </summary>
internal sealed class ProcessManufactOrdersCommandsProvider : ICommandsProvider
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
    if (items == null || items.Count != 1 || ((viewServices.GetService(typeof (IViewState)) is IViewState service ? (long) service.ViewState : 0L) & 2L) == 2L)
      return CommandsInfo.Empty;
    CommandsInfo groupCommands = new CommandsInfo();
    if (items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData)
    {
      int num = MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, MetaDataHelper.GetObjectTypeID("cadd92e9-306c-11d8-b4e9-00304f19f545")) ? 1 : 0;
      bool flag1 = MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, MetaDataHelper.GetObjectTypeID("cad00583-306c-11d8-b4e9-00304f19f545"));
      MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, MetaDataHelper.GetObjectTypeID("cad0016f-306c-11d8-b4e9-00304f19f545"));
      bool flag2 = MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, MetaDataHelper.GetObjectTypeID("cad00163-306c-11d8-b4e9-00304f19f545"));
      bool flag3 = MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, MetaDataHelper.GetObjectTypeID("cad00268-306c-11d8-b4e9-00304f19f545"));
      bool flag4 = MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, MetaDataHelper.GetObjectTypeID("cad015b1-306c-11d8-b4e9-00304f19f545"));
      RelationPath orderItemPath = ManufactOrdersAPI.FindOrderItemPath(items, viewServices, (object) null);
      if (num != 0)
        groupCommands.Add("MRP.ProcessOrder", new CommandInfo(0, new ClickEventHandler(ManufactOrdersAPI.ConvertManufactOrder)));
      if (flag1 && orderItemPath != null && orderItemPath.Items.Count > 0)
      {
        groupCommands.Add("MRP.ChangeInstanceVersion", new CommandInfo(0, new ClickEventHandler(ManufactOrdersAPI.MRPChangeInstanceVersion)));
        groupCommands.Add("MRP.ChangeInstance", new CommandInfo(0, new ClickEventHandler(ManufactOrdersAPI.MRPChangeInstance)));
        groupCommands.Add("MRP.ChangeTechRoute", new CommandInfo(0, new ClickEventHandler(ManufactOrdersAPI.MRPChangeTechRoute)));
      }
      if (flag2 | flag3 | flag4 && orderItemPath != null)
      {
        int count = orderItemPath.Items.Count;
      }
    }
    return groupCommands;
  }
}
