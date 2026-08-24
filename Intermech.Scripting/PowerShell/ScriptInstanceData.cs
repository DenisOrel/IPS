// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.PowerShell.ScriptInstanceData
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Collections.Generic;
using System.Management.Automation.Runspaces;

#nullable disable
namespace Intermech.Scripting.PowerShell;

internal sealed class ScriptInstanceData
{
  public ScriptInstanceData(ScriptInstanceBucket sharedData) => this.SharedData = sharedData;

  public ScriptInstanceBucket SharedData { get; private set; }

  public Runspace Runspace { get; set; }

  public ICollection<string> InitialFunctions { get; set; }

  public bool IsEmpty { get; set; }

  public int UseCount { get; set; }
}
