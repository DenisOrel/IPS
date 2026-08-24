// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ApplyChangesByEcoCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Commands;
using Intermech.DataFormats;
using Intermech.ECO.Client;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Compositions.CompositionService;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

/// <summary>Провести изменения в группе ПВ (по ИИ)</summary>
internal class ApplyChangesByEcoCommand
{
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    int lcStepId = MetaDataHelper.GetLCStepID(new Guid(RevHelper.lcActualize));
    IOutputView service1 = ServiceUtils.GetService<IOutputView>((object) ServicesManager.ServiceContainer, true);
    Dictionary<long, long> dictionary = new Dictionary<long, long>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ICompositionLoadService service2 = ServiceUtils.GetService<ICompositionLoadService>((object) sessionKeeper.Session, true);
      for (int index = 0; index < items.Count; ++index)
      {
        if (items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData && MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, new Guid("cad00348-306c-11d8-b4e9-00304f19f545")))
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(itemData.ObjectID, false);
          if (dbObject != null)
          {
            string caption = dbObject.Caption;
            IDBAttribute attributeById = dbObject.GetAttributeByID(MRP2Consts.attrIdChangesFromEcoAccepted);
            if (attributeById != null && attributeById.AsBoolean)
            {
              service1.WriteString("MRP2", $"По {caption} уже проведены изменения в производственных ведомостях");
            }
            else
            {
              IDBLifecycleStep lifecycleStep = sessionKeeper.Session.GetLifecycleStep(dbObject.LCStep, true);
              if (lcStepId != lifecycleStep.LCStep)
              {
                service1.WriteString("MRP2", dbObject.Caption + " не находится на шаге жизненного цикла \"Актуализация\"");
              }
              else
              {
                IDBRelationCollection relationCollection1 = sessionKeeper.Session.GetRelationCollection(RevHelper.idLinkRevision);
                relationCollection1.LocalTypesMode = true;
                IDBRelationCollection relationCollection2 = sessionKeeper.Session.GetRelationCollection(MRP2Consts.reltypeIdDocumentation);
                relationCollection2.LocalTypesMode = true;
                DBRecordSetParams paramSet1 = new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[2]
                {
                  new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID),
                  new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PRJLINK_ID)
                });
                ColumnDescriptor[] columns = new ColumnDescriptor[1]
                {
                  new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID)
                };
                DBRecordSetParams paramSet2 = new DBRecordSetParams(new ConditionStructure[1]
                {
                  new ConditionStructure(MetaDataHelper.GetAttributeID((object) "cad001c2-306c-11d8-b4e9-00304f19f545"), RelationalOperators.Equal, (object) 0L, LogicalOperators.AND, 0, false)
                }, columns);
                bool flag = false;
                foreach (DataRow row1 in (InternalDataCollectionBase) relationCollection1.ConsistFrom(paramSet1, dbObject.ObjectID).Rows)
                {
                  long int64Value1 = DataSetProcessor.GetInt64Value(row1, 0, 0L);
                  long int64Value2 = DataSetProcessor.GetInt64Value(row1, 1, 0L);
                  IDBAttribute relationAttribute = sessionKeeper.Session.GetRelationAttribute(int64Value2, (object) MRP2Consts.attrIdApplicabilityinPL, false);
                  if (relationAttribute != null)
                  {
                    QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(int64Value1);
                    DataTable dataTable = (DataTable) null;
                    if (MetaDataHelper.IsObjectTypeChildOf(objectInfo.ObjectTypeID, new Guid("cad00070-306c-11d8-b4e9-00304f19f545")))
                    {
                      paramSet2.Conditions[0].Value = (object) int64Value1;
                      dataTable = relationCollection2.EntersInVersion(paramSet2, int64Value1);
                    }
                    if (dataTable == null || dataTable.Rows.Count == 0)
                    {
                      dataTable = new DataTable();
                      dataTable.Clear();
                      dataTable.Columns.Add("F_OBJECT_ID");
                      DataRow row2 = dataTable.NewRow();
                      row2["F_OBJECT_ID"] = (object) int64Value1;
                      dataTable.Rows.Add(row2);
                    }
                    foreach (DataRow row3 in (InternalDataCollectionBase) dataTable.Rows)
                    {
                      long int64Value3 = DataSetProcessor.GetInt64Value(row3, 0, 0L);
                      foreach (object obj in relationAttribute.Values)
                      {
                        string str = obj.ToString();
                        if (int64Value3 != 0L && string.Empty != str)
                        {
                          string[] strArray = str.Split(':');
                          long int64 = Convert.ToInt64(strArray[0]);
                          string exitasm_id = strArray[1];
                          string from_complect = strArray[2];
                          string to_complect = strArray[3];
                          long objectId;
                          if (!dictionary.ContainsKey(int64))
                          {
                            CreateItemsVersionsCommand itemsVersionsCommand = new CreateItemsVersionsCommand();
                            ISelectedItems selectedItemsForObject = SelectedItemsHelper.CreateSelectedItemsForObject(sessionKeeper.Session.GetObjectBaseVersionByID(int64, true).ObjectID);
                            itemsVersionsCommand.Init(selectedItemsForObject, viewServices, additionalInfo);
                            itemsVersionsCommand.Execute();
                            if (itemsVersionsCommand.Result.Count <= 0)
                              throw new AbortException("Не создана версия ПВ");
                            objectId = itemsVersionsCommand.Result[0].ObjectId;
                            dictionary.Add(int64, objectId);
                          }
                          else
                            objectId = dictionary[int64];
                          flag = true;
                          ApplyChangesByEcoCommand.ApplyChangesInPL(sessionKeeper.Session, int64Value3, objectId, service2, exitasm_id, from_complect, to_complect, viewServices, additionalInfo);
                        }
                      }
                    }
                  }
                }
                if (flag)
                  sessionKeeper.Session.SetObjectAttributesValues(itemData.ObjectID, true, new AttributeValues[1]
                  {
                    new AttributeValues(MRP2Consts.attrIdChangesFromEcoAccepted, (object) true)
                  });
                else
                  service1.WriteString("MRP2", $"В {caption} нет указания о применении в ПВ");
              }
            }
          }
        }
      }
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="session"></param>
  /// <param name="replacePartId">Идентификатор версии изделия, которую будеи вставлять в ПВ</param>
  /// <param name="destPLID">Идентификатор ПВ</param>
  /// <param name="svc"></param>
  /// <param name="exitasm_id">Ид-ор выходной сборки в которой надо сделать замену</param>
  internal static void ApplyChangesInPL(
    IUserSession session,
    long replacePartId,
    long destPLID,
    ICompositionLoadService svc,
    string exitasm_id,
    string from_complect,
    string to_complect,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    QuickObjectInfo objectInfo = session.GetObjectInfo(replacePartId);
    IDBObjectCollection objectCollection = session.GetObjectCollection(objectInfo.ObjectTypeID);
    objectCollection.ShowAllModifications = true;
    List<long> list = objectCollection.Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(-3, RelationalOperators.Equal, (object) objectInfo.ID, LogicalOperators.AND, 0, false)
    }, new object[1]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID
    })
    {
      RecordCount = -1
    }).AsEnumerable().Select<DataRow, long>((System.Func<DataRow, long>) (r => Math.Abs(Convert.ToInt64(r[0])))).ToList<long>();
    string str = "";
    for (int index = 0; index < list.Count; ++index)
      str += $"([{MRP2Consts.attrIdArticleLink}] = {list[index]}) OR ";
    string filterIDs = str.TrimEnd(' ', 'O', 'R');
    IDBObject dbObject = session.GetObject(destPLID, true);
    try
    {
      dbObject.CheckEdit();
    }
    catch (KernelExceptionID ex)
    {
      if (ex.ErrorID == 139)
      {
        if (DialogResult.Yes != MessageBox.Show($"{dbObject.Caption}{LocalizationHolder.rm.GetString("msgCantEdit")}\r\n\r\n{ex.Message}", LocalizationHolder.rm.GetString("msgConfirmation"), MessageBoxButtons.YesNo))
          throw new AbortException();
        CreateItemsVersionsCommand itemsVersionsCommand = new CreateItemsVersionsCommand();
        ISelectedItems selectedItemsForObject = SelectedItemsHelper.CreateSelectedItemsForObject(dbObject.ObjectID);
        itemsVersionsCommand.Init(selectedItemsForObject, viewServices, additionalInfo);
        itemsVersionsCommand.Execute();
        if (itemsVersionsCommand.Result.Count <= 0)
          throw new AbortException();
        dbObject = session.GetObject(itemsVersionsCommand.Result[0].ObjectId);
        dbObject.CheckEdit();
      }
      else
        throw;
    }
    List<ObjInfoItem> objects = new List<ObjInfoItem>();
    objects.Add(new ObjInfoItem(dbObject.ObjectID, dbObject.ObjectType));
    ColumnDescriptor[] columns = new ColumnDescriptor[8]
    {
      new ColumnDescriptor((object) -21, AttributeSourceTypes.Relation, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -22, AttributeSourceTypes.Relation, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -7, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -26, AttributeSourceTypes.Relation, ColumnContents.String, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -6, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MRP2Consts.attrIdArticleLink, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MRP2Consts.attrIdPKDSE_Id, AttributeSourceTypes.Object, ColumnContents.String, ColumnNameMapping.ID, SortOrders.NONE, 0)
    };
    CompositionLoadingParams loadingParams = new CompositionLoadingParams((IEnumerable<ObjInfoItem>) objects, (IEnumerable<int>) null, (IEnumerable<int>) null, (IEnumerable<int>) new int[1]
    {
      MRP2Consts.reltypeIdProductComposition
    }, (IEnumerable<ColumnDescriptor>) columns, (IEnumerable<ConditionStructure>) null, true, false, -1, (VersionsRule) null, "cad00601-306c-11d8-b4e9-00304f19f545");
    DataTable source = svc.LoadComplexCompositions((object) session, loadingParams);
    if (source == null || source.Rows.Count <= 0)
      return;
    new ProductionListTree(source, objects[0].ObjectID, filterIDs).CheckOutCopiesAndReplacePartVersionDialog(session, replacePartId, (long) dbObject.VersionID, exitasm_id, from_complect, to_complect);
  }
}
