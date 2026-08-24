// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ImportService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.DataExchange;
using Intermech.Tools.Integrators;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ImportService(IIntegrator owner) : FileImportService(owner)
{
  private MGMechanicalDriver captureDriver;

  protected override void DoInitialize()
  {
    base.DoInitialize();
    this.captureDriver = new MGMechanicalDriver(this.Integrator);
  }

  protected override ICaptureChangesDriver GetCaptureChangesDriver()
  {
    return (ICaptureChangesDriver) this.captureDriver;
  }

  protected override void SetCaptureChangesParameters(bool extendedImport)
  {
    base.SetCaptureChangesParameters(extendedImport);
    if (!extendedImport)
      return;
    this.captureDriver.UpdateArticles = true;
    this.captureDriver.RecalculateMass = false;
  }

  protected override void ResetCaptureChangesParameters()
  {
    base.ResetCaptureChangesParameters();
    this.captureDriver.UpdateArticles = false;
    this.captureDriver.RecalculateMass = false;
  }
}
