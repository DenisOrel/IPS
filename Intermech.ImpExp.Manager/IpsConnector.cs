// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.IpsConnector
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.Client.Specialized;

#nullable disable
namespace Intermech.ImpExp.Manager;

public static class IpsConnector
{
  private static ConverterClientApplicationHost _clientApplicationHost;

  public static void Connect(string loginName, string password, string userRole)
  {
    if (IpsConnector._clientApplicationHost == null)
    {
      IpsConnector._clientApplicationHost = new ConverterClientApplicationHost(loginName, password, userRole);
    }
    else
    {
      IpsConnector._clientApplicationHost.LoginInfo.LoginName = loginName;
      IpsConnector._clientApplicationHost.LoginInfo.Password = password;
      IpsConnector._clientApplicationHost.LoginInfo.RoleName = userRole;
    }
    new ClientApplicationLifecycleHandler((IClientApplicationHost) IpsConnector._clientApplicationHost).Initialize();
  }
}
