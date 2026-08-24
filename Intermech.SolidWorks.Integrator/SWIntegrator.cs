// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SWIntegrator
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Runtime.ComInterop;
using Intermech.Tools;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.Tools.Integrators.CADInterface.ModelDrawings;
using Intermech.Win32;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class SWIntegrator : CADIntegrator
{
  protected override void DoCreateServices()
  {
    base.DoCreateServices();
    this.Services.Add((IIntegratorService) this.CreateSWDataExchangeExtensions());
    this.Services.Add((IIntegratorService) this.CreateArticleLaunchActionService());
    this.Services.Add((IIntegratorService) this.CreateCompositionCopyingService());
    this.Services.Add((IIntegratorService) this.CreateTechRequirementsService());
  }

  private SWDataExchangeExtensions CreateSWDataExchangeExtensions()
  {
    return new SWDataExchangeExtensions((IIntegrator) this);
  }

  private CompositionCopyingService CreateCompositionCopyingService()
  {
    return new CompositionCopyingService((IIntegrator) this);
  }

  protected override IIntegratorLicense CreateLicenseService()
  {
    return (IIntegratorLicense) new SWLicenseService((IIntegrator) this);
  }

  protected override IApplicationFileTypes CreateFileTypeService()
  {
    return (IApplicationFileTypes) new SWSimpleFileTypeService((IIntegrator) this);
  }

  private ITechRequirementsService CreateTechRequirementsService()
  {
    return (ITechRequirementsService) new CADTechRequirementsService((IIntegrator) this);
  }

  protected override CADInterfaceService CreateCADInterfaceService()
  {
    return new CADInterfaceService((IIntegrator) this, SWConsts.IntegratorAppName, (ComObjectProvider) new ProgIdProvider(SWConsts.ProgID, true));
  }

  protected override IStandardPartLibraryService CreateStandardPartLibraryService()
  {
    return (IStandardPartLibraryService) new CADStandardPartLibraryService((IIntegrator) this, RegistryHelper.GetValue<int>(RegistryHive.CurrentUser, "Software\\Intermech\\CAD\\SolidWorks\\SolidWorksExt", "StandardsMethod", 0) != 1 ? StandardLibraryMode.EmbeddedStandardSizes : StandardLibraryMode.SeparateStandardSizes, "SW Library");
  }

  protected override IModelDrawingsService CreateModelDrawingsService()
  {
    ICADSettingsService service = ServiceUtils.GetService<ICADSettingsService>((object) this, true);
    NormalModelDrawingsService modelDrawingsService = new NormalModelDrawingsService((IIntegrator) this, SWConsts.DrawingFileExtension, new string[2]
    {
      SWConsts.AssemblyFileExtension,
      SWConsts.PartFileExtension
    });
    modelDrawingsService.SettingsProvider = (IModelDrawingsServiceSettings) new CADModelDrawingsServiceSettings((IIntegrator) this, service);
    return (IModelDrawingsService) modelDrawingsService;
  }

  protected override IStandaloneViewService CreateStandaloneViewService()
  {
    IApplicationFileTypes service1 = ServiceUtils.GetService<IApplicationFileTypes>((object) this, true);
    IDocumentApiService service2 = ServiceUtils.GetService<IDocumentApiService>((object) this, true);
    SWStandaloneViewService standaloneViewService = new SWStandaloneViewService((IIntegrator) this);
    standaloneViewService.FileTypeService = service1;
    standaloneViewService.DocumentApiService = service2;
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

  protected override Guid GetPDMBrowserGuid() => new Guid("566f0c3f-a3a5-46d8-84d1-a8f609dac764");

  protected override PDMBrowserService CreatePDMBrowserService(Guid cadSystemId)
  {
    return (PDMBrowserService) new SWPDMBrowserService((IIntegrator) this, cadSystemId);
  }

  protected override bool IsAttributesSharedStorage() => false;

  protected override void DoConfigureServices()
  {
    base.DoConfigureServices();
    new IMViewerExtensionModule(ServiceUtils.GetService<IIMViewerClientService>((object) ApplicationServices.Container, true)).AttachTo((IIntegrator) this);
  }

  public override Guid Id
  {
    [DebuggerStepThrough] get => SWConsts.SWIntegratorId;
  }

  public override string GetServerObjectTemplate()
  {
    return this.GetServerObjectTemplateFromResource("Intermech.SolidWorks.Integrator.Resources.Integrator template.xml");
  }

  public override Image GetApplicationImage(AppImageSize imageSize)
  {
    if (imageSize == AppImageSize.Image16x16)
      return (Image) Intermech.SolidWorks.Integrator.Properties.Resources.sw16;
    return imageSize == AppImageSize.Image32x32 ? (Image) Intermech.SolidWorks.Integrator.Properties.Resources.sw32 : base.GetApplicationImage(imageSize);
  }

  public override string DisplayName
  {
    [DebuggerStepThrough] get => SWConsts.DisplayIntegratorName;
  }
}
