// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.ContextMenuCommandsProvider
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.ExternalSystemIntegration.Interfaces;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Data;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

internal class ContextMenuCommandsProvider : ICommandsProvider
{
  public static void AddContextMenuCommands()
  {
    ServiceHolder.Factory.ContextMenuTemplate["Create"]?.Nodes.Add(new MenuTemplateNode(Const.RequestCommandName, ServiceHolder.rm.GetString("ExtInt_14"), ServiceHolder.NamedImageList.ImageIndex(Const.RequestCommandImage), 20, 200));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable dataTable = sessionKeeper.Session.GetObjectCollection(Const.TypeSettingItemObjTypeID).Select(new DBRecordSetParams(new ConditionStructure[0], new object[1]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID
      }, (object[]) null, (SortOrders[]) null, 0L, (object) null, -1, true, "MyObjects"));
      if (dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        if (sessionKeeper.Session.GetObject(Convert.ToInt64(row[0]), false) is IObjTypeSettingItemObject settingItemObject)
          ServiceHolder.Factory.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID(settingItemObject.ObjTypeGUID), (ICommandsProvider) new ContextMenuCommandsProvider());
      }
    }
  }

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null || items.Count == 0)
      return CommandsInfo.Empty;
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add(Const.RequestCommandName, new CommandInfo(0, new ClickEventHandler(CreateRequestHelper.CreateRequestHandler)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }
}
