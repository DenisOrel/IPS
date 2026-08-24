// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGIntegratorModule`1
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGIntegratorModule<TIntegrator> : IntegratorModule<TIntegrator> where TIntegrator : class, IIntegrator, new()
{
  private readonly PluginContext pluginCtx;

  public MGIntegratorModule(PluginContext pluginCtx, string applicationName)
  {
    this.pluginCtx = pluginCtx;
    this.EnableLaunchHandler(applicationName);
  }

  protected override void DoInitialize()
  {
    base.DoInitialize();
    this.pluginCtx.IntegratorInstance = (IIntegrator) this.Integrator;
  }

  protected override void DoShutdown()
  {
    base.DoShutdown();
    this.pluginCtx.IntegratorInstance = (IIntegrator) null;
  }
}
