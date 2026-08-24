// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IDEPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Collections;
using Intermech.Mvp;
using Intermech.Mvp.Components;
using Intermech.Mvp.Components.Dialogs;
using Intermech.Runtime;
using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.ScriptPad.Presenters;
using Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal class IDEPresenter : Presenter<IIDEView>
{
  private readonly IDEModel model;
  private readonly IDESharedState sharedState;
  private List<OpenScriptData> openScripts;
  private List<IDEChildPresenter> childPresenters;
  private IScriptWindow lastActiveWindowWithScript;

  public IDEPresenter(IDEModel model)
    : this(model, IDEPresenter.CreateInitialSharedState(model))
  {
  }

  private IDEPresenter(IDEModel model, IDESharedState sharedState)
  {
    if (model == null)
      throw new ArgumentNullException(nameof (model));
    if (sharedState == null)
      throw new ArgumentNullException(nameof (sharedState));
    model.RequireFrozen();
    IDEPresenter.ValidateModel(model, sharedState);
    this.model = model;
    this.sharedState = sharedState;
    this.openScripts = new List<OpenScriptData>();
    this.childPresenters = new List<IDEChildPresenter>();
    this.childPresenters.Add((IDEChildPresenter) new EditIDESettingsCommandPresenter(this, this.sharedState, new Action<IDESettings, IDESettings>(this.UpdateAfterIDESettingsChanged)));
    this.childPresenters.Add((IDEChildPresenter) new RunCommandPresenter(this));
  }

  private static IDESharedState CreateInitialSharedState(IDEModel model)
  {
    return model != null ? new IDESharedState(model.SettingsService) : throw new ArgumentNullException(nameof (model));
  }

  private static void ValidateModel(IDEModel model, IDESharedState sharedState)
  {
    if (model.LanguageRegistry == null)
      throw PropertyExceptions.PropertyNotSetException((object) model, "LanguageRegistry");
    if (model.LanguageRegistry.Languages.Count == 0)
      throw PropertyExceptions.PropertyBadValueException((object) model, "SettingsService", "В реестре языков сценариев должен быть хотя бы один элемент.");
    if (model.SettingsService != sharedState.SettingsService)
      throw PropertyExceptions.PropertyBadValueException((object) model, "SettingsService", "Указанный экземпляр сервиса IDESettingsService не совпадает с уже использующимся экземпляром.");
  }

  public IDEModel Model => this.model;

  public IDEPresenter CreateLinkedCopy(IDEModel model)
  {
    return model != null ? new IDEPresenter(model, this.sharedState) : throw new ArgumentNullException(nameof (model));
  }

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.View.ViewClosing += new EventHandler<CancelEventArgs>(this.OnViewClosing);
    this.View.HotkeyPressed += new KeyEventHandler(this.ProcessHotKey);
    this.View.NewCommand.Click += new EventHandler<MultiCommandEventArgs>(this.ProcessNewCommand);
    this.View.OpenCommand.Click += new EventHandler(this.ProcessOpenCommand);
    this.View.SaveCommand.Click += new EventHandler(this.ProcessSaveCommand);
    this.View.SaveAsCommand.Click += new EventHandler(this.ProcessSaveAsCommand);
    this.View.SaveCopyCommand.Click += new EventHandler(this.ProcessSaveCopyCommand);
    this.View.ScriptWindows.ActiveWindowChanged += new EventHandler(this.OnActiveScriptWindowChanged);
    this.View.ScriptWindows.WindowClosing += new EventHandler(this.OnScriptWindowClosing);
    this.View.ScriptWindows.WindowClosed += new EventHandler(this.OnScriptWindowClosed);
    this.View.ScriptWindows.CloseableWindows = this.model.Mode == IDEMode.Normal;
    this.View.MaximizedAtStartup = this.model.Mode == IDEMode.Normal;
    this.UpdateNewAndOpenCommandsState();
    this.AttachIDEChildPresenters();
    this.sharedState.RegisterRunningIDE(this);
  }

  protected override void OnDetachView()
  {
    this.View.ViewClosing -= new EventHandler<CancelEventArgs>(this.OnViewClosing);
    this.View.HotkeyPressed -= new KeyEventHandler(this.ProcessHotKey);
    this.View.NewCommand.Click -= new EventHandler<MultiCommandEventArgs>(this.ProcessNewCommand);
    this.View.OpenCommand.Click -= new EventHandler(this.ProcessOpenCommand);
    this.View.SaveCommand.Click -= new EventHandler(this.ProcessSaveCommand);
    this.View.SaveAsCommand.Click -= new EventHandler(this.ProcessSaveAsCommand);
    this.View.SaveCopyCommand.Click -= new EventHandler(this.ProcessSaveCopyCommand);
    this.CloseAllOpenScripts();
    this.View.ScriptWindows.ActiveWindowChanged -= new EventHandler(this.OnActiveScriptWindowChanged);
    this.View.ScriptWindows.WindowClosing -= new EventHandler(this.OnScriptWindowClosing);
    this.View.ScriptWindows.WindowClosed -= new EventHandler(this.OnScriptWindowClosed);
    this.DisableNewAndOpenCommandState();
    this.DetachIDEChildPresenters();
    this.sharedState.UnregisterRunningIDE(this);
    base.OnDetachView();
  }

  protected override void OnAfterAttachView()
  {
    base.OnAfterAttachView();
    if (this.model.OpenAtStartup.Count == 0)
      return;
    foreach ((ScriptProject, bool) tuple in (IEnumerable<(ScriptProject, bool)>) this.model.OpenAtStartup)
      this.OpenScriptInternal(tuple.Item1, tuple.Item2);
  }

  private void UpdateNewAndOpenCommandsState()
  {
    if (this.IsNewScriptCommandAllowed())
    {
      LinkedList<MultiCommandItem> subItems = CollectionUtils.ConvertAsLinkedList<LanguageInfo, MultiCommandItem>((ICollection<LanguageInfo>) this.model.LanguageRegistry.Languages, (Converter<LanguageInfo, MultiCommandItem>) (item => new MultiCommandItem(item.Name, (object) item)));
      this.View.NewCommand.SetItems((ICollection<MultiCommandItem>) subItems);
      this.View.NewCommand.Enabled = subItems.Count != 0;
    }
    this.View.OpenCommand.Enabled = this.IsOpenScriptCommandAllowed();
  }

  private void DisableNewAndOpenCommandState()
  {
    this.View.NewCommand.Enabled = false;
    this.View.NewCommand.ClearItems();
    this.View.OpenCommand.Enabled = false;
  }

  internal bool IsNewScriptCommandAllowed()
  {
    return this.model.Mode == IDEMode.Normal && this.model.ScriptSystem != null;
  }

  internal bool IsOpenScriptCommandAllowed()
  {
    return this.model.Mode == IDEMode.Normal && this.model.ScriptSystem != null;
  }

  private void AttachIDEChildPresenters()
  {
    foreach (IDEChildPresenter childPresenter in this.childPresenters)
      childPresenter.AttachView();
  }

  private void DetachIDEChildPresenters()
  {
    foreach (IDEChildPresenter childPresenter in this.childPresenters)
      childPresenter.DetachView(true);
  }

  private void CloseAllOpenScripts()
  {
    for (IScriptWindow activeWindow = this.View.ScriptWindows.ActiveWindow; activeWindow != null; activeWindow = this.View.ScriptWindows.ActiveWindow)
    {
      OpenScriptData linkedScript = this.TryGetLinkedScript(activeWindow);
      if (linkedScript != null)
        this.CloseScriptInternal(linkedScript);
      else
        this.View.ScriptWindows.RemoveWindow(activeWindow);
    }
  }

  internal OpenScriptData TryGetActiveScript()
  {
    IScriptWindow activeWindow = this.View.ScriptWindows.ActiveWindow;
    return activeWindow == null ? (OpenScriptData) null : this.TryGetLinkedScript(activeWindow);
  }

  private OpenScriptData TryGetLinkedScript(IScriptWindow window) => window.Script;

  internal LanguageSessionData GetOrCreateLanguageSessionData(string languageName)
  {
    return this.sharedState.GetOrCreateLanguageSessionData(this.model, languageName);
  }

  internal ILanguageSession GetOrCreateLanguageSession(string languageName)
  {
    LanguageSessionData languageSessionData = this.GetOrCreateLanguageSessionData(languageName);
    if (languageSessionData.Session == null)
    {
      ILanguageSessionParameters parameters = (ILanguageSessionParameters) languageSessionData.SessionParameters.Clone();
      parameters.Stdout = this.View.OutputView;
      languageSessionData.Session = languageSessionData.LanguageDescriptor.Services.GetSessionService().CreateSession(parameters);
    }
    return languageSessionData.Session;
  }

  public void CreateScriptProject()
  {
    this.CheckViewIsPresent();
    this.CreateScriptInternal(this.model.LanguageRegistry.Languages[0]);
  }

  internal void CreateScriptInternal(LanguageInfo languageInfo)
  {
    this.OpenScriptInternal(this.model.ScriptSystem.CreateEmptyProject(languageInfo), false);
  }

  internal void SaveScriptAsInternal(OpenScriptData script)
  {
    ScriptProject emptyProject = this.model.ScriptSystem.CreateEmptyProject(script.LanguageInfo);
    if (!this.SaveScriptProjectChanges(emptyProject, script.Window.CodeEditor.GetScriptCode(), script.Encoding))
      return;
    script.Project = emptyProject;
    script.Modified = false;
    this.RaiseAfterSaveScriptProject(script.Project);
  }

  internal void SaveScriptInternal(OpenScriptData script)
  {
    if (!this.SaveScriptProjectChanges(script.Project, script.Window.CodeEditor.GetScriptCode(), script.Encoding))
      return;
    script.Modified = false;
    this.RaiseAfterSaveScriptProject(script.Project);
  }

  internal bool SaveScriptProjectChanges(
    ScriptProject project,
    string scriptCode,
    Encoding scriptEncoding)
  {
    project.File.SetContentAsText(scriptCode, scriptEncoding);
    IScriptSaveChangesBehavior saveChangesBehavior = project.Behaviors.GetSaveChangesBehavior(false);
    if (saveChangesBehavior != null)
    {
      ScriptBeforeSaveEventArgs e = new ScriptBeforeSaveEventArgs();
      e.CanSave = true;
      saveChangesBehavior.BeforeSave(e);
      if (!e.CanSave)
        return false;
    }
    IScriptProjectRepository repository = project.Behaviors.GetRepository();
    if (project.IsNew)
    {
      ScriptSaveAsParameters parameters = saveChangesBehavior?.TrySaveAs();
      if (parameters == null)
        return false;
      repository.Add(project, parameters);
    }
    else
      repository.Update(project);
    if (saveChangesBehavior != null)
    {
      ScriptAfterSaveEventArgs e = new ScriptAfterSaveEventArgs();
      saveChangesBehavior.AfterSave(e);
    }
    return true;
  }

  public void OpenScriptProject(ScriptProject scriptProject, bool readOnlyMode)
  {
    if (scriptProject == null)
      throw new ArgumentNullException(nameof (scriptProject));
    this.CheckViewIsPresent();
    this.OpenScriptInternal(scriptProject, readOnlyMode);
  }

  internal void OpenScriptInternal(
    ScriptProject scriptProject,
    bool readOnlyMode,
    IScriptWindow window = null)
  {
    if (readOnlyMode && scriptProject.IsNew)
      throw new ScriptDesignTimeException("Невозможно открыть новый сценарий в режиме \"только чтение\".");
    OpenScriptData projectOrRepositoryKey = this.FindOpenScriptByScriptProjectOrRepositoryKey(scriptProject);
    if (projectOrRepositoryKey != null)
    {
      if (projectOrRepositoryKey.Window == this.View.ScriptWindows.ActiveWindow)
        return;
      this.View.ScriptWindows.ActiveWindow = projectOrRepositoryKey.Window;
    }
    else
    {
      LanguageInfo languageInfo = scriptProject.LanguageInfo;
      Tuple<string, Encoding> tuple = this.GetOrCreateLanguageSession(languageInfo.Name).LoadScriptCode(scriptProject.File.GetContent());
      OpenScriptData script = new OpenScriptData(scriptProject, tuple.Item2, readOnlyMode);
      this.CreateScriptAdvancedFormattingActions(script);
      this.CreateScriptChildPresenters(script);
      if (script.Project.IsNew && !string.IsNullOrEmpty(tuple.Item1))
        script.Modified = true;
      if (window == null)
      {
        WinformsAdapterControl winformsAdapterControl = new WinformsAdapterControl();
        winformsAdapterControl.Name = "codeEditorControl";
        window = this.View.ScriptWindows.AddWindow();
        window.CodeEditor = (IScriptCodeEditorControl) winformsAdapterControl;
        window.CodeEditor.SetFont(this.sharedState.Settings.FontFamily, this.sharedState.Settings.FontSize);
      }
      window.CodeEditor.Initialize(languageInfo, tuple.Item1, script.ReadOnlyMode);
      window.CodeEditor.FocusAt(new TextCaretPosition(1, 1));
      if (script.ContextMenuActions.Count != 0)
        window.CodeEditor.SetContextMenuActions((IList<ITextEditorUIAction>) script.ContextMenuActions);
      if (this.sharedState.Settings.EnableCodeCompletion)
        this.SetScriptCodeCompletion(script, window);
      this.openScripts.Add(script);
      window.Script = script;
      script.Window = window;
      if (this.View.ScriptWindows.ActiveWindow == window && this.lastActiveWindowWithScript == null)
        this.OnActiveScriptWindowChanged((object) this.View.ScriptWindows, EventArgs.Empty);
      else
        window.Text = this.CreateScriptWindowCaption(script, window.Text);
      this.RaiseAfterOpenScriptProject(scriptProject);
    }
  }

  private void CreateScriptAdvancedFormattingActions(OpenScriptData script)
  {
    ITextEditorLanguageService textEditorService = this.GetOrCreateLanguageSessionData(script.LanguageInfo.Name).LanguageDescriptor.Services.GetTextEditorService(false);
    if (textEditorService == null)
      return;
    script.CommentSelectionAction = textEditorService.TryCreateCommentSelectionAction();
    script.UncommentSelectionAction = textEditorService.TryCreateUncommentSelectionAction();
    script.FormatIndentsAction = textEditorService.TryCreateFormatIndentsAction();
    script.ContextMenuActions.AddRange((IEnumerable<ITextEditorUIAction>) textEditorService.TryCreateContextMenu());
  }

  private void CreateScriptChildPresenters(OpenScriptData script)
  {
    script.ChildPresenters.Add((IDEChildPresenter) new ActiveScriptInfoPresenter(this, script, new Func<OpenScriptData, string, string>(this.CreateScriptWindowCaption)));
    script.ChildPresenters.Add((IDEChildPresenter) new ActiveScriptModifiedStatePresenter(this, script));
    script.ChildPresenters.Add((IDEChildPresenter) new ClipboardCommandsPresenter(this, script));
    script.ChildPresenters.Add((IDEChildPresenter) new UndoRedoCommandsPresenter(this, script));
    script.ChildPresenters.Add((IDEChildPresenter) new AdvancedFormattingCommandsPresenter(this, script));
    script.ChildPresenters.Add((IDEChildPresenter) new ReplaceWithCommandPresenter(this, script));
    script.ChildPresenters.Add((IDEChildPresenter) new FindReplaceCommandPresenter(this, script));
    script.ChildPresenters.Add((IDEChildPresenter) new CodeWorkspacePresenter(this, script, this.CreateCodeModel(script)));
  }

  private string CreateScriptWindowCaption(OpenScriptData script, string windowText)
  {
    string displayName = script.GetDisplayName();
    StringBuilder stringBuilder = new StringBuilder(displayName.Length + windowText.Length + 5);
    if (script.Modified)
    {
      stringBuilder.Append('*');
      stringBuilder.Append(' ');
    }
    if (string.IsNullOrEmpty(windowText))
      stringBuilder.Append(displayName);
    else
      stringBuilder.AppendFormat("{0} - {1}", (object) displayName, (object) windowText);
    return stringBuilder.ToString();
  }

  private ICodeModel CreateCodeModel(OpenScriptData script)
  {
    ICodeModel codeModel = (ICodeModel) null;
    LanguageSessionData languageSessionData = this.GetOrCreateLanguageSessionData(script.LanguageInfo.Name);
    ITextEditorLanguageService textEditorService = languageSessionData.LanguageDescriptor.Services.GetTextEditorService(false);
    IScriptTextEditorBehavior textEditorBehavior = script.Project.Behaviors.GetTextEditorBehavior(false);
    if (textEditorService != null && textEditorBehavior != null)
    {
      IScriptProjectOptionsBehavior projectOptionsBehavior = script.Project.Behaviors.GetProjectOptionsBehavior(false);
      Dictionary<string, string> scriptProjectOptions = projectOptionsBehavior != null ? projectOptionsBehavior.GetProjectOptions() : new Dictionary<string, string>(0);
      Dictionary<string, string> runtimeOptions = languageSessionData.Session.GetRuntimeOptions(scriptProjectOptions);
      Dictionary<string, string> codeModelOptions = textEditorBehavior.TryCreateCodeModelOptions(scriptProjectOptions, runtimeOptions);
      codeModel = textEditorService.TryCreateCodeModel(new Uri($"ips://scripts/{Guid.NewGuid()}"));
      codeModel.ParseOptions = codeModelOptions;
    }
    if (codeModel == null)
      codeModel = (ICodeModel) new EmptyCodeModel();
    return codeModel;
  }

  private void SetScriptCodeCompletion(OpenScriptData script, IScriptWindow window)
  {
    IScriptTextEditorBehavior textEditorBehavior = script.Project.Behaviors.GetTextEditorBehavior(false);
    if (textEditorBehavior == null)
      return;
    ICodeCompletionProvider completionProvider = textEditorBehavior.TryGetCodeCompletionProvider((ICollection<string>) this.sharedState.Settings.XmlDocPathList);
    if (completionProvider == null)
      return;
    window.CodeEditor.SetCodeCompletionProvider(completionProvider);
  }

  public void CloseScriptProject(ScriptProject scriptProject)
  {
    if (scriptProject == null)
      throw new ArgumentNullException(nameof (scriptProject));
    this.CheckViewIsPresent();
    OpenScriptData scriptByScriptProject = this.FindOpenScriptByScriptProject(scriptProject);
    if (scriptByScriptProject == null)
      return;
    this.CloseScriptInternal(scriptByScriptProject);
  }

  internal IScriptWindow CloseScriptInternal(OpenScriptData script, bool keepWindowOpen = false)
  {
    IScriptWindow window = script.Window;
    if (window != null)
    {
      if (keepWindowOpen)
      {
        if (this.lastActiveWindowWithScript == window)
        {
          this.DetachActiveScriptChildPresenters(script, false);
          this.lastActiveWindowWithScript = (IScriptWindow) null;
        }
        window.Text = string.Empty;
      }
      else
        this.View.ScriptWindows.RemoveWindow(window);
      window.Script = (OpenScriptData) null;
      script.Window = (IScriptWindow) null;
    }
    this.openScripts.Remove(script);
    this.RaiseAfterCloseScriptProject(script.Project);
    return !keepWindowOpen ? (IScriptWindow) null : window;
  }

  public bool HasOpenScripts()
  {
    this.CheckViewIsPresent();
    return this.openScripts.Count != 0;
  }

  public ScriptProject FindOpenScriptProject(object repositoryKey)
  {
    if (repositoryKey == null)
      throw new ArgumentNullException(nameof (repositoryKey));
    this.CheckViewIsPresent();
    return this.FindOpenScriptByRepositoryKey(repositoryKey)?.Project;
  }

  internal OpenScriptData FindOpenScriptByRepositoryKey(object repositoryKey)
  {
    foreach (OpenScriptData openScript in this.openScripts)
    {
      if (!openScript.Project.IsNew && openScript.Project.RepositoryKey.Equals(repositoryKey))
        return openScript;
    }
    return (OpenScriptData) null;
  }

  internal OpenScriptData FindOpenScriptByScriptProject(ScriptProject scriptProject)
  {
    foreach (OpenScriptData openScript in this.openScripts)
    {
      if (openScript.Project == scriptProject)
        return openScript;
    }
    return (OpenScriptData) null;
  }

  internal OpenScriptData FindOpenScriptByScriptProjectOrRepositoryKey(ScriptProject scriptProject)
  {
    OpenScriptData scriptByScriptProject = this.FindOpenScriptByScriptProject(scriptProject);
    if (scriptByScriptProject != null)
      return scriptByScriptProject;
    if (!scriptProject.IsNew)
    {
      OpenScriptData scriptByRepositoryKey = this.FindOpenScriptByRepositoryKey(scriptProject.RepositoryKey);
      if (scriptByRepositoryKey != null)
        return scriptByRepositoryKey;
    }
    return (OpenScriptData) null;
  }

  public void ReplaceScriptProject(
    ScriptProject scriptProject,
    ScriptProject anotherScriptProject,
    bool readOnlyMode)
  {
    if (scriptProject == null)
      throw new ArgumentNullException(nameof (scriptProject));
    if (anotherScriptProject == null)
      throw new ArgumentNullException(nameof (scriptProject));
    this.CheckViewIsPresent();
    if (this.FindOpenScriptByScriptProjectOrRepositoryKey(anotherScriptProject) != null)
      throw new InvalidOperationException("The another script project is already open.");
    OpenScriptData scriptByScriptProject = this.FindOpenScriptByScriptProject(scriptProject);
    if (scriptByScriptProject != null)
    {
      IScriptWindow window = this.CloseScriptInternal(scriptByScriptProject, true);
      this.OpenScriptInternal(anotherScriptProject, readOnlyMode, window);
    }
    else
      this.OpenScriptInternal(anotherScriptProject, readOnlyMode);
  }

  private void CheckViewIsPresent()
  {
    if (this.View == null)
      throw new InvalidOperationException("The IDE presenter must have a view.");
  }

  public event EventHandler<ScriptProjectEventArgs> AfterOpenScriptProject;

  private void RaiseAfterOpenScriptProject(ScriptProject scriptProject)
  {
    if (this.AfterOpenScriptProject == null)
      return;
    this.AfterOpenScriptProject((object) this, new ScriptProjectEventArgs(scriptProject));
  }

  public event EventHandler<ScriptProjectEventArgs> AfterCloseScriptProject;

  private void RaiseAfterCloseScriptProject(ScriptProject scriptProject)
  {
    if (this.AfterCloseScriptProject == null)
      return;
    this.AfterCloseScriptProject((object) this, new ScriptProjectEventArgs(scriptProject));
  }

  public event EventHandler<ScriptProjectEventArgs> AfterSaveScriptProject;

  private void RaiseAfterSaveScriptProject(ScriptProject scriptProject)
  {
    if (this.AfterSaveScriptProject == null)
      return;
    this.AfterSaveScriptProject((object) this, new ScriptProjectEventArgs(scriptProject));
  }

  private void ProcessNewCommand(object sender, MultiCommandEventArgs e)
  {
    LanguageInfo tag = (LanguageInfo) e.Item.Tag;
    try
    {
      this.CreateScriptInternal(tag);
    }
    catch (Exception ex)
    {
      this.ShowUnhandledException(ex);
    }
  }

  private void ProcessSaveCommand(object sender, EventArgs e)
  {
    OpenScriptData activeScript = this.TryGetActiveScript();
    try
    {
      if (!activeScript.Modified)
        return;
      this.SaveScriptInternal(activeScript);
    }
    catch (Exception ex)
    {
      this.ShowUnhandledException(ex);
    }
  }

  private void ProcessSaveAsCommand(object sender, EventArgs e)
  {
    OpenScriptData activeScript = this.TryGetActiveScript();
    if (activeScript.Project.IsNew)
      this.SaveScriptInternal(activeScript);
    else
      this.SaveScriptAsInternal(activeScript);
  }

  private void ProcessSaveCopyCommand(object sender, EventArgs e)
  {
    OpenScriptData activeScript = this.TryGetActiveScript();
    ScriptProject emptyProject = this.model.ScriptSystem.CreateEmptyProject(activeScript.LanguageInfo);
    emptyProject.File.SetContentAsText(activeScript.Window.CodeEditor.GetScriptCode(), activeScript.Encoding);
    IScriptSaveChangesBehavior saveChangesBehavior = emptyProject.Behaviors.GetSaveChangesBehavior(false);
    if (saveChangesBehavior == null)
      return;
    ScriptBeforeSaveEventArgs e1 = new ScriptBeforeSaveEventArgs()
    {
      CanSave = true
    };
    saveChangesBehavior.BeforeSave(e1);
    if (!e1.CanSave)
      return;
    ScriptSaveAsParameters parameters = saveChangesBehavior.TrySaveAs();
    if (parameters == null)
      return;
    emptyProject.Behaviors.GetRepository().Add(emptyProject, parameters);
    ScriptAfterSaveEventArgs e2 = new ScriptAfterSaveEventArgs();
    saveChangesBehavior.AfterSave(e2);
  }

  private void ProcessOpenCommand(object sender, EventArgs e)
  {
    try
    {
      ScriptProject scriptProject = this.model.ScriptSystem.TryOpenScript((ICollection<LanguageInfo>) this.model.LanguageRegistry.Languages);
      if (scriptProject == null)
        return;
      this.OpenScriptInternal(scriptProject, false);
    }
    catch (Exception ex)
    {
      this.ShowUnhandledException(ex);
    }
  }

  private void ProcessHotKey(object sender, KeyEventArgs e)
  {
    if (e.Control)
    {
      if (e.KeyCode == Keys.O)
      {
        e.Handled = true;
        this.View.OpenCommand.PerformClick();
        return;
      }
      if (e.KeyCode == Keys.S)
      {
        e.Handled = true;
        this.View.SaveCommand.PerformClick();
        return;
      }
    }
    if (e.Shift && e.KeyCode == Keys.F5)
    {
      e.Handled = true;
      this.View.StopRunCommand.PerformClick();
    }
    if (e.KeyCode != Keys.F5)
      return;
    e.Handled = true;
    this.View.RunCommand.PerformClick();
  }

  private void OnViewClosing(object sender, CancelEventArgs e)
  {
    foreach (OpenScriptData openScript in this.openScripts)
    {
      if (openScript.Modified)
      {
        bool? nullable = this.AskSaveScriptIfModified(openScript, true);
        if (!nullable.HasValue)
        {
          e.Cancel = true;
          break;
        }
        if (!nullable.Value)
          openScript.Modified = false;
      }
    }
  }

  private bool? AskSaveScriptIfModified(OpenScriptData script, bool allowCancel)
  {
    YesNoMessagePresenter messagePresenter = new YesNoMessagePresenter();
    messagePresenter.Icon = MessageIcon.Question;
    messagePresenter.Caption = "Сохранение изменений";
    messagePresenter.Text = $"Сценарий '{script.GetDisplayName()}' имеет несохраненные изменения. Сохранить их?";
    messagePresenter.AllowCancel = allowCancel;
    MvpContext.ViewService.ShowModal((IPresenter) messagePresenter);
    if (messagePresenter.IsCancelled)
      return new bool?();
    if (messagePresenter.IsSuccessful)
      this.SaveScriptInternal(script);
    return new bool?(messagePresenter.IsSuccessful);
  }

  private void OnScriptWindowClosing(object sender, EventArgs e)
  {
    OpenScriptData linkedScript = this.TryGetLinkedScript((IScriptWindow) sender);
    if (linkedScript == null || !linkedScript.Modified)
      return;
    this.AskSaveScriptIfModified(linkedScript, false);
  }

  private void OnScriptWindowClosed(object sender, EventArgs e)
  {
    IScriptWindow window = (IScriptWindow) sender;
    OpenScriptData linkedScript = this.TryGetLinkedScript(window);
    if (linkedScript == null)
      return;
    if (this.lastActiveWindowWithScript == window)
    {
      this.DetachActiveScriptChildPresenters(linkedScript, true);
      this.lastActiveWindowWithScript = (IScriptWindow) null;
    }
    linkedScript.Window = (IScriptWindow) null;
    window.Script = (OpenScriptData) null;
    this.CloseScriptInternal(linkedScript);
  }

  private void OnActiveScriptWindowChanged(object sender, EventArgs e)
  {
    IScriptWindow activeWindow = this.View.ScriptWindows.ActiveWindow;
    OpenScriptData linkedScript = activeWindow != null ? this.TryGetLinkedScript(activeWindow) : (OpenScriptData) null;
    if (linkedScript != null)
    {
      if (this.lastActiveWindowWithScript != null)
      {
        this.DetachActiveScriptChildPresenters(this.TryGetLinkedScript(this.lastActiveWindowWithScript), false);
        this.lastActiveWindowWithScript = (IScriptWindow) null;
      }
      this.AttachActiveScriptChildPresenters(linkedScript);
      this.lastActiveWindowWithScript = linkedScript.Window;
      linkedScript.Window.CodeEditor.FocusAt(linkedScript.Window.CodeEditor.GetCaretPosition());
    }
    else if (this.lastActiveWindowWithScript != null)
    {
      this.DetachActiveScriptChildPresenters(this.TryGetLinkedScript(this.lastActiveWindowWithScript), activeWindow == null);
      this.lastActiveWindowWithScript = (IScriptWindow) null;
    }
    this.RaiseActiveScriptChanges();
  }

  private void AttachActiveScriptChildPresenters(OpenScriptData script)
  {
    foreach (IDEChildPresenter childPresenter in script.ChildPresenters)
      childPresenter.AttachView();
  }

  private void DetachActiveScriptChildPresenters(OpenScriptData script, bool fullDetach)
  {
    foreach (IDEChildPresenter childPresenter in script.ChildPresenters)
      childPresenter.DetachView(fullDetach);
  }

  internal event EventHandler ActiveScriptChanged;

  private void RaiseActiveScriptChanges()
  {
    if (this.ActiveScriptChanged == null)
      return;
    this.ActiveScriptChanged((object) this, EventArgs.Empty);
  }

  private void UpdateAfterIDESettingsChanged(IDESettings oldSettings, IDESettings newSettings)
  {
    if (newSettings.FontFamily != oldSettings.FontFamily || newSettings.FontSize != oldSettings.FontSize)
    {
      foreach (OpenScriptData openScript in this.openScripts)
        openScript.Window.CodeEditor.SetFont(newSettings.FontFamily, newSettings.FontSize);
    }
    if (newSettings.EnableCodeCompletion == oldSettings.EnableCodeCompletion && (!newSettings.EnableCodeCompletion || CollectionUtils.ContentEqual<string>((ICollection<string>) newSettings.XmlDocPathList, (ICollection<string>) oldSettings.XmlDocPathList)))
      return;
    if (newSettings.EnableCodeCompletion)
    {
      foreach (OpenScriptData openScript in this.openScripts)
        this.SetScriptCodeCompletion(openScript, openScript.Window);
    }
    else
    {
      foreach (OpenScriptData openScript in this.openScripts)
        openScript.Window.CodeEditor.SetCodeCompletionProvider();
    }
  }

  internal void ShowUnhandledException(Exception exception)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(exception.Message);
    int num = (int) MessageBox.Show(stringBuilder.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
  }
}
