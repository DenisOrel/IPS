// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Presenters.FindReplaceCommandPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Presenters;

internal class FindReplaceCommandPresenter(IDEPresenter idePresenter, OpenScriptData script) : 
  ActiveScriptChildPresenter(idePresenter, script)
{
  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.IDEView.FindReplaceCommand.Click += new EventHandler(this.ProcessFindReplaceCommand);
    this.IDEView.FindReplaceCommand.Enabled = true;
  }

  protected override void OnDetachView(bool fullDetach)
  {
    this.IDEView.FindReplaceCommand.Click -= new EventHandler(this.ProcessFindReplaceCommand);
    this.IDEView.FindReplaceCommand.Enabled = false;
    base.OnDetachView(fullDetach);
  }

  private void ProcessFindReplaceCommand(object sender, EventArgs e)
  {
    this.CodeEditorControl.ShowFindReplaceDialog();
  }
}
