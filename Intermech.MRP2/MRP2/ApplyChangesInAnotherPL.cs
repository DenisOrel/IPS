// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ApplyChangesInAnotherPL
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Commands;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Compositions.CompositionService;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

internal class ApplyChangesInAnotherPL
{
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    INodeID itemId = items.GetItemID(0);
    if (itemId.CategoryID != 1 || !MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdProductionCopy))
      return;
    IDBTypedObjectID itemData = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long asInteger = (sessionKeeper.Session.GetObjectAttributeByID(itemData.ObjectID, MRP2Consts.attrIdArticleLink) ?? throw new NotificationException("У производственной копии отсутсвует ссылка на объект по которому она была создана")).AsInteger;
      if (Intermech.Consts.IsUndefinedObjectId(asInteger))
        throw new NotificationException("У производственной копии отсутсвует ссылка на объект по которому она была создана");
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(asInteger);
      IDBObjectCollection objectCollection1 = sessionKeeper.Session.GetObjectCollection(objectInfo.ObjectTypeID);
      objectCollection1.ShowAllModifications = true;
      DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(-3, RelationalOperators.Equal, (object) objectInfo.ID, LogicalOperators.AND, 0, false)
      }, new object[2]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID,
        (object) ObligatoryObjectAttributes.F_OBJECT_TYPE
      });
      paramSet.RecordCount = -1;
      List<long> list = objectCollection1.Select(paramSet).AsEnumerable().Select<DataRow, long>((System.Func<DataRow, long>) (r => Math.Abs(Convert.ToInt64(r[0])))).ToList<long>();
      IDBObjectCollection objectCollection2 = sessionKeeper.Session.GetObjectCollection(MRP2Consts.objtypeIdProductionCopy);
      ConditionStructure[] conditions = new ConditionStructure[list.Count];
      string str = "";
      for (int index = 0; index < list.Count; ++index)
      {
        conditions[index] = new ConditionStructure(MRP2Consts.attrIdArticleLink, RelationalOperators.Equal, (object) list[index], LogicalOperators.OR, 1, false);
        conditions[index].AttributeSource = AttributeSourceTypes.Object;
        str += $"([{MRP2Consts.attrIdArticleLink}] = {list[index]}) OR ";
      }
      string filterIDs = str.TrimEnd(' ', 'O', 'R');
      paramSet = new DBRecordSetParams(conditions, new object[2]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID,
        (object) ObligatoryObjectAttributes.F_OBJECT_TYPE
      });
      paramSet.RecordCount = -1;
      DataTable dataTable = objectCollection2.Select(paramSet);
      List<ObjInfoItem> objects = new List<ObjInfoItem>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        long int64 = Convert.ToInt64(row[0]);
        if (int64 != itemData.ObjectID)
          objects.Add(new ObjInfoItem(int64, Convert.ToInt32(row[1])));
      }
      if (objects.Count == 0)
        throw new NotificationException("Нет других производственных копий выпущенных с объектом\r\n" + objectInfo.Caption);
      ICompositionLoadService service = ServiceUtils.GetService<ICompositionLoadService>((object) sessionKeeper.Session, true);
      ListDescriptor rootDescriptor = new ListDescriptor(Intermech.Navigator.Consts.CategoryVersionsObjectNode, MRP2Consts.objtypeIdProductionLists, "", (IList) (service.LoadComplexCompositions((object) sessionKeeper.Session, (IEnumerable<ObjInfoItem>) objects, (IEnumerable<int>) new int[1]
      {
        MRP2Consts.reltypeIdProductComposition
      }, (IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(MRP2Consts.objtypeIdProductionLists), (IEnumerable<ColumnDescriptor>) new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID)
      }, false, false, (VersionsRule) null, (IEnumerable<ConditionStructure>) null, "cad00601-306c-11d8-b4e9-00304f19f545", (Dictionary<long, HybridDictionary>) null, -1, (IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(MRP2Consts.objtypeIdProductionObjects)) ?? throw new NotificationException("Объект не применяется в других производственных ведомостях")).AsEnumerable().Select<DataRow, long>((System.Func<DataRow, long>) (row => Convert.ToInt64(row[0]))).ToList<long>());
      object[] objArray = SelectionWindow.Select("Выберите производственные ведомости", "Выберите производственные ведомости в составе которых надо заменить копию " + objectInfo.Caption, (IDescriptor) rootDescriptor, typeof (IDBCheckedOutByID), SelectionOptions.HideTree | SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree);
      if (objArray == null)
        return;
      ColumnDescriptor[] columns = new ColumnDescriptor[7]
      {
        new ColumnDescriptor((object) -21, AttributeSourceTypes.Relation, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -22, AttributeSourceTypes.Relation, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -7, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -26, AttributeSourceTypes.Relation, ColumnContents.String, ColumnNameMapping.ID, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -6, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
        new ColumnDescriptor((object) MRP2Consts.attrIdArticleLink, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0)
      };
      CompositionLoadingParams loadingParams = new CompositionLoadingParams((IEnumerable<ObjInfoItem>) objects, (IEnumerable<int>) null, (IEnumerable<int>) null, (IEnumerable<int>) new int[1]
      {
        MRP2Consts.reltypeIdProductComposition
      }, (IEnumerable<ColumnDescriptor>) columns, (IEnumerable<ConditionStructure>) null, true, false, -1, (VersionsRule) null, "cad00601-306c-11d8-b4e9-00304f19f545");
      foreach (object obj in objArray)
      {
        objects.Clear();
        if (obj is IDBCheckedOutByID dbCheckedOutById)
        {
          if (Intermech.Consts.IsUndefinedObjectId(dbCheckedOutById.CheckedOutBy))
          {
            IDBObject dbObject = sessionKeeper.Session.GetObject(dbCheckedOutById.ObjectID);
            if (DialogResult.Yes == MessageBox.Show(dbObject.Caption + " требуется взять на редактирование, продолжить?", "Подтверждение", MessageBoxButtons.YesNo))
            {
              ObjectCopyCommand checkoutCommand = ObjectCommandFactory.CreateCheckoutCommand(true);
              checkoutCommand.ObjectId = dbObject.ObjectID;
              try
              {
                checkoutCommand.Execute();
                objects.Add(new ObjInfoItem(checkoutCommand.NewObjectId, dbObject.ObjectType));
              }
              catch (KernelExceptionID ex)
              {
                if (ex.ErrorID == 240 /*0xF0*/)
                {
                  if (DialogResult.Yes != MessageBox.Show($"{dbObject.Caption}{LocalizationHolder.rm.GetString("msgCantCheckOut")}\r\n\r\n{ex.Message}", LocalizationHolder.rm.GetString("msgConfirmation"), MessageBoxButtons.YesNo))
                    throw new AbortException();
                  CreateItemsVersionsCommand itemsVersionsCommand = new CreateItemsVersionsCommand();
                  ISelectedItems selectedItemsForObject = SelectedItemsHelper.CreateSelectedItemsForObject(dbObject.ObjectID);
                  itemsVersionsCommand.Init(selectedItemsForObject, viewServices, additionalInfo);
                  itemsVersionsCommand.Execute();
                  if (itemsVersionsCommand.Result.Count <= 0)
                    throw new AbortException();
                  objects.Add(new ObjInfoItem(itemsVersionsCommand.Result[0].ObjectId, dbObject.ObjectType));
                }
                else
                  throw;
              }
            }
          }
          else if (dbCheckedOutById.CheckedOutBy == sessionKeeper.Session.UserID)
            objects.Add(new ObjInfoItem(dbCheckedOutById.ObjectID, sessionKeeper.Session.GetObjectInfo(dbCheckedOutById.ObjectID).ObjectTypeID));
          else
            sessionKeeper.Session.GetObject(dbCheckedOutById.ObjectID).CheckEdit();
          DataTable source = service.LoadComplexCompositions((object) sessionKeeper.Session, loadingParams);
          if (source != null && source.Rows.Count > 0)
            new ProductionListTree(source, objects[0].ObjectID, filterIDs).CheckOutCopiesAndReplacePart(sessionKeeper.Session, itemData.ObjectID);
        }
      }
    }
  }
}
