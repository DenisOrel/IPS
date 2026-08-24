// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGPlugin
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.Interfaces.Plugins;
using Intermech.Runtime.ComInterop.LocalServer;
using Intermech.Tools.Integrators.Electrical;
using System;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGPlugin : ClientModularPackage
{
  public MGPlugin()
    : base(MGConsts.PluginName)
  {
  }

  protected override void CreateSubModules(InitializerModuleGroup subModules)
  {
    base.CreateSubModules(subModules);
    PluginContext pluginCtx1 = new PluginContext();
    subModules.Add((InitializerModule) new MGIntegratorModule<DXDIntegrator>(pluginCtx1, MGConsts.DXDIntegratorName));
    subModules.Add((InitializerModule) new DBAutoSetupModule(pluginCtx1));
    subModules.Add((InitializerModule) new DXDCommandsModule(pluginCtx1));
    PluginContext pluginCtx2 = new PluginContext();
    subModules.Add((InitializerModule) new MGIntegratorModule<ExPCBIntegrator>(pluginCtx2, MGConsts.ExPCBApplicationName));
    subModules.Add((InitializerModule) new DBAutoSetupModule(pluginCtx2));
    subModules.Add((InitializerModule) new ExPCBCommandsModule(pluginCtx2));
  }

  public override void Load(IServiceProvider serviceProvider)
  {
    base.Load(serviceProvider);
    if (!ComHost.Configuration.ComSupportActive)
      return;
    ComHost.ActivateClassFactory(typeof (DXDIntegratorAPI));
  }

  public override void Unload()
  {
    base.Unload();
    if (!ComHost.Configuration.ComSupportActive)
      return;
    ComHost.DeactivateClassFactory(typeof (DXDIntegratorAPI));
  }
}
