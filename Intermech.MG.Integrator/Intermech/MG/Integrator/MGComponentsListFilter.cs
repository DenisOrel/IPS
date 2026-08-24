// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGComponentsListFilter
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators.Electrical;
using System;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGComponentsListFilter : ComponentsListFilter
{
  private string _parameterName;

  public MGComponentsListFilter(MGIntegratorSettings settings, ComponentsListFilterType type)
    : base((ECADIntegratorSettings) settings, type)
  {
    this._parameterName = settings.FilterParameterName;
  }

  protected override bool CheckTable(
    IElectricalComponent component,
    out CompositionVariants variant)
  {
    variant = CompositionVariants.SpecificationAndElementsList;
    if (string.IsNullOrEmpty(this._parameterName))
      return true;
    string val = Convert.ToString(component.GetPropertyValue(this._parameterName));
    if (string.IsNullOrEmpty(val))
      return true;
    Tuple<StringKey, CompositionVariants> tuple = ((MGIntegratorSettings) this.settings).ComponentsFilter.Find((Predicate<Tuple<StringKey, CompositionVariants>>) (x => x.Item1.Equals(val)));
    if (tuple == null)
      return false;
    variant = tuple.Item2;
    return this.enabledVariants.Contains(tuple.Item2);
  }
}
