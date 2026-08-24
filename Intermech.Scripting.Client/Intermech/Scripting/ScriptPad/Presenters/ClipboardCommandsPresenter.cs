// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Presenters.ClipboardCommandsPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Presenters;

internal sealed class ClipboardCommandsPresenter(IDEPresenter idePresenter, OpenScriptData script) : 
  ActiveScriptChildPresenter(idePresenter, script)
{
  private IScriptCodeEditorCutCopyPaste cutCopyPasteSupport;

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.cutCopyPasteSupport = this.CodeEditorControl as IScriptCodeEditorCutCopyPaste;
    if (this.cutCopyPasteSupport != null)
    {
      this.IDEView.CutCommand.Click += new EventHandler(this.ProcessCutCommand);
      this.IDEView.CopyCommand.Click += new EventHandler(this.ProcessCopyCommand);
      this.IDEView.PasteCommand.Click += new EventHandler(this.ProcessPasteCommand);
      this.CodeEditorControl.SelectionChanged += new EventHandler(this.OnCodeEditorSelectionChanged);
      this.UpdateCutCopyCommandStates();
      this.UpdatePasteCommandStates();
    }
    else
      this.ClearViewState();
  }

  protected override void OnDetachView(bool fullDetach)
  {
    if (this.cutCopyPasteSupport != null)
    {
      this.IDEView.CutCommand.Click -= new EventHandler(this.ProcessCutCommand);
      this.IDEView.CopyCommand.Click -= new EventHandler(this.ProcessCopyCommand);
      this.IDEView.PasteCommand.Click -= new EventHandler(this.ProcessPasteCommand);
      this.CodeEditorControl.SelectionChanged -= new EventHandler(this.OnCodeEditorSelectionChanged);
      if (fullDetach)
        this.ClearViewState();
      this.cutCopyPasteSupport = (IScriptCodeEditorCutCopyPaste) null;
    }
    base.OnDetachView(fullDetach);
  }

  private void ClearViewState()
  {
    this.IDEView.CutCommand.Enabled = false;
    this.IDEView.CopyCommand.Enabled = false;
    this.IDEView.PasteCommand.Enabled = false;
  }

  private void UpdateCutCopyCommandStates()
  {
    bool flag = this.CodeEditorControl.HasSelection();
    this.IDEView.CutCommand.Enabled = flag;
    this.IDEView.CopyCommand.Enabled = flag;
  }

  private void UpdatePasteCommandStates()
  {
    this.IDEView.PasteCommand.Enabled = !this.Script.ReadOnlyMode;
  }

  private void OnCodeEditorSelectionChanged(object sender, EventArgs e)
  {
    this.UpdateCutCopyCommandStates();
  }

  private void ProcessCutCommand(object sender, EventArgs e)
  {
    try
    {
      this.cutCopyPasteSupport.Cut();
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }

  private void ProcessCopyCommand(object sender, EventArgs e)
  {
    try
    {
      this.cutCopyPasteSupport.Copy();
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }

  private void ProcessPasteCommand(object sender, EventArgs e)
  {
    try
    {
      this.cutCopyPasteSupport.Paste();
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }
}
