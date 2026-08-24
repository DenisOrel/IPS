// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.ScriptProjectErrorRecord
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class ScriptProjectErrorRecord
{
  public ScriptProjectErrorRecord(
    ScriptProject scriptProject,
    string scriptDisplayName,
    ScriptCompilationError error)
  {
    if (scriptProject == null)
      throw new ArgumentNullException(nameof (scriptProject));
    if (scriptDisplayName == null)
      throw new ArgumentNullException(nameof (scriptDisplayName));
    if (error == null)
      throw new ArgumentNullException(nameof (error));
    this.ScriptProject = scriptProject;
    this.ScriptDisplayName = scriptDisplayName;
    this.Error = error;
  }

  public ScriptProject ScriptProject { get; private set; }

  public string ScriptDisplayName { get; private set; }

  public ScriptCompilationError Error { get; private set; }
}
