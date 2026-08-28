// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PortalUpdate
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.MetadataUpdates;

#nullable disable
namespace Intermech.Portal.Server;

public class PortalUpdate : IUpdatable
{
  public string[] GetUpdateScripts()
  {
    return new string[2]
    {
      "intermech.portal.attributes.xml",
      "intermech.portal.objtypes.xml"
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
