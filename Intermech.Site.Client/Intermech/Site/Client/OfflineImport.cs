// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.OfflineImport
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal static class OfflineImport
{
  public static void OnImport(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      string[] offlineImportFilesList = ((IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector))).OfflineImportFilesList;
      if (offlineImportFilesList == null)
      {
        int num = (int) MessageBox.Show("Доступные файлы для импорта отсутствуют");
      }
      else
      {
        using (ImportFileSelector importFileSelector = new ImportFileSelector(offlineImportFilesList))
        {
          if (importFileSelector.ShowDialog() != DialogResult.OK)
            return;
          string[] selectedFiles = importFileSelector.SelectedFiles;
          if (selectedFiles == null)
            return;
          IPortalTasksQueue customService = (IPortalTasksQueue) sessionKeeper.Session.GetCustomService(typeof (IPortalTasksQueue));
          foreach (string updateGuid in selectedFiles)
            customService.StartUpdate(sessionKeeper.Session.SessionGUID, updateGuid, (object) null);
        }
      }
    }
  }
}
