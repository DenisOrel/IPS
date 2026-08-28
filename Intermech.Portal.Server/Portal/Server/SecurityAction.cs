// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.SecurityAction
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Protection;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class SecurityAction : PortalAction
{
  public string Login(
    string login,
    PswPackage password,
    string siteGUID,
    string computerName,
    int timeZone)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start Login with PswPackage login={login}");
    return this.OnAfterLogin(new PswPackageUserSessionCreator().Create(login, password, computerName, timeZone), login, siteGUID);
  }

  public string Login(
    string login,
    string password,
    string siteGUID,
    string computerName,
    int timeZone)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start Login login={login}");
    return this.OnAfterLogin(new StringUserSessionCreator().Create(login, password, computerName, timeZone), login, siteGUID);
  }

  private string OnAfterLogin(UserSession session, string login, string siteGUID)
  {
    SiteInfo site = ServiceUtils.GetService<ISitesCacheService>((object) session, true).GetSite(new Guid(siteGUID), true);
    if (session.GetRelation(site.ID, session.UserID, true) == null)
      throw new LoginException($"Пользователь с логином '{login}' не является пользователем информационной системы '{site.Caption}'");
    session.SetSessionPluginsData((object) ActionsHelper.SiteInfoKeyForSession, (object) site);
    if (TraceLog.Enabled)
      TraceLog.Write($"Start Login login={login}");
    return session.SessionGUID.ToString();
  }

  public void Logout(Guid sessionGuid)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start LogOut sessionGuid={sessionGuid}");
    this.GetUserSession(sessionGuid)?.Logout("PortalServer");
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"End LogOut sessionGuid={sessionGuid}");
  }

  public IUserSession GetSession(Guid sessionGuid)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetSession sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    if (userSession == null)
      throw new Exception($"Сессия {sessionGuid} не найдена");
    if (!TraceLog.Enabled)
      return userSession;
    TraceLog.Write($"End GetSession sessionGuid={sessionGuid}");
    return userSession;
  }
}
