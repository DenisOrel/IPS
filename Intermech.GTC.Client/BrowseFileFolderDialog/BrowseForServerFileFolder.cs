// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.BrowseFileFolderDialog.BrowseForServerFileFolder
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.GTC.Interfaces;
using Intermech.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.GTC.Client.BrowseFileFolderDialog;

public class BrowseForServerFileFolder
{
  public static string SelectFileFolder(string filter = "*")
  {
    string str = string.Empty;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IBrowseFileFolder)) is IBrowseFileFolder customService))
        throw new Exception("Service IBrowseFileFolder not found");
      using (Intermech.GTC.Client.BrowseFileFolderDialog.BrowseFileFolderDialog fileFolderDialog = new Intermech.GTC.Client.BrowseFileFolderDialog.BrowseFileFolderDialog(customService, filter))
      {
        if (fileFolderDialog.ShowDialog() == DialogResult.OK)
          str = fileFolderDialog.Path;
      }
    }
    return str;
  }
}
