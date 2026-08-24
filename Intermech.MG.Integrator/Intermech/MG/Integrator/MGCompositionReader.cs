// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGCompositionReader
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;
using Intermech.Tools.Integrators.Mechanical;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGCompositionReader : ElectricalCompositionReader<IMGProjectItem>
{
  private IMGProject _project;

  public MGCompositionReader(
    List<BoardData<IMGProjectItem>> boards,
    MGIntegratorSettings integratorSettings,
    IIntegratorOutput outputSvc,
    IMGProject project)
    : base(boards, (ECADIntegratorSettings) integratorSettings, outputSvc)
  {
    this._project = project;
  }

  public static LinkedList<InitialArticleData> ReadArticles(
    List<BoardData<IMGProjectItem>> boards,
    MGIntegratorSettings integratorSettings,
    IIntegratorOutput outputSvc,
    out ElectricalSchemeDescriptors bomAssemblies,
    IMGProject project)
  {
    return new MGCompositionReader(boards, integratorSettings, outputSvc, project).ReadArticles(out bomAssemblies);
  }

  protected override IComponentsListFilter GetFilter(ComponentsListFilterType type)
  {
    return (IComponentsListFilter) new MGComponentsListFilter((MGIntegratorSettings) this.integratorSettings, ComponentsListFilterType.CompositionAndElementsList);
  }

  protected override List<IElectricalComponent> ReadComponents(IMGProjectItem projectItem)
  {
    return projectItem.Components;
  }

  protected override string GetAssemblyPropertyValue(
    IValueBagContainer asmComponent,
    string propertyName)
  {
    return Convert.ToString(((IPropertiesCollection) asmComponent).GetPropertyValue(propertyName));
  }

  protected override void OnCreateRootAssembly(
    LinkedList<InitialArticleData> articleBlanks,
    Dictionary<InitialArticleData, BoardData<IMGProjectItem>> childAssemblies,
    out PrintBoardDescriptor descriptor)
  {
    InitialArticleData initialArticleData = new InitialArticleData(MechanicalArticleKind.Autodetect);
    initialArticleData.InitialDocumentType = ArticleInitialDocumentType.Normal;
    descriptor = this.MakeDescriptor(this._project.Properties, this.integratorSettings.DocumentAttributesTable, true);
    initialArticleData.DisplayName = $"{descriptor.Designation}({descriptor.Name})";
    initialArticleData.ArticleKey = descriptor.Designation;
    ElectricalArticleCache electricalArticleCache = new ElectricalArticleCache(this._project.Properties, ArticleTypes.VirtualAssembly);
    this.CreateComposition(electricalArticleCache, childAssemblies);
    initialArticleData.CustomSections.Set((object) electricalArticleCache);
    articleBlanks.AddFirst(initialArticleData);
  }

  protected override FunctionalGroup ReadFunctionalGroupFromComponent(
    IPropertiesCollection component)
  {
    return FunctionalGroupHelper.ReadFunctionalGroupFromComponent((MGIntegratorSettings) this.integratorSettings, component);
  }
}
