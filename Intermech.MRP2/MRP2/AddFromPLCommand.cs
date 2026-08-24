// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.AddFromPLCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

internal class AddFromPLCommand
{
  /// <summary>Команда меню добавить в состав из состава другой ПВ</summary>
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
    IDBTypedObjectID itemData = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
    long version = MRP2Service.GetPLNodeID(items).Version;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.GetObject(itemData.ObjectID).CheckEdit();
      IReadOnlyList<IDBObjectID> dbObjectIdList = SelectDialog.Objects((IReadOnlyCollection<int>) new int[1]
      {
        MRP2Consts.objtypeIdProductionLists
      }, "Выберите объект", options: SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect, operationName: "Add2ProductionList", disableGlobalContextMenuCommands: true);
      if (dbObjectIdList == null || dbObjectIdList.Count <= 0)
        return;
      using (IEnumerator<IDBObjectID> enumerator = dbObjectIdList.GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return;
        IDBObjectID current = enumerator.Current;
        IDBObject dbObject = sessionKeeper.Session.GetObject(current.Value);
        object[] objArray = SelectionWindow.Select("Выберите объект", (IDescriptor) new AdvRelationsDescriptor(Intermech.Navigator.Consts.CategoryAdvRelationsNode, 0, string.Empty, (List<long>) null, dbObject.ObjectID, dbObject.ObjectType, MRP2Consts.reltypeIdProductComposition, string.Empty, 0L, dbObject.OwnerID, 0L, 0, (List<int>) null, (long) dbObject.VersionID, dbObject.IsBaseVersion ? 1L : 0L), typeof (IDBTypedObjectID), SelectionOptions.HideViews | SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect | SelectionOptions.ForceFilterObjectsByRule);
        if (objArray == null)
          return;
        IDBTypedObjectID dbTypedObjectId = objArray[0] as IDBTypedObjectID;
        IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(MRP2Consts.reltypeIdProductComposition);
        AttributeValues[] vals = new AttributeValues[2]
        {
          new AttributeValues(MetaDataHelper.GetAttributeTypeID("cad00267-306c-11d8-b4e9-00304f19f545"), (object) new MeasuredValue(1.0, PDMPluginIDs.measureShtuk)),
          new AttributeValues(MRP2Consts.attrIdVersionNumberPL, (object) version)
        };
        long partObjectID = dbTypedObjectId.ObjectID;
        if (MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, MRP2Consts.objtypeIdProductionLists) && MetaDataHelper.IsObjectTypeChildOf(dbTypedObjectId.ObjectType, MRP2Consts.objtypeIdAssemblyCopy))
          partObjectID = MRP2Service.CreateProductionCopyWithReplacedPart(sessionKeeper.Session, dbTypedObjectId.ObjectID, MRP2Consts.objtypeIdExitAssembly, Guid.Empty, 0L, false, out Guid _, out Dictionary<Guid, Guid> _);
        IDBRelation dbRelation = relationCollection.Create(itemData.ObjectID, partObjectID, vals);
        ((INotificationService) ServicesManager.GetService(typeof (INotificationService)))?.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID, dbRelation.ProjID, dbRelation.RelationType));
      }
    }
  }
}
