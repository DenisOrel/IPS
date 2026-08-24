// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.TextEditorUserControl
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using Intermech.Scripting.Common.DesignTime;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using WpfBindingErrors;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal class TextEditorUserControl : UserControl, IComponentConnector
{
  internal TextEditor CodeEditor;
  private bool _contentLoaded;

  public TextEditorUserControl()
  {
    this.InitializeComponent();
    if (DesignerProperties.GetIsInDesignMode((DependencyObject) this))
      return;
    this.CodeEditor.TextArea.Caret.PositionChanged += new EventHandler(this.OnCodeEditorCaretPositionChanged);
    this.CodeEditor.TextArea.SelectionChanged += new EventHandler(this.OnCodeEditorSelectionChanged);
  }

  [Conditional("RELEASE")]
  private void EnableBindingExceptionThrower()
  {
    if (BindingExceptionThrower.IsAttached)
      return;
    BindingExceptionThrower.Attach();
  }

  private void OnCodeEditorCaretPositionChanged(object sender, EventArgs e)
  {
    if (!(this.DataContext is TextEditorViewModel dataContext))
      return;
    Caret caret = this.CodeEditor.TextArea.Caret;
    dataContext.UpdateCaretPosition(caret.Offset, caret.Line, caret.Column);
  }

  private void OnCodeEditorSelectionChanged(object sender, EventArgs e)
  {
    if (!(this.DataContext is TextEditorViewModel dataContext))
      return;
    dataContext.UpdateSelection(this.CodeEditor.SelectionStart, this.CodeEditor.SelectionLength);
  }

  private void OnCodeEditorContextMenuOpening(object sender, ContextMenuEventArgs e)
  {
    if (!(this.DataContext is TextEditorViewModel dataContext))
      return;
    DesignTimeTextEditorAdapter textEditorAdapter = dataContext.AsDesignTimeTextEditor();
    foreach (CodeEditorMenuItemViewModel menuItemViewModel in (Collection<CodeEditorMenuItemViewModel>) dataContext.ContextMenu.Items)
      menuItemViewModel.DesignTimeTextEditor = (ITextEditor) textEditorAdapter;
  }

  private void OnCodeEditorContextMenuClosing(object sender, ContextMenuEventArgs e)
  {
    if (!(this.DataContext is TextEditorViewModel dataContext))
      return;
    foreach (CodeEditorMenuItemViewModel menuItemViewModel in (Collection<CodeEditorMenuItemViewModel>) dataContext.ContextMenu.Items)
      menuItemViewModel.DesignTimeTextEditor = (ITextEditor) null;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/Intermech.Scripting.Client;component/scriptpad/views/avaloncodeeditor/texteditorusercontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
    {
      this.CodeEditor = (TextEditor) target;
      this.CodeEditor.ContextMenuOpening += new ContextMenuEventHandler(this.OnCodeEditorContextMenuOpening);
      this.CodeEditor.ContextMenuClosing += new ContextMenuEventHandler(this.OnCodeEditorContextMenuClosing);
    }
    else
      this._contentLoaded = true;
  }
}
