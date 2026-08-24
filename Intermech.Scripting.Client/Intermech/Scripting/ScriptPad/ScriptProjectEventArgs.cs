// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.ScriptProjectEventArgs
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal class ScriptProjectEventArgs : EventArgs
{
  private ScriptProject scriptProject;

  public ScriptProjectEventArgs(ScriptProject scriptProject)
  {
    this.scriptProject = scriptProject != null ? scriptProject : throw new ArgumentNullException(nameof (scriptProject));
  }

  public ScriptProject ScriptProject
  {
    [DebuggerStepThrough] get => this.scriptProject;
  }
}
