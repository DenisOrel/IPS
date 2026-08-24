// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGProject`2
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Data.SectionEntities;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;
using Intermech.Tools.Integrators.Mechanical;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal abstract class MGProject<TProject, TApp> : MGObject<TProject>, IMGProject, IDisposable
{
  protected MGIntegratorSettings integratorSettings;
  protected TApp app;
  protected readonly IIntegratorOutput outputSvc;

  public MGProject(
    TProject project,
    MGIntegratorSettings integratorSettings,
    IIntegratorOutput outputSvc,
    TApp application)
    : base(project)
  {
    this.app = application;
    this.integratorSettings = integratorSettings;
    this.outputSvc = outputSvc;
  }

  public abstract Dictionary<string, IMGProjectItem> GetProjectItems();

  protected abstract IValueBagContainer GetProperties();

  public IValueBagContainer Properties => this.GetProperties();

  public ICollection<InitialArticleData> GetArticles(SectionEntity documentItem)
  {
    LinkedList<InitialArticleData> articles = new LinkedList<InitialArticleData>();
    List<BoardData<IMGProjectItem>> projectBoards = this.GetProjectBoards();
    if (projectBoards != null)
    {
      ElectricalSchemeDescriptors bomAssemblies;
      articles = MGCompositionReader.ReadArticles(projectBoards, this.integratorSettings, this.outputSvc, out bomAssemblies, (IMGProject) this);
      documentItem.Sections.Set((object) bomAssemblies);
    }
    return (ICollection<InitialArticleData>) articles;
  }

  public List<BoardData<IMGProjectItem>> GetProjectBoards()
  {
    Dictionary<string, IMGProjectItem> projectItems = this.GetProjectItems();
    return this.GetBoardsReader(this.integratorSettings).GetBoards(projectItems);
  }

  protected abstract BoardReader<IMGProjectItem> GetBoardsReader(
    MGIntegratorSettings integratorSettings);

  public abstract bool IsValid();
}
