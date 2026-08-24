// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SWIntegratorModule
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.Tools.Integrators.CADInterface;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class SWIntegratorModule : CADIntegratorModule<SWIntegrator>
{
  public SWIntegratorModule() => this.EnableLaunchHandler(SWConsts.IntegratorAppName);
}
