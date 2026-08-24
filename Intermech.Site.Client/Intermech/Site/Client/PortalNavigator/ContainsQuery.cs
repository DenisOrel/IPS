// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.ContainsQuery
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Queries;
using System;
using System.Data;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class ContainsQuery(
  INodeQuerySupport support,
  int objTypeID,
  ConditionStructure[] conditions,
  IServiceProvider services) : ObjectsQuery(support, objTypeID, conditions, services)
{
  private bool _feetchAllRows;

  protected override DBRecordSetParams GetQueryParams(
    object bookmark,
    int count,
    RecordMapping mapping)
  {
    if (count != 2147483646)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        count = Convert.ToInt32(SiteClientConsts.CountRecordsInPackage(sessionKeeper.Session));
      this._feetchAllRows = false;
    }
    else
      this._feetchAllRows = true;
    return base.GetQueryParams(bookmark, count, mapping);
  }

  protected override DataTable GetDataTable(DBRecordSetParams queryParams)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      Guid connectGuid = Guid.Empty;
      IPortalConnector customService = (IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector));
      try
      {
        connectGuid = customService.Login(sessionKeeper.Session.SessionGUID);
        PublishObjectsTable publishObjectsTable = customService.SelectPublishObjects(connectGuid, this._objTypeID, DBQueryParams.FormingParams(queryParams));
        DataTable dataTable = new DataTable(publishObjectsTable.Name);
        for (int index = 0; index < publishObjectsTable.Columns.Length; ++index)
          dataTable.Columns.Add(new DataColumn(publishObjectsTable.Columns[index].Name, Helper.GetType((TypeCode) publishObjectsTable.Columns[index].TypeCode)));
        for (int index1 = 0; index1 < publishObjectsTable.Rows.Length; ++index1)
        {
          DataRow row = dataTable.NewRow();
          for (int index2 = 0; index2 < publishObjectsTable.Columns.Length; ++index2)
          {
            object obj = publishObjectsTable[index1][index2];
            row[index2] = obj ?? (object) DBNull.Value;
          }
          dataTable.Rows.Add(row);
        }
        dataTable.AcceptChanges();
        if (this._feetchAllRows || (long) dataTable.Rows.Count < SiteClientConsts.CountRecordsInPackage(sessionKeeper.Session))
          dataTable.ExtendedProperties[(object) "Eof"] = (object) true;
        else
          dataTable.ExtendedProperties[(object) "Eof"] = (object) false;
        return dataTable;
      }
      finally
      {
        if (connectGuid != Guid.Empty && customService != null)
          customService.Logout(connectGuid);
      }
    }
  }
}
