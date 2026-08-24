// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ContextCompositionCommandProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Pdm.ContextComposition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm;

public class ContextCompositionCommandProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add("EditDocument", new CommandInfo(0, new ClickEventHandler(this.EditContextComposition)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return new CommandsInfo();
  }

  public void EditContextComposition(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    int objectTypeId = MetaDataHelper.GetObjectTypeID("cad00132-306c-11d8-b4e9-00304f19f545");
    if (items.Count <= 0)
      return;
    IDBTypedObjectID parentData = items.GetParentData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData1))
      return;
    long prototypeObject = -1;
    long num = -1;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (parentData != null && parentData.ObjectType == objectTypeId)
      {
        IDBRelationID itemData2 = items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
        prototypeObject = parentData.ObjectID;
        num = itemData2 != null ? itemData2.Value : -1L;
      }
      else
      {
        IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID("cad00023-306c-11d8-b4e9-00304f19f545"));
        relationCollection.ObjectTypeID = objectTypeId;
        DataTable dataTable = relationCollection.EntersIn(new DBRecordSetParams((ConditionStructure[]) null, new object[3]
        {
          (object) ObligatoryObjectAttributes.F_OBJECT_ID,
          (object) ObligatoryObjectAttributes.CAPTION,
          (object) ObligatoryObjectAttributes.F_PRJLINK_ID
        }), itemData1.ID);
        if (dataTable.Rows.Count == 0)
        {
          Utils.OpenNewWindow((IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(itemData1.ObjectID), (IServiceProvider) ApplicationServices.Container);
          return;
        }
        if (dataTable.Rows.Count == 1)
        {
          prototypeObject = Convert.ToInt64(dataTable.Rows[0].ItemArray[0]);
          num = Convert.ToInt64(dataTable.Rows[0].ItemArray[2]);
        }
        else
        {
          List<(long, string, long)> assembliesTuples = new List<(long, string, long)>();
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            assembliesTuples.Add((Convert.ToInt64(row.ItemArray[0]), row.ItemArray[1].ToString(), Convert.ToInt64(row.ItemArray[2])));
          using (SelectAssembliesTCEForm assembliesTceForm = new SelectAssembliesTCEForm(assembliesTuples))
          {
            if (assembliesTceForm.ShowDialog() == DialogResult.OK)
            {
              if (assembliesTceForm.SelectedAssemblies != -1L && assembliesTceForm.SelectedAssemliesRelationID != -1L)
              {
                prototypeObject = assembliesTceForm.SelectedAssemblies;
                num = assembliesTceForm.SelectedAssemliesRelationID;
              }
              else
              {
                Utils.OpenNewWindow((IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(itemData1.ObjectID), (IServiceProvider) ApplicationServices.Container);
                return;
              }
            }
            else
            {
              Utils.OpenNewWindow((IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(itemData1.ObjectID), (IServiceProvider) ApplicationServices.Container);
              return;
            }
          }
        }
      }
      if (num != -1L)
      {
        IDBRelation relation = sessionKeeper.Session.GetRelation(num, false);
        if (relation != null)
        {
          IDBAttribute attributeById = relation.GetAttributeByID(MetaDataHelper.GetAttributeID((object) "cad00651-306c-11d8-b4e9-00304f19f545"));
          long objectId = itemData1.ObjectID;
          IDBObject dbObject = sessionKeeper.Session.GetObject(itemData1.ObjectID, false);
          if (dbObject != null)
          {
            if (dbObject.CheckoutBy == 0L)
              objectId = dbObject.CheckOut().ObjectID;
            else if (dbObject.CheckoutBy != sessionKeeper.Session.UserID)
            {
              QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(dbObject.CheckoutBy);
              throw new KernelException($"Технологическая сборочная единица [{dbObject.ObjectID}]'{dbObject.Caption}' взята на редактирование пользователем '{objectInfo.Caption}' редактирование невозможно.");
            }
          }
          ContextCompositionEditor compositionEditor = new ContextCompositionEditor(prototypeObject, objectId, attributeById.AsInteger, attributeById.Description, num);
          DockManager service = (DockManager) ApplicationServices.Container.GetService(typeof (DockManager));
          if (service == null)
            return;
          compositionEditor.Show(service);
          compositionEditor.Activate();
        }
        else
          Utils.OpenNewWindow((IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(itemData1.ObjectID), (IServiceProvider) ApplicationServices.Container);
      }
      else
        Utils.OpenNewWindow((IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(itemData1.ObjectID), (IServiceProvider) ApplicationServices.Container);
    }
  }
}
