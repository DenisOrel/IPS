// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SWDataExchangeExtensions
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.CADInterface.Proxies;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class SWDataExchangeExtensions(IIntegrator owner) : DataExchangeExtensions(owner)
{
  public override IDependencyFilterBehavior CreateDependencyFilterBehavior(CADSystemProxy cadProxy)
  {
    if (cadProxy == null)
      throw new ArgumentNullException(nameof (cadProxy));
    this.RequireReadyState();
    return (IDependencyFilterBehavior) new SWFileBehavior();
  }
}
