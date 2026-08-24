// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGProjectItem`1
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators.Electrical;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal abstract class MGProjectItem<TComponent> : MGObject<TComponent>, IMGProjectItem
{
  protected MGIntegratorSettings integratorSettings;
  protected IMGProject parent;

  public MGProjectItem(IMGProject parent, TComponent item, MGIntegratorSettings integratorSettings)
    : base(item)
  {
    this.integratorSettings = integratorSettings;
    this.parent = parent;
  }

  public abstract List<IElectricalComponent> Components { get; }

  public abstract IElectricalComponent AssemblyComponent { get; }
}
