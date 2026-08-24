// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Services.ScriptPadService
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Mvp;
using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.CSharp.DesignTime;
using Intermech.Scripting.Projects.DBScripts;
using Intermech.Scripting.ScriptPad;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.Services;

internal sealed class ScriptPadService : IScriptPadService
{
  private CSharpLanguageExtension csharpLanguageExtension;
  private IEnumerable<ILanguageExtension> otherLanguages;
  private ConcurrentDictionary<int, DBScriptProjectInitializer> dbScriptInitializers;
  private DBScriptFactory dbScriptFactory;
  private DBScriptRepository dbScriptRepository;
  private INotificationService notificationService;
  private WeakReference lastIdeRef;

  public ScriptPadService(
    CSharpLanguageExtension csharpLanguageExtension,
    IEnumerable<ILanguageExtension> otherLanguages,
    DBScriptFactory dBScriptFactory,
    DBScriptRepository dbScriptRepository,
    INotificationService notificationService)
  {
    if (csharpLanguageExtension == null)
      throw new ArgumentNullException(nameof (csharpLanguageExtension));
    if (otherLanguages == null)
      throw new ArgumentNullException(nameof (otherLanguages));
    if (dBScriptFactory == null)
      throw new ArgumentNullException(nameof (dBScriptFactory));
    if (dbScriptRepository == null)
      throw new ArgumentNullException(nameof (dbScriptRepository));
    if (notificationService == null)
      throw new ArgumentNullException(nameof (notificationService));
    this.csharpLanguageExtension = csharpLanguageExtension;
    this.otherLanguages = otherLanguages;
    this.dbScriptInitializers = new ConcurrentDictionary<int, DBScriptProjectInitializer>();
    this.dbScriptFactory = dBScriptFactory;
    this.dbScriptRepository = dbScriptRepository;
    this.notificationService = notificationService;
    this.notificationService.Subscribe("ObjectsCheckedOut", new NotificationEventHandler(this.AfterDBObjectCheckedOut));
    this.notificationService.Subscribe("ObjectsCheckedIn", new NotificationEventHandler(this.AfterDBObjectCheckedIn));
    this.notificationService.Subscribe("ObjectsChangesCancelled", new NotificationEventHandler(this.AfterDBObjectChangesCancelled));
  }

  public void RegisterScriptProjectInitializer(
    int scriptObjectTypeId,
    DBScriptProjectInitializer initializer)
  {
    ScriptPadService.CheckScriptObjectType(scriptObjectTypeId);
    if (initializer == null)
      throw new ArgumentNullException(nameof (initializer));
    this.dbScriptInitializers.AddOrUpdate(scriptObjectTypeId, initializer, (Func<int, DBScriptProjectInitializer, DBScriptProjectInitializer>) ((key, existingTemplate) => initializer));
  }

  public DBScriptProjectInitializer TryGetScriptProjectInitializer(int scriptObjectTypeId)
  {
    ScriptPadService.CheckScriptObjectType(scriptObjectTypeId);
    DBScriptProjectInitializer projectInitializer;
    return this.dbScriptInitializers.TryGetValue(scriptObjectTypeId, out projectInitializer) ? projectInitializer : (DBScriptProjectInitializer) null;
  }

  public DBScriptProject CreateEmptyScriptProject(int scriptObjectTypeId)
  {
    ScriptPadService.CheckScriptObjectType(scriptObjectTypeId);
    DBScriptProject emptyProject = (DBScriptProject) this.dbScriptFactory.CreateEmptyProject(this.csharpLanguageExtension.LanguageInfo);
    emptyProject.ObjectTypeId = scriptObjectTypeId;
    emptyProject.Behaviors.AddRepository((IScriptProjectRepository) this.dbScriptRepository);
    this.InitializeScriptProjectBehaviors(emptyProject);
    this.InitializeScriptProjectWithTemplate(emptyProject);
    return emptyProject;
  }

  public DBScriptProject GetScriptProject(long scriptId, bool initializeWhenEmpty = false)
  {
    DBScriptProject scriptProject = !Consts.IsUndefinedObjectId(scriptId) ? (DBScriptProject) this.dbScriptRepository.Get((object) new DBScriptRepositoryKey(scriptId)) : throw new ArgumentException("Не задан идентификатор сценария.", nameof (scriptId));
    this.InitializeScriptProjectBehaviors(scriptProject);
    if (initializeWhenEmpty && scriptProject.File.IsEmpty())
    {
      this.InitializeScriptProjectWithTemplate(scriptProject);
      if (!scriptProject.File.IsEmpty())
        this.dbScriptRepository.Update((ScriptProject) scriptProject);
    }
    return scriptProject;
  }

