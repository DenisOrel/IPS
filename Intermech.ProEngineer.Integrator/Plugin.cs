// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.Plugin
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.Interfaces.Plugins;
using Intermech.Tools.Integrators.CADInterface;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class Plugin : ClientModularPackage
{
  public Plugin()
    : base(PEConsts.IntegratorName)
  {
  }

  protected override void CreateSubModules(InitializerModuleGroup subModules)
  {
    base.CreateSubModules(subModules);
    CADIntegratorModule<PEIntegrator> module = new CADIntegratorModule<PEIntegrator>();
    module.EnableLaunchHandler(PEConsts.AppName);
    subModules.Add((InitializerModule) module);
    subModules.Add((InitializerModule) new StripVersionNumbersModule());
  }
}
