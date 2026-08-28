// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.Server.RequirementUpdate
// Assembly: Intermech.Requirement.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C85D341A-B4CB-4985-9EA3-68BB7F9530D7
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Requirement.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.MetadataUpdates;

#nullable disable
namespace Intermech.Requirement.Server;

public class RequirementUpdate : IUpdatable
{
  public string[] GetUpdateScripts()
  {
    return new string[1]
    {
      "Intermech.Requirement.Server.xml"
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
