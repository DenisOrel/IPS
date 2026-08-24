// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.FolderQuery
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class FolderQuery(
  INodeQuerySupport support,
  long objId,
  int objTypeID,
  RelatedObjectsRole role,
  int relTypeId,
  ConditionStructure[] conditions) : RelatedObjectsQuery(support, objId, objTypeID, role, relTypeId, conditions)
{
  protected override DataTable GetDataTable(DBRecordSetParams queryParams)
  {
    DataTable dt = base.GetDataTable(queryParams);
    if (dt != null && ((FolderObjectPart) this.Support).ParentCategoryID == Consts.IMHMaterialsNodeCategoryID)
      dt = this.ExcludeTableRefs(dt, queryParams);
    return dt;
  }

  protected override DBRecordSetParams GetQueryParams(object[] recordIds, RecordMapping mapping)
  {
    return base.GetQueryParams(recordIds, mapping);
  }

  private DataTable ExcludeTableRefs(DataTable dt, DBRecordSetParams queryParams)
  {
    DataTable dataTable = (DataTable) null;
    int columnIndex1 = Array.IndexOf<object>(queryParams.Columns, (object) ObligatoryObjectAttributes.F_OBJECT_TYPE);
    int columnIndex2 = Array.IndexOf<object>(queryParams.Columns, (object) ObligatoryObjectAttributes.F_OBJECT_ID);
    if (columnIndex1 > -1 && columnIndex2 > -1)
    {
      dataTable = dt.Clone();
      List<long> list = new List<long>(dt.Rows.Count);
      foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
      {
        if (Convert.ToInt32(row[columnIndex1]) != Intermech.Imbase.Consts.ImbaseTableRefTypeID)
          dataTable.Rows.Add(row.ItemArray);
        else
          list.Add(Convert.ToInt64(row[columnIndex2]));
      }
      (this.Support as FolderObjectPart).SetTableRefs(list);
    }
    return dataTable ?? dt;
  }
}
