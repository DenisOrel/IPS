// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptCompilationError
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting;

[Serializable]
public sealed class ScriptCompilationError
{
  public ScriptCompilationError(
    string errorNumber,
    string errorText,
    string fileName,
    int line,
    int column,
    bool isWarning)
  {
    this.ErrorNumber = errorNumber;
    this.ErrorText = errorText;
    this.FileName = fileName;
    this.Line = line;
    this.Column = column;
    this.IsWarning = isWarning;
  }

  public string ErrorNumber { get; private set; }

  public string ErrorText { get; private set; }

  public string FileName { get; private set; }

  public int Line { get; private set; }

  public int Column { get; private set; }

  public bool IsWarning { get; private set; }
}
