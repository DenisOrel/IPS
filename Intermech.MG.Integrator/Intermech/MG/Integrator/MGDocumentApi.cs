// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGDocumentApi
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Collections;
using Intermech.Data;
using Intermech.Data.SectionEntities;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Data;
using Intermech.Tools.Data;
using Intermech.Tools.DataExchange;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;
using Intermech.Tools.Integrators.Mechanical;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGDocumentApi : IArticleCADApiService, IDocumentCADApiService
{
  private readonly MGSettingsService settingsSvc;
  private readonly IAttributeCodecCollection apiSvc;
  private readonly IAttributeCodec _documentCodec;
  private readonly IAttributeCodec _partCodec;
  private readonly IAttributeCodec _assemblyCodec;
  private readonly MGMechanicalDriver driver;
  private readonly CaptureChangesDriverContext driverContext;
  private readonly IFileVault fileVault;
  private readonly MGIntegrator _integrator;

  public MGDocumentApi(MGMechanicalDriver driver, CaptureChangesDriverContext driverContext)
  {
    if (driver == null)
      throw new ArgumentNullException(nameof (driver));
    if (driverContext == null)
      throw new ArgumentNullException(nameof (driverContext));
    this.driver = driver;
    this.driverContext = driverContext;
    this.fileVault = ServiceUtils.GetService<IFileVault>((object) ServicesManager.ServiceContainer, true);
    this.settingsSvc = ServiceUtils.GetService<MGSettingsService>((object) driver.Integrator, true);
    this.apiSvc = ServiceUtils.GetService<IAttributeCodecCollection>((object) driver.Integrator, true);
    this._documentCodec = this.apiSvc.GetDocumentCodec();
    this._assemblyCodec = this.apiSvc.GetAssemblyCodec();
    this._partCodec = this.apiSvc.GetPartCodec();
    this._integrator = this.driver.Integrator as MGIntegrator;
  }

  public ICollection<InitialArticleData> ReadArticles(SectionEntity documentItem)
  {
    IMGApplication mgApplication = documentItem != null ? documentItem.Sections.Get<IMGApplication>() : throw new ArgumentNullException(nameof (documentItem));
    if (mgApplication.CurrentProject == null)
      return (ICollection<InitialArticleData>) new InitialArticleData[0];
    ICollection<InitialArticleData> articles = mgApplication.CurrentProject.GetArticles(documentItem);
    if (ImbaseSynchronizationHepler.Synchronize(articles, (ECADIntegratorSettings) this.settingsSvc.GetSettings(), this._partCodec))
      return articles;
    throw new AbortException("Не удалось синхронизировать компоненты с Imbase");
  }

  private ICollection<StringKey> GetAssemblyFileAttributes(SectionEntity articleItem)
  {
    return this.settingsSvc.AssemblyAttributes.GetAttributes(ObjectSection.TryGetObjectType(articleItem), false);
  }

  private ICollection<StringKey> GetPartFileAttributes(SectionEntity articleItem)
  {
    return this.settingsSvc.PartAttributes.GetAttributes(ObjectSection.TryGetObjectType(articleItem), false);
  }

  private ICollection<StringKey> GetVirtualAssemblyFileAttributes(SectionEntity articleItem)
  {
    return (ICollection<StringKey>) new List<StringKey>()
    {
      (StringKey) IDCache.Default.Name.Text,
      (StringKey) IDCache.Default.Designation.Text
    };
  }

  public ContainerValues ReadArticleProperties(SectionEntity articleItem)
  {
    ElectricalArticleCache electricalArticleCache = articleItem.Sections.Get<ElectricalArticleCache>();
    if (electricalArticleCache.ArticleType == ArticleTypes.Component)
      return this._partCodec.ReadFileProperties(electricalArticleCache.Article, this.GetPartFileAttributes(articleItem));
    if (this._integrator.IsReadOnlyDocument)
      return new ContainerValues(new ValueBag(), false);
    if (electricalArticleCache.ArticleType == ArticleTypes.Assembly)
      return this._assemblyCodec.ReadFileProperties(electricalArticleCache.Article, this.GetAssemblyFileAttributes(articleItem));
    return electricalArticleCache.ArticleType == ArticleTypes.VirtualAssembly ? this._documentCodec.ReadFileProperties(electricalArticleCache.Article, this.GetVirtualAssemblyFileAttributes(articleItem)) : new ContainerValues(new ValueBag(), false);
  }

  public bool WriteArticleProperties(SectionEntity articleItem, ContainerValues fileProperties)
  {
    ElectricalArticleCache electricalArticleCache = articleItem.Sections.Get<ElectricalArticleCache>();
    if (electricalArticleCache.ArticleType == ArticleTypes.Component)
      return this._partCodec.Formatter.Write(electricalArticleCache.Article, fileProperties);
    if (electricalArticleCache.ArticleType == ArticleTypes.Assembly && !this._integrator.IsReadOnlyDocument)
      this._assemblyCodec.Formatter.Write(electricalArticleCache.Article, fileProperties);
    return false;
  }

  public ValueBag DecodeArticleAttributes(SectionEntity articleItem, ContainerValues fileProperties)
  {
    ElectricalArticleCache electricalArticleCache = articleItem.Sections.Get<ElectricalArticleCache>();
    DecodeAttributesOptions decodeOptions = this.driver.MechanicalOperations.Articles.GetDecodeOptions(articleItem);
    ICollection<StringKey> attributeKeys = (ICollection<StringKey>) null;
    if (electricalArticleCache.ArticleType == ArticleTypes.Component)
      attributeKeys = this.GetPartFileAttributes(articleItem);
    else if (electricalArticleCache.ArticleType == ArticleTypes.Assembly)
      attributeKeys = this.GetAssemblyFileAttributes(articleItem);
    else if (electricalArticleCache.ArticleType == ArticleTypes.VirtualAssembly)
      attributeKeys = this.GetVirtualAssemblyFileAttributes(articleItem);
    DecodeAttributesParams decodeParams = new DecodeAttributesParams(electricalArticleCache.Article, attributeKeys, fileProperties, decodeOptions);
    if (electricalArticleCache.ArticleType == ArticleTypes.Component)
      return this._partCodec.Decode(decodeParams);
    if (this._integrator.IsReadOnlyDocument && ObjectSection.IsNewObject(articleItem))
      return new ValueBag();
    if (electricalArticleCache.ArticleType == ArticleTypes.Assembly)
      return this._assemblyCodec.Decode(decodeParams);
    if (electricalArticleCache.ArticleType != ArticleTypes.VirtualAssembly)
      return new ValueBag();
    if (decodeParams.Options.Properties.ContainsKey((StringKey) "DocumentType"))
      decodeParams.Options.Properties[(StringKey) "DocumentType"] = (object) -1;
    return this._documentCodec.Decode(decodeParams);
  }

  public void EncodeArticleAttributes(
    SectionEntity articleItem,
    ICollection<StringKey> attributeKeys,
    ValueBag attributes,
    ContainerValues fileProperties)
  {
    ElectricalArticleCache electricalArticleCache = articleItem.Sections.Get<ElectricalArticleCache>();
    EncodeAttributesOptions encodeOptions = this.driver.MechanicalOperations.Articles.GetEncodeOptions(articleItem);
    EncodeAttributesParams encodeParams = new EncodeAttributesParams(electricalArticleCache.Article, attributeKeys, attributes, fileProperties, encodeOptions);
    encodeParams.ContainerDisplayName = DisplaySection.GetQualifiedName(articleItem);
    if (electricalArticleCache.ArticleType == ArticleTypes.Component)
    {
      this._partCodec.Encode(encodeParams);
    }
    else
    {
      if (electricalArticleCache.ArticleType != ArticleTypes.Assembly || this._integrator.IsReadOnlyDocument)
        return;
      this._assemblyCodec.Encode(encodeParams);
    }
  }

  public ICollection<StringKey> GetArticleSyncAttributes(SectionEntity articleItem)
  {
    ElectricalArticleCache electricalArticleCache = articleItem.Sections.Get<ElectricalArticleCache>();
    if (electricalArticleCache.ArticleType == ArticleTypes.Component)
      return this.GetPartFileAttributes(articleItem);
    if (electricalArticleCache.ArticleType == ArticleTypes.Assembly)
      return this.GetAssemblyFileAttributes(articleItem);
    return electricalArticleCache.ArticleType == ArticleTypes.VirtualAssembly ? this.GetVirtualAssemblyFileAttributes(articleItem) : (ICollection<StringKey>) null;
  }

  public IFileDependenciesHandler TryGetFileDependenciesHandler(SectionEntity docItem)
  {
    return (IFileDependenciesHandler) null;
  }

  public string GetDocumentTypeAttributeName(SectionEntity docItem) => "Document type";

  public List<LocalId<int>> DetectNewDocumentType(SectionEntity docItem)
  {
    return CollectionUtils.CreateList<LocalId<int>>((LocalId<int>) this.settingsSvc.ProjectDocumentType);
  }

  private ICollection<StringKey> GetDocumentFileAttributes(SectionEntity docItem)
  {
    return this.settingsSvc.SynchronizedDocumentAttributes.GetAttributes(ObjectSection.TryGetObjectType(docItem), false);
  }

  public ContainerValues ReadDocumentProperties(SectionEntity docItem)
  {
    if (this._integrator.IsReadOnlyDocument)
      return new ContainerValues(new ValueBag(), false);
    IMGApplication mgApplication = docItem.Sections.Get<IMGApplication>();
    if (mgApplication.CurrentProject != null)
    {
      IValueBagContainer properties = mgApplication.CurrentProject.Properties;
      if (properties != null)
        return this._documentCodec.ReadFileProperties(properties, this.GetDocumentFileAttributes(docItem));
    }
    return new ContainerValues(new ValueBag(), false);
  }

  public bool WriteDocumentProperties(SectionEntity docItem, ContainerValues fileProperties)
  {
    IMGApplication mgApplication = docItem.Sections.Get<IMGApplication>();
    return mgApplication.CurrentProject != null && this._documentCodec.Formatter.Write(mgApplication.CurrentProject.Properties, fileProperties);
  }

  public void SaveDocumentFile(SectionEntity docItem)
  {
    string projectFile = docItem.Sections.Get<IMGApplication>().ProjectFile;
    if (this.driver.App.CloseProjectBeforeSave())
      docItem.Sections.Set((object) new ProjectInfo(projectFile, true));
    bool flag = this.driver.App.FileLocked(projectFile);
    int num = 600;
    for (int index = 0; flag && index < num; ++index)
    {
      Thread.Sleep(100);
      flag = this.driver.App.FileLocked(projectFile);
    }
  }

  public ValueBag DecodeDocumentAttributes(SectionEntity docItem, ContainerValues fileProperties)
  {
    if (this._integrator.IsReadOnlyDocument)
      return ObjectSection.IsNewObject(docItem) ? new ValueBag() : this.driver.Operations.Db.ReadObjectAttributes(docItem, (IDBAttributableTypeRef) new DirectObjectAttributesRef(ObjectSection.GetObjectType(docItem)));
    IMGApplication mgApplication = docItem.Sections.Get<IMGApplication>();
    if (mgApplication.CurrentProject != null)
    {
      IValueBagContainer properties = mgApplication.CurrentProject.Properties;
      if (properties != null)
      {
        DecodeAttributesOptions decodeOptions = this.driver.Operations.Documents.GetDecodeOptions(docItem);
        return this._documentCodec.Decode(new DecodeAttributesParams(properties, this.GetDocumentFileAttributes(docItem), fileProperties, decodeOptions));
      }
    }
    return new ValueBag();
  }

  public void EncodeDocumentAttributes(
    SectionEntity docItem,
    ICollection<StringKey> attributeKeys,
    ValueBag attributes,
    ContainerValues fileProperties)
  {
    if (this._integrator.IsReadOnlyDocument)
      return;
    IMGApplication mgApplication = docItem.Sections.Get<IMGApplication>();
    if (mgApplication.CurrentProject == null)
      return;
    IValueBagContainer properties = mgApplication.CurrentProject.Properties;
    if (properties == null)
      return;
    EncodeAttributesOptions encodeOptions = this.driver.Operations.Documents.GetEncodeOptions(docItem);
    this._documentCodec.Encode(new EncodeAttributesParams(properties, attributeKeys, attributes, fileProperties, encodeOptions)
    {
      ContainerDisplayName = DisplaySection.GetQualifiedName(docItem)
    });
  }

  public void ProcessDocumentAttributes(
    SectionEntity documentItem,
    ValueBag workingSet,
    ValueBag databaseSet)
  {
    if (documentItem == null)
      throw new ArgumentNullException(nameof (documentItem));
    if (workingSet == null)
      throw new ArgumentNullException(nameof (workingSet));
    if (databaseSet == null)
      throw new ArgumentNullException(nameof (databaseSet));
  }

  public ICollection<StringKey> GetDocumentSyncAttributes(SectionEntity docItem)
  {
    return this.GetDocumentFileAttributes(docItem);
  }

  public ValueBag TryReadDocumentRelationAttributes(
    SectionEntity projectDocument,
    SectionEntity partDocument)
  {
    return (ValueBag) null;
  }

  public List<string> GetSatelliteFiles(SectionEntity docItem)
  {
    return this.driver.App.GetSatelliteFiles(docItem.Sections.Get<IMGApplication>().ProjectFile);
  }

  public List<string> GetPrivateFiles(SectionEntity docItem) => new List<string>(0);
}
