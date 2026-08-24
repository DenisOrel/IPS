// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGExtendedSaveService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;
using Intermech.Tools.Integrators.Mechanical;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGExtendedSaveService(IIntegrator owner) : 
  ECADExtendedSaveService<MGSettingsService>(owner)
{
  protected override AppMechanicalDriver CreateMechanicalDriver()
  {
    return (AppMechanicalDriver) new MGMechanicalDriver(this.Integrator);
  }

  protected override IList<LocalId<int>> supportedDocumentTypes
  {
    get
    {
      return (IList<LocalId<int>>) new List<LocalId<int>>(1)
      {
        (LocalId<int>) this.SettingsService.GetSettings().AssemblyDocumentType
      };
    }
  }
}
