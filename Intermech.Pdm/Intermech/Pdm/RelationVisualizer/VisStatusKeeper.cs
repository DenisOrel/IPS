// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisStatusKeeper
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisStatusKeeper
{
  public static readonly VisStatusKeeper vsk = new VisStatusKeeper();
  internal Dictionary<VisStatusKey, Image> _pictCollection;
  internal Dictionary<VisStatusKey, string> _captCollection;
  internal HashSet<Guid> _disabledPlugins;

  private void _Init()
  {
    IElementStatusesClientService service = ServicesManager.GetService<IElementStatusesClientService>();
    IPluginStatusesTable customService = (IPluginStatusesTable) (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IPluginStatusesTable));
    if (service == null || customService == null)
      return;
    this._disabledPlugins = new HashSet<Guid>();
    service.DisabledPlugins.ForEach((Action<string>) (s => this._disabledPlugins.Add(new Guid(s))));
    foreach (KeyValuePair<string, ElementStatusesPluginDescription> plugin in service.Plugins)
    {
      string key1 = plugin.Key;
      Guid guid = new Guid(key1);
      DataTable pluginStatusesTable = customService.GetPluginStatusesTable(key1, false, (int[]) null);
      if (pluginStatusesTable != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) pluginStatusesTable.Rows)
        {
          int int32 = Convert.ToInt32(row[0]);
          string str = Convert.ToString(row[1]);
          Image statusIcon = service.GetStatusIcon(guid, int32);
          VisStatusKey key2 = new VisStatusKey(guid, int32);
          if (!this._captCollection.ContainsKey(key2))
            this._captCollection.Add(key2, str);
          if (!this._pictCollection.ContainsKey(key2))
            this._pictCollection.Add(key2, statusIcon);
        }
      }
    }
  }

  public static void Init()
  {
    if (VisStatusKeeper.vsk._pictCollection == null)
      VisStatusKeeper.vsk._pictCollection = new Dictionary<VisStatusKey, Image>();
    if (VisStatusKeeper.vsk._captCollection == null)
      VisStatusKeeper.vsk._captCollection = new Dictionary<VisStatusKey, string>();
    VisStatusKeeper.vsk._Init();
  }

  public static void Clear()
  {
    if (VisStatusKeeper.vsk._pictCollection != null)
      VisStatusKeeper.vsk._pictCollection.Clear();
    if (VisStatusKeeper.vsk._captCollection == null)
      return;
    VisStatusKeeper.vsk._captCollection.Clear();
  }

  public static void UpdateDisabledPlugins()
  {
    IElementStatusesClientService service = ServicesManager.GetService<IElementStatusesClientService>();
    if (service == null)
      return;
    VisStatusKeeper.UpdateDisabledPlugins(service);
  }

  public static void UpdateDisabledPlugins(IElementStatusesClientService _elStatusService)
  {
    if (VisStatusKeeper.vsk._disabledPlugins != null)
      VisStatusKeeper.vsk._disabledPlugins.Clear();
    else
      VisStatusKeeper.vsk._disabledPlugins = new HashSet<Guid>();
    _elStatusService.DisabledPlugins.ForEach((Action<string>) (s => VisStatusKeeper.vsk._disabledPlugins.Add(new Guid(s))));
  }

  public static List<VisStatus> MakeStatuses(byte[] status, IElementStatusesClientService svc)
  {
    List<VisStatus> visStatusList = new List<VisStatus>();
    Guid g1 = new Guid("{7074E0E4-B3AB-4B3E-AD56-050CD256AF10}");
    foreach (KeyValuePair<string, ElementStatusesPluginDescription> plugin in svc.Plugins)
    {
      string key = plugin.Key;
      Guid g2 = new Guid(key);
      if (!g2.Equals(g1) && !VisStatusKeeper.vsk._disabledPlugins.Contains(g2))
      {
        int elementStatuses32 = svc.GetElementStatuses32(key, status);
        VisStatus visStatus = new VisStatus(g2, elementStatuses32);
        if (visStatus.Icon != null)
          visStatusList.Add(visStatus);
      }
    }
    return visStatusList;
  }

  public static Image GetImage(VisStatusKey Key)
  {
    Image image = (Image) null;
    VisStatusKeeper.vsk._pictCollection.TryGetValue(Key, out image);
    return image;
  }

  public static string GetCapt(VisStatusKey Key)
  {
    string capt = (string) null;
    VisStatusKeeper.vsk._captCollection.TryGetValue(Key, out capt);
    return capt;
  }
}
