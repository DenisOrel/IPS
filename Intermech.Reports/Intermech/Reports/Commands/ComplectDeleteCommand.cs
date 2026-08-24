// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Commands.ComplectDeleteCommand
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Reports;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.ContextCommands;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Reports.Commands;

/// <summary>Выполнение команды меню "Удалить"</summary>
internal class ComplectDeleteCommand : ReportBaseCommand
{
  /// <summary>
  /// 
  /// </summary>
  protected override void DoExecute_Command()
  {
    bool flag = false;
    ColumnDescriptor[] columns = new ColumnDescriptor[2]
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Auto, ColumnContents.Text, ColumnNameMapping.FieldName, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MetaDataHelper.GetAttributeID((object) "cad0036f-306c-11d8-b4e9-00304f19f545"), AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.Guid, SortOrders.NONE, 0)
    };
    DataTable dataTable;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      dataTable = ServiceUtils.GetService<ICompositionLoadService>((object) sessionKeeper.Session, true).LoadComplexCompositions((object) sessionKeeper.Session.SessionGUID, (IEnumerable<ObjInfoItem>) this._objInfoList, (IEnumerable<int>) new int[1]
      {
        ReportsConsts.SimpleWithSortRelationID
      }, (IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(ReportsConsts.DocumentBaseTypeID), (IEnumerable<ColumnDescriptor>) columns, true, false, (VersionsRule) null, (IEnumerable<ConditionStructure>) null, string.Empty, (Dictionary<long, HybridDictionary>) null, -1);
    if (dataTable != null)
    {
      int columnIndex = dataTable.Columns.IndexOf("cad0036f-306c-11d8-b4e9-00304f19f545");
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        if (row[columnIndex] != DBNull.Value && Convert.ToInt64(row[columnIndex]) > 0L)
        {
          flag = true;
          break;
        }
      }
    }
    if (flag && MessageBox.Show(LocalizationHolder.rm.GetString("Reports_65"), LocalizationHolder.rm.GetString("Reports_46"), MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK)
      return;
    ObjectCommands.DeleteCommand(this._items, this._viewServices, this._additionalInfo);
  }
}
