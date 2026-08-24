// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ApplyChangesInPLbyECO
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.ECO.Client;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.MRP2;

/// <summary>применить изменения в ПВ (по группе ИИ)</summary>
internal class ApplyChangesInPLbyECO
{
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    int lcStepId = MetaDataHelper.GetLCStepID(new Guid(RevHelper.lcActualize));
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData && MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, MRP2Consts.objtypeIdProductionLists))
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          ICompositionLoadService service = ServiceUtils.GetService<ICompositionLoadService>((object) sessionKeeper.Session, true);
          IDBRelationCollection relationCollection1 = sessionKeeper.Session.GetRelationCollection(MRP2Consts.reltypeIdDocumentation);
          relationCollection1.LocalTypesMode = true;
          ColumnDescriptor[] columns = new ColumnDescriptor[1]
          {
            new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID)
          };
          DBRecordSetParams paramSet1 = new DBRecordSetParams(new ConditionStructure[1]
          {
            new ConditionStructure(MetaDataHelper.GetAttributeID((object) "cad001c2-306c-11d8-b4e9-00304f19f545"), RelationalOperators.Equal, (object) 0L, LogicalOperators.AND, 0, false)
          }, columns);
          IDBRelationCollection relationCollection2 = sessionKeeper.Session.GetRelationCollection(RevHelper.idLinkRevision);
          relationCollection2.LocalTypesMode = true;
          DBRecordSetParams paramSet2 = new DBRecordSetParams(new ConditionStructure[1]
          {
            new ConditionStructure(MRP2Consts.attrIdApplicabilityinPL, RelationalOperators.StartString, (object) $"{itemData.ID}:", LogicalOperators.AND, 0, false)
          }, new ColumnDescriptor[3]
          {
            new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PRJLINK_ID, AttributeSourceTypes.Relation, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
            new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PROJ_ID, AttributeSourceTypes.Relation, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.ASC, 1),
            new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID)
          });
          DataTable dataTable1 = relationCollection2.Select(paramSet2);
          List<long> objectIDs = new List<long>();
          foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
          {
            long int64Value = DataSetProcessor.GetInt64Value(row, 1, 0L);
            IDBObject dbObject = sessionKeeper.Session.GetObject(int64Value, true);
            IDBAttribute attributeById = dbObject.GetAttributeByID(MRP2Consts.attrIdChangesFromEcoAccepted);
            if (attributeById == null || !attributeById.AsBoolean)
            {
              IDBLifecycleStep lifecycleStep = sessionKeeper.Session.GetLifecycleStep(dbObject.LCStep, true);
              if (lcStepId == lifecycleStep.LCStep)
                objectIDs.Add(int64Value);
            }
          }
          if (objectIDs.Count == 0)
            throw new NotificationException("Не найдены извещения, которые ссылаются на текущую ведомость и находятся на шаге жизненного цикла \"Актуализация\" и изменения по которым не были внедрены в ПВ");
          object[] objArray = SelectionWindow.Select("Выберите извещения", "Выберите извещения, изменения по которорым требуется применить в ведомости", (IDescriptor) new ListDescriptor(Intermech.Navigator.Consts.CategoryVersionsObjectNode, RevHelper.idObjRevision, "Список извещений", (IList) objectIDs), typeof (IDBObjectID), SelectionOptions.HideTree | SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree);
          if (objArray == null)
            break;
          foreach (object obj1 in objArray)
          {
            IDBObject dbObject = sessionKeeper.Session.GetObject((obj1 as IDBObjectID).Value, true);
            if (MetaDataHelper.IsObjectTypeChildOf(dbObject.ObjectType, new Guid("cad00348-306c-11d8-b4e9-00304f19f545")))
            {
              foreach (DataRow row1 in (InternalDataCollectionBase) relationCollection2.ConsistFrom(paramSet2, dbObject.ObjectID).Rows)
              {
                long int64Value1 = DataSetProcessor.GetInt64Value(row1, 0, 0L);
                long int64Value2 = DataSetProcessor.GetInt64Value(row1, 2, 0L);
                IDBAttribute relationAttribute = sessionKeeper.Session.GetRelationAttribute(int64Value1, (object) MRP2Consts.attrIdApplicabilityinPL, true);
                if (relationAttribute != null)
                {
                  QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(int64Value2);
                  DataTable dataTable2 = (DataTable) null;
                  if (MetaDataHelper.IsObjectTypeChildOf(objectInfo.ObjectTypeID, new Guid("cad00070-306c-11d8-b4e9-00304f19f545")))
                  {
                    paramSet1.Conditions[0].Value = (object) int64Value2;
                    dataTable2 = relationCollection1.EntersInVersion(paramSet1, int64Value2);
                  }
                  if (dataTable2 == null || dataTable2.Rows.Count == 0)
                  {
                    dataTable2 = new DataTable();
                    dataTable2.Clear();
                    dataTable2.Columns.Add("F_OBJECT_ID");
                    DataRow row2 = dataTable2.NewRow();
                    row2["F_OBJECT_ID"] = (object) int64Value2;
                    dataTable2.Rows.Add(row2);
                  }
                  foreach (DataRow row3 in (InternalDataCollectionBase) dataTable2.Rows)
                  {
                    long int64Value3 = DataSetProcessor.GetInt64Value(row3, 0, 0L);
                    foreach (object obj2 in relationAttribute.Values)
                    {
                      string str = obj2.ToString();
                      if (string.Empty != str)
                      {
                        string[] strArray = str.Split(':');
                        long int64 = Convert.ToInt64(strArray[0]);
                        string exitasm_id = strArray[1];
                        string from_complect = strArray[2];
                        string to_complect = strArray[3];
                        if (itemData.ID == int64)
                          ApplyChangesByEcoCommand.ApplyChangesInPL(sessionKeeper.Session, int64Value3, itemData.ObjectID, service, exitasm_id, from_complect, to_complect, viewServices, additionalInfo);
                      }
                    }
                  }
                }
              }
              dbObject.SetAttributesValues(new AttributeValues[1]
              {
                new AttributeValues(MRP2Consts.attrIdChangesFromEcoAccepted, (object) true)
              });
            }
          }
        }
      }
    }
  }
}
