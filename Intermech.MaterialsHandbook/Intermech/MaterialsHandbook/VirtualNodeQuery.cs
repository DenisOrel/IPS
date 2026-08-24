// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.VirtualNodeQuery
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class VirtualNodeQuery(
  INodeQuerySupport support,
  int objTypeID,
  long objId,
  RelatedObjectsRole role,
  int relTypeId,
  ConditionStructure[] conditions) : RelatedObjectsQuery(support, objId, objTypeID, role, relTypeId, conditions)
{
  protected override DataTable GetDataTable(DBRecordSetParams queryParams)
  {
    return ((VirtualNodeObjectPart) this.Support).ParentCategoryID != Consts.IMHStandardNodeCategoryID ? (((VirtualNodeObjectPart) this.Support).CategoryID != Consts.IMHDetailsMaterialNodeCategoryID ? base.GetDataTable(queryParams) : this.GetDetailsMaterialDataTable(queryParams)) : this.GetStandartDataTable(queryParams);
  }

  protected override DBRecordSetParams GetQueryParams(
    object bookmark,
    int count,
    RecordMapping mapping)
  {
    DBRecordSetParams queryParams = new DBRecordSetParams();
    if (((VirtualNodeObjectPart) this.Support).ParentCategoryID == Consts.IMHStandardNodeCategoryID)
    {
      string conditionValue = string.Empty;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(this.objId, false);
        if (objectActualCopy != null)
        {
          IDBAttribute attributeById = objectActualCopy.GetAttributeByID(Intermech.Imbase.Consts.ClassifFolderKeyAttId);
          conditionValue = attributeById != null ? attributeById.AsString : string.Empty;
        }
      }
      if (!string.IsNullOrEmpty(conditionValue))
      {
        int attributeID = ((VirtualNodeObjectPart) this.Support).CategoryID == Consts.IMHMaterialsNodeCategoryID ? Intermech.Imbase.Consts.StandartAttrID : Intermech.Imbase.Consts.StandartAssortmentAttrID;
        queryParams = new DBRecordSetParams(new ConditionStructure[2]
        {
          new ConditionStructure(Intermech.Imbase.Consts.ClassifFolderKeyAttId, RelationalOperators.StartString, (object) conditionValue, LogicalOperators.AND, 0, false),
          new ConditionStructure(attributeID, RelationalOperators.NotEmpty, (object) null, LogicalOperators.NONE, 0, false)
        })
        {
          Columns = new object[mapping.Fields.Length],
          ColumnsInfo = new Intermech.Kernel.Search.ColumnInfo[mapping.Fields.Length],
          ColumnNames = new ColumnNameMapping[mapping.Fields.Length]
        };
        for (int index = 0; index < queryParams.Columns.Length; ++index)
        {
          queryParams.Columns[index] = ((NodeColumnID) mapping.Fields[index]).ID;
          queryParams.ColumnsInfo[index].AttributeSource = ((NodeColumnID) mapping.Fields[index]).AttrSource;
          queryParams.ColumnsInfo[index].AttributeID = ((NodeColumnID) mapping.Fields[index]).ID;
          queryParams.ColumnNames[index] = ColumnNameMapping.Index;
        }
      }
    }
    else
      queryParams = base.GetQueryParams(bookmark, count, mapping);
    return queryParams;
  }

  private DataTable GetStandartDataTable(DBRecordSetParams queryParams)
  {
    DataTable standartDataTable;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable dataTable = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeID)?.Select(queryParams);
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return (DataTable) null;
      standartDataTable = dataTable.Clone();
      int columnIndex1 = Array.IndexOf<object>(queryParams.Columns, (object) ObligatoryObjectAttributes.F_OBJECT_TYPE);
      int columnIndex2 = Array.IndexOf<object>(queryParams.Columns, (object) ObligatoryObjectAttributes.CAPTION);
      int columnIndex3 = Array.IndexOf<object>(queryParams.Columns, (object) ObligatoryObjectAttributes.F_OBJECT_ID);
      int columnIndex4 = ((VirtualNodeObjectPart) this.Support).CategoryID == Consts.IMHMaterialsNodeCategoryID ? Array.IndexOf<object>(queryParams.Columns, (object) Intermech.Imbase.Consts.StandartAttrID) : Array.IndexOf<object>(queryParams.Columns, (object) Intermech.Imbase.Consts.StandartAssortmentAttrID);
      int columnIndex5 = Array.IndexOf<object>(queryParams.Columns, (object) Intermech.Imbase.Consts.ClassAttrID);
      Dictionary<string, int> dictionary = new Dictionary<string, int>(dataTable.Rows.Count);
      if (columnIndex5 > -1)
      {
        foreach (DataRow row1 in (InternalDataCollectionBase) dataTable.Rows)
        {
          string key = $"{row1[columnIndex4]} ({row1[columnIndex5]})";
          object obj = row1[columnIndex3];
          if (!dictionary.ContainsKey(key))
          {
            dictionary.Add(key, Intermech.Imbase.Consts.ImbaseFolderTypeID);
            DataRow row2 = standartDataTable.NewRow();
            row2.ItemArray = row1.ItemArray;
            row2[columnIndex1] = (object) Intermech.Imbase.Consts.ImbaseFolderTypeID;
            row2[columnIndex3] = obj;
            row2[columnIndex2] = (object) key;
            row2[columnIndex4] = (object) Convert.ToString(row1[columnIndex4]);
            standartDataTable.Rows.Add(row2);
          }
        }
      }
      else
      {
        foreach (DataRow row3 in (InternalDataCollectionBase) dataTable.Rows)
        {
          string key = Convert.ToString(row3[columnIndex4]);
          object obj = row3[columnIndex3];
          if (!dictionary.ContainsKey(key))
          {
            dictionary.Add(key, Intermech.Imbase.Consts.ImbaseFolderTypeID);
            DataRow row4 = standartDataTable.NewRow();
            row4.ItemArray = row3.ItemArray;
            row4[columnIndex1] = (object) Intermech.Imbase.Consts.ImbaseFolderTypeID;
            row4[columnIndex3] = obj;
            row4[columnIndex2] = (object) key;
            row4[columnIndex4] = (object) key;
            standartDataTable.Rows.Add(row4);
          }
        }
      }
    }
    return standartDataTable;
  }

  private DataTable GetDetailsMaterialDataTable(DBRecordSetParams queryParams)
  {
    DataTable materialDataTable = new DataTable();
    int columnIndex1 = Array.IndexOf<object>(queryParams.Columns, (object) ObligatoryObjectAttributes.F_OBJECT_NAME);
    int columnIndex2 = Array.IndexOf<object>(queryParams.Columns, (object) ObligatoryObjectAttributes.F_OBJECT_TYPE);
    int num = Array.IndexOf<object>(queryParams.Columns, (object) ObligatoryObjectAttributes.F_OBJECT_ID);
    int columnIndex3 = Array.IndexOf<object>(queryParams.Columns, (object) ObligatoryObjectAttributes.CAPTION);
    materialDataTable.Columns.Add(Convert.ToString(columnIndex1));
    materialDataTable.Columns.Add(Convert.ToString(columnIndex2));
    materialDataTable.Columns.Add(Convert.ToString(num));
    materialDataTable.Columns.Add(Convert.ToString(columnIndex3));
    DataTable dataTable = (DataTable) null;
    DataSet imbaseDs = IMHHelper.GetImbaseDS("MATERIAL_GROUPS_TABLE_NAME");
    if (imbaseDs != null && imbaseDs.Tables.Contains("IMS_DATA"))
      dataTable = imbaseDs.Tables["IMS_DATA"];
    if (dataTable == null)
      return materialDataTable;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
      {
        Guid objectGuidByName = customService.GetObjectGuidByName("MATERIAL_GROUPS_TABLE_NAME");
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(objectGuidByName);
        string str = Convert.ToString((object) customService.GetObjectGuidByName("MATERIAL_GROUPS_COLUMN_NAME"));
        if (!dataTable.Columns.Contains(str))
          return materialDataTable;
        foreach (DataRow row1 in (InternalDataCollectionBase) dataTable.Rows)
        {
          DataRow row2 = materialDataTable.NewRow();
          row2[columnIndex1] = (object) ImbaseHelper.MakeInternalImbaseKey(objectInfo.ObjectID, Convert.ToInt64(row1["F_KEY"]));
          row2[columnIndex2] = (object) Intermech.Imbase.Consts.ImbaseFolderTypeID;
          row2[columnIndex3] = row1[str];
          materialDataTable.Rows.Add(row2);
        }
      }
    }
    return materialDataTable;
  }
}
