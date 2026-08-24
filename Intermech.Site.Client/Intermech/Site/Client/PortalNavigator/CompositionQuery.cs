// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.CompositionQuery
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

internal sealed class CompositionQuery : ObjectsQuery
{
  private long _objId;

  public CompositionQuery(
    INodeQuerySupport support,
    int objTypeID,
    long objId,
    IServiceProvider services)
    : base(support, objTypeID, (ConditionStructure[]) null, services)
  {
    this._objId = objId;
  }

  protected override DBRecordSetParams GetQueryParams(
    object bookmark,
    int count,
    RecordMapping mapping)
  {
    return base.GetQueryParams(bookmark, 2147483646, mapping);
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
        queryParams.RecordCount = -1;
        return Helper.ConvertToDataTable(customService.SelectComposition(sessionKeeper.Session.SessionGUID, connectGuid, this._objId, DBQueryParams.FormingParams(queryParams), 1));
      }
      finally
      {
        if (connectGuid != Guid.Empty && customService != null)
          customService.Logout(connectGuid);
      }
    }
  }
}
