// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.SignUpdate
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.MetadataUpdates;

#nullable disable
namespace Intermech.Signs;

public class SignUpdate : IUpdatable
{
  public string[] GetUpdateScripts()
  {
    return new string[4]
    {
      "Intermech.Signs.xml",
      "Intermech.Signs_CertSheets.xml",
      "Intermech.Signs_Portable.xml",
      "Intermech.Signs_Staff.xml"
    };
  }

  public void BeforeExecScript(IUserSession session, string scriptName)
  {
  }

  public void AfterExecScript(IUserSession session, string scriptName)
  {
  }

  public void AfterExecAllScripts(IUserSession session)
  {
  }
}
