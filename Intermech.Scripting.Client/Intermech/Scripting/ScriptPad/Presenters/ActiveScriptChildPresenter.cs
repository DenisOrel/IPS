// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Presenters.ActiveScriptChildPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Presenters;

internal abstract class ActiveScriptChildPresenter : IDEChildPresenter
{
  private OpenScriptData script;
  private IScriptCodeEditorControl codeEditorControl;

  protected ActiveScriptChildPresenter(IDEPresenter idePresenter, OpenScriptData script)
    : base(idePresenter)
  {
    this.script = script != null ? script : throw new ArgumentNullException(nameof (script));
  }

  protected OpenScriptData Script
  {
    [DebuggerStepThrough] get => this.script;
  }

  protected IScriptCodeEditorControl CodeEditorControl
  {
    [DebuggerStepThrough] get => this.codeEditorControl;
  }

  protected override void DoValidate()
  {
    base.DoValidate();
    if (this.script.Window == null)
      throw new InvalidOperationException("The active script must have a window.");
  }

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.codeEditorControl = this.script.Window.CodeEditor;
  }

  protected override void OnDetachView(bool fullDetach)
  {
    this.codeEditorControl = (IScriptCodeEditorControl) null;
    base.OnDetachView(fullDetach);
  }
}
