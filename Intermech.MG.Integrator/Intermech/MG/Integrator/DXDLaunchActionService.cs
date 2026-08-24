// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDLaunchActionService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.LaunchActions;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class DXDLaunchActionService(MGIntegrator owner) : Intermech.Tools.Integrators.LaunchActionService((IIntegrator) owner)
{
  protected override void OpenDocumentFileFromDisk(LaunchParams launchParams)
  {
    using (DXDApiSession dxdApiSession = new DXDApiSession(this.Integrator))
      dxdApiSession.Application.OpenProject(launchParams.ResultFilePath, true);
  }
}
