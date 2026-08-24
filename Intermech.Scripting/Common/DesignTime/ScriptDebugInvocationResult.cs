// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ScriptDebugInvocationResult
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public class ScriptDebugInvocationResult
{
  public ScriptDebugInvocationResult(object returnValue, bool isError)
  {
    this.ReturnValue = returnValue;
    this.IsError = isError;
  }

  public object ReturnValue { get; private set; }

  public bool IsError { get; private set; }
}
