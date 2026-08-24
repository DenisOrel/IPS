// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Helper
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;

#nullable disable
namespace Intermech.Site.Client;

internal static class Helper
{
  private static bool _initialized = false;
  public static long SiteID = 0;
  public static Guid SiteGuid = Guid.Empty;
  public static char? SiteCode = new char?();

  public static bool Initialized
  {
    get
    {
      if (!Helper._initialized)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
          Helper.Init(sessionKeeper.Session);
      }
      return Helper._initialized;
    }
    set => Helper._initialized = value;
  }

  public static void Init(IUserSession session)
  {
    ISitesCacheService customService = (ISitesCacheService) session.GetCustomService(typeof (ISitesCacheService));
    if (customService.Info == null)
      return;
    Helper.SiteID = customService.Info.ID;
    Helper.SiteGuid = customService.Info.GUID;
    Helper.SiteCode = new char?(customService.Info.Code);
    Helper.Initialized = true;
  }

  public static bool CheckAccess(ActionType actionType)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetSystemSecurity().CheckAccess(actionType);
  }
}
