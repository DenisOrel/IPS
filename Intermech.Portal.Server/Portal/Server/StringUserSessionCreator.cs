// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.StringUserSessionCreator
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Kernel;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class StringUserSessionCreator : UserSessionCreator<string>
{
  protected override void OnLogin(
    UserSession session,
    string login,
    string password,
    string computerName,
    TimeSpan timeZoneOffset)
  {
    session.Login(login, password, computerName, timeZoneOffset, 0L, this.sessionName);
  }
}
