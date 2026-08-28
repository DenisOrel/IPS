// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.SiteCodeHelper
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal static class SiteCodeHelper
{
  public static char? GetSiteCode(IDBObject obj, Guid attributeGuid)
  {
    IDBAttribute attributeByGuid = obj.GetAttributeByGuid(attributeGuid);
    return attributeByGuid != null && !string.IsNullOrEmpty(attributeByGuid.AsString) ? new char?(attributeByGuid.AsString[0]) : new char?();
  }
}
