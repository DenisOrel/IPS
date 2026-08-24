// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SWFileTypesDispatcher
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.Tools.Integrators;
using System.Collections.Generic;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class SWFileTypesDispatcher(IIntegrator owner) : CompositeFileTypesService(owner)
{
  protected override ICollection<IApplicationFileTypes> GetSubServices()
  {
    ICollection<IApplicationFileTypes> subServices = base.GetSubServices();
    subServices.Add((IApplicationFileTypes) new SWOldFileTypeService(this.Integrator));
    subServices.Add((IApplicationFileTypes) new SW2015FileTypeService(this.Integrator));
    return subServices;
  }
}
