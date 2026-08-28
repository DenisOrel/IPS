// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Server.Objects.ComplectDBObject
// Assembly: Intermech.Reports.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B97D7940-CE11-4EF0-80CD-76A0AE479D33
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Reports.Server.dll

using Intermech.Expert;
using Intermech.Interfaces;
using Intermech.Interfaces.Reports;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Reports.Server.Objects;

public class ComplectDBObject(UserSession uSession, DataTable objectsTable) : DBObject(uSession, objectsTable)
{
  internal static readonly List<int> ChildType2Remove = new List<int>();

  static ComplectDBObject()
  {
    ComplectDBObject.ChildType2Remove.Add(ReportsConsts.DocumentBaseTypeID);
    ComplectDBObject.ChildType2Remove.Add(ReportsConsts.DocPackageBaseTypeID);
  }

  protected override void DoDeleteObj_DeleteDownLinks(DataTable table)
  {
    if (table != null && table.Rows.Count != 0)
    {
      List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive((IEnumerable<int>) ComplectDBObject.ChildType2Remove);
      childrenIdRecursive.Sort();
      Dictionary<int, List<IMSApplicability>> dictionary1 = new Dictionary<int, List<IMSApplicability>>();
      List<IMSApplicability> imsApplicabilityList;
      foreach (IMSApplicability typeApplicability in MetaDataHelper.GetObjectTypeApplicabilities(this.ObjectType))
      {
        if (typeApplicability != null && (typeApplicability.RelationConstraintMode == RelationConstraintModes.ChildDelete || typeApplicability.RelationConstraintMode == RelationConstraintModes.ChildForcedDelete) && childrenIdRecursive.BinarySearch(typeApplicability.ChildObjectTypeID) >= 0)
        {
          if (!dictionary1.TryGetValue(typeApplicability.RelationTypeID, out imsApplicabilityList))
          {
            imsApplicabilityList = new List<IMSApplicability>();
            dictionary1.Add(typeApplicability.RelationTypeID, imsApplicabilityList);
          }
          imsApplicabilityList.Add(typeApplicability);
        }
      }
      if (dictionary1.Count > 0)
      {
        Dictionary<long, int> dictionary2 = new Dictionary<long, int>(table.Rows.Count);
        List<long> list = new List<long>(table.Rows.Count);
        int columnIndex1 = 0;
        int columnIndex2 = 3;
        int columnIndex3 = 6;
        int columnIndex4 = 7;
        foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        {
          int int32_1 = Convert.ToInt32(row[columnIndex2]);
          int int32_2 = Convert.ToInt32(row[columnIndex4]);
          if (dictionary1.TryGetValue(int32_1, out imsApplicabilityList))
          {
            bool flag = false;
            foreach (IMSApplicability imsApplicability in imsApplicabilityList)
            {
              if (imsApplicability != null && MetaDataHelper.IsObjectTypeChildOf(int32_2, imsApplicability.ChildObjectTypeID))
              {
                flag = true;
                break;
              }
            }
            if (flag)
            {
              long int64_1 = Convert.ToInt64(row[columnIndex3]);
              long int64_2 = Convert.ToInt64(row[columnIndex1]);
              list.Add(int64_2);
              if (int64_1 != 0L && !dictionary2.ContainsKey(int64_1))
                dictionary2.Add(int64_1, int32_2);
            }
          }
        }
        if (dictionary2.Count > 0)
        {
          Dictionary<long, long> dictionary3 = new Dictionary<long, long>(dictionary2.Count);
          List<int> intList1 = new List<int>(dictionary2.Count);
          foreach (int num in dictionary2.Values)
          {
            if (num != -1)
              intList1.Add(num);
          }
          intList1.Sort();
          for (int index = intList1.Count - 1; index > 0; --index)
          {
            if (intList1[index] == intList1[index - 1])
              intList1.RemoveAt(index);
          }
          List<int> intList2 = new List<int>(intList1.Count);
          foreach (int objTypeID in intList1)
          {
            IMSObjectType objectType = MetaDataHelper.GetObjectType(objTypeID);
            if (objectType != null && objectType.IsLocalType)
              intList2.Add(objTypeID);
          }
          if (intList2.Count != 0)
          {
            if (intList2.Count != intList1.Count)
              intList2.AddRange((IEnumerable<int>) ComplectDBObject.ChildType2Remove);
          }
          else
            intList2 = intList1;
          List<int> intList3 = intList2;
          List<ColumnDescriptor> columnDescriptorList = new List<ColumnDescriptor>(2)
          {
            new ColumnDescriptor((object) -2, AttributeSourceTypes.Auto, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0),
            new ColumnDescriptor((object) new Guid(ExpertAttrGUIDs.attrObjectForDoc), AttributeSourceTypes.Auto, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0)
          };
          List<ConditionStructure> conditionStructureList1 = new List<ConditionStructure>(1);
          List<long> longList = new List<long>(dictionary2.Count);
          longList.AddRange((IEnumerable<long>) dictionary2.Keys);
          conditionStructureList1.Add(new ConditionStructure(-2, RelationalOperators.In, (object) longList.ToArray(), LogicalOperators.NONE, 0, false));
          IDBObjectCollection objectCollection = this.Session.GetObjectCollection(-1);
          DBRecordSetParams paramSet = new DBRecordSetParams(conditionStructureList1.ToArray(), columnDescriptorList.ToArray());
          DataTable toTable = (DataTable) null;
          foreach (int num in intList3)
          {
            objectCollection.ObjectTypeID = num;
            DataTable fromTable = objectCollection.Select(paramSet);
            if (toTable != null)
              DataSetProcessor.AddTable(toTable, fromTable, false);
            else
              toTable = fromTable;
          }
          if (toTable != null && toTable.Rows.Count != 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) toTable.Rows)
            {
              if (row[0] != DBNull.Value && row[1] != DBNull.Value)
                dictionary3.Add(Convert.ToInt64(row[0]), Convert.ToInt64(row[1]));
            }
          }
          if (dictionary3.Count > 0)
          {
            List<ConditionStructure> conditionStructureList2 = new List<ConditionStructure>();
            List<long> partIdList = new List<long>(dictionary3.Count);
            partIdList.AddRange((IEnumerable<long>) dictionary3.Keys);
            DataTable parentSostavData = DataHelper.GetParentSostavData((IEnumerable<long>) partIdList, this.Session, (IEnumerable<int>) new int[1]
            {
              -1
            }, false, (IEnumerable<ConditionStructure>) conditionStructureList2.ToArray(), (IEnumerable<ColumnDescriptor>) null);
            if (parentSostavData != null && parentSostavData.Rows.Count > 0)
            {
              Dictionary<long, List<DataRow>> dictionary4 = new Dictionary<long, List<DataRow>>(parentSostavData.Rows.Count);
              int columnIndex5 = parentSostavData.Columns.IndexOf(DataHelper.Consts.cnt_fld_PartObjID);
              int columnIndex6 = parentSostavData.Columns.IndexOf("F_PROJ_ID");
              int columnIndex7 = parentSostavData.Columns.IndexOf("F_OBJECT_TYPE");
              int columnIndex8 = parentSostavData.Columns.IndexOf("F_PRJLINK_ID");
              GenericListHelper.MakeUnique<long>(list);
              List<DataRow> dataRowList;
              foreach (DataRow row in (InternalDataCollectionBase) parentSostavData.Rows)
              {
                long int64_3 = Convert.ToInt64(row[columnIndex8]);
                if (list.BinarySearch(int64_3) < 0)
                {
                  long int64_4 = Convert.ToInt64(row[columnIndex5]);
                  if (!dictionary4.TryGetValue(int64_4, out dataRowList))
                  {
                    dataRowList = new List<DataRow>();
                    dictionary4.Add(int64_4, dataRowList);
                  }
                  dataRowList.Add(row);
                }
              }
              foreach (KeyValuePair<long, long> keyValuePair in dictionary3)
              {
                if (dictionary4.TryGetValue(keyValuePair.Key, out dataRowList))
                {
                  foreach (DataRow dataRow in dataRowList)
                  {
                    long int64 = Convert.ToInt64(dataRow[columnIndex8]);
                    if (Convert.ToInt64(dataRow[columnIndex6]) == Math.Abs(keyValuePair.Value) || dataRowList.Count == 1)
                    {
                      int int32 = Convert.ToInt32(dataRow[columnIndex7]);
                      if (childrenIdRecursive.BinarySearch(int32) < 0 && this.Session.GetRelation(int64, false) is DBRelation relation)
                      {
                        relation.SenderObject = (IDBObject) this;
                        relation.Delete((long) Intermech.Consts.PurgeMode);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
    base.DoDeleteObj_DeleteDownLinks(table);
  }
}
