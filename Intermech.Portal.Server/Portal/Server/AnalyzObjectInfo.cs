// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.AnalyzObjectInfo
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using System;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class AnalyzObjectInfo : AnalyzInfo
{
  public bool Deleted;
  public bool InComposition;
  public bool WithComposition;

  public AnalyzObjectInfo(long id, Guid guid, bool publishEnable)
    : base(id, guid, publishEnable)
  {
    this.InComposition = true;
    this.WithComposition = false;
    this.Deleted = false;
  }
}
