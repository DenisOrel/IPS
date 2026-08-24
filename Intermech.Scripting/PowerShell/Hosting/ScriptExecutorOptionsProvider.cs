// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.PowerShell.Hosting.ScriptExecutorOptionsProvider
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.PowerShell.Hosting;

/// <summary>
/// Базовый класс для поставщиков опций выполнения PowerShell-сценариев.
/// Реализация класса является thread safe.
/// </summary>
public abstract class ScriptExecutorOptionsProvider
{
  private object syncRoot;
  private int? runspaceUseLimit;
  private bool? logAllInvocations;

  /// <summary>Создает объект.</summary>
  protected ScriptExecutorOptionsProvider() => this.syncRoot = new object();

  public int RunspaceUseCountLimit
  {
    get
    {
      lock (this.syncRoot)
      {
        if (!this.runspaceUseLimit.HasValue)
          this.runspaceUseLimit = new int?(this.GetRunspaceUseCountLimitOption("PowerShellScripts.RunspaceUseCountLimit", 50));
        return this.runspaceUseLimit.Value;
      }
    }
  }

  public bool LogAllInvocations
  {
    get
    {
      lock (this.syncRoot)
      {
        if (!this.logAllInvocations.HasValue)
          this.logAllInvocations = new bool?(this.GetLogAllInvocationsOption("PowerShellScripts.LogAllInvocations", false));
        return this.logAllInvocations.Value;
      }
    }
  }

  protected abstract int GetRunspaceUseCountLimitOption(string optionName, int defaultValue);

  protected abstract bool GetLogAllInvocationsOption(string optionName, bool defaultValue);
}
