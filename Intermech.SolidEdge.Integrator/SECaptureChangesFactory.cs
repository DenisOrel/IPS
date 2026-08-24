// Decompiled with JetBrains decompiler
// Type: Intermech.SolidEdge.Integrator.SECaptureChangesFactory
// Assembly: Intermech.SolidEdge.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 213B90F8-0434-43B8-B8F6-9AF19E139193
// Assembly location: D:\IPS\Client\Intermech.SolidEdge.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;

#nullable disable
namespace Intermech.SolidEdge.Integrator;

internal sealed class SECaptureChangesFactory(IIntegrator owner) : CADCaptureChangesFactory(owner)
{
  protected override CICaptureChangesDriver DoCreateDriver()
  {
    return (CICaptureChangesDriver) new SECaptureChangesDriver(this.Integrator);
  }
}
