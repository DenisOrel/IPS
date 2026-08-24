// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGSettingsService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.Tools.Integrators;
using Intermech.Tools.Settings;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.MG.Integrator;

internal abstract class MGSettingsService : 
  IntegratorSettingsService<MGIntegratorSettings>,
  IDocumentAttributesSettingsService,
  IIntegratorSettingsService,
  IIntegratorService,
  IIntegratorSettingsViewModelService
{
  protected GlobalId<int> projectDocumentType;
  private SynchronizedMGArticleAttributes _assemblyAttributes;
  private SynchronizedMGArticleAttributes _partAttributes;
  private SynchronizedMGDocumentAttributes _docAttributes;

  public MGSettingsService(IIntegrator owner)
    : base(owner)
  {
  }

  protected override void DoAfterInitialize()
  {
    base.DoAfterInitialize();
    this._assemblyAttributes = new SynchronizedMGArticleAttributes(this, true);
    this._partAttributes = new SynchronizedMGArticleAttributes(this, false);
    this._docAttributes = new SynchronizedMGDocumentAttributes(this);
  }

  protected override IntegratorSettingsCodec CreateSettingsCodec()
  {
    return (IntegratorSettingsCodec) new SettingsCodec(this.Integrator);
  }

  protected override IntegratorSettingsValidator CreateSettingsValidator()
  {
    return (IntegratorSettingsValidator) new SettingsValidator(this.Integrator.DisplayName);
  }

  object IIntegratorSettingsViewModelService.CreateViewModel(ISettingsObject settingsObject)
  {
    if (settingsObject == null)
      throw new ArgumentNullException(nameof (settingsObject));
    this.RequireReadyState();
    return this.CreateSettingsSurrogate((MGIntegratorSettings) settingsObject);
  }

  protected abstract object CreateSettingsSurrogate(MGIntegratorSettings settings);

  ISettingsObject IIntegratorSettingsViewModelService.CreateSettingsFromViewModel(
    object viewModelObject)
  {
    if (viewModelObject == null)
      throw new ArgumentNullException(nameof (viewModelObject));
    this.RequireReadyState();
    MGIntegratorSettings settingsFromViewModel = this.RestoreSettings(viewModelObject);
    settingsFromViewModel.AssemblyDocumentType = this.projectDocumentType;
    return (ISettingsObject) settingsFromViewModel;
  }

  protected abstract MGIntegratorSettings RestoreSettings(object viewModelObject);

  public ISynchronizedObjectAttributes AssemblyAttributes
  {
    [DebuggerStepThrough] get
    {
      this.RequireReadyState();
      return (ISynchronizedObjectAttributes) this._assemblyAttributes;
    }
  }

  public ISynchronizedObjectAttributes PartAttributes
  {
    [DebuggerStepThrough] get
    {
      this.RequireReadyState();
      return (ISynchronizedObjectAttributes) this._partAttributes;
    }
  }

  public ISynchronizedObjectAttributes SynchronizedDocumentAttributes
  {
    [DebuggerStepThrough] get
    {
      this.RequireReadyState();
      return (ISynchronizedObjectAttributes) this._docAttributes;
    }
  }

  public GlobalId<int> ProjectDocumentType
  {
    get => this.projectDocumentType;
    set => this.projectDocumentType = value;
  }
}
