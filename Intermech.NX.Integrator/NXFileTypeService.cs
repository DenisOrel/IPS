// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXFileTypeService
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Tools.Integrators;
using System.Collections.Generic;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXFileTypeService(IIntegrator owner) : CompositeFileTypesService(owner)
{
  protected override ICollection<IApplicationFileTypes> GetSubServices()
  {
    ICollection<IApplicationFileTypes> subServices = base.GetSubServices();
    subServices.Add((IApplicationFileTypes) new NX11OrOlderFileTypeService(this.Integrator));
    subServices.Add((IApplicationFileTypes) new NX12OrNewerFileTypeService(this.Integrator));
    return subServices;
  }
}
