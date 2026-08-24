// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXSettingsFactory
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.Tools.Settings;
using System.Collections.Generic;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXSettingsFactory(CADIntegrator integrator) : CADSettingsFactory(integrator)
{
  protected override CADSettings DoCreateSettingsObject() => (CADSettings) new NXSettings();

  protected override CADSettingsViewModel DoCreateSettingsViewModel()
  {
    return (CADSettingsViewModel) new NXSettingsViewModel(this);
  }

  protected override CADSettingsCodec DoCreateCodec(
    string integratorName,
    ISettingsObjectFactory factory)
  {
    return (CADSettingsCodec) new NXSettingsCodec(integratorName, factory);
  }

  protected override void DoCreateValidatorChecks(
    CADIntegrator integrator,
    List<ISettingsValidatorCheck> checkList)
  {
    base.DoCreateValidatorChecks(integrator, checkList);
    checkList.Add((ISettingsValidatorCheck) new DocumentTypeFileExtensionsCheck((IIntegrator) integrator));
  }
}
