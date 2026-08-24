// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ScriptDebugTraceListener
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Используется для перехвата отладочного вывода из C#-сценариев.
/// Реализация не является thread safe.
/// </summary>
internal sealed class ScriptDebugTraceListener : TraceListener
{
  private int threadId;
  private List<string> lines;
  private string lastLine;

  public ScriptDebugTraceListener()
  {
    this.lines = new List<string>();
    this.lastLine = string.Empty;
  }

  /// <summary>
  /// Возвращает или задает идентификатор thread,
  /// для которого выполняется перехват отладочного вывода.
  /// </summary>
  public int ThreadId
  {
    [DebuggerStepThrough] get => this.threadId;
    [DebuggerStepThrough] set => this.threadId = value;
  }

  public bool IsEmpty
  {
    [DebuggerStepThrough] get => this.lines.Count == 0 && this.lastLine == string.Empty;
  }

  public void Clear()
  {
    this.lines.Clear();
    this.lastLine = string.Empty;
  }

  public List<string> ToList()
  {
    if (this.lastLine != string.Empty)
      this.WriteLine(string.Empty);
    return new List<string>((IEnumerable<string>) this.lines);
  }

  public override bool IsThreadSafe
  {
    [DebuggerStepThrough] get => false;
  }

  public override void Write(string message)
  {
    if (this.threadId != Thread.CurrentThread.ManagedThreadId || string.IsNullOrEmpty(message))
      return;
    this.lastLine += message;
  }

  public override void WriteLine(string message)
  {
    if (this.threadId != Thread.CurrentThread.ManagedThreadId || message == null)
      return;
    if (message != string.Empty)
      this.lastLine += message;
    this.lines.Add(this.lastLine);
    this.lastLine = string.Empty;
  }
}
