// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SWPDMBrowserService
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class SWPDMBrowserService(IIntegrator owner, Guid cadSystemId) : PDMBrowserService(owner, cadSystemId)
{
  protected override ISynchronizeActionReloadStrategy DoCreateSynchronizeActionReloadStrategy()
  {
    return (ISynchronizeActionReloadStrategy) new SWSynchronizeActionReloadStrategy(this.Integrator);
  }
}
