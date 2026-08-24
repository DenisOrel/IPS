// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ScriptCompiledCodeCache`1
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Concurrent;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Сервис для кэширования результатов компиляции сценариев.
/// Реализация класса является thread safe и может вызываться из изолированных AppDomain.
/// </summary>
public class ScriptCompiledCodeCache<TCompiledCode> : ScriptExecutorServiceBase
{
  private ConcurrentDictionary<ScriptCodeKey, TCompiledCode> items;

  /// <summary>Создает объект.</summary>
  public ScriptCompiledCodeCache()
  {
    this.items = new ConcurrentDictionary<ScriptCodeKey, TCompiledCode>();
  }

  public bool ContainsKey(ScriptCodeKey key)
  {
    return key != null ? this.items.ContainsKey(key) : throw new ArgumentNullException(nameof (key));
  }

  public TCompiledCode TryGet(ScriptCodeKey key)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    TCompiledCode compiledCode;
    return this.items.TryGetValue(key, out compiledCode) ? compiledCode : default (TCompiledCode);
  }

  public void Update(ScriptCodeKey key, TCompiledCode compiledCodeInfo)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    this.items[key] = compiledCodeInfo;
  }
}
