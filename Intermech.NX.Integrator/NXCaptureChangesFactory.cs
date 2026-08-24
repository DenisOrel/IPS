// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXCaptureChangesFactory
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Runtime;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System.Diagnostics;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXCaptureChangesFactory(IIntegrator owner) : CADCaptureChangesFactory(owner)
{
  private ICADSettingsService settingsService;

  public ICADSettingsService SettingsService
  {
    [DebuggerStepThrough] get
    {
      lock (this.Integrator.SyncRoot)
        return this.settingsService;
    }
    [DebuggerStepThrough] set
    {
      lock (this.Integrator.SyncRoot)
      {
        this.RequireNotInitialized();
        this.settingsService = value;
      }
    }
  }

  protected override void DoInitialize()
  {
    base.DoInitialize();
    if (this.SettingsService == null)
      throw PropertyExceptions.PropertyNotSetException((object) this, "SettingsService");
  }

  protected override CICaptureChangesDriver DoCreateDriver()
  {
    return (CICaptureChangesDriver) new NXCaptureChangesDriver(this.Integrator);
  }
}
