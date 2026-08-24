// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptCompilationException
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Scripting;

[Serializable]
public class ScriptCompilationException : ScriptingException
{
  private const string errorsProperty = "Errors";
  private IList<ScriptCompilationError> errors;

  public ScriptCompilationException(string message)
    : base(message)
  {
    this.errors = (IList<ScriptCompilationError>) new ScriptCompilationError[0];
  }

  public ScriptCompilationException(string message, IList<ScriptCompilationError> errors)
    : base(message)
  {
    this.errors = errors != null ? errors : throw new ArgumentNullException(nameof (errors));
  }

  public ScriptCompilationException(string message, Exception innerException)
    : base(message, innerException)
  {
    this.errors = (IList<ScriptCompilationError>) new ScriptCompilationError[0];
  }

  public static ScriptCompilationException FromErrors(
    string languageName,
    IList<ScriptCompilationError> errors)
  {
    if (languageName == null)
      throw new ArgumentNullException(nameof (languageName));
    if (errors == null)
      throw new ArgumentNullException(nameof (errors));
    ScriptCompilationError compilationError = errors.Count != 0 ? errors[0] : throw new ArgumentException("Список ошибок не должен быть пуст.", nameof (errors));
    return new ScriptCompilationException($"Ошибка компиляции {languageName}-сценария в позиции ({compilationError.Line}, {compilationError.Column}). {compilationError.ErrorNumber}: {compilationError.ErrorText}", errors);
  }

  public IList<ScriptCompilationError> Errors
  {
    [DebuggerStepThrough] get => this.errors;
  }

  protected ScriptCompilationException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
    this.errors = (IList<ScriptCompilationError>) info.GetValue(nameof (Errors), typeof (IList<ScriptCompilationError>));
  }

  public override void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    base.GetObjectData(info, context);
    info.AddValue("Errors", (object) this.errors);
  }
}
