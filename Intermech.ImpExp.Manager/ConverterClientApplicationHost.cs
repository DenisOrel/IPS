// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.ConverterClientApplicationHost
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.Client.Specialized;
using System;

#nullable disable
namespace Intermech.ImpExp.Manager;

internal class ConverterClientApplicationHost : IClientApplicationHost
{
  private SimpleSessionPoolLoginInfo _loginInfo = new SimpleSessionPoolLoginInfo();

  public ConverterClientApplicationHost(string loginName, string password, string roleName)
  {
    this._loginInfo.LoginName = loginName;
    this._loginInfo.Password = password;
    this._loginInfo.RoleName = roleName;
  }

  public Func<SimpleSessionPoolLoginInfo> LoginInfoProvider
  {
    get => (Func<SimpleSessionPoolLoginInfo>) (() => this._loginInfo);
  }

  public SimpleSessionPoolLoginInfo LoginInfo => this._loginInfo;
}
