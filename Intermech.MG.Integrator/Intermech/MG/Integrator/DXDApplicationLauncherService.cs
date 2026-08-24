// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDApplicationLauncherService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class DXDApplicationLauncherService(IIntegrator owner) : 
  ApplicationLauncherService(owner),
  IApplicationLauncherService
{
  protected override void DoLaunchApplication()
  {
    using (DXDApiSession dxdApiSession = new DXDApiSession(this.Integrator))
      dxdApiSession.Application.SwitchToApp();
  }
}
