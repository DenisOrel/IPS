// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.DrawCreator.DrawCreatorProvider
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.CADInterface.Proxies;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.NX.Integrator.DrawCreator;

internal sealed class DrawCreatorProvider
{
  private readonly IObjectCreatorService _objectCreatorService;
  private readonly IFileVault _fileVaultService;
  private readonly object _syncRoot;
  private NXIntegrator _nxIntegrator;
  private bool _isEnabled;
  private NXModelDrawingsService _modelDrawingsService;
  private DrawCreatorResult _lastResult;

  public DrawCreatorProvider(
    IObjectCreatorService objectCreatorService,
    IFileVault fileVaultService)
  {
    if (objectCreatorService == null)
      throw new ArgumentNullException(nameof (objectCreatorService));
    if (fileVaultService == null)
      throw new ArgumentNullException(nameof (fileVaultService));
    this._objectCreatorService = objectCreatorService;
    this._fileVaultService = fileVaultService;
    this._syncRoot = new object();
  }

  public bool Enabled
  {
    [DebuggerStepThrough] get
    {
      lock (this._syncRoot)
        return this._isEnabled;
    }
    set
    {
      lock (this._syncRoot)
      {
        if (this._isEnabled == value)
          return;
        this.BeforeEnabledChanged(value);
        this._isEnabled = value;
      }
    }
  }

  public NXIntegrator NXIntegrator
  {
    get
    {
      lock (this._syncRoot)
        return this._nxIntegrator;
    }
    set
    {
      lock (this._syncRoot)
      {
        if (this._isEnabled)
          throw new InvalidOperationException();
        this._nxIntegrator = value;
      }
    }
  }

  private void BeforeEnabledChanged(bool newValue)
  {
    if (newValue)
    {
      this._objectCreatorService.SelectCustomServiceEvent += new EventHandler<ObjectCreatorCustomServiceEventArgs>(this.SelectCustomServiceHandler);
      this._objectCreatorService.AfterObjectCreatedEvent += new AfterObjectCreatedEventHandler(this.AfterObjectCreatedEvent);
      this._objectCreatorService.ObjectCreatorCanceledEvent += new ObjectCreatorCanceledEventHandler(this.AfterObjectCancelledEvent);
    }
    else
    {
      this._objectCreatorService.SelectCustomServiceEvent -= new EventHandler<ObjectCreatorCustomServiceEventArgs>(this.SelectCustomServiceHandler);
      this._objectCreatorService.AfterObjectCreatedEvent -= new AfterObjectCreatedEventHandler(this.AfterObjectCreatedEvent);
      this._objectCreatorService.ObjectCreatorCanceledEvent -= new ObjectCreatorCanceledEventHandler(this.AfterObjectCancelledEvent);
    }
  }

  private void AfterObjectCancelledEvent(object sender, ObjectCreatorCanceledEventArgs ea)
  {
    lock (this._syncRoot)
    {
      if (this._lastResult == null)
        return;
      this._lastResult = (DrawCreatorResult) null;
    }
  }

  private void AfterObjectCreatedEvent(object sender, AfterObjectCreatedEventArgs e)
  {
    lock (this._syncRoot)
    {
      if (this._lastResult == null)
        return;
      try
      {
        if (!(sender is IObjectCreatorService) || Intermech.Consts.IsUndefinedObjectId(this._lastResult.ModelID))
          return;
        this.LinkDrawingToModel(e.ObjectID, this._lastResult.ModelID);
      }
      finally
      {
        this._lastResult = (DrawCreatorResult) null;
      }
    }
  }

