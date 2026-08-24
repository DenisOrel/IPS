// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXIntegratorPatchesModule
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.Client.DBPatches;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.DBPatches;
using System;
using System.Threading;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXIntegratorPatchesModule : InitializerModule
{
  private IStartupService startupService;
  private Func<PatchRunner> patchRunnerFactory;

  public NXIntegratorPatchesModule(
    IStartupService startupService,
    Func<PatchRunner> patchRunnerFactory)
  {
    if (startupService == null)
      throw new ArgumentNullException(nameof (startupService));
    if (patchRunnerFactory == null)
      throw new ArgumentNullException(nameof (patchRunnerFactory));
    this.startupService = startupService;
    this.patchRunnerFactory = patchRunnerFactory;
  }

  protected override void DoInitialize()
  {
    base.DoInitialize();
    this.startupService.StartupComplete += new EventHandler(this.OnStartupCompleted);
  }

  protected override void DoShutdown()
  {
    this.startupService.StartupComplete -= new EventHandler(this.OnStartupCompleted);
    base.DoShutdown();
  }

  private void OnStartupCompleted(object sender, EventArgs e)
  {
    ThreadPool.QueueUserWorkItem(new WaitCallback(this.ApplyPatchesInBackground));
  }

  private void ApplyPatchesInBackground(object arg)
  {
    this.patchRunnerFactory().Run((AbstractPatch) new NXDrawingsTypeSettingsPatch());
  }
}
