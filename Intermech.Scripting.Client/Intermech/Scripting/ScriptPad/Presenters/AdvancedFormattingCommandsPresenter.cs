// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Presenters.AdvancedFormattingCommandsPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Presenters;

internal sealed class AdvancedFormattingCommandsPresenter(
  IDEPresenter idePresenter,
  OpenScriptData script) : ActiveScriptChildPresenter(idePresenter, script)
{
  protected override void OnAttachView()
  {
    base.OnAttachView();
    if (!this.Script.ReadOnlyMode)
    {
      this.IDEView.CommentSelectionCommand.Click += new EventHandler(this.ProcessCommentSelectionCommand);
      this.IDEView.UncommentSelectionCommand.Click += new EventHandler(this.ProcessUncommentSelectionCommand);
      this.IDEView.FormatIndentsCommand.Click += new EventHandler(this.ProcessFormatIndentsCommand);
      this.IDEView.CommentSelectionCommand.Enabled = this.Script.CommentSelectionAction != null;
      this.IDEView.UncommentSelectionCommand.Enabled = this.Script.UncommentSelectionAction != null;
      this.IDEView.FormatIndentsCommand.Enabled = this.Script.FormatIndentsAction != null;
    }
    else
      this.ClearViewState();
  }

  protected override void OnDetachView(bool fullDetach)
  {
    if (!this.Script.ReadOnlyMode)
    {
      this.IDEView.CommentSelectionCommand.Click -= new EventHandler(this.ProcessCommentSelectionCommand);
      this.IDEView.UncommentSelectionCommand.Click -= new EventHandler(this.ProcessUncommentSelectionCommand);
      this.IDEView.FormatIndentsCommand.Click -= new EventHandler(this.ProcessFormatIndentsCommand);
      if (fullDetach)
        this.ClearViewState();
    }
    base.OnDetachView(fullDetach);
  }

  private void ClearViewState()
  {
    this.IDEView.CommentSelectionCommand.Enabled = false;
    this.IDEView.UncommentSelectionCommand.Enabled = false;
    this.IDEView.FormatIndentsCommand.Enabled = false;
  }

  private void ProcessCommentSelectionCommand(object sender, EventArgs e)
  {
    try
    {
      this.Script.CommentSelectionAction.Invoke(this.CodeEditorControl.GetScriptCodeAsTextEditor());
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }

  private void ProcessUncommentSelectionCommand(object sender, EventArgs e)
  {
    try
    {
      this.Script.UncommentSelectionAction.Invoke(this.CodeEditorControl.GetScriptCodeAsTextEditor());
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }

  private void ProcessFormatIndentsCommand(object sender, EventArgs e)
  {
    try
    {
      this.Script.FormatIndentsAction.Invoke(this.CodeEditorControl.GetScriptCodeAsTextEditor());
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }
}
