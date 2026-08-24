// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Presenters.ReplaceWithCommandPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Presenters;

internal sealed class ReplaceWithCommandPresenter(IDEPresenter idePresenter, OpenScriptData script) : 
  ActiveScriptChildPresenter(idePresenter, script)
{
  private bool isCommandAllowed;

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.isCommandAllowed = !this.Script.ReadOnlyMode && this.Script.Project.Behaviors.GetReplacementBehavior(false) != null;
    if (this.isCommandAllowed)
    {
      this.IDEView.ReplaceWithCommand.Click += new EventHandler(this.ProcessReplaceWithCommand);
      this.IDEView.ReplaceWithCommand.Enabled = true;
    }
    else
      this.ClearViewState();
  }

  protected override void OnDetachView(bool fullDetach)
  {
    if (this.isCommandAllowed)
    {
      this.IDEView.ReplaceWithCommand.Click -= new EventHandler(this.ProcessReplaceWithCommand);
      if (fullDetach)
        this.ClearViewState();
      this.isCommandAllowed = false;
    }
    base.OnDetachView(fullDetach);
  }

  private void ClearViewState() => this.IDEView.ReplaceWithCommand.Enabled = false;

  private void ProcessReplaceWithCommand(object sender, EventArgs e)
  {
    try
    {
      IScriptReplacementBehavior replacementBehavior = this.Script.Project.Behaviors.GetReplacementBehavior();
      ScriptProject anotherScriptProject = replacementBehavior.TryGetAnotherScriptProject();
      if (anotherScriptProject == null)
        return;
      this.IDEPresenter.ReplaceScriptProject(this.Script.Project, anotherScriptProject, false);
      replacementBehavior.AfterReplace(anotherScriptProject);
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }
}
