// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Presenters.UndoRedoCommandsPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Presenters;

internal sealed class UndoRedoCommandsPresenter(IDEPresenter idePresenter, OpenScriptData script) : 
  ActiveScriptChildPresenter(idePresenter, script)
{
  private IScriptCodeEditorUndoRedo undoRedoSupport;

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.undoRedoSupport = this.CodeEditorControl as IScriptCodeEditorUndoRedo;
    if (this.undoRedoSupport != null)
    {
      this.IDEView.UndoCommand.Click += new EventHandler(this.ProcessUndoCommand);
      this.IDEView.RedoCommand.Click += new EventHandler(this.ProcessRedoCommand);
      this.CodeEditorControl.ScriptCodeChanged += new EventHandler(this.OnScriptCodeChanged);
      this.UpdateUndoRedoCommandsState();
    }
    else
      this.ClearViewState();
  }

  protected override void OnDetachView(bool fullDetach)
  {
    if (this.undoRedoSupport != null)
    {
      this.IDEView.UndoCommand.Click -= new EventHandler(this.ProcessUndoCommand);
      this.IDEView.RedoCommand.Click -= new EventHandler(this.ProcessRedoCommand);
      this.CodeEditorControl.ScriptCodeChanged -= new EventHandler(this.OnScriptCodeChanged);
      if (fullDetach)
        this.ClearViewState();
    }
    base.OnDetachView(fullDetach);
  }

  private void ClearViewState()
  {
    this.IDEView.UndoCommand.Enabled = false;
    this.IDEView.RedoCommand.Enabled = false;
  }

  private void UpdateUndoRedoCommandsState()
  {
    this.IDEView.UndoCommand.Enabled = this.undoRedoSupport.CanUndo;
    this.IDEView.RedoCommand.Enabled = this.undoRedoSupport.CanRedo;
  }

  private void OnScriptCodeChanged(object sender, EventArgs e)
  {
    this.UpdateUndoRedoCommandsState();
  }

  private void ProcessUndoCommand(object sender, EventArgs e)
  {
    try
    {
      this.undoRedoSupport.Undo();
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }

  private void ProcessRedoCommand(object sender, EventArgs e)
  {
    try
    {
      this.undoRedoSupport.Redo();
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }
}
