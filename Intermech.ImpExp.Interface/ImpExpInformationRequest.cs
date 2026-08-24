// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImpExpInformationRequest
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.InformationCollector;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Interface;

internal class ImpExpInformationRequest : Intermech.Interfaces.Client.InformationRequest.InformationRequest
{
  private IUserSession _session;

  public ImpExpInformationRequest(IUserSession session) => this._session = session;

  protected override List<FileInfo> GetServerLogFiles(ref long logSize)
  {
    List<FileInfo> serverLogFiles = new List<FileInfo>();
    try
    {
      if (this._session != null)
      {
        serverLogFiles = (this._session.GetCustomService(typeof (IServerInformationCollector)) as IServerInformationCollector).LogFiles();
        for (int index = 0; index < serverLogFiles.Count - 1; ++index)
        {
          FileInfo fileInfo = serverLogFiles[index];
          logSize += fileInfo.Length;
        }
      }
    }
    catch (Exception ex)
    {
    }
    return serverLogFiles;
  }

  protected override void GetUserInformation(InformationNode clientNode)
  {
    if (this._session == null)
      return;
    clientNode.Add(new InformationNode("UserName", this._session.UserName));
    try
    {
      QuickObjectInfo objectInfo = this._session.GetObjectInfo(this._session.RoleID);
      clientNode.Add(new InformationNode("UserRole", objectInfo.Caption));
    }
    catch
    {
      clientNode.Add(new InformationNode("UserRole", this._session.RoleID.ToString()));
    }
  }

  protected override InformationNode PluginsInformation()
  {
    IImpExpPluginsManager service = (IImpExpPluginsManager) ServicesManager.GetService(typeof (IImpExpPluginsManager));
    InformationNode informationNode = new InformationNode("Plugins");
    if (service != null)
    {
      foreach (IPlugin plugins in service.PluginsList)
        informationNode.Add(new InformationNode("Plugin")
        {
          new InformationNode("name", plugins.Name, NodeType.Attribute),
          new InformationNode("location", plugins.Location, NodeType.Attribute),
          new InformationNode("version", plugins.GetType().Assembly.GetName().Version.ToString(), NodeType.Attribute)
        });
    }
    return informationNode;
  }

  protected override IServerInformationCollector GetServerInformationCollector(IUserSession session)
  {
    IUserSession userSession;
    if (this._session == null)
    {
      if (session == null)
        return (IServerInformationCollector) null;
      userSession = session;
    }
    else
      userSession = this._session;
    return userSession.GetCustomService(typeof (IServerInformationCollector)) as IServerInformationCollector;
  }

  protected override InformationNode ServerInformation()
  {
    if (this._session == null)
      return new InformationNode(nameof (ServerInformation));
    try
    {
      return (this._session.GetCustomService(typeof (IServerInformationCollector)) as IServerInformationCollector).CollectServerInformation();
    }
    catch (Exception ex)
    {
      return new InformationNode("ServerException")
      {
        IPSInformation.ExceptionInformation(ex)
      };
    }
  }
}
