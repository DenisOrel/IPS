// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGIntegrator
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.StandaloneView;

#nullable disable
namespace Intermech.MG.Integrator;

internal abstract class MGIntegrator : ConfigurableIntegrator
{
  protected ImportService CreateFileImportService()
  {
    IApplicationFileTypes service = ServiceUtils.GetService<IApplicationFileTypes>((object) this, true);
    ImportService fileImportService = new ImportService((IIntegrator) this);
    fileImportService.FileTypeService = service;
    return fileImportService;
  }

  protected MGExtendedSaveService CreateExtendedSaveService()
  {
    MGSettingsService service = ServiceUtils.GetService<MGSettingsService>((object) this, true);
    MGExtendedSaveService extendedSaveService = new MGExtendedSaveService((IIntegrator) this);
    extendedSaveService.SettingsService = service;
    return extendedSaveService;
  }

  protected StandaloneViewService CreateStandaloneViewService()
  {
    IApplicationFileTypes service1 = ServiceUtils.GetService<IApplicationFileTypes>((object) this, true);
    IDocumentApiService service2 = ServiceUtils.GetService<IDocumentApiService>((object) this, true);
    StandaloneViewService standaloneViewService = new StandaloneViewService((IIntegrator) this);
    standaloneViewService.FileTypeService = service1;
    standaloneViewService.DocumentApiService = service2;
    return standaloneViewService;
  }

  protected ApplicationLauncherService CreateApplicationLauncherService()
  {
    IApplicationApiService service1 = ServiceUtils.GetService<IApplicationApiService>((object) this, true);
    MGSettingsService service2 = ServiceUtils.GetService<MGSettingsService>((object) this, true);
    ApplicationLauncherService applicationLauncherService = this.GetApplicationLauncherService((IIntegrator) this);
    applicationLauncherService.ApiService = service1;
    applicationLauncherService.SettingsService = (IIntegratorSettingsService) service2;
    return applicationLauncherService;
  }

  protected override IPersistentIntegratorSettingsService GetSettingsService()
  {
    return ServiceUtils.GetService<IPersistentIntegratorSettingsService>((object) this, true);
  }

  protected override IIntegratorSettingsViewModelService TryGetSettingsViewModelService()
  {
    return ServiceUtils.GetService<IIntegratorSettingsViewModelService>((object) this, true);
  }

  protected abstract ApplicationLauncherService GetApplicationLauncherService(IIntegrator owner);

  protected abstract MGSettingsService CreateSettingsService();

  public virtual bool IsReadOnlyDocument => false;
}