  private void InitializeScriptProjectWithTemplate(DBScriptProject scriptProject)
  {
    DBScriptProjectInitializer projectInitializer = this.TryGetScriptProjectInitializer(scriptProject.ObjectTypeId);
    if (projectInitializer == null)
      return;
    if (!string.IsNullOrEmpty(projectInitializer.NameTemplate) && string.IsNullOrEmpty(scriptProject.Name))
      scriptProject.Name = projectInitializer.NameTemplate;
    if (string.IsNullOrEmpty(projectInitializer.ScriptCodeTemplate))
      return;
    scriptProject.File.SetContentAsText(projectInitializer.ScriptCodeTemplate, this.dbScriptRepository.AllowedEncoding);
  }

  private void InitializeScriptProjectBehaviors(DBScriptProject scriptProject)
  {
    scriptProject.Behaviors.AddDisplayBehavior((IScriptDisplayBehavior) new DBScriptDisplayBehavior(scriptProject));
    scriptProject.Behaviors.AddProjectOptionsBehavior((IScriptProjectOptionsBehavior) new DBScriptProjectOptionsBehavior(scriptProject));
    if (scriptProject.LanguageInfo.Name == "C#")
      scriptProject.Behaviors.AddTextEditorBehavior((IScriptTextEditorBehavior) new CSharpTextEditorBehavior((ScriptProject) scriptProject));
    this.TryGetScriptProjectInitializer(scriptProject.ObjectTypeId)?.Initialize(scriptProject);
  }

  private static void CheckScriptObjectType(int scriptObjectTypeId)
  {
    if (scriptObjectTypeId == -1)
      throw new ArgumentException("Не задан идентификатор типа сценариев.", nameof (scriptObjectTypeId));
  }

  public IDEPresenter OpenIDEWindow()
  {
    IDEPresenter ideInstance = this.GetIDEInstance();
    if (ideInstance.IsAttachedToView)
      MvpContext.ViewService.ActivateView((IView) ideInstance.View);
    else
      MvpContext.ViewService.Show((IPresenter) ideInstance);
    return ideInstance;
  }

  private IDEPresenter GetIDEInstance()
  {
    IDEPresenter ideInstance = this.TryGetIDEInstance();
    if (ideInstance == null)
    {
      ideInstance = this.CreateIDEInstance();
      this.lastIdeRef = new WeakReference((object) ideInstance);
    }
    return ideInstance;
  }

  private IDEPresenter TryGetIDEInstance()
  {
    if (this.lastIdeRef != null && this.lastIdeRef.IsAlive)
    {
      IDEPresenter target = (IDEPresenter) this.lastIdeRef.Target;
      if (target != null && target.IsAttachedToView)
        return target;
    }
    return (IDEPresenter) null;
  }

  private IDEPresenter CreateIDEInstance()
  {
    IDEModel model = new IDEModel()
    {
      SettingsService = (IDESettingsService) new ScriptPadSettingsService("ScriptPadSettings.xml"),
      LanguageRegistry = this.CreateScriptLanguageRegistry()
    };
    model.ScriptSystem = (IScriptSystemService) new DiskScriptSystemService(model.LanguageRegistry, Environment.GetFolderPath(Environment.SpecialFolder.Personal));
    model.Freeze();
    IDEPresenter ideInstance = new IDEPresenter(model);
    ideInstance.AfterSaveScriptProject += new EventHandler<ScriptProjectEventArgs>(this.AfterSaveScriptProject);
    return ideInstance;
  }

  private void AfterSaveScriptProject(object sender, ScriptProjectEventArgs e)
  {
    if (!(e.ScriptProject is DBScriptProject))
      return;
    this.notificationService.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", ((DBScriptProject) e.ScriptProject).ObjectId));
  }

  private void AfterDBObjectChangesCancelled(object sender, NotificationEventArgs e)
  {
    IDEPresenter ideInstance = this.TryGetIDEInstance();
    if (ideInstance == null)
      return;
    DBObjectsEventArgs objectsEventArgs = (DBObjectsEventArgs) e;
    for (int index = 0; index < objectsEventArgs.ItemsCount; ++index)
    {
      long objectId = objectsEventArgs.ObjectIDs[index];
      this.ReloadScriptProject(ideInstance, objectId, -objectId, true);
    }
  }

