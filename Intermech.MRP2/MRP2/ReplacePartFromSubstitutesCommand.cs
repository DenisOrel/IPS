// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ReplacePartFromSubstitutesCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.Interfaces;
using Intermech.Search.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

/// <summary>команда меню заменить из конструкторских замен</summary>
internal class ReplacePartFromSubstitutesCommand
{
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    INodeID itemId = items.GetItemID(0);
    if (itemId.CategoryID != 1 || !MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdProductionCopy))
      return;
    long version = MRP2Service.GetPLNodeID(items).Version;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      SubstituteObjects.InitStaticFields(sessionKeeper.Session);
      if (!(items.GetItemData(0, typeof (IDBRelationID)) is IDBRelationID itemData1) || Intermech.Consts.IsUndefinedRelationId(itemData1.Value))
        return;
      IDBRelation relation1 = sessionKeeper.Session.GetRelation(itemData1.Value);
      IDBAttribute attributeById1 = relation1.GetAttributeByID(MRP2Consts.attrIdCreatedByRelation);
      if (attributeById1 == null)
      {
        int num1 = (int) IMMessageBox.Show("Ошибка", "У связи отсутствует атрибут \"Создан на основе связи\", поэтому нельзя найти конструкторские заменители", MessageBoxButtons.OK);
      }
      else
      {
        IDBRelation relation2 = sessionKeeper.Session.GetRelation(new Guid(attributeById1.AsString), false);
        if (relation2 == null)
        {
          int num2 = (int) IMMessageBox.Show("Ошибка", "Не найдена конструкторская связь, по которой была создана производственная копия,\r\nвозможно она была удалена", MessageBoxButtons.OK);
        }
        else
        {
          IDBTypedObjectID itemData2 = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
          IDBObject dbObject = sessionKeeper.Session.GetObject(itemData1.ProjID);
          dbObject.CheckEdit();
          long objectId = dbObject.ObjectID;
          IDBAttribute attributeById2 = relation1.GetAttributeByID(MRP2Consts.attrIdReplacedBy);
          if (attributeById2 != null && attributeById2.AsInteger != 0L)
            throw new Exception("Нельзя заменить уже замененную позицию");
          IDBAttribute attributeById3 = relation1.GetAttributeByID(MRP2Consts.attrIdDeleteTag);
          if (attributeById3 != null && attributeById3.AsInteger != 0L)
            throw new Exception("Нельзя заменить исключенную позицию");
          IDBAttribute attributeById4 = relation1.GetAttributeByID(SubstituteObjects.attrSubstituteGroupNo);
          long copyGroupId = attributeById4 != null && attributeById4.AsInteger != 0L ? attributeById4.AsInteger : throw new Exception("Текущая позиция не использовалась в допустимых заменах");
          if (!(sessionKeeper.Session.GetCustomService(typeof (ISubstitutesService)) is ISubstitutesService customService1))
            return;
          IDBAttribute attributeById5 = relation2.GetAttributeByID(SubstituteObjects.attrSubstituteGroupNo);
          if (attributeById5 == null)
          {
            int num3 = (int) IMMessageBox.Show("Ошибка", "Конструкторская связь, не участвует в допустимых заменах", MessageBoxButtons.OK);
          }
          else
          {
            long asInteger = attributeById5.AsInteger;
            SubstituteObjects substitutes = customService1.LoadSubstitutes(sessionKeeper.Session.SessionGUID, "cad001e2-306c-11d8-b4e9-00304f19f545", (List<long>) null, relation2.ProjID, relation2.RelationType);
            for (int index = substitutes.Groups.Count - 1; index >= 0; --index)
            {
              if (asInteger != substitutes.Groups[index])
                substitutes.RemoveGroup(substitutes.Groups[index]);
            }
            substitutes.RebuildGroups();
            List<long> relationIds;
            if (SelectSubstituteDlg.Execute(substitutes, out relationIds) != DialogResult.OK)
              return;
            if (!(sessionKeeper.Session.GetCustomService(typeof (IMRP2ServerService)) is IMRP2ServerService customService2))
              throw new Exception("IMRP2ServerService not found");
            sessionKeeper.Session.StartLogHistory();
            try
            {
              customService2.ReplacePartFromSubstitute(sessionKeeper.Session.SessionGUID, itemData1.Value, copyGroupId, version, objectId, relationIds, substitutes);
              NotificationHelper.Notify((object) null, sessionKeeper.Session.GetModificationsHistoryList());
            }
            finally
            {
              sessionKeeper.Session.StopLogHistory();
            }
          }
        }
      }
    }
  }
}
