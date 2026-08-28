// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.CreateAutoTransferPacket
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal class CreateAutoTransferPacket : CreateAutoTransferBase
{
  private readonly long _packetID;

  public CreateAutoTransferPacket(
    IUserSession session,
    SiteInfo info,
    PackAnalyzInfo packAnalyzInfo,
    long packetID)
    : base(session, info, packAnalyzInfo)
  {
    this._packetID = packetID;
  }

  public override void OnCreate()
  {
    if (TraceLog.Enabled)
      TraceLog.Write("Start create update packet (packet)");
    new PacketAction().ImportPackets(this.session, this.info, Guid.NewGuid(), new long[1]
    {
      this._packetID
    }, this.RecipientIDs);
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write("Update packet created (packet)");
  }
}
