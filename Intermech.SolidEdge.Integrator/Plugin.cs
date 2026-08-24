// Decompiled with JetBrains decompiler
// Type: Intermech.SolidEdge.Integrator.Plugin
// Assembly: Intermech.SolidEdge.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 213B90F8-0434-43B8-B8F6-9AF19E139193
// Assembly location: D:\IPS\Client\Intermech.SolidEdge.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.Interfaces.Plugins;
using Intermech.Tools.Integrators.CADInterface;
using System;

#nullable disable
namespace Intermech.SolidEdge.Integrator;

internal class Plugin : ClientModularPackage
{
  internal static readonly string ProgID = "SePdm.SECADSystem";
  internal static readonly Guid ClsID = new Guid("F909FC49-93BB-4A37-BE91-6809783852EF");
  internal static readonly Guid IntegratorId = new Guid("95613B2A-0878-49C0-8FB7-C1A2827CD788");
  internal static readonly string StandardLibrary = "SE Library";
  internal static readonly string IntegratorAppName = "Solid Edge";
  internal static readonly string IntegratorName = Localization.rm.GetString("SolidEdge.Integrator_2");

  public Plugin()
    : base(Plugin.IntegratorName)
  {
  }

  protected override void CreateSubModules(InitializerModuleGroup subModules)
  {
    base.CreateSubModules(subModules);
    CADIntegratorModule<SEIntegrator> module = new CADIntegratorModule<SEIntegrator>();
    module.EnableLaunchHandler(Plugin.IntegratorAppName);
    subModules.Add((InitializerModule) module);
  }
}
