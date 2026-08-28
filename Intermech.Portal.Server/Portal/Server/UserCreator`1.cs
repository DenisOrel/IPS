// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.UserCreator`1
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using System;
using System.Data;

#nullable disable
namespace Intermech.Portal.Server;

internal abstract class UserCreator<TPasswordType>
{
  public void ChangeUserPassword(IUserSession session, string login, TPasswordType newPassword)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start ChangeUserPassword login={login} sessionGuid={session.SessionGUID}");
    SiteInfo sessionPluginsData = (SiteInfo) session.GetSessionPluginsData((object) ActionsHelper.SiteInfoKeyForSession);
    UserAction.CheckPresentUser(session, sessionPluginsData, login);
    this.SetPassword((ISiteServerService) session.GetCustomService(typeof (ISiteServerService)), session, login, newPassword);
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"End ChangeUserPassword info={sessionPluginsData.Code} login={login}");
  }

  public void AddUser(
    IUserSession session,
    string userName,
    string login,
    TPasswordType password,
    Guid userGuid)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start AddUser login={login} sessionGuid={session.SessionGUID}");
    SiteInfo sessionPluginsData = (SiteInfo) session.GetSessionPluginsData((object) ActionsHelper.SiteInfoKeyForSession);
    ISiteServerService customService = (ISiteServerService) session.GetCustomService(typeof (ISiteServerService));
    ConditionStructure conditionStructure1 = new ConditionStructure(new Guid("cad00018-306c-11d8-b4e9-00304f19f545"), RelationalOperators.Equal, (object) login, LogicalOperators.AND, 0);
    ConditionStructure conditionStructure2 = new ConditionStructure(0, RelationalOperators.EntersIn, (object) sessionPluginsData.ID, LogicalOperators.AND, 0, false);
    DataTable dataTable = session.GetObjectCollection(session.IdentHelper.UsersTypeID).Select(new DBRecordSetParams(new ConditionStructure[2]
    {
      conditionStructure1,
      conditionStructure2
    }, new object[1]{ (object) -2 }));
    if (dataTable.Rows.Count != 0)
    {
      IDBAttribute attributeByGuid = session.GetObject(Convert.ToInt64(dataTable.Rows[0][0])).GetAttributeByGuid(new Guid("cad0001d-306c-11d8-b4e9-00304f19f545"));
      if (attributeByGuid.AsString != userName)
        attributeByGuid.AsString = userName;
      this.ChangeUserPassword(session, login, password);
    }
    else
    {
      long user = this.CreateUser(customService, session, userName, login, password, userGuid, sessionPluginsData.Code);
      IDBRelationCollection relationCollection = session.GetRelationCollection(session.IdentHelper.SimpleRelationTypeID);
      relationCollection.Create(sessionPluginsData.ID, user);
      relationCollection.Create(session.GetObjectInfo(PortalConsts.objectReplicatorRole).ObjectID, user);
    }
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"End AddUser info={sessionPluginsData.Code} login={login}");
  }

  protected abstract long CreateUser(
    ISiteServerService service,
    IUserSession session,
    string userName,
    string login,
    TPasswordType password,
    Guid userGuid,
    char siteCode);

  protected abstract void SetPassword(
    ISiteServerService service,
    IUserSession session,
    string login,
    TPasswordType newPassword);
}
