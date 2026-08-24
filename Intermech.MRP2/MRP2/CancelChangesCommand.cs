// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.CancelChangesCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Commands;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Search.UI;
using System;
using System.Data;

#nullable disable
namespace Intermech.MRP2;

/// <summary>Команда меню отменить изменения</summary>
internal class CancelChangesCommand
{
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null || items.Count == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.StartLogHistory();
      try
      {
        for (int index = 0; index < items.Count; ++index)
        {
          IDBTypedObjectID itemData = items.GetItemData(index, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
          IDBObject dbObj = sessionKeeper.Session.GetObject(itemData.ObjectID, false);
          if (dbObj.CheckoutBy == sessionKeeper.Session.UserID)
            CancelChangesCommand.CancelChangeMRPObject(dbObj, (IDBRelationCollection) null);
        }
        NotificationHelper.Notify((object) null, sessionKeeper.Session.GetModificationsHistoryList());
      }
      finally
      {
        sessionKeeper.Session.StopLogHistory();
      }
    }
  }

  /// <summary>
  /// при отмеме изменений - отменяем все что внутри по составу (но созданные копии не удаляются!)
  /// </summary>
  /// <param name="dbObj"></param>
  /// <param name="relCollection"></param>
  private static void CancelChangeMRPObject(IDBObject dbObj, IDBRelationCollection relCollection)
  {
    if (relCollection == null)
      relCollection = dbObj.Session.GetRelationCollection(MRP2Consts.reltypeIdProductComposition);
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(-6, RelationalOperators.Equal, (object) dbObj.Session.UserID, LogicalOperators.AND, 0, false)
    }, new object[2]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID,
      (object) ObligatoryObjectAttributes.F_CHKOUT_BY
    });
    foreach (DataRow row in (InternalDataCollectionBase) relCollection.ConsistFrom(paramSet, dbObj.ObjectID).Rows)
    {
      if (Convert.ToInt64(row[1]) == dbObj.Session.UserID)
      {
        long int64 = Convert.ToInt64(row[0]);
        CancelChangesCommand.CancelChangeMRPObject(dbObj.Session.GetObject(int64), relCollection);
      }
    }
    ObjectCopyCommand cancelChangesCommand = ObjectCommandFactory.CreateCancelChangesCommand(true);
    cancelChangesCommand.ObjectId = dbObj.ObjectID;
    cancelChangesCommand.Execute();
  }
}
