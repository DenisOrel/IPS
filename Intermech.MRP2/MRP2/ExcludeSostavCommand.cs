// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ExcludeSostavCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MRP2;

internal class ExcludeSostavCommand
{
  /// <summary>
  /// Если у исключаемого объекта есть состав, то создадим новую производственную копию порожденную от нее
  /// и ей уже поставим признак исключения.
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    INodeID itemId = items.GetItemID(0);
    if (itemId.CategoryID != 1)
      return;
    bool flag = MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdDocument);
    if (((MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdProductionLists) ? 1 : (MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdProductionCopy) ? 1 : 0)) | (flag ? 1 : 0)) == 0)
      return;
    long version = MRP2Service.GetPLNodeID(items).Version;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(items.GetItemData(0, typeof (IDBRelationID)) is IDBRelationID itemData1) || Consts.IsUndefinedRelationId(itemData1.Value))
        return;
      IDBRelation relation = sessionKeeper.Session.GetRelation(itemData1.Value);
      sessionKeeper.Session.GetObject(relation.ProjID).CheckEdit();
      IDBTypedObjectID itemData2 = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      if (items.GetItemData(0, typeof (NavigatorTreeNode)) is NavigatorTreeNode itemData3 && itemData3.HasChildren && itemData3.Children.Count == 0)
        itemData3.Reload();
      if (itemData3 != null && (itemData3.HasChildren || itemData3.Children.Count > 0) && itemData2 != null && !flag)
      {
        IDBObject objectCopy = MRP2Consts.CreateObjectCopy(sessionKeeper.Session, itemData2.ObjectID, itemData2.ObjectType);
        objectCopy.CommitCreation(true, false);
        relation.ReplacePartObject(objectCopy.ObjectID);
      }
      AttributeValues[] valuesList = new AttributeValues[3]
      {
        new AttributeValues(MRP2Consts.attrIdDeleteTag, (object) true),
        new AttributeValues(MRP2Consts.attrIdVersionNumberPL, (object) version),
        new AttributeValues(MRP2Consts.attrIdChangeCode, (object) MRP2Consts.ProductionLinkFlag.Deleted)
      };
      relation.SetAttributesValues(valuesList);
      INotificationService service = ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false);
      if (service == null)
        return;
      service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsRemoved", relation.RelationID, relation.ProjID, relation.RelationType));
      service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", relation.RelationID, relation.ProjID, relation.RelationType));
    }
  }
}
