// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ListInstancesWindow.ListInstancesCommandProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Pdm.GroupInstances;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.ListInstancesWindow;

internal class ListInstancesCommandProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo groupCommands = new CommandsInfo();
    if (items.GetItemData(0, typeof (IListInstancesInfo)) is IListInstancesInfo)
      groupCommands.Add("PDM.AddInstance", new CommandInfo(0, new ClickEventHandler(ListInstancesCommandProvider.AddInstance)));
    return groupCommands;
  }

  private static void AddInstance(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    IListInstancesInfo itemData = items.GetItemData(0, typeof (IListInstancesInfo)) as IListInstancesInfo;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(itemData.InitInstanceGUID);
      long[] numArray = SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("Attribute.Pdm_27"), LocalizationHolder.rm.GetString("Attribute.Pdm_28"), objectInfo.ObjectTypeID, SelectionOptions.Default);
      if (numArray == null || numArray.Length == 0)
        return;
      IDBTransactions customService = (IDBTransactions) sessionKeeper.Session.GetCustomService(typeof (IDBTransactions));
      customService.StartTransaction();
      try
      {
        List<long> objectIDs = new List<long>(numArray.Length + 1)
        {
          objectInfo.ObjectID
        };
        int attributeTypeId = MetaDataHelper.GetAttributeTypeID("cad001f9-306c-11d8-b4e9-00304f19f545");
        if (itemData.NumGroupInstance == Guid.Empty)
        {
          itemData.NumGroupInstance = Guid.NewGuid();
          IDBObject dbObject = sessionKeeper.Session.GetObject(objectInfo.ObjectID);
          try
          {
            Helper.AddNumInstance(sessionKeeper.Session, dbObject, attributeTypeId, itemData.NumGroupInstance);
          }
          catch (Exception ex)
          {
            throw new Exception($"Нельзя {dbObject.NameInMessages} сделать исполнением: {ex.Message}");
          }
        }
        for (int index = 0; index < numArray.Length; ++index)
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(numArray[index]);
          objectIDs.Add(dbObject.ObjectID);
          try
          {
            Helper.AddNumInstance(sessionKeeper.Session, dbObject, attributeTypeId, itemData.NumGroupInstance);
          }
          catch (Exception ex)
          {
            throw new Exception($"Нельзя добавить {dbObject.NameInMessages} в исполнения: {ex.Message}");
          }
        }
        customService.Commit();
        ((INotificationService) ServicesManager.GetService(typeof (INotificationService))).FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", (IList<long>) objectIDs));
      }
      catch
      {
        customService.Rollback();
        throw;
      }
    }
  }
}
