// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.FunctionalGroupHelper
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators.Electrical;
using System;

#nullable disable
namespace Intermech.MG.Integrator;

internal static class FunctionalGroupHelper
{
  public static FunctionalGroup ReadFunctionalGroupFromComponent(
    MGIntegratorSettings settings,
    IPropertiesCollection component)
  {
    if (string.IsNullOrEmpty(settings.FGPosDesignation))
      return (FunctionalGroup) null;
    string posDesignation = Convert.ToString(component.GetPropertyValue(settings.FGPosDesignation));
    return string.IsNullOrEmpty(posDesignation) ? (FunctionalGroup) null : new FunctionalGroup(string.IsNullOrEmpty(settings.FGName) ? string.Empty : Convert.ToString(component.GetPropertyValue(settings.FGName)), string.IsNullOrEmpty(settings.FGDesignation) ? string.Empty : Convert.ToString(component.GetPropertyValue(settings.FGDesignation)), posDesignation);
  }
}
