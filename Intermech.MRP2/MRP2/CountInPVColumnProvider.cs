// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.CountInPVColumnProvider
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Client.Core.Navigator.Classes.Providers;
using Intermech.Extensions;
using Intermech.Interfaces.Compositions;
using Intermech.Kernel.Search;
using Intermech.Navigator.Data;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualColumns;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.MRP2;

internal class CountInPVColumnProvider : INavigatorVirtualColumnProvider, ISpecialFieldsSupported
{
  private List<ObjInfoItem> objInfoList;
  internal static readonly VirtualQueryResultColumn VirtualColumnCountPList = new VirtualQueryResultColumn("F_COUNT_PLIST", typeof (NodeDelayedValue));

  public CountInPVColumnProvider(List<ObjInfoItem> objInfoList) => this.objInfoList = objInfoList;

  public DataTable GetDataTable(INodeQuery nodeQuery, NavigatorVirtualColumnProviderArgs args)
  {
    VirtualQueryResultColumn.AddVirtualColumns(args.SourceTable, args.Mapping, (System.Func<VirtualQueryResultColumn, object>) (virtualColumn => (object) NodeDelayedValue.EmptyValue));
    this.UpdateCounts(args);
    return args.SourceTable;
  }

  public List<object> GetSpecialFields()
  {
    return new List<object>()
    {
      (object) CountInPVColumnProvider.VirtualColumnCountPList
    };
  }

  public object MapColumnToField(INodeItems nodeItems, NodeColumn column)
  {
    return column.ID.Equals((object) MRP2Consts.attrIdCountForPL) ? (object) CountInPVColumnProvider.VirtualColumnCountPList : (object) null;
  }

  public void UpdateCounts(NavigatorVirtualColumnProviderArgs args)
  {
    int columnIndex1 = ((IEnumerable<object>) args.Mapping.Fields).IndexOfFirst<object>((Predicate<object>) (item => item == CountInPVColumnProvider.VirtualColumnCountPList));
    if (columnIndex1 == -1)
      return;
    int columnIndex2 = ((IEnumerable<object>) args.Mapping.Fields).IndexOfFirst<object>((Predicate<object>) (item => item is NodeColumnID nodeColumnId && nodeColumnId.ID.Equals((object) ObligatoryObjectAttributes.F_OBJECT_ID)));
    if (columnIndex2 == -1)
      return;
    foreach (DataRow row in (InternalDataCollectionBase) args.SourceTable.Rows)
    {
      if (row[columnIndex1] is NodeDelayedValue nodeDelayedValue)
        nodeDelayedValue.Value = row[columnIndex2];
    }
  }
}
