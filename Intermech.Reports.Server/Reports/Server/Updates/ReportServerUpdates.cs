// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Server.Updates.ReportServerUpdates
// Assembly: Intermech.Reports.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B97D7940-CE11-4EF0-80CD-76A0AE479D33
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Reports.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.MetadataUpdates;

#nullable disable
namespace Intermech.Reports.Server.Updates;

internal class ReportServerUpdates : IUpdatable
{
  public string[] GetUpdateScripts()
  {
    return new string[1]{ "Intermech.Reports.Server_0.xml" };
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
