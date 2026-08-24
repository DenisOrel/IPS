// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ReplacePartCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

internal class ReplacePartCommand
{
  /// <summary>Команда меню заменить в составе</summary>
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
    long version = MRP2Service.GetPLNodeID(items).Version;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(items.GetItemData(0, typeof (IDBRelationID)) is IDBRelationID itemData1) || Consts.IsUndefinedRelationId(itemData1.Value))
        return;
      IDBRelation relation = sessionKeeper.Session.GetRelation(itemData1.Value);
      IDBTypedObjectID itemData2 = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
      IDBObject dbObject = sessionKeeper.Session.GetObject(itemData1.ProjID);
      dbObject.CheckEdit();
      IDBAttribute attributeById1 = relation.GetAttributeByID(MRP2Consts.attrIdReplacedBy);
      if (attributeById1 != null && attributeById1.AsInteger != 0L)
        throw new Exception("Нельзя заменить уже замененную позицию");
      IDBAttribute attributeById2 = relation.GetAttributeByID(MRP2Consts.attrIdDeleteTag);
      if (attributeById2 != null && attributeById2.AsInteger != 0L)
        throw new Exception("Нельзя заменить исключенную позицию");
      MeasuredValue cnt = (MeasuredValue) null;
      IDBAttribute attributeById3 = relation.GetAttributeByID(MRP2Consts.attrIdCount);
      if (attributeById3 != null && !attributeById3.IsNull && attributeById3 is IDBMeasureAttribute measureAttribute)
        cnt = measureAttribute.Value;
      List<Tuple<IDBObject, MeasuredValue>> tupleList = AddSostavCommand.SelectProductionCopy(sessionKeeper.Session, dbObject.ObjectType, version, SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect, cnt);
      if (tupleList == null || tupleList.Count == 0)
        return;
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(relation.RelationType);
      List<AttributeValues> attributeValuesList1 = new List<AttributeValues>()
      {
        new AttributeValues(MRP2Consts.attrIdVersionNumberPL, (object) version)
      };
      if (tupleList[0].Item2 != null)
        attributeValuesList1.Add(new AttributeValues(MRP2Consts.attrIdCount, (object) tupleList[0].Item2));
      NewRelationProperties properties = new NewRelationProperties()
      {
        ProjectObjectID = dbObject.ObjectID,
        PartObjectID = tupleList[0].Item1.ObjectID,
        PrototypeRelationID = relation.RelationID,
        ValuesList = attributeValuesList1.ToArray()
      };
      IDBRelation dbRelation = relationCollection.Create(properties);
      List<AttributeValues> attributeValuesList2 = new List<AttributeValues>()
      {
        new AttributeValues(MRP2Consts.attrIdReplacedBy, (object) tupleList[0].Item1.ObjectID),
        new AttributeValues(MRP2Consts.attrIdDeleteTag, (object) 2),
        new AttributeValues(MRP2Consts.attrIdVersionNumberPL, (object) version),
        new AttributeValues(MRP2Consts.attrIdChangeCode, (object) MRP2Consts.ProductionLinkFlag.Deleted)
      };
      relation.SetAttributesValues(attributeValuesList2.ToArray());
      INotificationService service = (INotificationService) ServicesManager.GetService(typeof (INotificationService));
      if (service == null)
        return;
      service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsRemoved", relation.RelationID, relation.ProjID, relation.RelationType));
      service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", relation.RelationID, relation.ProjID, relation.RelationType));
      service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID, dbRelation.ProjID, dbRelation.RelationType));
    }
  }
}
