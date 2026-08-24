// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.PELaunchActionService
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.Tools.LaunchActions;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class PELaunchActionService(IIntegrator owner) : CADLaunchActionService(owner)
{
  protected override void OpenDocumentFileFromDisk(LaunchParams launchParams)
  {
    new CheckNoVersionFilesTask(launchParams.ResultFilePath).Perform();
    base.OpenDocumentFileFromDisk(launchParams);
  }
}
