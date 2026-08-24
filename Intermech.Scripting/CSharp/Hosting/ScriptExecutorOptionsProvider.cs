// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.Hosting.ScriptExecutorOptionsProvider
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.CSharp.Hosting;

/// <summary>
/// Базовый класс для поставщиков опций выполнения C#-сценариев.
/// Реализация класса является thread safe.
/// </summary>
public abstract class ScriptExecutorOptionsProvider
{
  private object syncRoot;
  private bool? logAllInvocations;

  /// <summary>Создает объект.</summary>
  protected ScriptExecutorOptionsProvider() => this.syncRoot = new object();

  public bool LogAllInvocations
  {
    get
    {
      lock (this.syncRoot)
      {
        if (!this.logAllInvocations.HasValue)
          this.logAllInvocations = new bool?(this.GetLogAllInvocationsOption("CSharpScripts.LogAllInvocations", false));
        return this.logAllInvocations.Value;
      }
    }
  }

  protected abstract bool GetLogAllInvocationsOption(string optionName, bool defaultValue);
}
