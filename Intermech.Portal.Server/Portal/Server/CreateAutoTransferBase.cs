// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.CreateAutoTransferBase
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Portal.Server;

internal abstract class CreateAutoTransferBase
{
  protected IUserSession session;
  protected SiteInfo info;
  protected PackAnalyzInfo packAnalyzInfo;

  public CreateAutoTransferBase(IUserSession session, SiteInfo info, PackAnalyzInfo packAnalyzInfo)
  {
    this.session = session;
    this.packAnalyzInfo = packAnalyzInfo;
    this.info = info;
  }

  public static void Create(
    IUserSession session,
    SiteInfo info,
    PackAnalyzInfo packAnalyzInfo,
    GroupPublishItem packet,
    List<TransferedObject> trObjects)
  {
    (packet == null || !(packet is PublishPacket) ? (CreateAutoTransferBase) new CreateAutoTransferObjects(session, info, packAnalyzInfo, trObjects) : (CreateAutoTransferBase) new CreateAutoTransferPacket(session, info, packAnalyzInfo, packet.DBObject.ObjectID)).OnCreate();
  }

  public abstract void OnCreate();

  protected long[] RecipientIDs => this.GetSiteIDs(this.packAnalyzInfo.SiteForUpdate);

  protected long[] GetSiteIDs(string siteCodes)
  {
    ISitesCacheService customService = (ISitesCacheService) this.session.GetCustomService(typeof (ISitesCacheService));
    List<long> longList = new List<long>(siteCodes.Length);
    for (int index = 0; index < siteCodes.Length; ++index)
    {
      SiteInfo site = customService.GetSite(siteCodes[index]);
      longList.Add(site.ID);
    }
    return longList.ToArray();
  }
}
