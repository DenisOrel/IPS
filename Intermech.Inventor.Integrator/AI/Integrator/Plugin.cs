// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.Plugin
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.Interfaces.Plugins;
using Intermech.Tools.Integrators.CADInterface;

#nullable disable
namespace Intermech.AI.Integrator;

internal sealed class Plugin : ClientModularPackage
{
  public Plugin()
    : base(AIConsts.IntegratorName)
  {
  }

  protected override void CreateSubModules(InitializerModuleGroup subModules)
  {
    base.CreateSubModules(subModules);
    CADIntegratorModule<AIIntegrator> module = new CADIntegratorModule<AIIntegrator>();
    module.EnableLaunchHandler(AIConsts.ApplicationName);
    subModules.Add((InitializerModule) module);
    subModules.Add((InitializerModule) new AIFamilyFilesModule());
  }
}
