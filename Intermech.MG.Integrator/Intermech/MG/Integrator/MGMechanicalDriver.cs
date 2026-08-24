// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGMechanicalDriver
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using ImSSP;
using Intermech.Data.SectionEntities;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Tools.Components.Integrators.Electrical;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;
using Intermech.Tools.Integrators.Mechanical;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGMechanicalDriver(IIntegrator integrator) : ECADMechanicalDriver(integrator)
{
  private IApplicationFileTypes fileTypeSvc;
  private MGDocumentApi _mgDocumentApi;
  private MGIntegratorSettings integratorSettings;

  public MGIntegratorSettings IntegratorSettings => this.integratorSettings;

  protected override void InitializeDriver()
  {
    base.InitializeDriver();
    this.fileTypeSvc = ServiceUtils.GetService<IApplicationFileTypes>((object) this.Integrator, true);
    this.integratorSettings = ServiceUtils.GetService<MGSettingsService>((object) this.Integrator, true).GetSettings();
  }

  public IMGApplication App
  {
    get
    {
      return ServiceUtils.GetService<IApplicationApiService>((object) this.Integrator, true).GetApplicationObject() as IMGApplication;
    }
  }

  protected override void ClearDriver()
  {
    if (this.Active && this.SaveChangesMode != SaveChangesMode.Checkin)
    {
      ProjectInfo projectInfo = this.DriverContext.Database.GetEntryPointDocument(true).Sections.Get<ProjectInfo>(new ProjectInfo());
      if (projectInfo != null && projectInfo.NeedOpenAfterSave)
        this.App.OpenProject(projectInfo.ProjectFile, false);
    }
    base.ClearDriver();
    this.fileTypeSvc = (IApplicationFileTypes) null;
    this.integratorSettings = (MGIntegratorSettings) null;
    this._mgDocumentApi = (MGDocumentApi) null;
  }

  protected override void InitializeDriverContextServices()
  {
    base.InitializeDriverContextServices();
    this._mgDocumentApi = new MGDocumentApi(this, this.DriverContext);
  }

  protected override IArticleExternalKeysService CreateDefaultArticleExternalKeysService()
  {
    return (IArticleExternalKeysService) new MGArticleExternalKeysService((MechanicalDriver) this, this.DriverContext);
  }

  protected override IArticleTypesService CreateDefaultArticleTypesService()
  {
    return (IArticleTypesService) new MGArticleTypesService((MechanicalDriver) this, this.DriverContext);
  }

  protected override IArticleStructureService CreateDefaultArticleStructureService()
  {
    return (IArticleStructureService) new ElectricalArticleStructureService((AppMechanicalDriver) this, this.DriverContext);
  }

  protected override void ValidateRootFile(string rootFilePath, long rootObjectId)
  {
    base.ValidateRootFile(rootFilePath, rootObjectId);
    this.CheckIfDocument(rootFilePath);
  }

  private void CheckIfDocument(string rootFilePath)
  {
    if (!this.fileTypeSvc.IsApplicationFile(rootFilePath))
      throw new FaultException(string.Format(sc_14675.ssp_mentor_14676(), (object) rootFilePath));
  }

  protected override ICollection<Type> GetRemovableSectionTypes()
  {
    ICollection<Type> removableSectionTypes = base.GetRemovableSectionTypes();
    removableSectionTypes.Add(typeof (IMGApplication));
    removableSectionTypes.Add(typeof (ElectricalArticleCache));
    return removableSectionTypes;
  }

  protected override IDocumentCADApiService DoTryGetDocumentApiService(SectionEntity documentItem)
  {
    return (IDocumentCADApiService) this._mgDocumentApi;
  }

  protected override IArticleCADApiService DoTryGetArticleApiService(SectionEntity articleItem)
  {
    return articleItem.Sections.Contains<ElectricalArticleCache>() ? (IArticleCADApiService) this._mgDocumentApi : base.DoTryGetArticleApiService(articleItem);
  }

  public override DocumentFileData OpenDocumentFile(SectionEntity documentItem, string fullPath)
  {
    if (documentItem == null)
      throw new ArgumentNullException(nameof (documentItem));
    if (fullPath == null)
      throw new ArgumentNullException(nameof (fullPath));
    this.App.OpenProject(fullPath, false);
    DocumentFileData documentFileData = new DocumentFileData(fullPath);
    documentFileData.CustomSections.Set((object) this.App, typeof (IMGApplication));
    return documentFileData;
  }

  public override bool IsDocumentTypeSupported(int documentType)
  {
    return this.integratorSettings.AssemblyDocumentType.Id == documentType;
  }

  public override MechanicalDocumentKind GetMechanicalDocumentKindByType(int documentType)
  {
    return MechanicalDocumentKind.AssemblyModel;
  }

  public override List<LocalId<int>> GetTypesByMechanicalDocumentKind(
    MechanicalDocumentKind documentKind)
  {
    if (documentKind != MechanicalDocumentKind.AssemblyModel)
      throw new NotSupportedException();
    return new List<LocalId<int>>()
    {
      (LocalId<int>) this.integratorSettings.AssemblyDocumentType
    };
  }

  protected override ImbaseObjectArticleHandler CreateImbaseObjectArticleHandler(
    SectionEntity articleEntity)
  {
    return this.integratorSettings.ImbaseSync ? (ImbaseObjectArticleHandler) new ECADImbaseObjectArticleHandler((MechanicalDriver) this, this.DriverContext, articleEntity, (ECADIntegratorSettings) this.integratorSettings) : base.CreateImbaseObjectArticleHandler(articleEntity);
  }
}
