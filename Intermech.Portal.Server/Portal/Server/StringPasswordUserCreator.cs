// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.StringPasswordUserCreator
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class StringPasswordUserCreator : UserCreator<string>
{
  protected override long CreateUser(
    ISiteServerService service,
    IUserSession session,
    string userName,
    string login,
    string password,
    Guid userGuid,
    char siteCode)
  {
    return service.AddUser((object) session, userName, login, password, userGuid, siteCode);
  }

  protected override void SetPassword(
    ISiteServerService service,
    IUserSession session,
    string login,
    string newPassword)
  {
    service.ChangeUserPassword((object) session, login, newPassword);
  }
}