  private void AfterDBObjectCheckedIn(object sender, NotificationEventArgs e)
  {
    IDEPresenter ideInstance = this.TryGetIDEInstance();
    if (ideInstance == null)
      return;
    DBObjectsEventArgs objectsEventArgs = (DBObjectsEventArgs) e;
    for (int index = 0; index < objectsEventArgs.ItemsCount; ++index)
    {
      long objectId = objectsEventArgs.ObjectIDs[index];
      this.ReloadScriptProject(ideInstance, objectId, -objectId, true);
    }
  }

  private void AfterDBObjectCheckedOut(object sender, NotificationEventArgs e)
  {
    IDEPresenter ideInstance = this.TryGetIDEInstance();
    if (ideInstance == null)
      return;
    DBObjectsCheckOutEventArgs checkOutEventArgs = (DBObjectsCheckOutEventArgs) e;
    for (int index = 0; index < checkOutEventArgs.ItemsCount; ++index)
      this.ReloadScriptProject(ideInstance, checkOutEventArgs.ObjectIDs[index], checkOutEventArgs.NewObjectIDs[index], false);
  }

  private void ReloadScriptProject(
    IDEPresenter ide,
    long oldScriptId,
    long newScriptId,
    bool readOnlyMode)
  {
    DBScriptRepositoryKey repositoryKey = new DBScriptRepositoryKey(oldScriptId);
    DBScriptProject openScriptProject = (DBScriptProject) ide.FindOpenScriptProject((object) repositoryKey);
    if (openScriptProject == null)
      return;
    DBScriptProject scriptProject = this.GetScriptProject(newScriptId, false);
    ide.ReplaceScriptProject((ScriptProject) openScriptProject, (ScriptProject) scriptProject, readOnlyMode);
  }

  public void OpenScriptInIDEWindow(
    ScriptProject scriptProject,
    OpenInScriptPadParameters parameters)
  {
    if (scriptProject == null)
      throw new ArgumentNullException(nameof (scriptProject));
    if (parameters == null)
      throw new ArgumentNullException(nameof (parameters));
    this.OpenIDEWindow().OpenScriptProject(scriptProject, parameters.ReadOnlyMode);
  }

  public ScriptProject OpenScriptInDialogMode(
    ScriptProject scriptProject,
    OpenInScriptPadParameters parameters,
    Form ownerForm = null)
  {
    if (scriptProject == null)
      throw new ArgumentNullException(nameof (scriptProject));
    if (parameters == null)
      throw new ArgumentNullException(nameof (parameters));
    IDEPresenter ideInstance = this.GetIDEInstance();
    IDEModel model = ideInstance.Model.Clone();
    model.Mode = IDEMode.SingleDocumentDialog;
    model.OpenAtStartup.Add((scriptProject, parameters.ReadOnlyMode));
    model.Freeze();
    ScriptProject resultScriptProject = scriptProject;
    IDEPresenter linkedCopy = ideInstance.CreateLinkedCopy(model);
    linkedCopy.AfterOpenScriptProject += (EventHandler<ScriptProjectEventArgs>) ((s, e) => resultScriptProject = e.ScriptProject);
    MvpContext.ViewService.ShowModal((IPresenter) linkedCopy, (object) ownerForm);
    return resultScriptProject;
  }

  private LanguageRegistry CreateScriptLanguageRegistry()
  {
    LanguageRegistry languageRegistry = new LanguageRegistry();
    languageRegistry.Add(this.CreateCSharpLanguageDescriptor());
    foreach (ILanguageExtension otherLanguage in this.otherLanguages)
      languageRegistry.Add(this.CreateAdditionalLanguageDescriptor(otherLanguage));
    return languageRegistry;
  }

  private LanguageDescriptor CreateCSharpLanguageDescriptor()
  {
    ILanguageSessionService languageSessionService = this.csharpLanguageExtension.CreateLanguageSessionService();
    LanguageDescriptor languageDescriptor = new LanguageDescriptor(this.csharpLanguageExtension.LanguageInfo);
    languageDescriptor.Services.AddSessionService(languageSessionService);
    languageDescriptor.Services.AddTextEditorService((ITextEditorLanguageService) new CSharpTextEditorLanguageService(this.csharpLanguageExtension.LanguageInfo));
    languageDescriptor.Services.AddCustomService(typeof (ILanguageExtension), (object) this.csharpLanguageExtension);
    return languageDescriptor;
  }

  private LanguageDescriptor CreateAdditionalLanguageDescriptor(ILanguageExtension languageExtension)
  {
    LanguageDescriptor languageDescriptor = new LanguageDescriptor(languageExtension.LanguageInfo);
    languageDescriptor.Services.AddSessionService(languageExtension.CreateLanguageSessionService());
    languageDescriptor.Services.AddCustomService(typeof (ILanguageExtension), (object) languageExtension);
    return languageDescriptor;
  }
}
