// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Projects.DiskScripts.DebugParametersPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Mvp;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.Projects.DiskScripts;

internal sealed class DebugParametersPresenter : Presenter<IDebugParametersView>
{
  private List<string> scriptArguments;

  public List<string> ScriptArguments
  {
    [DebuggerStepThrough] get => this.scriptArguments;
    [DebuggerStepThrough] set => this.scriptArguments = value;
  }

  protected override void DoValidate()
  {
    base.DoValidate();
    if (this.ScriptArguments == null)
      throw new PresenterPropertyException("ScriptArguments");
  }

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.View.ScriptArguments = (ICollection<string>) this.ScriptArguments;
    this.View.OperationConfirmed += new EventHandler(this.OnApplyChanges);
  }

  protected override void OnDetachView()
  {
    this.View.ScriptArguments = (ICollection<string>) null;
    this.View.OperationConfirmed -= new EventHandler(this.OnApplyChanges);
    base.OnDetachView();
  }

  private void OnApplyChanges(object sender, EventArgs e)
  {
    this.ScriptArguments = new List<string>((IEnumerable<string>) this.View.ScriptArguments);
  }
}
