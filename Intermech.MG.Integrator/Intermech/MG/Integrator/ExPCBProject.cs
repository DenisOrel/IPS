// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBProject
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;
using MGCPCB;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBProject : MGProject<MGCPCB.Document, Application>
{
  public ExPCBProject(
    MGCPCB.Document project,
    MGIntegratorSettings integratorSettings,
    IIntegratorOutput outputSvc,
    Application application,
    string pcbFile)
    : base(project, integratorSettings, outputSvc, application)
  {
    this.ValidateDocument(project);
  }

  public override Dictionary<string, IMGProjectItem> GetProjectItems()
  {
    this.ValidateDocument(this.Instance);
    Dictionary<string, IMGProjectItem> projectItems = new Dictionary<string, IMGProjectItem>(1);
    ExPCBDocument exPcbDocument = new ExPCBDocument(this, this.Instance, this.integratorSettings);
    this.relatedObjects.Add((IDisposable) exPcbDocument);
    projectItems.Add(this.Instance.FullName, (IMGProjectItem) exPcbDocument);
    return projectItems;
  }

  protected override IValueBagContainer GetProperties()
  {
    this.ValidateDocument(this.Instance);
    return (IValueBagContainer) new ExPCBAssemblyComponent(this.Instance, this.integratorSettings);
  }

  private void ValidateDocument(MGCPCB.Document document)
  {
    // ISSUE: reference to a compiler-generated method
    int token = LicenseServer.GetToken(document.Validate(0));
    // ISSUE: reference to a compiler-generated method
    document.Validate(token);
  }

  public override bool IsValid() => this.Instance.IsValid();

  protected override BoardReader<IMGProjectItem> GetBoardsReader(
    MGIntegratorSettings integratorSettings)
  {
    return (BoardReader<IMGProjectItem>) new ExPCBProjectBoardsReader((ECADIntegratorSettings) integratorSettings);
  }
}
