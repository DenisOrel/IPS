// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PortalAction
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal class PortalAction
{
  protected IUserSession GetUserSession(Guid sessionGuid)
  {
    IUserSession sessionById;
    ((IServerSession) (sessionById = UserSession.GetSessionByID(sessionGuid))).CheckLogin();
    return sessionById;
  }

  protected SiteInfo GetSiteInfo(IUserSession session)
  {
    return (SiteInfo) session.GetSessionPluginsData((object) ActionsHelper.SiteInfoKeyForSession);
  }
}
