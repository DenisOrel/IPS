// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ConnectionSetting
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

public class ConnectionSetting
{
  public readonly string ConnectionString;
  public readonly string UserName;
  public readonly string Password;

  public ConnectionSetting(string connString, string userName, string password)
  {
    this.ConnectionString = connString;
    this.UserName = userName;
    this.Password = password;
  }
}
