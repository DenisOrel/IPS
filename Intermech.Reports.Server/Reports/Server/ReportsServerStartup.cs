// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Server.ReportsServerStartup
// Assembly: Intermech.Reports.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B97D7940-CE11-4EF0-80CD-76A0AE479D33
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Reports.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Plugins;
using Intermech.Interfaces.Reports;
using Intermech.Interfaces.Server;
using Intermech.Localization;
using Intermech.Reports.Server.Objects;
using System;

#nullable disable
namespace Intermech.Reports.Server;

public class ReportsServerStartup : IPackage
{
  private string _name;
  private IPluginManager _manager;
  private IReportsServerService _reportSrvService;

  public void Unload()
  {
    if (this._manager != null)
      this._manager.LoadComplete -= new EventHandler(this._manager_LoadComplete);
    this._manager_Unload();
  }

  public string Name
  {
    get => this._name ?? (this._name = LocalizationHolder.rm.GetString("Reports.Server_1"));
  }

  public void Load(IServiceProvider serviceProvider)
  {
    ReportsServerServiceCache.ServiceProvider = serviceProvider;
    this._manager = serviceProvider.GetService(typeof (IPluginManager)) as IPluginManager;
    if (this._manager == null)
      return;
    this._manager.LoadComplete += new EventHandler(this._manager_LoadComplete);
  }

  private void _manager_LoadComplete(object sender, EventArgs e)
  {
    ReportsServerServiceCache.EventLogHelper = ReportsServerServiceCache.ServiceProvider.GetService(typeof (IEventLogHelper)) as IEventLogHelper;
    this._reportSrvService = (IReportsServerService) new ReportsServerService();
    if (ReportsServerServiceCache.ServiceProvider.GetService(typeof (ICustomServices)) is ICustomServices service1)
      service1.AddService(typeof (IReportsServerService), (object) this._reportSrvService);
    service1?.AddService(typeof (IReportsServerUtils), (object) this._reportSrvService);
    ServerServices.AddService(typeof (IReportsServerService), (object) this._reportSrvService);
    ServerServices.AddService(typeof (IReportsServerUtils), (object) this._reportSrvService);
    if (!(ServerServices.GetService(typeof (IDBObjectService)) is ICreatorContainer service2))
      return;
    service2.AddCreator((object) ReportsConsts.DocPackageBaseTypeGuid, (object) new ComplectDBObjectCreator());
  }

  private void _manager_Unload()
  {
    ServiceUtils.GetService<ICustomServices>((object) ReportsServerServiceCache.ServiceProvider, false)?.RemoveService(typeof (IReportsServerService));
    ServerServices.RemoveService(typeof (IReportsServerService));
  }
}
