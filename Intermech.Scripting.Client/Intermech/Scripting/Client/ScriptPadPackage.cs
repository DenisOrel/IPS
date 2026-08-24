// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Client.ScriptPadPackage
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.ApplicationModel;
using Intermech.Scripting.Addons;
using Intermech.Scripting.CSharp;
using Intermech.Scripting.Services;
using Ninject;
using Ninject.Modules;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

#nullable disable
namespace Intermech.Scripting.Client;

public sealed class ScriptPadPackage : IOCBasedPackage
{
  public ScriptPadPackage(IOCBasedPackageParameters createParameters)
    : base(createParameters, "Среда разработчика сценариев (Script pad)")
  {
    this.DoInitializeMainIOCContainer(createParameters.IOCContainer);
  }

  private void DoInitializeMainIOCContainer(IKernel mainIOCContainer)
  {
    mainIOCContainer.Load((INinjectModule) new CSharpNinjectModule());
    mainIOCContainer.Load((INinjectModule) new ScriptPadServiceNinjectModule());
  }

  [Conditional("DEBUG")]
  private void LoadAdditionalLanguages(IKernel mainIOCContainer)
  {
    string path = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "Intermech.Scripting.IronPython" + ".dll");
    if (!Path.IsPathRooted(path) || !File.Exists(path))
      return;
    foreach (Type exportedType in Assembly.Load("Intermech.Scripting.IronPython").GetExportedTypes())
    {
      if (!exportedType.IsAbstract && !exportedType.IsGenericTypeDefinition && typeof (INinjectModule).IsAssignableFrom(exportedType))
        mainIOCContainer.Load((INinjectModule) Activator.CreateInstance(exportedType));
    }
  }

  protected override void DoInitializeIOCContainer()
  {
    base.DoInitializeIOCContainer();
    this.IOCContainer.Load((INinjectModule) new AddonsNinjectModule());
    this.IOCContainer.Bind<MainMenuModule>().ToSelf().InSingletonScope();
    this.IOCContainer.Bind<NavigatorCommandProvider>().ToSelf();
    this.IOCContainer.Bind<NavigatorCommandModule>().ToSelf().InSingletonScope();
  }

  protected override void CreateSubModules(LazyInitializerModuleGroup subModules)
  {
    base.CreateSubModules(subModules);
    subModules.Add<MainMenuModule>();
    subModules.Add<NavigatorCommandModule>();
    subModules.Add<AddonsModule>();
  }
}
