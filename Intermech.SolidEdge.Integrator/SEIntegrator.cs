// Decompiled with JetBrains decompiler
// Type: Intermech.SolidEdge.Integrator.SEIntegrator
// Assembly: Intermech.SolidEdge.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 213B90F8-0434-43B8-B8F6-9AF19E139193
// Assembly location: D:\IPS\Client\Intermech.SolidEdge.Integrator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Runtime.ComInterop;
using Intermech.Tools;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.Tools.Integrators.CADInterface.ModelDrawings;
using System;
using System.Diagnostics;
using System.Drawing;

#nullable disable
namespace Intermech.SolidEdge.Integrator;

internal sealed class SEIntegrator : CADIntegrator
{
  protected override void DoCreateServices()
  {
    base.DoCreateServices();
    this.Services.Add((IIntegratorService) this.CreateTechRequirementsService());
  }

  protected override IIntegratorLicense CreateLicenseService()
  {
    return (IIntegratorLicense) new SELicenseService((IIntegrator) this);
  }

  protected override IApplicationFileTypes CreateFileTypeService()
  {
    return (IApplicationFileTypes) new SEFileTypeService((IIntegrator) this);
  }

  protected override CADInterfaceService CreateCADInterfaceService()
  {
    return new CADInterfaceService((IIntegrator) this, Plugin.IntegratorAppName, (ComObjectProvider) new ProgIdProvider(Plugin.ProgID, true));
  }

  protected override IStandardPartLibraryService CreateStandardPartLibraryService()
  {
    return (IStandardPartLibraryService) new CADStandardPartLibraryService((IIntegrator) this, StandardLibraryMode.SeparateStandardSizes, "SE Library");
  }

  protected override IModelDrawingsService CreateModelDrawingsService()
  {
    ICADSettingsService service = ServiceUtils.GetService<ICADSettingsService>((object) this, true);
    NormalModelDrawingsService modelDrawingsService = new NormalModelDrawingsService((IIntegrator) this, SEConsts.DrawingFileExtension, new string[4]
    {
      SEConsts.AssemblyFileExtension,
      SEConsts.PartFileExtension,
      SEConsts.SheetMetalFileExtension,
      SEConsts.WeldmentFileExtension
    });
    modelDrawingsService.SettingsProvider = (IModelDrawingsServiceSettings) new CADModelDrawingsServiceSettings((IIntegrator) this, service);
    return (IModelDrawingsService) modelDrawingsService;
  }

  protected override CADAuthenticFilesService CreateAuthenticFilesService()
  {
    return (CADAuthenticFilesService) new SEAuthenticFilesService((IIntegrator) this);
  }

  protected override CADCaptureChangesFactory CreateCaptureChangesFactory()
  {
    return (CADCaptureChangesFactory) new SECaptureChangesFactory((IIntegrator) this);
  }

  protected override Guid GetPDMBrowserGuid() => new Guid("f909fc49-93bb-4a37-be91-6809783852ef");

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
    [DebuggerStepThrough] get => Plugin.IntegratorName;
  }

  public override Guid Id
  {
    [DebuggerStepThrough] get => Plugin.IntegratorId;
  }

  public override string GetServerObjectTemplate()
  {
    return this.GetServerObjectTemplateFromResource("Intermech.SolidEdge.Integrator.Resources.Integrator template.xml");
  }

  public override Image GetApplicationImage(AppImageSize imageSize)
  {
    if (imageSize == AppImageSize.Image16x16)
      return (Image) Intermech.SolidEdge.Integrator.Properties.Resources.SE_16x16;
    return imageSize == AppImageSize.Image32x32 ? (Image) Intermech.SolidEdge.Integrator.Properties.Resources.SE_32x32 : base.GetApplicationImage(imageSize);
  }
}