  private void LinkDrawingToModel(long drawingObjectID, long modelID)
  {
    string fullName1 = this._fileVaultService.PublishTree(drawingObjectID, true, VersionsRuleSources.GetEditorRule(), (IFileArea) this._fileVaultService.WorkArea);
    string fullName2 = this._fileVaultService.PublishTree(modelID, true, VersionsRuleSources.GetEditorRule(), (IFileArea) this._fileVaultService.WorkArea);
    using (CADApiSession cadApiSession = new CADApiSession((IIntegrator) this.NXIntegrator))
    {
      CADSystemProxy application = cadApiSession.Application;
      CADDocumentProxy drawingDoc = application.OpenDocument(fullName1, false);
      CADDocumentProxy modelDoc = application.OpenDocument(fullName2, false);
      this.LinkNXDrawingToNXModel(drawingDoc, modelDoc);
      drawingDoc.Save();
      drawingDoc.Close();
      modelDoc.Close();
    }
  }

  private void LinkNXDrawingToNXModel(CADDocumentProxy drawingDoc, CADDocumentProxy modelDoc)
  {
    (this.TryGetDefaultOrUnnamedNXConfiguration(drawingDoc) ?? throw new Exception("Невозможно добавить в чертеж NX ссылку на модель, так как не удалось получить конфигурацию по умолчанию для чертежа.")).AddComponent(this.TryGetDefaultOrUnnamedNXConfiguration(modelDoc) ?? throw new Exception("Невозможно добавить в чертеж NX ссылку на модель, так как не удалось получить конфигурацию по умолчанию для модели."));
  }

  private ModelConfigurationProxy TryGetDefaultOrUnnamedNXConfiguration(CADDocumentProxy nxDoc)
  {
    ModelConfigurationProxy unnamedNxConfiguration = nxDoc.DefaultConfiguration;
    if (unnamedNxConfiguration == null)
    {
      string safeName = nxDoc.CADSystem.Builder.ConfigurationNameMangler.ToSafeName(nxDoc.FullName, string.Empty);
      unnamedNxConfiguration = nxDoc.GetConfiguration(safeName, false);
    }
    return unnamedNxConfiguration;
  }

  private void SelectCustomServiceHandler(object sender, ObjectCreatorCustomServiceEventArgs e)
  {
    lock (this._syncRoot)
      this.TryAddChoiceDocumentPage(e);
  }

  private void TryAddChoiceDocumentPage(ObjectCreatorCustomServiceEventArgs e)
  {
    if (e.Handled)
      return;
    CADSettings integratorSettings = this.TryGetIntegratorSettings();
    if (integratorSettings == null)
      return;
    List<int> intList = new List<int>();
    DocumentGroup byName1 = integratorSettings.FileDocumentGroups.FindByName("AssemblyDrawing", false);
    DocumentGroup byName2 = integratorSettings.FileDocumentGroups.FindByName("PartDrawing", false);
    intList.AddRange((IEnumerable<int>) byName1.AsIdList());
    intList.AddRange((IEnumerable<int>) byName2.AsIdList());
    if (!intList.Contains(e.ObjectTypeId))
      return;
    if (this._modelDrawingsService == null)
      this._modelDrawingsService = this.CreateRestrictedModelDrawingsService();
    this._lastResult = new DrawCreatorResult();
    e.CustomServiceType = typeof (Intermech.NX.Integrator.DrawCreator.DrawCreator);
    e.ConstructorParams = new object[4]
    {
      (object) this.NXIntegrator,
      (object) this._fileVaultService,
      (object) this._modelDrawingsService,
      (object) this._lastResult
    };
    e.Handled = true;
  }

  private CADSettings TryGetIntegratorSettings()
  {
    try
    {
      return ServiceUtils.GetService<ICADSettingsService>((object) this._nxIntegrator, true).GetCADSettings();
    }
    catch
    {
      return (CADSettings) null;
    }
  }

  private NXModelDrawingsService CreateRestrictedModelDrawingsService()
  {
    NXModelDrawingsService modelDrawingsService = ((NXModelDrawingsService) ServiceUtils.GetService<IModelDrawingsService>((object) this._nxIntegrator, true)).CloneUninitialized();
    modelDrawingsService.UseDefaultSuffixOnly = true;
    modelDrawingsService.Initialize();
    return modelDrawingsService;
  }
}
