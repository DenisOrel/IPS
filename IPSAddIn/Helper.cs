// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Helper
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using DXP;
using EDP;
using SCH;
using System;
using System.IO;

#nullable disable
namespace CSharpPlugin;

internal static class Helper
{
  public static ISch_ServerInterface SCHServer
  {
    get
    {
      ISch_ServerInterface schServer = SCH.GlobalVars.SchServer;
      if (schServer == null)
      {
        IClient client = DXP.GlobalVars.Client;
        Helper.CheckEntity((object) client, typeof (IClient));
        client.StartServer("SCH");
        schServer = SCH.GlobalVars.SchServer;
      }
      Helper.CheckEntity((object) schServer, typeof (ISch_ServerInterface));
      return schServer;
    }
  }

  public static IWorkspace Workspace
  {
    get
    {
      IClient client = DXP.GlobalVars.Client;
      Helper.CheckEntity((object) client, typeof (IClient));
      IWorkspace dxpWorkspace = client.GetDXPWorkspace() as IWorkspace;
      Helper.CheckEntity((object) dxpWorkspace, typeof (IWorkspace));
      return dxpWorkspace;
    }
  }

  public static string SearchFile(string fileName, string folder, SearchOption option)
  {
    string[] files = Directory.GetFiles(folder, fileName, option);
    if (files.Length != 0)
      return files[0];
    DirectoryInfo parent = new DirectoryInfo(folder).Parent;
    return parent == null ? string.Empty : Helper.SearchFile(fileName, parent.FullName, option);
  }

  public static void CheckEntity(object entity, Type entityType)
  {
    if (entity == null)
      throw new Exception("Нельзя привести полученный COM-объект к " + entityType.Name);
  }

  public static void SetModified(string documentFile)
  {
    DXP.GlobalVars.Client.OpenDocument("SCH", documentFile)?.SetModified(true);
  }
}
