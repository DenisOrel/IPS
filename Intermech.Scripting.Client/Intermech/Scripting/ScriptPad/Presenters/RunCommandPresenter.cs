// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Presenters.RunCommandPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Collections;
using Intermech.Mvp;
using Intermech.Mvp.Components;
using Intermech.Mvp.Components.Dialogs;
using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Presenters;

internal sealed class RunCommandPresenter(IDEPresenter idePresenter) : IDEChildPresenter(idePresenter)
{
  private static readonly ScriptProjectErrorRecord[] emptyErrors = new ScriptProjectErrorRecord[0];
  private RunCommandPresenter.RunContext runContext;
  private StoppableTask backgroundRunner;

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.IDEView.RunCommand.Click += new EventHandler(this.ProcessRunCommand);
    this.IDEView.StopRunCommand.Click += new EventHandler(this.ProcessStopRunCommand);
    this.IDEView.EditRunParametersCommand.Click += new EventHandler(this.ProcessEditRunParametersCommand);
    this.IDEView.EditExecutorSettingsCommand.Click += new EventHandler<MultiCommandEventArgs>(this.ProcessEditExecutorSettingsCommand);
    this.IDEView.ErrorsView.ShowSelectedError += new EventHandler(this.OnShowScriptCompilationError);
    this.IDEPresenter.ActiveScriptChanged += new EventHandler(this.OnActiveScriptChanged);
    this.UpdateRunCommandsState(this.IDEPresenter.TryGetActiveScript());
    this.UpdateExecutorSettingsCommandState();
  }

  protected override void OnDetachView(bool fullDetach)
  {
    if (this.runContext != null && this.backgroundRunner != null)
    {
      this.backgroundRunner.OnCompleted -= new EventHandler(this.OnBackgroundRunCompleted);
      this.backgroundRunner.Abort();
    }
    this.IDEView.RunCommand.Click -= new EventHandler(this.ProcessRunCommand);
    this.IDEView.StopRunCommand.Click -= new EventHandler(this.ProcessStopRunCommand);
    this.IDEView.EditRunParametersCommand.Click -= new EventHandler(this.ProcessEditRunParametersCommand);
    this.IDEView.EditExecutorSettingsCommand.Click -= new EventHandler<MultiCommandEventArgs>(this.ProcessEditExecutorSettingsCommand);
    this.IDEView.ErrorsView.ShowSelectedError -= new EventHandler(this.OnShowScriptCompilationError);
    if (fullDetach)
      this.ClearViewState();
    base.OnDetachView(fullDetach);
  }

  private void ClearViewState()
  {
    this.IDEView.RunCommand.Enabled = false;
    this.IDEView.StopRunCommand.Enabled = false;
    this.IDEView.EditRunParametersCommand.Enabled = false;
    this.IDEView.EditExecutorSettingsCommand.Enabled = false;
    this.IDEView.EditExecutorSettingsCommand.ClearItems();
  }

  private void UpdateRunCommandsState(OpenScriptData activeScript = null)
  {
    this.IDEView.RunCommand.Enabled = this.runContext == null && activeScript != null && activeScript.Project.Behaviors.GetDebugBehavior(false) != null;
    this.IDEView.EditRunParametersCommand.Enabled = this.IDEView.RunCommand.Enabled;
    this.IDEView.StopRunCommand.Enabled = this.runContext != null;
  }

  private void UpdateExecutorSettingsCommandState()
  {
    LinkedList<MultiCommandItem> subItems = CollectionUtils.ConvertAsLinkedList<LanguageInfo, MultiCommandItem>((ICollection<LanguageInfo>) this.IDEPresenter.Model.LanguageRegistry.Languages, (Converter<LanguageInfo, MultiCommandItem>) (item => new MultiCommandItem(item.Name, (object) item)));
    this.IDEView.EditExecutorSettingsCommand.SetItems((ICollection<MultiCommandItem>) subItems);
    this.IDEView.EditExecutorSettingsCommand.Enabled = subItems.Count != 0;
  }

  private void ProcessRunCommand(object sender, EventArgs e)
  {
    OpenScriptData activeScript = this.IDEPresenter.TryGetActiveScript();
    try
    {
      this.ProcessRunInternal(activeScript);
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }

  private void ProcessRunInternal(OpenScriptData script)
  {
    string unsavedScriptCode = script.Window.CodeEditor.GetScriptCode();
    if (string.IsNullOrEmpty(unsavedScriptCode))
      throw new ScriptDesignTimeException("Выполнение невозможно, так как код сценария пуст.");
    IScriptDebugBehavior debugBehavior = script.Project.Behaviors.GetDebugBehavior();
    this.runContext = new RunCommandPresenter.RunContext();
    this.runContext.Script = script;
    this.runContext.DebugBehavior = debugBehavior;
    this.runContext.LanguageSessionData = this.IDEPresenter.GetOrCreateLanguageSessionData(script.LanguageInfo.Name);
    this.runContext.LanguageSession = this.IDEPresenter.GetOrCreateLanguageSession(script.LanguageInfo.Name);
    if (this.backgroundRunner == null)
    {
      this.backgroundRunner = new StoppableTask();
      this.backgroundRunner.OnCompleted += new EventHandler(this.OnBackgroundRunCompleted);
    }
    this.backgroundRunner.Start((Func<object>) (() => (object) this.runContext.DebugBehavior.Execute(this.runContext.LanguageSession, unsavedScriptCode)));
    this.UpdateRunCommandsState(script);
  }

  private void OnBackgroundRunCompleted(object sender, EventArgs e)
  {
    switch (((StoppableTask) sender).State)
    {
      case StoppableTaskState.Finished:
        this.PostToViewThread(new Action(this.OnRunSuccessfullyFinished));
        break;
      case StoppableTaskState.Failed:
        this.PostToViewThread(new Action(this.OnRunFailed));
        break;
      case StoppableTaskState.Aborted:
        this.PostToViewThread(new Action(this.OnProgramAborted));
        break;
    }
  }

  private void OnRunSuccessfullyFinished()
  {
    try
    {
      this.IDEView.ErrorsView.SetErrors((ICollection<ScriptProjectErrorRecord>) RunCommandPresenter.emptyErrors);
      this.ShowRunResult(this.runContext.Script, (ScriptDebugInvocationResult) this.backgroundRunner.Result);
    }
    finally
    {
      this.ResetRunState();
    }
  }

  private void ShowRunResult(OpenScriptData script, ScriptDebugInvocationResult result)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (result.IsError)
      stringBuilder.AppendFormat("Script pad: сценарий '{0}' завершен с ошибкой", (object) script.GetDisplayName());
    else
      stringBuilder.AppendFormat("Script pad: сценарий '{0}' успешно завершен", (object) script.GetDisplayName());
    stringBuilder.Append('.');
    stringBuilder.Append(' ');
    stringBuilder.AppendFormat("Результат выполнения: {0}", (object) this.ConvertReturnValueToString(result.ReturnValue));
    this.IDEView.OutputView.WriteLine(stringBuilder.ToString());
  }

  private string ConvertReturnValueToString(object returnValue)
  {
    return returnValue != null ? Convert.ToString(returnValue) : "<null>";
  }

  private void OnRunFailed()
  {
    try
    {
      this.ShowErrors(this.runContext.Script, this.backgroundRunner.Exception);
      this.ShowRunException(this.runContext.Script, this.backgroundRunner.Exception);
    }
    finally
    {
      this.ResetRunState();
    }
  }

  private void ShowErrors(OpenScriptData script, Exception x)
  {
    if (x is ScriptCompilationException)
    {
      ScriptProject scriptProject = script.Project;
      string scriptDisplayName = script.GetDisplayName();
      this.IDEView.ErrorsView.SetErrors((ICollection<ScriptProjectErrorRecord>) CollectionUtils.ConvertAsArray<ScriptCompilationError, ScriptProjectErrorRecord>((ICollection<ScriptCompilationError>) ((ScriptCompilationException) x).Errors, (Converter<ScriptCompilationError, ScriptProjectErrorRecord>) (compilationError => new ScriptProjectErrorRecord(scriptProject, scriptDisplayName, compilationError))));
    }
    else
      this.IDEView.ErrorsView.SetErrors((ICollection<ScriptProjectErrorRecord>) RunCommandPresenter.emptyErrors);
  }

  private void ShowRunException(OpenScriptData script, Exception x)
  {
    switch (x)
    {
      case ScriptInvocationException _:
        this.ShowScriptInvocationException(script, x.InnerException);
        break;
      case ScriptCompilationException _:
        ScriptCompilationException compilationException = (ScriptCompilationException) x;
        int num1 = (int) MessageBox.Show(x.Message, "Ошибка компиляции сценария", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        if (compilationException.Errors.Count == 0)
          break;
        ScriptCompilationError error = compilationException.Errors[0];
        script.Window.CodeEditor.FocusAt(new TextCaretPosition(error.Line, error.Column));
        break;
      case ScriptStructureException _:
        int num2 = (int) MessageBox.Show(x.Message, "Ошибка в структуре сценария", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        break;
      case ScriptExecutorException _:
        int num3 = (int) MessageBox.Show(x.Message, "Невозможно выполнить сценарий", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        break;
      case TargetInvocationException _:
        this.ShowScriptInvocationException(script, x.InnerException);
        break;
      default:
        this.ShowScriptInvocationException(script, x);
        break;
    }
  }

  private void ShowScriptInvocationException(OpenScriptData script, Exception scriptException)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine(scriptException.Message);
    stringBuilder.AppendLine();
    stringBuilder.AppendFormat("Тип: {0}", (object) scriptException.GetType()).AppendLine();
    stringBuilder.AppendLine();
    stringBuilder.AppendLine("Стек:");
    stringBuilder.AppendLine(scriptException.StackTrace);
    string str = stringBuilder.ToString();
    this.IDEView.OutputView.WriteLine($"Script pad: при выполнении сценария '{script.GetDisplayName()}' произошло необработанное исключение");
    this.IDEView.OutputView.WriteLine(str);
    int num = (int) MessageBox.Show(str, "Ошибка выполнения сценария", MessageBoxButtons.OK, MessageBoxIcon.Hand);
  }

  private void OnProgramAborted()
  {
    try
    {
      this.IDEView.ErrorsView.SetErrors((ICollection<ScriptProjectErrorRecord>) RunCommandPresenter.emptyErrors);
      this.IDEView.OutputView.WriteLine($"Script pad: выполнение сценария '{this.runContext.Script.GetDisplayName()}' прервано");
      this.runContext.LanguageSessionData.ShutdownSession();
      this.runContext.LanguageSession = (ILanguageSession) null;
    }
    finally
    {
      this.ResetRunState();
    }
  }

  private void ResetRunState()
  {
    this.runContext = (RunCommandPresenter.RunContext) null;
    this.UpdateRunCommandsState(this.IDEPresenter.TryGetActiveScript());
  }

  private void ProcessStopRunCommand(object sender, EventArgs e)
  {
    try
    {
      this.backgroundRunner.Abort();
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }

  private void ProcessEditExecutorSettingsCommand(object sender, MultiCommandEventArgs e)
  {
    LanguageInfo tag = (LanguageInfo) e.Item.Tag;
    try
    {
      LanguageSessionData languageSessionData = this.IDEPresenter.GetOrCreateLanguageSessionData(tag.Name);
      if (this.runContext != null && this.runContext.LanguageSessionData == languageSessionData)
      {
        MvpContext.ViewService.ShowModal((IPresenter) new SimpleMessagePresenter($"В данный момент невозможно изменить параметры исполнителя, так как выполнение сценария '{this.runContext.Script.GetDisplayName()}' еще не закончено.", "Сообщение", MessageIcon.Warning));
      }
      else
      {
        ILanguageSessionService sessionService = languageSessionData.LanguageDescriptor.Services.GetSessionService();
        if (!sessionService.EditSessionParameters(languageSessionData.SessionParameters))
          return;
        languageSessionData.ShutdownSession();
        sessionService.SaveSessionParameters((ISettingsContainer) this.IDEPresenter.Model.SettingsService, languageSessionData.SessionParameters);
        this.IDEPresenter.Model.SettingsService.Flush();
      }
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }

  private void ProcessEditRunParametersCommand(object sender, EventArgs e)
  {
    OpenScriptData activeScript = this.IDEPresenter.TryGetActiveScript();
    try
    {
      activeScript.Project.Behaviors.GetDebugBehavior().EditArguments();
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }

  private void OnActiveScriptChanged(object sender, EventArgs e)
  {
    this.UpdateRunCommandsState(this.IDEPresenter.TryGetActiveScript());
  }

  private void OnShowScriptCompilationError(object sender, EventArgs e)
  {
    ScriptProjectErrorRecord selectedError = this.IDEView.ErrorsView.TryGetSelectedError();
    if (selectedError == null)
      return;
    OpenScriptData scriptByScriptProject = this.IDEPresenter.FindOpenScriptByScriptProject(selectedError.ScriptProject);
    if (scriptByScriptProject == null)
      return;
    this.IDEView.ScriptWindows.ActiveWindow = scriptByScriptProject.Window;
    scriptByScriptProject.Window.CodeEditor.FocusAt(new TextCaretPosition(selectedError.Error.Line, selectedError.Error.Column));
  }

  private sealed class RunContext
  {
    public OpenScriptData Script { get; set; }

    public IScriptDebugBehavior DebugBehavior { get; set; }

    public LanguageSessionData LanguageSessionData { get; set; }

    public ILanguageSession LanguageSession { get; set; }
  }
}
