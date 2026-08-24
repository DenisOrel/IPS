// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.Plugin
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.NX.Integrator.DrawCreator;
using Intermech.Tools.Integrators.CADInterface;
using Ninject.Activation;
using System;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class Plugin(IOCBasedPackageParameters createParameters) : IOCBasedPackage(createParameters, Plugin.IntegratorName)
{
  internal static readonly Guid NXCLSID = new Guid("666F0C3F-A3A5-46D8-84D1-A8F609DAC764");
  internal static readonly Guid IntegratorId = new Guid("713D84FC-EDD2-4F39-A121-08F4CE1C357E");
  internal static readonly string StandardLibrary = "NX Library";
  internal static readonly string IntegratorAppName = "NX Unigraphics";
  internal static readonly string IntegratorName = Localization.rm.GetString("NX.Integrator_2");

  protected override void DoInitializeIOCContainer()
  {
    base.DoInitializeIOCContainer();
    this.IOCContainer.Bind<CADIntegratorModule<NXIntegrator>>().ToSelf().InSingletonScope().OnActivation((Action<IContext, CADIntegratorModule<NXIntegrator>>) ((context, obj) => obj.EnableLaunchHandler(Plugin.IntegratorAppName)));
    this.IOCContainer.Bind<NXIntegratorPatchesModule>().ToSelf();
    this.IOCContainer.Bind<DrawCreatorProvider>().ToSelf().InSingletonScope();
    this.IOCContainer.Bind<DrawCreatorModule>().ToSelf();
  }

  protected override void CreateSubModules(LazyInitializerModuleGroup subModules)
  {
    base.CreateSubModules(subModules);
    subModules.Add<CADIntegratorModule<NXIntegrator>>();
    subModules.Add<NXIntegratorPatchesModule>();
    subModules.Add<DrawCreatorModule>();
  }
}
