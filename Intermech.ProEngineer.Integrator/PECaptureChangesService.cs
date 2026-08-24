// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.PECaptureChangesService
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.Tools.DataExchange;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class PECaptureChangesService(IIntegrator owner, CADCaptureChangesFactory factory) : 
  CADCaptureChangesService(owner, factory)
{
  protected override void OnAfterCaptureChanges(CaptureChangesResult result)
  {
    base.OnAfterCaptureChanges(result);
    new CheckNoVersionFilesTask(result.FullPath).Perform();
  }
}
