// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ScriptDebugInvocationParameters
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public sealed class ScriptDebugInvocationParameters
{
  public ScriptDebugInvocationParameters()
  {
    this.Arguments = new List<object>();
    this.ProjectOptions = new Dictionary<string, string>(0);
  }

  public List<object> Arguments { get; private set; }

  /// <summary>
  /// Возвращает опции сценария, которые могут включать опции языка и опции среды выполнения.
  /// Свойство используется для передачи деталей выполнения сценария в <see cref="T:Intermech.Scripting.Common.DesignTime.ILanguageSession" />.
  /// </summary>
  public Dictionary<string, string> ProjectOptions { get; set; }
}
