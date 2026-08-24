// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Presenters.ActiveScriptModifiedStatePresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Presenters;

internal sealed class ActiveScriptModifiedStatePresenter(
  IDEPresenter idePresenter,
  OpenScriptData script) : ActiveScriptChildPresenter(idePresenter, script)
{
  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.Script.ModifiedChanged += new EventHandler(this.OnScriptModified);
    this.CodeEditorControl.ScriptCodeChanged += new EventHandler(this.OnScriptCodeChanged);
    this.UpdateSaveCommandState();
    this.IDEView.SaveAsCommand.Enabled = this.IDEPresenter.IsNewScriptCommandAllowed();
    this.IDEView.SaveCopyCommand.Enabled = true;
  }

  protected override void OnDetachView(bool fullDetach)
  {
    this.Script.ModifiedChanged -= new EventHandler(this.OnScriptModified);
    this.CodeEditorControl.ScriptCodeChanged -= new EventHandler(this.OnScriptCodeChanged);
    if (fullDetach)
      this.ClearViewState();
    base.OnDetachView(fullDetach);
  }

  private void ClearViewState()
  {
    this.IDEView.SaveCommand.Enabled = false;
    this.IDEView.SaveAsCommand.Enabled = false;
    this.IDEView.SaveCopyCommand.Enabled = false;
  }

  private void UpdateSaveCommandState() => this.IDEView.SaveCommand.Enabled = this.Script.Modified;

  private void OnScriptCodeChanged(object sender, EventArgs e)
  {
    if (this.Script.Modified)
      return;
    this.Script.Modified = true;
  }

  private void OnScriptModified(object sender, EventArgs e) => this.UpdateSaveCommandState();
}
