// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXDataExchangeExtensions
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.CADInterface.Proxies;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXDataExchangeExtensions(IIntegrator owner) : DataExchangeExtensions(owner)
{
  public override IDependencyFilterBehavior CreateDependencyFilterBehavior(CADSystemProxy cadProxy)
  {
    if (cadProxy == null)
      throw new ArgumentNullException(nameof (cadProxy));
    this.RequireReadyState();
    return (IDependencyFilterBehavior) new NXFileBehavior(cadProxy);
  }
}
