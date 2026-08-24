// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGInterfaceService`1
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Runtime;
using Intermech.Runtime.ComInterop;
using Intermech.Runtime.ComInterop.Proxies;
using Intermech.Tools.Integrators;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Intermech.MG.Integrator;

internal abstract class MGInterfaceService<TApplication> : 
  ApplicationApiService<TApplication>,
  IDocumentApiService,
  IExternalApiService,
  IIntegratorService,
  IAttributeCodecCollection
  where TApplication : class, IMGApplication
{
  public bool OpenFromIPS;
  protected ComObjectProvider cadObjectProvider;
  protected MGSettingsService settingsSvc;
  protected IApplicationFileTypes fileTypeSvc;
  protected IAttributeCodec documentCodec;
  protected IAttributeCodec assemblyCodec;
  protected IAttributeCodec partCodec;
  protected OpenDocumentsApi openDocumentsApi;

  public MGInterfaceService(IIntegrator owner, string applicationName, string progID)
    : base(owner, applicationName)
  {
    this.cadObjectProvider = (ComObjectProvider) new ProgIdProvider(progID, false);
  }

  public MGSettingsService SettingsService
  {
    [DebuggerStepThrough] get
    {
      lock (this.Integrator.SyncRoot)
        return this.settingsSvc;
    }
    [DebuggerStepThrough] set
    {
      lock (this.Integrator.SyncRoot)
      {
        this.RequireNotInitialized();
        this.settingsSvc = value;
      }
    }
  }

  public IApplicationFileTypes FileTypeService
  {
    [DebuggerStepThrough] get
    {
      lock (this.Integrator.SyncRoot)
        return this.fileTypeSvc;
    }
    [DebuggerStepThrough] set
    {
      lock (this.Integrator.SyncRoot)
      {
        this.RequireNotInitialized();
        this.fileTypeSvc = value;
      }
    }
  }

  protected abstract IAttributeCodec GetArticleCodec(
    MGSettingsService settingsSvc,
    bool isAssemblyCodec);

  protected abstract IAttributeCodec GetDocumentCodec(MGSettingsService settingsSvc);

  protected override void DoInitialize()
  {
    base.DoInitialize();
    if (this.SettingsService == null)
      throw PropertyExceptions.PropertyNotSetException((object) this, "SettingsService");
    if (this.FileTypeService == null)
      throw PropertyExceptions.PropertyNotSetException((object) this, "FileTypeService");
    this.documentCodec = this.GetDocumentCodec(this.settingsSvc);
    this.partCodec = this.GetArticleCodec(this.settingsSvc, false);
    this.assemblyCodec = this.GetArticleCodec(this.settingsSvc, true);
    this.openDocumentsApi = new OpenDocumentsApi(this.fileTypeSvc, (IExternalApiService) this);
    this.openDocumentsApi.OnFindOpenDocument += new Func<string, IOpenDocument>(this.FindOpenDocument);
    this.openDocumentsApi.OnOpenDocument += new Func<string, IOpenDocument>(this.OpenDocument);
    this.openDocumentsApi.OnValidateDocument += new Action<IOpenDocument>(this.ValidateDocument);
    this.openDocumentsApi.OnGetDocumentCodec += new Func<IOpenDocument, IAttributeCodec>(this.GetDocumentCodec);
    this.openDocumentsApi.OnGetDocumentAttributeContainer += new Func<IOpenDocument, IValueBagContainer>(this.GetDocumentAttributeContainer);
    this.openDocumentsApi.OnCloseDocument += new Action<IOpenDocument>(this.CloseDocument);
  }

  public IAttributeCodec GetDocumentCodec()
  {
    this.RequireReadyState();
    return this.documentCodec;
  }

  public IAttributeCodec GetPartCodec()
  {
    this.RequireReadyState();
    return this.partCodec;
  }

  public IAttributeCodec GetAssemblyCodec()
  {
    this.RequireReadyState();
    return this.assemblyCodec;
  }

  public IOpenDocumentsApi OpenDocuments
  {
    [DebuggerStepThrough] get
    {
      this.RequireReadyState();
      return (IOpenDocumentsApi) this.openDocumentsApi;
    }
  }

  private IOpenDocument FindOpenDocument(string fullPath)
  {
    IMGApplication applicationObject = (IMGApplication) this.GetApplicationObject();
    if (applicationObject.CurrentProject == null)
      return (IOpenDocument) null;
    return !applicationObject.ProjectFile.Equals(fullPath) ? (IOpenDocument) null : (IOpenDocument) new OpenMGProject(applicationObject.CurrentProject, fullPath);
  }

  private void ValidateDocument(IOpenDocument openDocument)
  {
    if (!(openDocument is OpenMGProject))
      throw new InvalidOperationException("Документы данного типа не поддерживаются интегратором.");
  }

  private IAttributeCodec GetDocumentCodec(IOpenDocument openDocument) => this.GetDocumentCodec();

  private IValueBagContainer GetDocumentAttributeContainer(IOpenDocument openDocument)
  {
    return ((OpenMGProject) openDocument).Properties;
  }

  private IOpenDocument OpenDocument(string fullPath)
  {
    IMGApplication applicationObject = (IMGApplication) this.GetApplicationObject();
    applicationObject.OpenProject(fullPath, false);
    return applicationObject.CurrentProject == null ? (IOpenDocument) null : (IOpenDocument) new OpenMGProject(applicationObject.CurrentProject, fullPath);
  }

  private void CloseDocument(IOpenDocument openDocument)
  {
    IMGApplication applicationObject = (IMGApplication) this.GetApplicationObject();
    if (applicationObject.CurrentProject == null)
      return;
    OpenMGProject openMgProject = (OpenMGProject) openDocument;
    if (!applicationObject.ProjectFile.Equals(openMgProject.FullPath))
      return;
    applicationObject.CloseProject();
  }

  protected override bool IsInstalled() => this.cadObjectProvider.IsRegistered();

  protected override bool IsRunning()
  {
    // ISSUE: reference to a compiler-generated field
    if (MGInterfaceService<TApplication>.\u003C\u003Eo__23.\u003C\u003Ep__1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      MGInterfaceService<TApplication>.\u003C\u003Eo__23.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (MGInterfaceService<TApplication>)));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, bool> target = MGInterfaceService<TApplication>.\u003C\u003Eo__23.\u003C\u003Ep__1.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, bool>> p1 = MGInterfaceService<TApplication>.\u003C\u003Eo__23.\u003C\u003Ep__1;
    // ISSUE: reference to a compiler-generated field
    if (MGInterfaceService<TApplication>.\u003C\u003Eo__23.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      MGInterfaceService<TApplication>.\u003C\u003Eo__23.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (MGInterfaceService<TApplication>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj = MGInterfaceService<TApplication>.\u003C\u003Eo__23.\u003C\u003Ep__0.Target((CallSite) MGInterfaceService<TApplication>.\u003C\u003Eo__23.\u003C\u003Ep__0, this.FindRunningCadObject(), (object) null);
    return target((CallSite) p1, obj);
  }

  protected abstract string[] ApplicationClassID { get; }

  protected abstract void OnCadObjectInitialize(object cadObject);

  private object FindRunningCadObject() => this.cadObjectProvider.TryGetRunningInstance();

  protected object FindOrCreateCadObject()
  {
    try
    {
      object orCreateCadObject = this.FindRunningCadObject();
      // ISSUE: reference to a compiler-generated field
      if (MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MGInterfaceService<TApplication>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (MGInterfaceService<TApplication>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__0.Target((CallSite) MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__0, orCreateCadObject, (object) null);
      if (target1((CallSite) p1, obj1))
      {
        orCreateCadObject = this.cadObjectProvider.CreateInstance();
        this.OpenFromIPS = true;
        // ISSUE: reference to a compiler-generated field
        if (MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MGInterfaceService<TApplication>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target2 = MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p3 = MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (MGInterfaceService<TApplication>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__2.Target((CallSite) MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__2, orCreateCadObject, (object) null);
        if (target2((CallSite) p3, obj2))
          throw new Exception($"Невозможно запустить приложение {this.ApplicationName}!");
        // ISSUE: reference to a compiler-generated field
        if (MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__4 = CallSite<Action<CallSite, MGInterfaceService<TApplication>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "OnCadObjectInitialize", (IEnumerable<Type>) null, typeof (MGInterfaceService<TApplication>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__4.Target((CallSite) MGInterfaceService<TApplication>.\u003C\u003Eo__28.\u003C\u003Ep__4, this, orCreateCadObject);
      }
      return orCreateCadObject;
    }
    catch (COMException ex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("Не удалось инициализировать CAD-интерфейс для приложения {0}.", (object) this.ApplicationName);
      stringBuilder.Append(' ');
      stringBuilder.Append(ex.Message);
      throw new ApplicationProxyException(stringBuilder.ToString());
    }
  }
}
