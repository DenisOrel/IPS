// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXIntegrator
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.NX.Integrator.Properties;
using Intermech.Tools;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.Tools.Integrators.CADInterface.ModelDrawings;
using Intermech.Tools.Integrators.StandaloneView;
using System;
using System.Diagnostics;
using System.Drawing;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXIntegrator : CADIntegrator
{
  protected override void DoCreateServices()
  {
    base.DoCreateServices();
    this.Services.Add((IIntegratorService) this.CreateNXDataExchangeExtensions());
    this.Services.Add((IIntegratorService) this.CreateNXMultiCADService());
    this.Services.Add((IIntegratorService) this.CreateArticleLaunchActionService());
    this.Services.Add((IIntegratorService) this.CreateCompositionCopyingService());
    this.Services.Add((IIntegratorService) this.CreateTechRequirementsService());
  }

  private NXDataExchangeExtensions CreateNXDataExchangeExtensions()
  {
    return new NXDataExchangeExtensions((IIntegrator) this);
  }

  private NXMultiCADService CreateNXMultiCADService() => new NXMultiCADService((IIntegrator) this);

  private CompositionCopyingService CreateCompositionCopyingService()
  {
    return new CompositionCopyingService((IIntegrator) this);
  }

  private ITechRequirementsService CreateTechRequirementsService()
  {
    return (ITechRequirementsService) new CADTechRequirementsService((IIntegrator) this);
  }

  protected override ICADSettingsFactory CreateSettingsFactory()
  {
    return (ICADSettingsFactory) new NXSettingsFactory((CADIntegrator) this);
  }

  protected override IIntegratorLicense CreateLicenseService()
  {
    return (IIntegratorLicense) new NXLicenseService((IIntegrator) this);
  }

  protected override IApplicationFileTypes CreateFileTypeService()
  {
    return (IApplicationFileTypes) new NXFileTypeService((IIntegrator) this);
  }

  protected override CADInterfaceService CreateCADInterfaceService()
  {
    return (CADInterfaceService) new NXCADInterfaceService((IIntegrator) this);
  }

  protected override IStandardPartLibraryService CreateStandardPartLibraryService()
  {
    return (IStandardPartLibraryService) new CADStandardPartLibraryService((IIntegrator) this, StandardLibraryMode.SeparateStandardSizes, "NX Library");
  }

  protected override IModelDrawingsService CreateModelDrawingsService()
  {
    ICADSettingsService service = ServiceUtils.GetService<ICADSettingsService>((object) this, true);
    NXModelDrawingsService modelDrawingsService = new NXModelDrawingsService((IIntegrator) this);
    modelDrawingsService.SettingsProvider = (IModelDrawingsServiceSettings) new CADModelDrawingsServiceSettings((IIntegrator) this, service);
    return (IModelDrawingsService) modelDrawingsService;
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

  protected override Guid GetPDMBrowserGuid() => new Guid("666f0c3f-a3a5-46d8-84d1-a8f609dac764");

  protected override CADCaptureChangesFactory CreateCaptureChangesFactory()
  {
    ICADSettingsService service = ServiceUtils.GetService<ICADSettingsService>((object) this, true);
    return (CADCaptureChangesFactory) new NXCaptureChangesFactory((IIntegrator) this)
    {
      SettingsService = service
    };
  }

  protected override IStandaloneViewService CreateStandaloneViewService()
  {
    StandaloneViewServiceBase standaloneViewService = (StandaloneViewServiceBase) base.CreateStandaloneViewService();
    standaloneViewService.TempFileStrategy = (TempFileStrategy) new SameDirectoryTempFileStrategy();
    return (IStandaloneViewService) standaloneViewService;
  }

  protected override void DoConfigureServices()
  {
    base.DoConfigureServices();
    new IMViewerExtensionModule(ServiceUtils.GetService<IIMViewerClientService>((object) ApplicationServices.Container, true)).AttachTo((IIntegrator) this);
  }

  public override string DisplayName
  {
    [DebuggerStepThrough] get => Plugin.IntegratorName;
  }

  public override Guid Id
  {
    [DebuggerStepThrough] get => Plugin.IntegratorId;
  }

  public override string GetServerObjectTemplate()
  {
    return this.GetServerObjectTemplateFromResource("Intermech.NX.Integrator.Resources.Integrator template.xml");
  }

  public override Image GetApplicationImage(AppImageSize imageSize)
  {
    if (imageSize == AppImageSize.Image16x16)
      return (Image) Resources.NX_16x16;
    return imageSize == AppImageSize.Image32x32 ? (Image) Resources.NX_32x32 : base.GetApplicationImage(imageSize);
  }
}
