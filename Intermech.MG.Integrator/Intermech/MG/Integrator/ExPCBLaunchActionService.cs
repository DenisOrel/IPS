// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBLaunchActionService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.LaunchActions;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBLaunchActionService(MGIntegrator owner) : Intermech.Tools.Integrators.LaunchActionService((IIntegrator) owner)
{
  protected override void OpenDocumentFileFromDisk(LaunchParams launchParams)
  {
    using (ExPCBApiSession exPcbApiSession = new ExPCBApiSession(this.Integrator))
      exPcbApiSession.Application.OpenProject(launchParams.ResultFilePath, true);
  }
}
