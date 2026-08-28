// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.UserSessionCreator`1
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Kernel;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal abstract class UserSessionCreator<TPassword>
{
  protected readonly string sessionName = "PortalServer";

  public UserSession Create(string login, TPassword password, string computerName, int timeZone)
  {
    UserSession session = new UserSession();
    this.OnLogin(session, login, password, computerName, new TimeSpan(timeZone, 0, 0));
    return session;
  }

  protected abstract void OnLogin(
    UserSession session,
    string login,
    TPassword password,
    string computerName,
    TimeSpan timeZoneOffset);
}
