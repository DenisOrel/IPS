// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.CodeEditorMenuItemViewModel
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using Intermech.UI;
using System;
using System.Windows.Input;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal class CodeEditorMenuItemViewModel : ViewModel, ICommand
{
  private ITextEditorUIAction textEditorAction;
  private bool canExecute;
  private ITextEditor designTimeTextEditor;

  public CodeEditorMenuItemViewModel(ITextEditorUIAction textEditorAction)
  {
    this.textEditorAction = textEditorAction != null ? textEditorAction : throw new ArgumentNullException(nameof (textEditorAction));
  }

  public string Text => this.textEditorAction.Text;

  public ITextEditor DesignTimeTextEditor
  {
    get => this.designTimeTextEditor;
    set
    {
      if (this.designTimeTextEditor == value)
        return;
      this.designTimeTextEditor = value;
      if (value != null)
      {
        bool flag = this.textEditorAction.CanInvoke(value);
        if (this.canExecute != flag)
        {
          this.canExecute = flag;
          EventHandler canExecuteChanged = this.CanExecuteChanged;
          if (canExecuteChanged != null)
            canExecuteChanged((object) this, EventArgs.Empty);
        }
      }
      else if (this.canExecute)
      {
        this.canExecute = false;
        EventHandler canExecuteChanged = this.CanExecuteChanged;
        if (canExecuteChanged != null)
          canExecuteChanged((object) this, EventArgs.Empty);
      }
      this.RaisePropertyChanged(nameof (DesignTimeTextEditor));
    }
  }

  public bool CanExecute(object parameter) => this.canExecute;

  public void Execute(object parameter)
  {
    if (!this.canExecute || this.designTimeTextEditor == null)
      return;
    this.textEditorAction.Invoke(this.designTimeTextEditor);
  }

  public event EventHandler CanExecuteChanged;
}
