// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.Plugin
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.SolidWorks.Integrator.Extensions;
using Ninject;
using Ninject.Modules;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class Plugin(IOCBasedPackageParameters createParameters) : IOCBasedPackage(createParameters, SWConsts.DisplayIntegratorName)
{
  protected override void DoInitializeIOCContainer()
  {
    base.DoInitializeIOCContainer();
    this.IOCContainer.Bind<SWIntegratorModule>().ToSelf().InSingletonScope();
    this.IOCContainer.Load((INinjectModule) new ExtensionsNinjectModule());
  }

  protected override void CreateSubModules(LazyInitializerModuleGroup subModules)
  {
    base.CreateSubModules(subModules);
    subModules.Add<SWIntegratorModule>();
    subModules.Add<NavigatorCommandModule>();
  }
}
