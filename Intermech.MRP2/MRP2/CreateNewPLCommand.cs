// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.CreateNewPLCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Client.Core.Navigator.Classes.ObjectNode;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

internal class CreateNewPLCommand
{
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    long num = 0;
    IObjectCreatorService service1 = ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService;
    service1.AfterObjectCreatedEvent += new AfterObjectCreatedEventHandler(CreateProtoCommand.OnCreateNewObject);
    try
    {
      num = service1.CreateObjectByTypeDialog(MRP2Consts.objtypeIdProductionLists);
      AfterObjectCreatorDialogHandlers.Handle(num, 0, items, viewServices, additionalInfo);
    }
    finally
    {
      service1.AfterObjectCreatedEvent -= new AfterObjectCreatedEventHandler(CreateProtoCommand.OnCreateNewObject);
    }
    if (Consts.IsUndefinedObjectId(num))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(num);
      dbObject.CheckEdit();
      int versionId = dbObject.VersionID;
      long objectId = dbObject.ObjectID;
      int objectType = dbObject.ObjectType;
      List<Tuple<IDBObject, MeasuredValue>> tupleList = new List<Tuple<IDBObject, MeasuredValue>>();
      for (int index = 0; index < items.Count; ++index)
      {
        if (items.GetItemData(0, typeof (IDBObjectID)) is IDBObjectID itemData)
        {
          Tuple<IDBObject, MeasuredValue> tuple = AddSostavCommand.MakeCopyForAdding(sessionKeeper.Session, itemData.Value, objectType, (long) versionId);
          if (tuple != null)
            tupleList.Add(tuple);
        }
      }
      if (tupleList.Count == 0)
        return;
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(MRP2Consts.reltypeIdProductComposition);
      INotificationService service2 = (INotificationService) ServicesManager.GetService(typeof (INotificationService));
      foreach (Tuple<IDBObject, MeasuredValue> tuple in tupleList)
      {
        MeasuredValue initValue = tuple.Item2 ?? new MeasuredValue(1.0, PDMPluginIDs.measureShtuk);
        AttributeValues[] vals = new AttributeValues[2]
        {
          new AttributeValues(MRP2Consts.attrIdCount, (object) initValue),
          new AttributeValues(MRP2Consts.attrIdVersionNumberPL, (object) versionId)
        };
        IDBRelation dbRelation = relationCollection.Create(objectId, tuple.Item1.ObjectID, vals);
        service2?.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID, dbRelation.ProjID, dbRelation.RelationType));
      }
    }
  }
}
