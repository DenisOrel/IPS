// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ScriptInvocationData
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Базовый класс для контейнера параметров, которые описывают обращение к сценарию.
/// Сам контейнер и его данные размещаются в основном AppDomain приложения.
/// Реализация не обязана быть thread safe.
/// </summary>
public class ScriptInvocationData : MarshalByRefObject
{
  private Lazy<Dictionary<string, object>> cache;

  /// <summary>Создает объект.</summary>
  public ScriptInvocationData()
  {
    this.cache = new Lazy<Dictionary<string, object>>((Func<Dictionary<string, object>>) (() => new Dictionary<string, object>()), false);
  }

  /// <summary>
  /// Возвращает кэш объектов, используемых инфраструктурой выполнения сценариев.
  /// Кэш существует только во время выполнения сценария, а после этого очищается.
  /// Если объекты в кэше поддерживают IDisposable, то у них будет вызван Dispose().
  /// </summary>
  public Dictionary<string, object> Cache
  {
    [DebuggerStepThrough] get => this.cache.Value;
  }

  internal void Clear()
  {
    if (!this.cache.IsValueCreated)
      return;
    Dictionary<string, object> cachedObjects = this.cache.Value;
    if (cachedObjects.Count == 0)
      return;
    this.DisposeCachedObjects(cachedObjects);
    cachedObjects.Clear();
  }

  private void DisposeCachedObjects(Dictionary<string, object> cachedObjects)
  {
    foreach (KeyValuePair<string, object> cachedObject in cachedObjects)
      DisposeUtils.TryDispose(cachedObject.Value);
  }
}
