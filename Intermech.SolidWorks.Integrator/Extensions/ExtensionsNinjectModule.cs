// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.Extensions.ExtensionsNinjectModule
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Ninject.Modules;

#nullable disable
namespace Intermech.SolidWorks.Integrator.Extensions;

internal sealed class ExtensionsNinjectModule : NinjectModule
{
  public override void Load()
  {
    this.Bind<NavigatorCommandProvider>().ToSelf();
    this.Bind<NavigatorCommandModule>().ToSelf();
  }
}
