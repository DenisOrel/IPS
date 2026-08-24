// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.EmptyCodeModel
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public class EmptyCodeModel : ICodeModel
{
  public Action<string> Log { get; set; }

  public Dictionary<string, string> ParseOptions { get; set; }

  public void ChangeText(List<ScriptTextChange> changes)
  {
  }

  public CodeModelSynchronizationStatus CheckSynchronizationStatus()
  {
    return CodeModelSynchronizationStatus.Synchronized;
  }

  public void CloseText(bool throwIfError)
  {
  }

  public void OpenText(string text)
  {
  }
}
