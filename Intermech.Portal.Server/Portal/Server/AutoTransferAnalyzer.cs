// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.AutoTransferAnalyzer
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class AutoTransferAnalyzer : UnitAnalyzer
{
  private readonly string _enabledSites;

  public AutoTransferAnalyzer(
    IUserSession session,
    ISitesCacheService cacheService,
    SiteInfo info,
    TransferedObject unit,
    XmlNode rootNode,
    string enabledSites)
    : base(session, cacheService, info, unit, rootNode, true)
  {
    this._enabledSites = enabledSites;
  }

  public override void Analysis(
    IDBObjectCollection publishObjects,
    List<Guid> importedObjects,
    PackAnalyzInfo packAnalyzInfo,
    Dictionary<Guid, int> partCounter)
  {
    if (this._enabledSites.Length == 0)
      throw new Exception(LocalizationHolder.rm.GetString("PortalServer_32"));
    foreach (char enabledSite in this._enabledSites)
    {
      if (!enabledSite.Equals(this.info.Code) && this.SiteForUpdate.IndexOf(enabledSite) < 0)
      {
        if (this.cacheService.GetSite(enabledSite) == null)
          throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_33"), (object) enabledSite));
        this.SiteForUpdate += enabledSite.ToString();
      }
    }
  }
}
