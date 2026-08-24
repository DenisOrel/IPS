// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGCaptureChangesService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.DataExchange;
using Intermech.Tools.Integrators;
using System.Diagnostics;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGCaptureChangesService(IIntegrator owner) : CaptureChangesService(owner)
{
  private MGMechanicalDriver driver;

  protected override void DoInitialize()
  {
    base.DoInitialize();
    this.driver = new MGMechanicalDriver(this.Integrator);
  }

  protected override ICaptureChangesDriver Driver
  {
    [DebuggerStepThrough] get => (ICaptureChangesDriver) this.driver;
  }

  protected override void ConfigureDriverParameters(CaptureChangesOptions options)
  {
    base.ConfigureDriverParameters(options);
  }

  protected override void ResetDriverParameters() => base.ResetDriverParameters();
}
