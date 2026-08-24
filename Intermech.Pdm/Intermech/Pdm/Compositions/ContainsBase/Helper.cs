// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ContainsBase.Helper
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Kernel.Search;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Pdm.Compositions.ContainsBase;

internal static class Helper
{
  public static List<ColumnDescriptor> FormingColumns(
    RecordMapping mapping,
    params AttributeSourceTypes[] sourceTypes)
  {
    List<ColumnDescriptor> columnDescriptorList = new List<ColumnDescriptor>();
    foreach (NodeColumnID field in mapping.Fields)
    {
      if (field != null && (sourceTypes.Length != 0 && ((IEnumerable<AttributeSourceTypes>) sourceTypes).Contains<AttributeSourceTypes>(field.AttrSource) || sourceTypes.Length == 0))
      {
        ColumnDescriptor columnDescriptor = new ColumnDescriptor();
        columnDescriptor.AttributeID = (object) field.AttributeID;
        columnDescriptor.AttributeSource = field.AttrSource;
        columnDescriptor.Contents = ColumnContents.Text;
        columnDescriptor.ColumnName = ColumnNameMapping.Index;
        int index = mapping.SortFields != null ? Array.IndexOf<object>(mapping.SortFields, (object) field) : -1;
        if (index >= 0)
        {
          columnDescriptor.OrderByID = index;
          columnDescriptor.Sort = (SortOrders) Convert.ToInt32((object) mapping.SortOrders[index]);
        }
        else
        {
          columnDescriptor.Sort = SortOrders.NONE;
          columnDescriptor.OrderByID = -1;
        }
        columnDescriptorList.Add(columnDescriptor);
      }
    }
    return columnDescriptorList;
  }
}
