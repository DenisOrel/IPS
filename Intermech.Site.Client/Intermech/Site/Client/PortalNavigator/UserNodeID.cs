// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.UserNodeID
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class UserNodeID : INodeID, IUserNodeID
{
  private object _cookie;
  private long _userID;
  private string _userName;
  private Guid _siteGuid;

  public UserNodeID(long userId, string userName, Guid siteGuid)
  {
    this._userID = userId;
    this._userName = userName;
    this._siteGuid = siteGuid;
  }

  public int CategoryID
  {
    [DebuggerStepThrough] get => SiteClientConsts.CategoryUserNode;
  }

  public int TypeID
  {
    [DebuggerStepThrough] get => 0;
  }

  public object Cookie
  {
    [DebuggerStepThrough] get => this._cookie;
    [DebuggerStepThrough] set => this._cookie = value;
  }

  public long UserID => this._userID;

  public string UserName => this._userName;

  public Guid SiteGuid => this._siteGuid;
}
