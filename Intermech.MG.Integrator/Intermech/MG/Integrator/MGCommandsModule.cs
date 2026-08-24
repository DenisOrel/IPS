// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGCommandsModule
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;
using System;
using System.IO;

#nullable disable
namespace Intermech.MG.Integrator;

internal abstract class MGCommandsModule : InitializerModule
{
  private readonly PluginContext pluginCtx;
  private IFactory navigatorFactorySvc;
  private ECADCommandsProvider commandsProvider;

  public MGCommandsModule(PluginContext pluginCtx) => this.pluginCtx = pluginCtx;

  protected override void DoInitialize()
  {
    base.DoInitialize();
    if (this.pluginCtx.IntegratorInstance == null)
      return;
    this.navigatorFactorySvc = (IFactory) ServicesManager.GetService(typeof (IFactory));
    this.commandsProvider = this.CreateCommandsProvider(this.pluginCtx.IntegratorInstance);
    this.commandsProvider.UpdateMenuTemplate();
    this.navigatorFactorySvc.AddCommandsProvider((ICommandsProvider) this.commandsProvider);
    ServiceUtils.GetService<IFileVault>((object) ServicesManager.ServiceContainer, true).ReadOnlyLocalFiles.CanControlAttributeEvent += new EventHandler<CanControlFileAttributeEventArgs>(this.ReadOnlyLocalFiles_CanControlAttributeEvent);
  }

  private void ReadOnlyLocalFiles_CanControlAttributeEvent(
    object sender,
    CanControlFileAttributeEventArgs e)
  {
    if (!e.CanControl || !Path.GetFileName(e.LocalFilePath).ToLower().Equals("icdb.dat"))
      return;
    e.CanControl = false;
  }

  protected abstract ECADCommandsProvider CreateCommandsProvider(IIntegrator integrator);

  protected override void DoShutdown()
  {
    ServiceUtils.GetService<IFileVault>((object) ServicesManager.ServiceContainer, true).ReadOnlyLocalFiles.CanControlAttributeEvent -= new EventHandler<CanControlFileAttributeEventArgs>(this.ReadOnlyLocalFiles_CanControlAttributeEvent);
    base.DoShutdown();
    if (this.commandsProvider != null)
    {
      this.navigatorFactorySvc.RemoveCommandsProvider((ICommandsProvider) this.commandsProvider);
      this.commandsProvider = (ECADCommandsProvider) null;
    }
    this.navigatorFactorySvc = (IFactory) null;
  }
}
