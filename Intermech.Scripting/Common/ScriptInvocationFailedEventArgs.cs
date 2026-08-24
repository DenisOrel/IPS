// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ScriptInvocationFailedEventArgs
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common;

public class ScriptInvocationFailedEventArgs : ScriptInvocationEventArgs
{
  public ScriptInvocationFailedEventArgs(
    string scriptCode,
    object[] arguments,
    ScriptInvocationException exception)
    : base(scriptCode, arguments)
  {
    this.Exception = exception != null ? exception : throw new ArgumentNullException(nameof (exception));
  }

  public ScriptInvocationException Exception { get; private set; }
}
