// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.AIIntegrator
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using Intermech.AI.Integrator.Properties;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Tools;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.Tools.Integrators.CADInterface.ModelDrawings;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.AI.Integrator;

internal sealed class AIIntegrator : CADIntegrator
{
  protected override IIntegratorLicense CreateLicenseService()
  {
    return (IIntegratorLicense) new AILicenseService((IIntegrator) this);
  }

  protected override IApplicationFileTypes CreateFileTypeService()
  {
    return (IApplicationFileTypes) new AIFileTypeService((IIntegrator) this);
  }

  protected override CADInterfaceService CreateCADInterfaceService()
  {
    return (CADInterfaceService) new AICADInterfaceService((IIntegrator) this);
  }

  protected override IStandardPartLibraryService CreateStandardPartLibraryService()
  {
    return (IStandardPartLibraryService) new AIStandardPartLibraryService((IIntegrator) this, StandardLibraryMode.SeparateStandardSizes, "AI Library");
  }

  protected override IModelDrawingsService CreateModelDrawingsService()
  {
    ICADSettingsService service = ServiceUtils.GetService<ICADSettingsService>((object) this, true);
    AIModelDrawingsService modelDrawingsService = new AIModelDrawingsService((IIntegrator) this, AIConsts.DrawingFileExtension, new string[2]
    {
      AIConsts.AssemblyFileExtension,
      AIConsts.PartFileExtension
    });
    modelDrawingsService.SettingsProvider = (IModelDrawingsServiceSettings) new CADModelDrawingsServiceSettings((IIntegrator) this, service);
    return (IModelDrawingsService) modelDrawingsService;
  }

  private CompositionCopyingService CreateCompositionCopyingService()
  {
    return new CompositionCopyingService((IIntegrator) this);
  }

  private ITechRequirementsService CreateTechRequirementsService()
  {
    return (ITechRequirementsService) new CADTechRequirementsService((IIntegrator) this);
  }

  protected override Guid GetPDMBrowserGuid() => new Guid("4C38B1F3-C932-458D-9900-A33BE7506CB7");

  protected override void DoCreateServices()
  {
    base.DoCreateServices();
    this.Services.Add((IIntegratorService) this.CreateCompositionCopyingService());
    this.Services.Add((IIntegratorService) this.CreateTechRequirementsService());
  }

  protected override void DoConfigureServices()
  {
    base.DoConfigureServices();
    new IMViewerExtensionModule(ServiceUtils.GetService<IIMViewerClientService>((object) ApplicationServices.Container, true)).AttachTo((IIntegrator) this);
  }

  public override string DisplayName => AIConsts.IntegratorName;

  public override Guid Id => AIConsts.IntegratorId;

  public override string GetServerObjectTemplate()
  {
    return this.GetServerObjectTemplateFromResource("Intermech.AI.Integrator.Resources.Integrator template.xml");
  }

  public override Image GetApplicationImage(AppImageSize imageSize)
  {
    if (imageSize == AppImageSize.Image16x16)
      return (Image) Resources.IR_AI2009;
    return imageSize == AppImageSize.Image32x32 ? (Image) Resources.IR_AI2009_32x32 : base.GetApplicationImage(imageSize);
  }
}
