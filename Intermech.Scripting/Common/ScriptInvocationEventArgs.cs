// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ScriptInvocationEventArgs
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common;

public class ScriptInvocationEventArgs : EventArgs
{
  public ScriptInvocationEventArgs(string scriptCode, object[] arguments)
  {
    if (scriptCode == null)
      throw new ArgumentNullException(nameof (scriptCode));
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    this.ScriptCode = scriptCode;
    this.Arguments = arguments;
  }

  public string ScriptCode { get; private set; }

  public object[] Arguments { get; private set; }
}
