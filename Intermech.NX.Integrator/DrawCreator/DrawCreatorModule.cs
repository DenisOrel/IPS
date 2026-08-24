// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.DrawCreator.DrawCreatorModule
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.Tools.Integrators.CADInterface;
using System;

#nullable disable
namespace Intermech.NX.Integrator.DrawCreator;

internal sealed class DrawCreatorModule : InitializerModule
{
  private DrawCreatorProvider _drawCreatorProvider;
  private CADIntegratorModule<NXIntegrator> _nxModule;

  public DrawCreatorModule(
    DrawCreatorProvider drawCreatorProvider,
    CADIntegratorModule<NXIntegrator> nxModule)
  {
    if (drawCreatorProvider == null)
      throw new ArgumentNullException(nameof (drawCreatorProvider));
    this._nxModule = nxModule != null ? nxModule : throw new ArgumentNullException(nameof (nxModule));
    this._drawCreatorProvider = drawCreatorProvider;
  }

  protected override void DoInitialize()
  {
    base.DoInitialize();
    this._drawCreatorProvider.NXIntegrator = this._nxModule.Integrator;
    this._drawCreatorProvider.Enabled = true;
  }

  protected override void DoShutdown()
  {
    this._drawCreatorProvider.Enabled = false;
    this._drawCreatorProvider.NXIntegrator = (NXIntegrator) null;
    base.DoShutdown();
  }
}
