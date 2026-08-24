// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.SettingsValidator
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.Settings;
using System;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class SettingsValidator(string integratorName) : IntegratorSettingsValidator(integratorName)
{
  protected override string DoValidate(
    ISettingsObject settingsObject,
    SettingsValidatorContext context)
  {
    return settingsObject != null ? base.DoValidate(settingsObject, context) : throw new ArgumentNullException(nameof (settingsObject));
  }
}
