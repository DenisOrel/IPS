// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.SeparateDeliveryCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

/// <summary>команда меню изменить способ доставки</summary>
internal class SeparateDeliveryCommand
{
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    long versionPL = MRP2Service.GetPLNodeID(items).Version;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      for (int index = 0; index < items.Count; ++index)
      {
        INodeID itemId = items.GetItemID(index);
        if (itemId.CategoryID == 1 && MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdProductionCopy))
        {
          IDBObjectID itemData1 = (IDBObjectID) items.GetItemData(index, typeof (IDBObjectID));
          IDBObject obj = sessionKeeper.Session.GetObject(itemData1.Value);
          obj.CheckEdit();
          IDBAttribute dbAttribute1 = obj.Attributes.FindByID(MRP2Consts.attrIdSeparateDelivery);
          bool val;
          if (dbAttribute1 == null || !dbAttribute1.AsBoolean)
          {
            if (IMMessageBox.Show(obj.Caption, $"Объект {obj.Caption} будет поставляться отдельно?\r\n(все входящие копии будут взяты на редактирование и помечены как удаленные)", MessageBoxButtons.YesNo) == DialogResult.Yes)
              val = true;
            else
              continue;
          }
          else if (IMMessageBox.Show(obj.Caption, $"Объект {obj.Caption}\r\n будет поставляться совместно?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            val = false;
          else
            continue;
          if (dbAttribute1 == null)
            dbAttribute1 = obj.Attributes.AddAttribute(MRP2Consts.attrIdSeparateDelivery, false);
          dbAttribute1.AsBoolean = val;
          MRP2Service.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", obj.ObjectID, obj.ObjectType));
          if (val)
            MRP2Service.CheckOutTreeCopy(obj, new MRP2Service.DoOperationDelegate(handler));
          else if (items.GetItemData(index, typeof (IDBRelationID)) is IDBRelationID itemData2 && !Intermech.Consts.IsUndefinedRelationId(itemData2.Value))
          {
            IDBRelation relation = sessionKeeper.Session.GetRelation(itemData2.Value);
            IDBAttribute byId1 = relation.Attributes.FindByID(MRP2Consts.attrIdDeleteTag);
            if (byId1 != null)
              byId1.AsBoolean = false;
            IDBAttribute byId2 = relation.Attributes.FindByID(MRP2Consts.attrIdChangeCode);
            if (byId2 != null)
              byId2.Value = (object) MRP2Consts.ProductionLinkFlag.Modified;
            IDBAttribute dbAttribute2 = relation.Attributes.AddAttribute(MRP2Consts.attrIdVersionNumberPL, false);
            if (dbAttribute2 != null)
            {
              dbAttribute2.AsInteger = versionPL;

              void handler(IDBObject o, IDBRelation rel)
              {
                if (o != null && o.CheckoutBy != o.Session.UserID)
                  return;
                if (o != null)
                  o.Attributes.AddAttribute(MRP2Consts.attrIdSeparateDelivery, false).AsBoolean = val;
                if (!val)
                  return;
                AttributeValues[] valuesList = new AttributeValues[3]
                {
                  new AttributeValues(MRP2Consts.attrIdDeleteTag, (object) true),
                  new AttributeValues(MRP2Consts.attrIdVersionNumberPL, (object) versionPL),
                  new AttributeValues(MRP2Consts.attrIdChangeCode, (object) MRP2Consts.ProductionLinkFlag.Deleted)
                };
                rel.SetAttributesValues(valuesList);
                if (rel.ProjID != obj.ObjectID)
                  return;
                MRP2Service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsChanged", rel.RelationID, rel.ProjID, rel.RelationType));
              }
            }
            MRP2Service.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsChanged", relation.RelationID, relation.ProjID, relation.RelationType));
          }
        }
      }
    }
  }
}
