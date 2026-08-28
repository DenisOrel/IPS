// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PackAnalyzInfo
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Portal.Server;

internal class PackAnalyzInfo
{
  public Dictionary<Guid, Intermech.Portal.Server.AnalyzObjectInfo> AnalyzObjectInfo;
  public List<Tuple<Guid, Guid, Guid, Intermech.Portal.Server.AnalyzRelationInfo>> AnalyzRelationInfo;
  public bool IsAutoTransfer;
  public string SiteForUpdate = string.Empty;

  public PackAnalyzInfo()
  {
    this.AnalyzObjectInfo = new Dictionary<Guid, Intermech.Portal.Server.AnalyzObjectInfo>();
    this.AnalyzRelationInfo = new List<Tuple<Guid, Guid, Guid, Intermech.Portal.Server.AnalyzRelationInfo>>();
  }

  public void Destroy()
  {
    this.AnalyzObjectInfo = (Dictionary<Guid, Intermech.Portal.Server.AnalyzObjectInfo>) null;
    this.AnalyzRelationInfo = (List<Tuple<Guid, Guid, Guid, Intermech.Portal.Server.AnalyzRelationInfo>>) null;
  }
}
