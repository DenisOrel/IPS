// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.IUserNodeID
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal interface IUserNodeID
{
  long UserID { get; }

  string UserName { get; }

  Guid SiteGuid { get; }
}
