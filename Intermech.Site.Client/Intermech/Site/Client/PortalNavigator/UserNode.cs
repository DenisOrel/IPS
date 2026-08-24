// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.UserNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class UserNode : CompositeNode, IContextAware, INodeNotifications
{
  private long _userID;
  private IServiceProvider _services;

  public UserNode(long userID) => this._userID = userID;

  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._services;
    set => this._services = value;
  }

  public ProcessResult Process(NotificationEventArgs e, object AdditionalInfo)
  {
    return ProcessResult.None;
  }
}
