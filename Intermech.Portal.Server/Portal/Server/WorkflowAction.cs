// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.WorkflowAction
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class WorkflowAction : PortalAction
{
  public ProcessTemplateInfo[] GetProcessTemplates(Guid sessionGuid, Guid siteGuid)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetProcessTemplates siteGuid={siteGuid} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    IDBObjectCollection objectCollection = userSession.GetObjectCollection(PortalConsts.objtypePublishProcessesTemplates);
    ConditionStructure[] conditions = ConditionStructure.Join(new ConditionStructure[2]
    {
      new ConditionStructure(PortalConsts.attributeOwner, RelationalOperators.Equal, (object) (((ISitesCacheService) userSession.GetCustomService(typeof (ISitesCacheService))).GetSite(siteGuid) ?? throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_60"), (object) siteGuid))).Code.ToString(), LogicalOperators.AND, 0),
      new ConditionStructure(-16, RelationalOperators.Equal, (object) 1, LogicalOperators.AND, 0, false)
    }, ActionsHelper.GetConditionOnEnabledObjects(userSession));
    ColumnDescriptor[] columns = new ColumnDescriptor[2]
    {
      new ColumnDescriptor((object) ActionsHelper.GetAttributeTypeID(userSession, PortalConsts.attributePublishObjectGUID), SortOrders.NONE, 0),
      new ColumnDescriptor((object) ActionsHelper.GetAttributeTypeID(userSession, new Guid("cad002c3-306c-11d8-b4e9-00304f19f545")), SortOrders.NONE, 0)
    };
    DataTable dataTable = objectCollection.Select(new DBRecordSetParams(conditions, columns));
    if (dataTable.Rows.Count > 0)
    {
      List<ProcessTemplateInfo> processTemplateInfoList = new List<ProcessTemplateInfo>(dataTable.Rows.Count);
      for (int index = 0; index < dataTable.Rows.Count; ++index)
        processTemplateInfoList.Add(new ProcessTemplateInfo(new Guid(Convert.ToString(dataTable.Rows[index][0])), Convert.ToString(dataTable.Rows[index][1])));
      if (TraceLog.Enabled)
        TraceLog.Write($"End GetProcessTemplates site={siteInfo.Code} siteGuid={siteGuid}");
      return processTemplateInfoList.ToArray();
    }
    if (TraceLog.Enabled)
      TraceLog.Write($"End GetProcessTemplates EMPTY site={siteInfo.Code} siteGuid={siteGuid}");
    return (ProcessTemplateInfo[]) null;
  }
}
