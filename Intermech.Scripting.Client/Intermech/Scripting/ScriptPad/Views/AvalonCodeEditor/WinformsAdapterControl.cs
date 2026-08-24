// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.WinformsAdapterControl
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Search;
using Intermech.Collections;
using Intermech.Scripting.Common;
using Intermech.Scripting.Common.DesignTime;
using Intermech.UI.Winforms;
using Intermech.UI.Wpf.Controls;
using Intermech.UI.Wpf.WinformsInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal class WinformsAdapterControl : 
  System.Windows.Forms.UserControl,
  IScriptCodeEditorControl,
  IScriptCodeEditorUndoRedo,
  IScriptCodeEditorCutCopyPaste
{
  private TextEditorViewModel textEditorVM;
  private TextEditor textEditorControl;
  private TextArea textAreaControl;
  private string fakeFileName;
  private bool suppressScriptCodeEditorEvents;
  private ICodeCompletionProvider codeCompletionProvider;
  private CompletionWindow completionWindow;
  private OverloadInsightWindow insightWindow;
  private Form hotkeyForm;
  private bool hotkeysAllowed;
  private GoToLineForm gotoTool;
  private SearchPanel searchPanel;
  private FindReplaceManager findReplaceManager;
  private FoldingManager foldingManager;
  private CodeFoldingStrategy foldingStrategy;
  private HoverInfoProvider codeHoverInfoProvider;
  private System.Windows.Controls.ToolTip codeHoverInfoTooltip;
  private static readonly SyntaxDefinitions globalSyntaxDefinitions = new SyntaxDefinitions();
  private IContainer components;
  private WpfElementHost wpfElementHost;
  private TextEditorUserControl wpfWrapperControl;

  public WinformsAdapterControl()
  {
    this.InitializeComponent();
    if (DesignerServices.IsInDesignMode((Component) this, true))
      return;
    WinformsAdapterControl.globalSyntaxDefinitions.LoadSyntaxDefinitions();
    this.InitializeWpfControls();
  }

  private void InitializeWpfControls()
  {
    this.textEditorVM = new TextEditorViewModel();
    TextEditorUserControl child = (TextEditorUserControl) this.wpfElementHost.Child;
    child.DataContext = (object) this.textEditorVM;
    child.Loaded += new RoutedEventHandler(this.OnWpfUserControlLoaded);
    this.textEditorControl = child.CodeEditor;
    this.textAreaControl = this.textEditorControl.TextArea;
    this.InitializeSearchPanel();
  }

  private void DisposeWinformsAdapterControl() => this.UninstallFindReplaceDialog();

  private void InitializeSearchPanel()
  {
    this.searchPanel = SearchPanel.Install(this.textAreaControl);
    TextEditorHotkeyHelper.PatchHotkeys(this.textEditorControl);
  }

  private void InstallFindReplaceDialog(Form parentForm)
  {
    this.findReplaceManager = new FindReplaceManager()
    {
      OwnerWindow = (IFindReplaceTextEditorWindow) new FindReplaceOwnerWindowAdapter(this.GetRealParentForm(parentForm)),
      CurrentEditor = (IFindReplaceTextEditor) new FindReplaceTextEditorAdapter(this.textEditorControl),
      ShowSearchIn = false
    };
    this.textEditorControl.TextArea.CommandBindings.Add(this.findReplaceManager.FindBinding);
    this.textEditorControl.TextArea.CommandBindings.Add(this.findReplaceManager.ReplaceBinding);
    this.textEditorControl.TextArea.CommandBindings.Add(this.findReplaceManager.FindNextBinding);
  }

  private void UninstallFindReplaceDialog()
  {
    if (this.findReplaceManager == null)
      return;
    this.textEditorControl.TextArea.CommandBindings.Remove(this.findReplaceManager.FindBinding);
    this.textEditorControl.TextArea.CommandBindings.Remove(this.findReplaceManager.ReplaceBinding);
    this.textEditorControl.TextArea.CommandBindings.Remove(this.findReplaceManager.FindNextBinding);
    this.findReplaceManager.CloseWindow();
    this.findReplaceManager = (FindReplaceManager) null;
  }

  private Form GetRealParentForm(Form actualParentForm)
  {
    System.Windows.Forms.Control realParentForm = (System.Windows.Forms.Control) actualParentForm;
    while (realParentForm.Parent != null)
      realParentForm = realParentForm.Parent;
    return (Form) realParentForm;
  }

  public void Initialize(LanguageInfo languageInfo, string scriptCode, bool readOnlyMode)
  {
    if (languageInfo == null)
      throw new ArgumentNullException(nameof (languageInfo));
    if (scriptCode == null)
      throw new ArgumentNullException(nameof (scriptCode));
    this.suppressScriptCodeEditorEvents = true;
    try
    {
      this.SetContextMenuActions((IList<ITextEditorUIAction>) null);
      this.SetCodeCompletionProvider((ICodeCompletionProvider) null);
      this.fakeFileName = "script" + languageInfo.SourceExtension;
      this.SetScriptText(scriptCode, readOnlyMode);
      this.SelectHighlighter(languageInfo);
    }
    finally
    {
      this.suppressScriptCodeEditorEvents = false;
    }
  }

  private void SetScriptText(string scriptCode, bool readOnlyMode)
  {
    this.textEditorVM.Document.Text = scriptCode;
    this.textEditorVM.Document.UndoStack.ClearAll();
    this.textEditorVM.IsReadOnly = readOnlyMode;
  }

  private void SelectHighlighter(LanguageInfo languageInfo)
  {
    IHighlightingDefinition definitionByExtension = HighlightingManager.Instance.GetDefinitionByExtension(languageInfo.SourceExtension);
    if (definitionByExtension == null)
      return;
    this.textEditorControl.SyntaxHighlighting = definitionByExtension;
  }

  public void SetContextMenuActions(IList<ITextEditorUIAction> contextMenuActions = null)
  {
    this.textEditorVM.ContextMenu.Items.Clear();
    if (contextMenuActions == null || contextMenuActions.Count == 0)
      return;
    foreach (ITextEditorUIAction contextMenuAction in (IEnumerable<ITextEditorUIAction>) contextMenuActions)
      this.textEditorVM.ContextMenu.Items.Add(new CodeEditorMenuItemViewModel(contextMenuAction));
  }

  public void SetCodeCompletionProvider(ICodeCompletionProvider codeCompletionProvider = null)
  {
    if (this.codeCompletionProvider != null)
    {
      this.DeactivateCodeCompletionSystem();
      this.codeCompletionProvider = (ICodeCompletionProvider) null;
    }
    if (codeCompletionProvider == null)
      return;
    this.codeCompletionProvider = codeCompletionProvider;
    this.ActivateCodeCompletionSystem();
  }

  public void SetCodeFoldingProvider(CodeFoldingProvider codeFoldingProvider = null)
  {
    if (codeFoldingProvider == null)
    {
      if (this.foldingManager != null)
      {
        FoldingManager.Uninstall(this.foldingManager);
        this.foldingManager = (FoldingManager) null;
      }
      this.foldingStrategy = (CodeFoldingStrategy) null;
    }
    else
    {
      if (this.foldingManager != null)
      {
        FoldingManager.Uninstall(this.foldingManager);
        this.foldingManager = FoldingManager.Install(this.textAreaControl);
      }
      else
        this.foldingManager = FoldingManager.Install(this.textAreaControl);
      this.foldingStrategy = new CodeFoldingStrategy(this.foldingManager, codeFoldingProvider);
    }
  }

  public void UpdateRegionFoldings() => this.foldingStrategy?.UpdateFoldings();

  public void SetCodeHoverInfoProvider(HoverInfoProvider provider = null)
  {
    if (this.codeHoverInfoProvider != null)
    {
      this.DeactivateCodeHoverInfoSystem();
      this.codeHoverInfoProvider = (HoverInfoProvider) null;
    }
    if (provider == null)
      return;
    this.codeHoverInfoProvider = provider;
    this.ActivateCodeHoverInfoSystem();
  }

  public void SetFont(string fontFamily, int fontSize)
  {
    if (fontFamily == null)
      throw new ArgumentNullException(nameof (fontFamily));
    if (fontSize <= 0)
      throw new ArgumentOutOfRangeException(nameof (fontSize));
    this.textEditorVM.FontFamily = fontFamily;
    this.textEditorVM.FontSize = fontSize;
  }

  public TextCaretPosition GetCaretPosition()
  {
    Caret caret = this.textAreaControl.Caret;
    return new TextCaretPosition(caret.Line, caret.Column);
  }

  public int GetCaretOffset() => this.textEditorVM.CaretOffset;

  public void FocusAt(TextCaretPosition caretPosition)
  {
    if (caretPosition == null)
      throw new ArgumentNullException(nameof (caretPosition));
    Caret caret = this.textAreaControl.Caret;
    if (caret.Line != caretPosition.Line || caret.Column != caretPosition.Column)
    {
      this.textEditorControl.SelectionLength = 0;
      caret.Position = new TextViewPosition(caretPosition.Line, caretPosition.Column);
      this.textAreaControl.Caret.BringCaretToView();
    }
    if (this.textEditorControl.IsFocused)
      return;
    this.textEditorControl.Focus();
  }

  public event EventHandler CaretPositionChanged;

  private void RaiseCaretPositionChanged()
  {
    if (this.CaretPositionChanged == null || this.suppressScriptCodeEditorEvents)
      return;
    this.CaretPositionChanged((object) this, EventArgs.Empty);
  }

  public string GetScriptCode() => this.textEditorVM.Document.Text;

  public ITextEditor GetScriptCodeAsTextEditor()
  {
    return (ITextEditor) this.textEditorVM.AsDesignTimeTextEditor();
  }

  private void OnTextAreaControlCaretPositionChanged(object sender, EventArgs e)
  {
    this.RaiseCaretPositionChanged();
  }

  private void OnTextEditorControlTextChanged(object sender, EventArgs e)
  {
    this.RaiseScriptCodeChanged();
  }

  public event EventHandler ScriptCodeChanged;

  private void RaiseScriptCodeChanged()
  {
    if (this.ScriptCodeChanged == null || this.suppressScriptCodeEditorEvents)
      return;
    this.ScriptCodeChanged((object) this, EventArgs.Empty);
  }

  private void OnTextEditorDocumentChanged(object sender, DocumentChangeEventArgs e)
  {
    ScriptTextChange scriptTextChange = new ScriptTextChange(e.Offset, e.RemovalLength, e.InsertedText.Text);
    this.RaiseScriptTextChanged(e);
  }

  public event EventHandler<ScriptTextChangedEventArgs> ScriptTextChanged;

  private void RaiseScriptTextChanged(DocumentChangeEventArgs e)
  {
    if (this.ScriptTextChanged == null || this.suppressScriptCodeEditorEvents)
      return;
    this.ScriptTextChanged((object) this, new ScriptTextChangedEventArgs(new ScriptTextChange(e.Offset, e.RemovalLength, e.InsertedText.Text)));
  }

  public void Select(TextCaretPosition startPosition, TextCaretPosition endPosition)
  {
    if (startPosition == null)
      throw new ArgumentNullException(nameof (startPosition));
    if (endPosition == null)
      throw new ArgumentNullException(nameof (endPosition));
    this.textAreaControl.Selection = Selection.Create(this.textAreaControl, this.textEditorVM.Document.GetOffset(startPosition.Line, startPosition.Column), this.textEditorVM.Document.GetOffset(endPosition.Line, endPosition.Column));
  }

  public bool HasSelection() => this.textEditorControl.SelectionLength != 0;

  public Intermech.Scripting.ScriptPad.TextSegment TryGetSelection()
  {
    int selectionLength = this.textEditorControl.SelectionLength;
    return selectionLength != 0 ? new Intermech.Scripting.ScriptPad.TextSegment(this.textEditorControl.SelectionStart, selectionLength) : (Intermech.Scripting.ScriptPad.TextSegment) null;
  }

  private void OnTextAreaControlSelectionChanged(object sender, EventArgs e)
  {
    this.RaiseSelectionChanged();
  }

  public event EventHandler SelectionChanged;

  private void RaiseSelectionChanged()
  {
    if (this.SelectionChanged == null || this.suppressScriptCodeEditorEvents)
      return;
    this.SelectionChanged((object) this, EventArgs.Empty);
  }

  public void ShowFindReplaceDialog() => this.findReplaceManager.ShowAsFind();

  private void OnWpfUserControlLoaded(object sender, RoutedEventArgs e)
  {
    this.textEditorControl.TextChanged += new EventHandler(this.OnTextEditorControlTextChanged);
    this.textEditorControl.Document.Changed += new EventHandler<DocumentChangeEventArgs>(this.OnTextEditorDocumentChanged);
    this.textAreaControl.SelectionChanged += new EventHandler(this.OnTextAreaControlSelectionChanged);
    this.textAreaControl.Caret.PositionChanged += new EventHandler(this.OnTextAreaControlCaretPositionChanged);
  }

  bool IScriptCodeEditorUndoRedo.CanUndo => this.textEditorControl.CanUndo;

  bool IScriptCodeEditorUndoRedo.CanRedo => this.textEditorControl.CanRedo;

  void IScriptCodeEditorUndoRedo.Undo() => this.textEditorControl.Undo();

  void IScriptCodeEditorUndoRedo.Redo() => this.textEditorControl.Redo();

  void IScriptCodeEditorCutCopyPaste.Cut() => this.textEditorControl.Cut();

  void IScriptCodeEditorCutCopyPaste.Copy() => this.textEditorControl.Copy();

  void IScriptCodeEditorCutCopyPaste.Paste() => this.textEditorControl.Paste();

  private void ActivateCodeCompletionSystem()
  {
    if (string.IsNullOrEmpty(this.textEditorVM.Document.FileName))
      this.textEditorVM.SetDocumentFileName(this.fakeFileName);
    this.textAreaControl.TextEntering += new TextCompositionEventHandler(this.OnTextAreaControlTextEntering);
    this.textAreaControl.TextEntered += new TextCompositionEventHandler(this.OnTextAreaControlTextEntered);
    this.textEditorControl.CommandBindings.Add(new CommandBinding((ICommand) new RoutedCommand()
    {
      InputGestures = {
        (InputGesture) new KeyGesture(Key.Space, System.Windows.Input.ModifierKeys.Control)
      }
    }, new ExecutedRoutedEventHandler(this.OnCtrlSpaceCommand)));
    this.textEditorControl.CommandBindings.Add(new CommandBinding((ICommand) new RoutedCommand()
    {
      InputGestures = {
        (InputGesture) new KeyGesture(Key.Space, System.Windows.Input.ModifierKeys.Control | System.Windows.Input.ModifierKeys.Shift)
      }
    }, new ExecutedRoutedEventHandler(this.OnCtrlShiftSpaceCommand)));
  }

  private void DeactivateCodeCompletionSystem()
  {
    if (this.insightWindow != null)
      this.insightWindow.Close();
    if (this.completionWindow == null)
      return;
    this.completionWindow.Close();
  }

  private void OnTextAreaControlTextEntering(object sender, TextCompositionEventArgs e)
  {
    if (this.completionWindow == null || string.IsNullOrEmpty(e.Text) || char.IsLetterOrDigit(e.Text[0]))
      return;
    this.completionWindow.CompletionList.RequestInsertion((EventArgs) e);
  }

  private void OnTextAreaControlTextEntered(object sender, TextCompositionEventArgs e)
  {
    this.ShowCompletionWindow(e.Text, false, true);
  }

  private void OnCtrlSpaceCommand(object sender, ExecutedRoutedEventArgs e)
  {
    this.ShowCompletionWindow((string) null, true, false);
  }

  private void OnCtrlShiftSpaceCommand(object sender, ExecutedRoutedEventArgs e)
  {
    this.ShowCompletionWindow((string) null, false, true);
  }

  private void ShowCompletionWindow(string enteredText, bool manualMode, bool includeInsightItems)
  {
    if (string.IsNullOrEmpty(this.textEditorVM.Document.FileName) || this.codeCompletionProvider == null)
      return;
    if (this.insightWindow != null & manualMode && !includeInsightItems)
      this.insightWindow.Close();
    if (this.insightWindow != null && (enteredText == ")" || enteredText == "]"))
      this.insightWindow.Close();
    if (this.insightWindow != null || this.completionWindow != null)
      return;
    CodeCompletionResultBuilder resultBuilder = new CodeCompletionResultBuilder();
    try
    {
      this.codeCompletionProvider.GetResult((IReadOnlyTextDocument) new ReadOnlyTextDocumentAdapter(this.textEditorVM.Document), this.textEditorControl.CaretOffset, manualMode, includeInsightItems, (ICodeCompletionResultBuilder) resultBuilder);
    }
    catch (Exception ex)
    {
      return;
    }
    if (this.insightWindow == null && resultBuilder.OverloadInsightItems.Count != 0)
    {
      if (this.completionWindow != null)
        this.completionWindow.Close();
      this.insightWindow = new OverloadInsightWindow(this.textAreaControl);
      this.insightWindow.Provider = (IOverloadProvider) new OverloadInsightProvider(resultBuilder.OverloadInsightItems);
      this.insightWindow.Closed += (EventHandler) ((o, args) => this.insightWindow = (OverloadInsightWindow) null);
      this.insightWindow.Show();
    }
    else
    {
      if (this.insightWindow != null || this.completionWindow != null || resultBuilder.CompletionDataItems.Count == 0)
        return;
      this.completionWindow = new CompletionWindow(this.textAreaControl);
      this.completionWindow.SizeToContent = SizeToContent.WidthAndHeight;
      this.completionWindow.CloseWhenCaretAtBeginning = manualMode;
      this.completionWindow.CompletionList.CompletionData.AddRange<ICompletionData>((IEnumerable<ICompletionData>) resultBuilder.CompletionDataItems);
      if (resultBuilder.CompletionTriggerWord != string.Empty)
      {
        this.completionWindow.StartOffset -= resultBuilder.CompletionTriggerWord.Length;
        this.completionWindow.CompletionList.SelectItem(resultBuilder.CompletionTriggerWord);
      }
      this.completionWindow.Closed += (EventHandler) ((s, e) => this.completionWindow = (CompletionWindow) null);
      this.completionWindow.Show();
    }
  }

  private void ActivateCodeHoverInfoSystem()
  {
    this.codeHoverInfoTooltip = new System.Windows.Controls.ToolTip();
    this.codeHoverInfoTooltip.PlacementTarget = (UIElement) this.textEditorControl;
    this.textEditorControl.MouseHover += new System.Windows.Input.MouseEventHandler(this.OnTextEditorControlMouseOver);
    this.textEditorControl.MouseHoverStopped += new System.Windows.Input.MouseEventHandler(this.OnTextEditorControlMouseOverStopper);
  }

  private void DeactivateCodeHoverInfoSystem()
  {
    this.textEditorControl.MouseHover -= new System.Windows.Input.MouseEventHandler(this.OnTextEditorControlMouseOver);
    this.textEditorControl.MouseHoverStopped -= new System.Windows.Input.MouseEventHandler(this.OnTextEditorControlMouseOverStopper);
    this.codeHoverInfoTooltip.IsOpen = false;
    this.codeHoverInfoTooltip = (System.Windows.Controls.ToolTip) null;
  }

  private void OnTextEditorControlMouseOver(object sender, System.Windows.Input.MouseEventArgs e)
  {
    if (this.codeHoverInfoProvider == null)
      return;
    TextViewPosition? positionFromPoint = this.textEditorControl.GetPositionFromPoint(e.GetPosition((IInputElement) this.textEditorControl));
    if (!positionFromPoint.HasValue)
      return;
    HoverInfo hoverInfoIfPossible = this.codeHoverInfoProvider.TryGetHoverInfoIfPossible(this.textEditorVM.Document.GetOffset(positionFromPoint.Value.Location));
    if (hoverInfoIfPossible != null && !string.IsNullOrEmpty(hoverInfoIfPossible.Text))
    {
      this.codeHoverInfoTooltip.Content = (object) hoverInfoIfPossible.Text;
      this.codeHoverInfoTooltip.IsOpen = true;
    }
    e.Handled = true;
  }

  private void OnTextEditorControlMouseOverStopper(object sender, System.Windows.Input.MouseEventArgs e)
  {
    if (!this.codeHoverInfoTooltip.IsOpen)
      return;
    this.codeHoverInfoTooltip.IsOpen = false;
    e.Handled = true;
  }

  private void OnAvalonCodeEditorAdapterLoad(object sender, EventArgs e)
  {
    Form parentForm = this.ParentForm;
    if (parentForm == null)
      return;
    this.InstallFindReplaceDialog(parentForm);
    this.InstallHotkeyHandler(parentForm);
  }

  private void OnAvalonCodeEditorAdapterEnter(object sender, EventArgs e)
  {
    this.ToggleHotkeyHandler(true);
  }

  private void OnAvalonCodeEditorAdapterLeave(object sender, EventArgs e)
  {
    this.ToggleHotkeyHandler(false);
  }

  private void InstallHotkeyHandler(Form form)
  {
    this.hotkeyForm = form;
    this.textEditorControl.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.OnTextEditorControlPreviewKeyDown);
  }

  private void UninstallHotkeyHandler()
  {
    if (this.hotkeyForm == null)
      return;
    this.textEditorControl.PreviewKeyDown -= new System.Windows.Input.KeyEventHandler(this.OnTextEditorControlPreviewKeyDown);
    this.hotkeyForm = (Form) null;
  }

  private void ToggleHotkeyHandler(bool enabled) => this.hotkeysAllowed = enabled;

  private void OnTextEditorControlPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
  {
    if (!this.hotkeysAllowed || e.KeyboardDevice.Modifiers != System.Windows.Input.ModifierKeys.Control || e.Key != Key.G)
      return;
    e.Handled = true;
    this.GoToLine();
  }

  private void SearchText()
  {
    if (!this.searchPanel.IsClosed)
      return;
    this.searchPanel.SearchPattern = this.textAreaControl.Selection.IsEmpty ? string.Empty : this.textAreaControl.Selection.GetText();
    this.searchPanel.Open();
    this.searchPanel.Reactivate();
  }

  public void GoToLine()
  {
    if (this.textEditorVM.Document.LineCount == 0)
      return;
    if (this.gotoTool == null)
      this.gotoTool = this.CreateGoToTool();
    this.gotoTool.MaxLineNumber = this.textEditorVM.Document.LineCount;
    this.gotoTool.LineNumber = this.GetCaretPosition().Line;
    if (this.gotoTool.ShowDialog() != DialogResult.OK)
      return;
    this.GoToLine(this.gotoTool.LineNumber);
  }

  public void GoToLine(int line)
  {
    if (line <= 0 || line > this.textEditorVM.Document.LineCount)
      throw new ArgumentOutOfRangeException(nameof (line));
    this.FocusAt(new TextCaretPosition(line, 1));
  }

  private GoToLineForm CreateGoToTool()
  {
    GoToLineForm goToTool = new GoToLineForm();
    goToTool.Owner = this.ParentForm;
    return goToTool;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
      this.DisposeWinformsAdapterControl();
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.wpfElementHost = new WpfElementHost();
    this.wpfWrapperControl = new TextEditorUserControl();
    this.SuspendLayout();
    this.wpfElementHost.Dock = DockStyle.Fill;
    this.wpfElementHost.Location = new System.Drawing.Point(0, 0);
    this.wpfElementHost.Name = "wpfElementHost";
    this.wpfElementHost.Size = new System.Drawing.Size(302, 262);
    this.wpfElementHost.TabIndex = 0;
    this.wpfElementHost.Text = "wpfElementHost";
    this.wpfElementHost.Child = (UIElement) this.wpfWrapperControl;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((System.Windows.Forms.Control) this.wpfElementHost);
    this.Name = nameof (WinformsAdapterControl);
    this.Size = new System.Drawing.Size(302, 262);
    this.Load += new EventHandler(this.OnAvalonCodeEditorAdapterLoad);
    this.Enter += new EventHandler(this.OnAvalonCodeEditorAdapterEnter);
    this.Leave += new EventHandler(this.OnAvalonCodeEditorAdapterLeave);
    this.ResumeLayout(false);
  }
}
