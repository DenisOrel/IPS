// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.InfoAction
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Portal;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class InfoAction : PortalAction
{
  public char GetSiteCode(Guid sessionGuid, string siteGuid)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetSiteCode siteGuid={siteGuid} sessionGuid={sessionGuid}");
    SiteInfo siteInfo = GuidHelper.IsGuid(siteGuid) ? ServiceUtils.GetService<ISitesCacheService>((object) this.GetUserSession(sessionGuid), true).GetSite(new Guid(siteGuid), true) : throw new ArgumentException();
    if (TraceLog.Enabled)
      TraceLog.Write($"End GetSiteCode siteGuid={siteGuid}");
    return siteInfo.Code;
  }

  public DateTime GetLastSitesInfoUpdate() => DateTime.UtcNow;

  public SiteInfo[] GetSitesInfo(Guid sessionGuid)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetSitesInfo sessionGuid={sessionGuid}");
    try
    {
      IUserSession userSession = this.GetUserSession(sessionGuid);
      SystemTypes filterType = SystemTypes.Unknown;
      if (!((PortalSettings) ServerServices.GetService(typeof (PortalSettings))).SitesSystemTypesIgnore)
        filterType = this.GetSiteInfo(userSession).SystemType;
      return SiteInfoHelper.GetSitesFromDB(userSession, filterType);
    }
    finally
    {
      if (TraceLog.Enabled)
        TraceLog.Write($"End GetSitesInfo sessionGuid={sessionGuid}");
    }
  }
}
