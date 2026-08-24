// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.CheckInCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Commands;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using System;
using System.Data;

#nullable disable
namespace Intermech.MRP2;

internal class CheckInCommand
{
  /// <summary>
  /// при завершении редактирования, завершаем редактирование всего что внутри по составу
  /// </summary>
  /// <param name="keeper"></param>
  /// <param name="relCollection"></param>
  /// <param name="objectID"></param>
  public static void CheckInMRPObject(
    SessionKeeper keeper,
    IDBRelationCollection relCollection,
    long objectID)
  {
    if (relCollection == null)
      relCollection = keeper.Session.GetRelationCollection(MRP2Consts.reltypeIdProductComposition);
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(-6, RelationalOperators.Equal, (object) keeper.Session.UserID, LogicalOperators.AND, 0, false)
    }, new object[2]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID,
      (object) ObligatoryObjectAttributes.F_CHKOUT_BY
    });
    foreach (DataRow row in (InternalDataCollectionBase) relCollection.ConsistFrom(paramSet, objectID).Rows)
    {
      if (Convert.ToInt64(row[1]) == keeper.Session.UserID)
      {
        long int64 = Convert.ToInt64(row[0]);
        CheckInCommand.CheckInMRPObject(keeper, relCollection, int64);
      }
    }
    ObjectCopyCommand checkinCommand = ObjectCommandFactory.CreateCheckinCommand(true);
    checkinCommand.ObjectId = objectID;
    checkinCommand.Execute();
  }

  public static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null || items.Count == 0)
      return;
    for (int index = 0; index < items.Count; ++index)
    {
      IDBTypedObjectID itemData = items.GetItemData(index, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      using (SessionKeeper keeper = new SessionKeeper())
      {
        IDBObject dbObject = keeper.Session.GetObject(itemData.ObjectID, false);
        if (dbObject.CheckoutBy == keeper.Session.UserID)
          CheckInCommand.CheckInMRPObject(keeper, (IDBRelationCollection) null, dbObject.ObjectID);
      }
    }
  }
}
