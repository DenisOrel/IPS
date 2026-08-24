// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.SelectPLCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using Intermech.Search.UI;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

internal class SelectPLCommand
{
  /// <summary>Команда меню Указать ПВ</summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    INodeID itemId = items.GetItemID(0);
    if (itemId.CategoryID != 1 || !MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdProductionLists) && !MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdProductionCopy))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(items.GetItemData(0, typeof (IDBRelationID)) is IDBRelationID itemData1) || Consts.IsUndefinedRelationId(itemData1.Value))
        return;
      IDBTypedObjectID itemData2 = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
      sessionKeeper.Session.GetObject(itemData1.ProjID).CheckEdit();
      IDBObject dbObject = sessionKeeper.Session.GetObject(itemData2.ObjectID);
      dbObject.CheckEdit();
      IReadOnlyList<IDBObjectID> dbObjectIdList = SelectDialog.Objects((IReadOnlyCollection<int>) new int[1]
      {
        MRP2Consts.objtypeIdProductionLists
      }, "Выберите объект", options: SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect, operationName: nameof (SelectPLCommand), disableGlobalContextMenuCommands: true);
      if (dbObjectIdList == null || dbObjectIdList.Count <= 0)
        return;
      if (!(sessionKeeper.Session.GetCustomService(typeof (IMRP2ServerService)) is IMRP2ServerService customService))
        throw new Exception("IMRP2ServerService not found");
      sessionKeeper.Session.StartLogHistory();
      try
      {
        customService.SetPLForCopy(sessionKeeper.Session.SessionGUID, itemData1.Value, dbObjectIdList[0].Value, dbObject.ObjectID);
        NotificationHelper.Notify((object) null, sessionKeeper.Session.GetModificationsHistoryList());
      }
      finally
      {
        sessionKeeper.Session.StopLogHistory();
      }
    }
  }
}
