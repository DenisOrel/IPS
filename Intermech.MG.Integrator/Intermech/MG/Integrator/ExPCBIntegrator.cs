// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBIntegrator
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.MG.Integrator.Properties;
using Intermech.Tools;
using Intermech.Tools.Integrators;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBIntegrator : MGIntegrator
{
  protected override void DoCreateServices()
  {
    base.DoCreateServices();
    this.Services.Add((IIntegratorService) new ExPCBFileTypeService((IIntegrator) this));
    this.Services.Add((IIntegratorService) this.CreateSettingsService());
    this.Services.Add((IIntegratorService) this.CreateApiService());
    this.Services.Add((IIntegratorService) this.CreateFileImportService());
    this.Services.Add((IIntegratorService) new MGCaptureChangesService((IIntegrator) this));
    this.Services.Add((IIntegratorService) this.CreateExtendedSaveService());
    this.Services.Add((IIntegratorService) new MGEmbedAttributesService((IIntegrator) this));
    this.Services.Add((IIntegratorService) this.CreateStandaloneViewService());
    this.Services.Add((IIntegratorService) this.CreateApplicationLauncherService());
    this.Services.Add((IIntegratorService) this.CreateLaunchActionService());
  }

  private LaunchActionService CreateLaunchActionService()
  {
    ExPCBLaunchActionService launchActionService = new ExPCBLaunchActionService((MGIntegrator) this);
    launchActionService.FileTypeService = ServiceUtils.GetService<IApplicationFileTypes>((object) this, true);
    launchActionService.FileVault = ClientContext.FileVault;
    return (LaunchActionService) launchActionService;
  }

  protected override MGSettingsService CreateSettingsService()
  {
    ExPCBSettingsService settingsService = new ExPCBSettingsService((IIntegrator) this);
    settingsService.ProjectDocumentType = DBHelper.CreateObjectTypeGID(MGConsts.ObjTypeExPCBProject);
    return (MGSettingsService) settingsService;
  }

  private ExPCBInterfaceService CreateApiService()
  {
    MGSettingsService service1 = ServiceUtils.GetService<MGSettingsService>((object) this, true);
    IApplicationFileTypes service2 = ServiceUtils.GetService<IApplicationFileTypes>((object) this, true);
    ExPCBInterfaceService apiService = new ExPCBInterfaceService((IIntegrator) this);
    apiService.SettingsService = service1;
    apiService.FileTypeService = service2;
    return apiService;
  }

  public override Guid Id => MGConsts.ExPCBIntegratorId;

  public override string DisplayName => MGConsts.ExPCBIntegratorName;

  public override Image GetApplicationImage(AppImageSize imageSize)
  {
    if (imageSize == AppImageSize.Image16x16)
      return (Image) Resources.epcb_16x16;
    return imageSize == AppImageSize.Image32x32 ? (Image) Resources.epcb_32x32 : base.GetApplicationImage(imageSize);
  }

  protected override ApplicationLauncherService GetApplicationLauncherService(IIntegrator owner)
  {
    return (ApplicationLauncherService) new ExPCBApplicationLauncherService(owner);
  }

  public override bool IsReadOnlyDocument => true;
}
