// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.PEIntegrator
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Runtime.ComInterop;
using Intermech.Tools;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.Tools.Integrators.CADInterface.ModelDrawings;
using Intermech.Tools.Integrators.StandaloneView;
using System;
using System.Diagnostics;
using System.Drawing;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class PEIntegrator : CADIntegrator
{
  protected override void DoCreateServices()
  {
    base.DoCreateServices();
    this.Services.Add((IIntegratorService) new PEPackAndGoService((IIntegrator) this));
    this.Services.Add((IIntegratorService) this.CreateArticleLaunchActionService());
    this.Services.Add((IIntegratorService) this.CreateTechRequirementsService());
  }

  protected override IIntegratorLicense CreateLicenseService()
  {
    return (IIntegratorLicense) new PELicenseService((IIntegrator) this);
  }

  protected override IApplicationFileTypes CreateFileTypeService()
  {
    return (IApplicationFileTypes) new PEFileTypeService((IIntegrator) this);
  }

  protected override CADLaunchActionService CreateLaunchActionService()
  {
    return (CADLaunchActionService) new PELaunchActionService((IIntegrator) this);
  }

  protected override CADFileImportSupportService CreateFileImportService(
    CADCaptureChangesFactory factory)
  {
    return factory != null ? (CADFileImportSupportService) new PEFileImportService((IIntegrator) this, factory) : throw new ArgumentNullException(nameof (factory));
  }

  protected override CADCaptureChangesFactory CreateCaptureChangesFactory()
  {
    return (CADCaptureChangesFactory) new PECaptureChangesFactory((IIntegrator) this);
  }

  protected override CADCaptureChangesService CreateCaptureChangesService(
    CADCaptureChangesFactory factory)
  {
    return factory != null ? (CADCaptureChangesService) new PECaptureChangesService((IIntegrator) this, factory) : throw new ArgumentNullException(nameof (factory));
  }

  protected override CADExtendedSaveService CreateExtendedSaveService(
    CADCaptureChangesFactory factory)
  {
    return factory != null ? (CADExtendedSaveService) new PEExtendedSaveService((IIntegrator) this, factory) : throw new ArgumentNullException(nameof (factory));
  }

  protected override CADInterfaceService CreateCADInterfaceService()
  {
    return new CADInterfaceService((IIntegrator) this, PEConsts.AppName, (ComObjectProvider) new ProgIdProvider(PEConsts.ProgID, true));
  }

  protected override IStandardPartLibraryService CreateStandardPartLibraryService()
  {
    return (IStandardPartLibraryService) new PEStandardPartLibraryService((IIntegrator) this);
  }

  protected override IModelDrawingsService CreateModelDrawingsService()
  {
    ICADSettingsService service = ServiceUtils.GetService<ICADSettingsService>((object) this, true);
    NormalModelDrawingsService modelDrawingsService = new NormalModelDrawingsService((IIntegrator) this, PEConsts.DrawingFileExtension, new string[3]
    {
      PEConsts.AssemblyFileExtension,
      PEConsts.ManufacturingFileExtension,
      PEConsts.PartFileExtension
    });
    modelDrawingsService.SettingsProvider = (IModelDrawingsServiceSettings) new CADModelDrawingsServiceSettings((IIntegrator) this, service);
    return (IModelDrawingsService) modelDrawingsService;
  }

  protected override CADAuthenticFilesService CreateAuthenticFilesService()
  {
    return (CADAuthenticFilesService) new PEAuthenticFilesService((IIntegrator) this);
  }

  protected override IStandaloneViewService CreateStandaloneViewService()
  {
    StandaloneViewServiceBase standaloneViewService = (StandaloneViewServiceBase) base.CreateStandaloneViewService();
    standaloneViewService.TempFileStrategy = (TempFileStrategy) new SameDirectoryTempFileStrategy();
    return (IStandaloneViewService) standaloneViewService;
  }

  private CADArticleLaunchActionService CreateArticleLaunchActionService()
  {
    ICADSettingsService service1 = ServiceUtils.GetService<ICADSettingsService>((object) this, true);
    ICADInterfaceService service2 = ServiceUtils.GetService<ICADInterfaceService>((object) this, true);
    return new CADArticleLaunchActionService((IIntegrator) this)
    {
      SettingsService = service1,
      ApiService = service2
    };
  }

  protected override Guid GetPDMBrowserGuid() => new Guid("ce06f8e5-46ae-47d4-9c07-4eb144dd3c14");

  private ITechRequirementsService CreateTechRequirementsService()
  {
    return (ITechRequirementsService) new CADTechRequirementsService((IIntegrator) this);
  }

  protected override void DoConfigureServices()
  {
    base.DoConfigureServices();
    new IMViewerExtensionModule(ServiceUtils.GetService<IIMViewerClientService>((object) ApplicationServices.Container, true)).AttachTo((IIntegrator) this);
  }

  public override string DisplayName
  {
    [DebuggerStepThrough] get => PEConsts.IntegratorName;
  }

  public override Guid Id
  {
    [DebuggerStepThrough] get => PEConsts.PEIntegratorId;
  }

  public override string GetServerObjectTemplate()
  {
    return this.GetServerObjectTemplateFromResource("Intermech.ProEngineer.Integrator.Resources.Integrator template.xml");
  }

  public override Image GetApplicationImage(AppImageSize imageSize)
  {
    if (imageSize == AppImageSize.Image16x16)
      return (Image) Intermech.ProEngineer.Integrator.Properties.Resources.pe16;
    return imageSize == AppImageSize.Image32x32 ? (Image) Intermech.ProEngineer.Integrator.Properties.Resources.pe32 : base.GetApplicationImage(imageSize);
  }
}
