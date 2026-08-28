// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.AnalyzInfo
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Portal.Server;

internal class AnalyzInfo
{
  public long ID;
  public Guid GUID;
  public bool PublishEnable;
  public List<Guid> LinkGuids;

  public AnalyzInfo(long id, Guid guid, bool publishEnable)
  {
    this.ID = id;
    this.GUID = guid;
    this.PublishEnable = publishEnable;
    this.LinkGuids = new List<Guid>(0);
  }
}
