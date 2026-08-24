// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Presenters.CodeWorkspacePresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common;
using Intermech.Scripting.Common.DesignTime;
using Intermech.UI.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Presenters;

internal sealed class CodeWorkspacePresenter : ActiveScriptChildPresenter
{
  private static readonly TimeSpan DocumentChangesCheckInterval = TimeSpan.FromMilliseconds(100.0);
  private static readonly TimeSpan DocumentChangesReactionInterval = TimeSpan.FromMilliseconds(1000.0);
  private ICodeModel codeModel;
  private CodeModelErrorController codeWorkspaceErrors;
  private bool codeWorkspaceOpen;
  private CodeNavigationProvider codeNavigationProvider;
  private CodeFoldingProvider codeFoldingProvider;
  private ScriptTextChangesBuilder textChangesBuilder;
  private DelayedUserInputHandler textChangesHandler;
  private bool isFirstAttach;

  public CodeWorkspacePresenter(
    IDEPresenter idePresenter,
    OpenScriptData script,
    ICodeModel codeModel)
    : base(idePresenter, script)
  {
    if (codeModel == null)
      throw new ArgumentNullException(nameof (codeModel));
    this.InitializeCodeModel(codeModel);
    this.isFirstAttach = true;
  }

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.OpenCodeWorkspace();
    this.CodeEditorControl.ScriptTextChanged += new EventHandler<ScriptTextChangedEventArgs>(this.OnScriptTextChanged);
    this.CodeEditorControl.CaretPositionChanged += new EventHandler(this.OnCaretPositionChanged);
    this.Script.Window.NavigateToCode += new EventHandler<NavigateToCodeEventArgs>(this.OnNavigateToCode);
    if (this.codeNavigationProvider.IsSupported)
    {
      this.Script.Window.EnableNavigationPanel = true;
      if (this.isFirstAttach)
        this.UpdateNavigationPanel();
    }
    if (this.codeFoldingProvider.IsSupported)
    {
      this.CodeEditorControl.SetCodeFoldingProvider(this.codeFoldingProvider);
      if (this.isFirstAttach)
        this.CodeEditorControl.UpdateRegionFoldings();
    }
    if (this.isFirstAttach)
      this.isFirstAttach = false;
    this.textChangesHandler.ProcessUserInput += new EventHandler(this.ApplyDocumentChanges);
    this.textChangesHandler.Start();
    this.EnableCodeModelLogging(true);
  }

  protected override void OnDetachView(bool fullDetach)
  {
    this.CloseCodeWorkspace();
    this.textChangesHandler.Stop();
    this.textChangesHandler.ProcessUserInput -= new EventHandler(this.ApplyDocumentChanges);
    if (this.codeNavigationProvider.IsSupported)
      this.Script.Window.EnableNavigationPanel = false;
    if (this.codeFoldingProvider.IsSupported)
      this.CodeEditorControl.SetCodeFoldingProvider();
    this.CodeEditorControl.ScriptTextChanged -= new EventHandler<ScriptTextChangedEventArgs>(this.OnScriptTextChanged);
    this.CodeEditorControl.CaretPositionChanged -= new EventHandler(this.OnCaretPositionChanged);
    this.Script.Window.NavigateToCode -= new EventHandler<NavigateToCodeEventArgs>(this.OnNavigateToCode);
    base.OnDetachView(fullDetach);
  }

  private void OnScriptTextChanged(object sender, ScriptTextChangedEventArgs e)
  {
    if (!this.IsCodeWorkspaceAllowed())
      return;
    this.textChangesBuilder.Add(e.TextChange);
    this.textChangesHandler.RegisterUserInput();
  }

  private void OnCaretPositionChanged(object sender, EventArgs e)
  {
    this.UpdateNavigationPanelSelection();
  }

  private void OnNavigateToCode(object sender, NavigateToCodeEventArgs e)
  {
    NavigationItem navigationItem = e.NavigationItem;
    NavigationPosition start = navigationItem.SelectionRange.Start;
    TextCaretPosition textCaretPosition = new TextCaretPosition(start.Line, start.Character);
    NavigationPosition end = navigationItem.SelectionRange.End;
    TextCaretPosition endPosition = new TextCaretPosition(end.Line, end.Character);
    this.CodeEditorControl.FocusAt(textCaretPosition);
    this.CodeEditorControl.Select(textCaretPosition, endPosition);
  }

  private void ApplyDocumentChanges(object sender, EventArgs e)
  {
    if (this.textChangesBuilder.IsEmpty || !this.IsCodeWorkspaceAllowed())
      return;
    if (this.CheckCodeWorkspaceSynchronization())
    {
      List<ScriptTextChange> textChanges = this.textChangesBuilder.Build();
      if (textChanges.Count == 0)
        return;
      this.UpdateCodeWorkspace(textChanges);
    }
    if (this.codeNavigationProvider.IsSupportedAndAllowed && this.CheckCodeWorkspaceSynchronization())
    {
      this.UpdateNavigationPanel();
      this.UpdateNavigationPanelSelection();
    }
    if (!this.codeFoldingProvider.IsSupportedAndAllowed || !this.CheckCodeWorkspaceSynchronization())
      return;
    this.CodeEditorControl.UpdateRegionFoldings();
  }

  private void InitializeCodeModel(ICodeModel codeModel)
  {
    this.codeModel = codeModel;
    this.codeWorkspaceErrors = new CodeModelErrorController();
    this.codeNavigationProvider = new CodeNavigationProvider(this.codeModel);
    this.codeNavigationProvider.CodeModelRecoveryAction = new Action<Exception>(this.RecoverCodeWorkspaceAfterGetNavigationItems);
    this.codeFoldingProvider = new CodeFoldingProvider(this.codeModel);
    this.codeFoldingProvider.CodeModelRecoveryAction = new Action<Exception>(this.RecoverCodeWorkspaceAfterGetFoldingRegions);
    this.textChangesBuilder = new ScriptTextChangesBuilder();
    this.textChangesHandler = new DelayedUserInputHandler(CodeWorkspacePresenter.DocumentChangesCheckInterval, CodeWorkspacePresenter.DocumentChangesReactionInterval);
  }

  private void OpenCodeWorkspace()
  {
    this.CloseCodeWorkspace();
    if (!this.codeWorkspaceErrors.IsCapabilityAllowed)
      return;
    try
    {
      this.codeModel.OpenText(this.CodeEditorControl.GetScriptCode());
      this.codeWorkspaceErrors.Reset();
      this.codeWorkspaceOpen = true;
    }
    catch (Exception ex)
    {
      this.UpdateOutput("При подключении к языковому сервису произошла ошибка.");
      this.UpdateOutput($"[Debug]: {ex.GetType()}, {ex.Message}");
      this.codeWorkspaceErrors.RegisterError();
      if (this.codeWorkspaceErrors.IsCapabilityAllowed)
        return;
      this.UpdateOutput("Языковой сервис был полностью отключен из-за нескольких ошибок подряд.");
    }
  }

  private void CloseCodeWorkspace()
  {
    if (!this.codeWorkspaceOpen)
      return;
    try
    {
      if (this.textChangesHandler.IsStarted)
        this.textChangesHandler.CancelUserInput();
      this.textChangesBuilder.Clear();
      this.codeModel.CloseText(false);
      this.codeNavigationProvider.Errors.Reset();
      this.codeFoldingProvider.Errors.Reset();
    }
    catch (Exception ex)
    {
      this.UpdateOutput("При отключении от языкового сервиса произошла ошибка.");
      this.UpdateOutput($"[Debug]: {ex.GetType()}, {ex.Message}");
    }
    finally
    {
      this.codeWorkspaceOpen = false;
    }
  }

  private bool IsCodeWorkspaceAllowed() => this.codeWorkspaceErrors.IsCapabilityAllowed;

  private bool IsCodeWorkspaceOpen() => this.codeWorkspaceOpen;

  private void RecoverCodeWorkspaceAfterError()
  {
    if (this.codeModel.CheckSynchronizationStatus() != CodeModelSynchronizationStatus.SynchronizationLost)
      return;
    this.UpdateOutput("Выполняется попытка восстановить подключение к языковому сервису...");
    this.OpenCodeWorkspace();
  }

  private bool CheckCodeWorkspaceSynchronization()
  {
    CodeModelSynchronizationStatus notSupportedValue = this.codeModel.CheckSynchronizationStatus();
    switch (notSupportedValue)
    {
      case CodeModelSynchronizationStatus.NonSynchronized:
        return false;
      case CodeModelSynchronizationStatus.Synchronized:
        return true;
      case CodeModelSynchronizationStatus.SynchronizationLost:
        this.UpdateOutput("Подключение к языковому сервису утеряно. Соответствующие функции редактора могут быть недоступны в течение нескольких секунд. Повторите попытку позже.");
        this.OpenCodeWorkspace();
        return false;
      default:
        throw new NotSupportedEnumException((Enum) notSupportedValue);
    }
  }

  private void UpdateCodeWorkspace(List<ScriptTextChange> textChanges)
  {
    try
    {
      this.codeModel.ChangeText(textChanges);
    }
    catch (Exception ex)
    {
      this.UpdateOutput("При обновлении данных языкового сервиса произошла ошибка.");
      this.UpdateOutput($"[Debug]: {ex.GetType()}, {ex.Message}");
      this.RecoverCodeWorkspaceAfterError();
    }
  }

  private void UpdateNavigationPanel()
  {
    IList<NavigationItem> navigationItemsIfPossible = this.codeNavigationProvider.TryGetNavigationItemsIfPossible();
    if (navigationItemsIfPossible == null)
      return;
    List<NavigationItem> navigationItemList = new List<NavigationItem>();
    NavigationItem memberToSelect = (NavigationItem) null;
    NavigationItem navigationItemToSelect = this.GetNavigationItemToSelect(this.Script.Window.SelectedType, (IEnumerable<NavigationItem>) navigationItemsIfPossible);
    if (navigationItemToSelect != null)
    {
      navigationItemList = navigationItemToSelect.Children;
      memberToSelect = this.GetNavigationItemToSelect(this.Script.Window.SelectedMember, (IEnumerable<NavigationItem>) navigationItemList);
    }
    this.Script.Window.UpdateNavigationTypes(navigationItemsIfPossible, navigationItemToSelect);
    this.Script.Window.UpdateNavigationMembers((IList<NavigationItem>) navigationItemList, memberToSelect);
  }

  private NavigationItem GetNavigationItemToSelect(
    NavigationItem oldSelectedItem,
    IEnumerable<NavigationItem> items)
  {
    if (!items.Any<NavigationItem>())
      return (NavigationItem) null;
    NavigationItem navigationItemToSelect = (NavigationItem) null;
    IOrderedEnumerable<NavigationItem> source = items.OrderBy<NavigationItem, NavigationRange>((Func<NavigationItem, NavigationRange>) (item => item.Range));
    if (oldSelectedItem != null)
    {
      foreach (NavigationItem navigationItem in items)
      {
        if (navigationItem.FullName == oldSelectedItem.FullName && navigationItem.Type == oldSelectedItem.Type || navigationItem.Range.CompareTo((object) oldSelectedItem.Range) == 0)
        {
          navigationItemToSelect = navigationItem;
          break;
        }
      }
      if (navigationItemToSelect == null)
      {
        foreach (NavigationItem navigationItem in (IEnumerable<NavigationItem>) source)
        {
          if (oldSelectedItem.Range.Start.CompareTo((object) navigationItem.Range.Start) < 0)
          {
            navigationItemToSelect = navigationItem;
            break;
          }
        }
      }
      if (navigationItemToSelect == null)
        navigationItemToSelect = source.Last<NavigationItem>();
    }
    else
      navigationItemToSelect = source.First<NavigationItem>();
    return navigationItemToSelect;
  }

  private void UpdateNavigationPanelSelection()
  {
    TextCaretPosition caretPosition = this.CodeEditorControl.GetCaretPosition();
    this.UpdateNavigationPanelSelection(new NavigationPosition(caretPosition.Line, caretPosition.Column));
  }

  private void UpdateNavigationPanelSelection(NavigationPosition caretPosition)
  {
    List<NavigationItem> navigationItemList = new List<NavigationItem>();
    NavigationItem memberToSelect = (NavigationItem) null;
    List<NavigationItem> navigationTypes = this.Script.Window.NavigationTypes;
    NavigationItem navigationItemToSelect = this.GetNavigationItemToSelect(caretPosition, (IEnumerable<NavigationItem>) navigationTypes);
    if (navigationItemToSelect != null)
    {
      navigationItemList = navigationItemToSelect.Children;
      memberToSelect = this.GetNavigationItemToSelect(caretPosition, (IEnumerable<NavigationItem>) navigationItemList);
    }
    if (object.Equals((object) this.Script.Window.SelectedType, (object) navigationItemToSelect))
    {
      this.Script.Window.UpdateNavigationMembersSelection(memberToSelect);
    }
    else
    {
      this.Script.Window.UpdateNavigationTypesSelection(navigationItemToSelect);
      this.Script.Window.UpdateNavigationMembers((IList<NavigationItem>) navigationItemList, memberToSelect);
    }
  }

  private NavigationItem GetNavigationItemToSelect(
    NavigationPosition caretPosition,
    IEnumerable<NavigationItem> items)
  {
    if (!items.Any<NavigationItem>())
      return (NavigationItem) null;
    NavigationItem navigationItemToSelect = (NavigationItem) null;
    IOrderedEnumerable<NavigationItem> source = items.OrderBy<NavigationItem, NavigationRange>((Func<NavigationItem, NavigationRange>) (item => item.Range));
    foreach (NavigationItem navigationItem in (IEnumerable<NavigationItem>) source)
    {
      if (navigationItem.Range.Start.CompareTo((object) caretPosition) <= 0 && navigationItem.Range.End.CompareTo((object) caretPosition) >= 0)
        navigationItemToSelect = navigationItem;
    }
    if (navigationItemToSelect == null)
    {
      foreach (NavigationItem navigationItem in (IEnumerable<NavigationItem>) source)
      {
        if (caretPosition.CompareTo((object) navigationItem.Range.Start) < 0)
        {
          navigationItemToSelect = navigationItem;
          break;
        }
      }
    }
    if (navigationItemToSelect == null)
      navigationItemToSelect = source.First<NavigationItem>();
    return navigationItemToSelect;
  }

  private void EnableCodeModelLogging(bool enableLogging)
  {
    if (enableLogging)
      this.codeModel.Log += new Action<string>(this.UpdateOutput);
    else
      this.codeModel.Log -= new Action<string>(this.UpdateOutput);
  }

  private void UpdateOutput(string message) => this.IDEView.OutputView.WriteLine("> " + message);

  private void RecoverCodeWorkspaceAfterGetNavigationItems(Exception x)
  {
    this.UpdateOutput("При обращении к языковому сервису навигационных списков произошла ошибка. Не удалось обновить информацию.");
    this.UpdateOutput($"[Debug]: {x.GetType()}, {x.Message}");
    this.RecoverCodeWorkspaceAfterError();
  }

  private void RecoverCodeWorkspaceAfterGetFoldingRegions(Exception x)
  {
    this.UpdateOutput("При обращении к сервису получения директив произошла ошибка. Не удалось обновить информацию.");
    this.UpdateOutput($"[Debug]: {x.GetType()}, {x.Message}");
    this.RecoverCodeWorkspaceAfterError();
  }

  private void RecoverCodeWorkspaceAfterGetHoverInfo(Exception x)
  {
    this.UpdateOutput("При обращении к сервису получения всплывающих подсказок по коду сценария произошла ошибка. Не удалось обновить информацию.");
    this.UpdateOutput($"[Debug]: {x.GetType()}, {x.Message}");
    this.RecoverCodeWorkspaceAfterError();
  }
}
