// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.PowerShell.ScriptInstanceBucket
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.PowerShell;

/// <summary>
/// Элемент кэша Powershell-сценариев. Он содержит все объекты, относящиеся к одному сценарию.
/// Реализация класса является thread safe.
/// </summary>
internal sealed class ScriptInstanceBucket
{
  private ScriptCodeKey scriptCodeKey;
  private ConcurrentBag<ScriptInstanceData> cachedRunspaces;
  private string[] serviceProperties;
  private object syncRoot;
  private static readonly string[] emptyServiceProperties = new string[0];

  public ScriptInstanceBucket(ScriptCodeKey scriptCodeKey)
  {
    this.scriptCodeKey = scriptCodeKey;
    this.cachedRunspaces = new ConcurrentBag<ScriptInstanceData>();
    this.serviceProperties = ScriptInstanceBucket.emptyServiceProperties;
    this.syncRoot = new object();
  }

  public ScriptCodeKey ScriptCodeKey
  {
    [DebuggerStepThrough] get => this.scriptCodeKey;
  }

  public ConcurrentBag<ScriptInstanceData> CachedRunspaces
  {
    [DebuggerStepThrough] get => this.cachedRunspaces;
  }

  public string[] ServiceProperties
  {
    [DebuggerStepThrough] get => this.serviceProperties;
  }

  public void SetServiceProperties(string[] value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    Interlocked.Exchange<string[]>(ref this.serviceProperties, value);
  }
}
