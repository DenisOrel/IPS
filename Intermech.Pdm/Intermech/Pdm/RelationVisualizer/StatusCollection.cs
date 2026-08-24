// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.StatusCollection
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class StatusCollection
{
  private static bool isInitializeChache = false;
  private static List<Status> statusCollect = new List<Status>();
  private static Dictionary<string, List<Status>> statusDict = new Dictionary<string, List<Status>>();

  private static List<Status> UpdateStatus(byte[] status, IElementStatusesClientService svc)
  {
    List<Status> statusList = new List<Status>();
    foreach (KeyValuePair<string, ElementStatusesPluginDescription> plugin in svc.Plugins)
    {
      string key = plugin.Key;
      Guid pluginGuid = new Guid(key);
      int elementStatuses32 = svc.GetElementStatuses32(key, status);
      Status status1 = StatusCollection.GetStatus(pluginGuid, elementStatuses32);
      if (status1 != null)
      {
        Image statusIcon = svc.GetStatusIcon(pluginGuid, elementStatuses32);
        if (statusIcon != null)
        {
          status1.Img = statusIcon;
          status1.StatusKey = status;
          statusList.Add(status1);
        }
      }
    }
    StatusCollection.statusDict.Add(Convert.ToBase64String(status), statusList);
    return statusList.Count == 0 ? (List<Status>) null : statusList;
  }

  public static List<Status> GetStatus(byte[] status, IElementStatusesClientService svc)
  {
    List<Status> statusList = (List<Status>) null;
    if (!StatusCollection.statusDict.TryGetValue(Convert.ToBase64String(status), out statusList))
      return StatusCollection.UpdateStatus(status, svc);
    return statusList.Count == 0 ? (List<Status>) null : statusList;
  }

  public static void InitializeStatusCollection(
    IUserSession session,
    IElementStatusesClientService svc)
  {
    if (StatusCollection.isInitializeChache)
      return;
    StatusCollection.isInitializeChache = true;
    IPluginStatusesTable customService = (IPluginStatusesTable) session.GetCustomService(typeof (IPluginStatusesTable));
    if (customService == null || svc == null)
      return;
    foreach (KeyValuePair<string, ElementStatusesPluginDescription> plugin in svc.Plugins)
    {
      string key = plugin.Key;
      Guid pluginGuid = new Guid(key);
      if (svc.DisabledPlugins.IndexOf(key) < 0)
      {
        DataTable pluginStatusesTable = customService.GetPluginStatusesTable(key, false, (int[]) null);
        if (pluginStatusesTable != null)
        {
          foreach (DataRow row in (InternalDataCollectionBase) pluginStatusesTable.Rows)
          {
            int int32 = Convert.ToInt32(row[0]);
            string caption = Convert.ToString(row[1]);
            Status status = new Status(pluginGuid, int32, caption, (Image) null);
            if (!StatusCollection.Contains(status))
              StatusCollection.statusCollect.Add(status);
          }
        }
      }
    }
  }

  private static bool Contains(Status status)
  {
    foreach (object obj in StatusCollection.statusCollect)
    {
      if (obj.Equals((object) status))
        return true;
    }
    return false;
  }

  private static Status GetStatus(Guid pluginGuid, int statusId)
  {
    foreach (Status status in StatusCollection.statusCollect)
    {
      if (status.PluginGuid.Equals(pluginGuid) && status.StatusId == statusId)
        return status;
    }
    return (Status) null;
  }
}
